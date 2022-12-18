using System;
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

    }
}
