using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings
{
    [Serializable]
    public class MatchSettingsDto
    {
        [SerializeField]
        bool _isRadarOn;

        [SerializeField]
        float _halfLength;

        [SerializeField]
        MatchDifficultyEnum _matchDifficulty;

        public MatchSettingsDto(MatchSettingsDto matchSettings)
        {
            _halfLength = matchSettings.HalfLength;
            _isRadarOn = matchSettings.IsRadarOn;
            _matchDifficulty = matchSettings.MatchDifficulty;
        }

        public bool IsRadarOn { get => _isRadarOn; set => _isRadarOn = value; }
        public float HalfLength { get => _halfLength; set => _halfLength = value; }
        public MatchDifficultyEnum MatchDifficulty { get => _matchDifficulty; set => _matchDifficulty = value; }
    }
}
