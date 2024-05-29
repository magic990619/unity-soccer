using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings
{
    [Serializable]
    public class SettingsDto
    {
        [SerializeField]
        MatchSettingsDto _matchSettings;

        [SerializeField]
        SoundSettingsDto _soundSettings;

        public MatchSettingsDto MatchSettings { get => _matchSettings; set => _matchSettings = value; }
        public SoundSettingsDto SoundSettings { get => _soundSettings; set => _soundSettings = value; }
    }
}
