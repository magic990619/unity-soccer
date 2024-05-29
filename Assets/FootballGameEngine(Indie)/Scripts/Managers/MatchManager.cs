using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Storage.MatchDifficulties;
using Assets.FootballGameEngine_Indie_.Scripts.Triggers;
using Patterns.Singleton;
using RobustFSM.Interfaces;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Managers
{
    [RequireComponent(typeof(MatchManagerFSM))]
    public class MatchManager : Singleton<MatchManager>
    {
        [SerializeField]
        MatchDifficultyTeamParam _cpuTeamParams;
        [SerializeField]
        MatchDifficultyTeamParam _userTeamParams;

        [Header("Entities")]

        [SerializeField]
        ThrowInTrigger _leftThrowInTrigger;

        [SerializeField]
        ThrowInTrigger _rightThrowInTrigger;

        [SerializeField]
        Team _teamAway;

        [SerializeField]
        Team _teamHome;

        [SerializeField]
        Transform _rootTeam;

        [SerializeField]
        Transform _transformCentreSpot;

        [Header("Teams Data")]

        [SerializeField]
        InGameTeamDto _awayTeamData;

        [SerializeField]
        InGameTeamDto _homeTeamData;

        private GameTimeEnum _currGameTime;
        
        public GameTimeEnum CurrGameTime { get => _currGameTime; set => _currGameTime = value; }

        /// <summary>
        /// A reference to how long each half length is in actual time(m)
        /// </summary>
        public float ActualHalfLength { get; set; } = 1f;

        /// <summary>
        /// A reference to the normal half length
        /// </summary>
        public float NormalTimeHalfLength { get; set; } = 45;

        /// <summary>
        /// A reference to the next time that we have to stop the game
        /// </summary>
        public float NextStopTime { get; set; }

        /// <summary>
        /// A reference to the extra-time half length
        /// </summary>
        public float ExtraTimeHalfLength { get; set; } = 15;

        /// <summary>
        /// A reference to the current game half in play
        /// </summary>
        public int CurrentHalf { get; set; }

        /// <summary>
        /// Property to get or set this instance's fsm
        /// </summary>
        public IFSM FSM { get; set; }

        /// <summary>
        /// A reference to the match status of this instance
        /// </summary>
        public MatchStatuses MatchStatus { get; set; }

        public Vector3 CachedBallPosition { get; set; }


        public Action OnBroadcastTakeCornerKick;
        public Action OnBroadcastTakeKickOff;
        public Action OnBroadcastTakeGoalKick;
        public Action OnContinueToNormalTime;
        public Action OnContinueToSecondHalf;

        public Action OnEnterExhaustHalfState;
        public Action OnEnterWaitForKickToComplete;
        public Action OnEnterWaitForMatchOnInstruction;

        public Action OnExitHalfTimeState;
        public Action OnExitMatchOverState;
        public Action OnExitMatchPausedState;
        public Action OnExitWaitForKickToComplete;
        public Action OnExitWaitForMatchOnInstruction;
        public Action OnExitExhaustHalfState;
        public Action OnExitNormalTimeIsOverState;
        public Action OnExitMatchPaused;

        public Action OnFinishBroadcastMessage;
        public Action OnInitialize;
        public Action OnMatchPaused;
        public Action OnMesssagedToSwitchToMatchOn;
        public Action OnStopMatch;

        public Action<string> OnEnterHalfTimeState;
        public Action<string> OnEnterMatchOverState;
        public Action<string> OnEnterMatchPausedState;
        public Action<string> OnGoalScored;
        public Action<string> OnStartBroadcastMessage;
        public Action<string> OnEnterNormalTimeIsOverState;

        public Action<int, int, int> OnTick;
        public Action<Vector3> OnBroadcastTakeThrowIn;

        public override void Awake()
        {
            base.Awake();

            FSM = GetComponent<MatchManagerFSM>();
        }

        public void Init(MatchDifficultyTeamParam cpuMatchDifficultyParams, MatchDifficultyTeamParam userMatchDifficultyParams, MatchSettingsDto matchSettings, InGameTeamDto awayTeamData, InGameTeamDto homeTeamData)
        {
            _cpuTeamParams = cpuMatchDifficultyParams;
            _userTeamParams = userMatchDifficultyParams;

            _awayTeamData = awayTeamData;
            _homeTeamData = homeTeamData;

            _currGameTime = GameTimeEnum.NormalTime;

            ActualHalfLength = matchSettings.HalfLength;
        }

        public void Invoke_OnContinueToNormalTime()
        {
            ActionUtility.Invoke_Action(OnContinueToNormalTime);
        }

        public void Instance_OnContinueToSecondHalf()
        {
            ActionUtility.Invoke_Action(OnContinueToSecondHalf);
        }

        public void Instance_ExitMatchPaused()
        {
            ActionUtility.Invoke_Action(OnExitMatchPaused);
        }

        public void Instance_OnMatchPlayPaused()
        {
            ActionUtility.Invoke_Action(OnMatchPaused);
        }

        /// <summary>
        /// Raises the event that this instance has been messaged to switch to match on
        /// </summary>
        public void Instance_OnMessagedSwitchToMatchOn()
        {
            ActionUtility.Invoke_Action(OnMesssagedToSwitchToMatchOn);
        }

        public void Invoke_OnInitialize()
        {
            ActionUtility.Invoke_Action(OnInitialize);
        }

        public Team TeamAway { get => _teamAway; }
        public Team TeamHome { get => _teamHome; }

        /// <summary>
        /// Property to access the team root transform
        /// </summary>
        public Transform RootTeam { get => _rootTeam; }
        public Transform TransformCentreSpot { get => _transformCentreSpot; set => _transformCentreSpot = value; }
        public InGameTeamDto AwayTeamData { get => _awayTeamData; set => _awayTeamData = value; }
        public InGameTeamDto HomeTeamData { get => _homeTeamData; set => _homeTeamData = value; }
        public ThrowInTrigger LeftThrowInTrigger { get => _leftThrowInTrigger; set => _leftThrowInTrigger = value; }
        public ThrowInTrigger RightThrowInTrigger { get => _rightThrowInTrigger; set => _rightThrowInTrigger = value; }
        public MatchDifficultyTeamParam CpuTeamParams { get => _cpuTeamParams; set => _cpuTeamParams = value; }
        public MatchDifficultyTeamParam UserTeamParams { get => _userTeamParams; set => _userTeamParams = value; }
    }
}
