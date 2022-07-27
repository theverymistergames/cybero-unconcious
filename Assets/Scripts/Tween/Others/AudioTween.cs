using System;
using MisterGames.Common.Routines;
using UnityEngine;

namespace Tween {
    [Serializable]internal struct TweenAudio {
        public AudioClip clip;
        public float volume;
        public float delay;
        public bool loop;
    }
    
    [Serializable]public class AudioTween : Tween {
        [SerializeField] private TweenAudio[] audios;
        private AudioSource _source;
        private float _oldProgress = -1;
        private readonly SingleJobHandler _handler = new SingleJobHandler();

        public override void Init(GameObject gameobj, TimeDomain domain) {
            base.Init(gameobj, domain);

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
            Jobs.Do(Domain.Delay(audio.clip.length)).Then(() => PlayLoopSound(audio)).StartFrom(_handler);
        }
    
        public override void Pause() {
            base.Pause();

            _handler.Pause();
            _source.Pause();
        }

        public override void Play() {
            base.Play();
            
            _handler.Resume();
            _source.Play();
        }
        
        public override void Stop() {
            base.Stop();
            
            _handler.Stop();
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