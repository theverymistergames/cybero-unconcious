using System;
using MisterGames.Tick.Core;
using MisterGames.Tick.Jobs;
using UnityEngine;

namespace Tween {

    [Serializable]
    internal struct TweenAudio {
        public AudioClip clip;
        public float volume;
        public float delay;
        public bool loop;
    }
    
    [Serializable]
    public class AudioTween : Tween {

        [SerializeField] private TweenAudio[] audios;

        private AudioSource _source;
        private float _oldProgress = -1;
        private Job _job;

        public override void Init(GameObject gameobj, PlayerLoopStage stage) {
            base.Init(gameobj, stage);

            _source = tweenableObject.GetComponent<AudioSource>();

            if (_source == null) throw new Exception("No audio source detected");
        }

        private void PlaySound(TweenAudio audio) {
            if (audio.loop) {
                PlayLoopSound(audio);
            } else {
                _source.PlayOneShot(audio.clip, audio.volume);            
            }
        }

        private void PlayLoopSound(TweenAudio audio) {
            _source.PlayOneShot(audio.clip, audio.volume);

            _job.Dispose();

            _job = JobSequence.Create(timeSourceStage)
                .Delay(audio.clip.length)
                .Action(() => PlayLoopSound(audio))
                .Push()
                .Start();
        }
    
        public override void Pause() {
            base.Pause();

            _job.Stop();
            _source.Pause();
        }

        public override void Play() {
            base.Play();
            
            _job.Start();
            _source.Play();
        }
        
        public override void Stop() {
            base.Stop();
            
            _job.Stop();
            _source.Stop();
        }

        protected override void ProceedUpdate(float progress) {
            if (_source == null) return;
            
            foreach (var audio in audios) {
                if (audio.delay >= _oldProgress && audio.delay <= progress) {
                    PlaySound(audio);
                }
            }

            _oldProgress = progress;
        }
    }
}
