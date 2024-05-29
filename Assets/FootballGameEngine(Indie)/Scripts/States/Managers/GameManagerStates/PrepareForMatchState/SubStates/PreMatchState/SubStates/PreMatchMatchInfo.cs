using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.MainState;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates.PreMatchState.SubStates
{
    public class PreMatchMatchInfo : BState
    {
        public GameManager Owner { get => ((GameManagerFSM)SuperMachine).Owner; }

        public override void Initialize()
        {
            base.Initialize();

            // init
            Init();
        }

        public override void Enter()
        {
            base.Enter();

            // initialize the utility menu
            InitializeUtilityMenu();

            //init the pre-match menu
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu
                .PreMatchMatchInfoSubMenu
                .Init(Owner.MatchDayMatchSettings.HalfLength.ToString(),
                    Owner.MatchDayMatchSettings.MatchDifficulty.ToString(),
                    Owner.CpuTeamMatchDayTeam,
                    Owner.UserTeamMatchDayTeam);

            // enable the prematch match info menu
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchMatchInfoSubMenu
                .Root
                .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // disable the prematch match info menu
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchMatchInfoSubMenu
                .Root
                .SetActive(false);
        }

        private void Init()
        {
            InitPreMatchBtns();
        }
        private void InitPreMatchBtns()
        {
            // match settings button
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu
                .PreMatchMatchInfoSubMenu
                .BtnMatchSettings
                .onClick.AddListener(() =>
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Machine.ChangeState<PreMatchMatchSettings>();
                });

            // team management button
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu
                .PreMatchMatchInfoSubMenu
                .BtnTeamManagement
                .onClick.AddListener(() =>
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Machine.ChangeState<PreMatchTeamManagement>();
                });
        }

        private void InitializeUtilityMenu()
        {
            Owner.InitializeUtilityMenu(true,
                true,
                "Pre-Match",
                delegate
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Owner.Fsm.GetState<PrepareForMatchMainState>().ChangeState<SelectTeamsKits>(); 
                },
                delegate
                {
                    // play sound
                    Owner.OnButtonClicked();
                    SuperMachine.ChangeState<GameOnMainState>();
                });
        }
    }
}
