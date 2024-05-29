using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings
{
    [Serializable]
    public class SoundSettingsDto
    {
        [SerializeField]
        bool _isSoundOn;

        [SerializeField]
        float _musicVolume;

        [SerializeField]
        float _soundVolume;
        private bool isSoundEnabled;

        public SoundSettingsDto(bool isSoundOn, float musicVolume, float soundVolume)
        {
            _isSoundOn = isSoundOn;
            _musicVolume = musicVolume;
            _soundVolume = soundVolume;
        }

        public bool IsSoundOn { get => _isSoundOn; set => _isSoundOn = value; }
        public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }
        public float SfxVolume { get => _soundVolume; set => _soundVolume = value; }
    }
}
