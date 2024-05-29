using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Storage.MatchDifficulties;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Tactics;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using Patterns.Singleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.FootballGameEngine_Indie.Scripts.Managers
{
    /// <summary>
    /// Manages the entire game
    /// </summary>
    [RequireComponent(typeof(GameManagerFSM))]
    public class GameManager : Singleton<GameManager>
    {
        [Header("Database Settings")]
        [SerializeField]
        string _settingsSaveFileName;

        [SerializeField]
        string _teamsDataSaveFileName;

        [SerializeField]
        SettingsDto _defaultSettings;

        [SerializeField]
        List<MatchDifficulty> _matchDifficulties;

        private GameManagerFSM _fsm;

        public int SelectedCpuTeamId = 1;
        public int SelectedUserTeamId = 0;
        public int SelectedCpuTeamKitId = 1;
        public int SelectedUserTeamKitId = 0;

        #region MatchDayStuff

        public MatchSettingsDto MatchDayMatchSettings { get; set; }

        // temp variable to store teams that have been chosen for match day
        public TeamDto CpuTeamMatchDayTeam { get; set; }
        public TeamDto UserTeamMatchDayTeam { get; set; }

        #endregion

        public List<TeamDto> TeamsData { get; set; }

        public string TeamsDataSaveFileName { get => _teamsDataSaveFileName; set => _teamsDataSaveFileName = value; }
        public string SettingsSaveFileName { get => _settingsSaveFileName; set => _settingsSaveFileName = value; }

        public SettingsDto DefaultSettings { get => _defaultSettings; set => _defaultSettings = value; }

        public GameManagerFSM Fsm { get => _fsm; set => _fsm = value; }

        public override void Awake()
        {
            base.Awake();

            _fsm = GetComponent<GameManagerFSM>();
        }

        public AttackTactic GetTeamAttackTactic(AttackTypeEnum attackType)
        {
            // get attack tactic
            AttackTactic attackTactic = DataManager.Instance.AttackTactics
                .Where(aT => aT.AttackType == attackType)
                .FirstOrDefault();

            // return result
            return attackTactic;
        }

        public DefendTactic GetTeamDefenceTactic(DefenceTypeEnum defenceType)
        {
            // get attack tactic
            DefendTactic defenceTactic = DataManager.Instance.DefendTactics
                .Where(dT => dT.DefendType == defenceType)
                .FirstOrDefault();

            // return result
            return defenceTactic;
        }

        public MatchDifficultyTeamParam GetMatchDifficultyTeamParam(bool isTeamUserControlled, MatchDifficultyEnum matchDifficultyEnum)
        {
            if(isTeamUserControlled == true)
            {
                return _matchDifficulties.Find(mDTP => mDTP.MatchDifficultyType == matchDifficultyEnum)
                    .MatchDifficultyUserTeamParams;
            }
            else
            {
                return _matchDifficulties.Find(mDTP => mDTP.MatchDifficultyType == matchDifficultyEnum)
                    .MatchDifficultyCPUTeamParams;
            }
        }

        public void InitializeUtilityMenu(bool isBackButtonActive, bool isContinueButtonActive, string heading, UnityAction onClickBackButton = null, UnityAction onClickContinueButton = null)
        {
            GraphicsManager.Instance.UtilityMainMenu.Init(isBackButtonActive,
                isContinueButtonActive,
                heading,
                onClickBackButton,
                onClickContinueButton);
        }

        public void OnBallKicked()
        {
            SoundManager.Instance.PlayAudioClip(4);
        }

        public void OnButtonClicked()
        {
            SoundManager.Instance.PlayAudioClip(5);
        }

        public void OnGoalScored()
        {
            SoundManager.Instance.PlayAudioClip(3);
        }
    }
}
