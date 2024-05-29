using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Storage.MatchDifficulties
{
    [Serializable]
    public class MatchDifficulty
    {
        [SerializeField]
        string _name;

        [SerializeField]
        MatchDifficultyEnum _matchDifficultyType;

        [SerializeField]
        MatchDifficultyTeamParam _matchDifficultyCPUTeamParams;
        [SerializeField]
        MatchDifficultyTeamParam _teamMatchDifficultyUserTeamParams;

        public string Name { get => _name; set => _name = value; }
        public MatchDifficultyEnum MatchDifficultyType { get => _matchDifficultyType; set => _matchDifficultyType = value; }
        public MatchDifficultyTeamParam MatchDifficultyCPUTeamParams { get => _matchDifficultyCPUTeamParams; set => _matchDifficultyCPUTeamParams = value; }
        public MatchDifficultyTeamParam MatchDifficultyUserTeamParams { get => _teamMatchDifficultyUserTeamParams; set => _teamMatchDifficultyUserTeamParams = value; }
    }
}
