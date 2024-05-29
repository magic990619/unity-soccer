using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Components
{
    [Serializable]
    public class AudioSourceComponentDto
    {
        [SerializeField]
        private bool _canLoop;

        [SerializeField]
        private bool _canPlayOnAwake;

        //[SerializeField]
        //private bool _createNewOnPlay;

        [SerializeField]
        [Range(0f, 1f)]
        private float _volume;

        [SerializeField]
        private int _id;

        [SerializeField]
        private SoundTypeEnum _soundype;

        [SerializeField]
        private AudioClip _audioClip;

        public bool CanLoop { get => _canLoop; set => _canLoop = value; }
        public bool CanPlayOnAwake { get => _canPlayOnAwake; set => _canPlayOnAwake = value; }
        public float Volume { get => _volume; set => _volume = value; }
        public int Id { get => _id; set => _id = value; }
        public SoundTypeEnum Soundype { get => _soundype; set => _soundype = value; }
        public AudioClip AudioClip { get => _audioClip; set => _audioClip = value; }
        //public bool CreateNewOnPlay { get => _createNewOnPlay; set => _createNewOnPlay = value; }
    }
}
