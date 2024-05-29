using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates.PreMatchState.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates
{
    public class PreMatch : BHState
    {
        public GameManager Owner { get => ((GameManagerFSM)SuperMachine).Owner; }

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<PreMatchMatchInfo>();
            AddState<PreMatchMatchSettings>();
            AddState<PreMatchTeamManagement>();

            // set initial state
            SetInitialState<PreMatchMatchInfo>();
        }

        public override void Initialize()
        {
            // call child states now
            base.Initialize();

            // disable all submenus
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.DisableAllSubMenus();
        }

        public override void Enter()
        {
            // clone the match settings
            Owner.MatchDayMatchSettings = new MatchSettingsDto(Owner.DefaultSettings.MatchSettings);

            // get the selected teams and set them
            Owner.CpuTeamMatchDayTeam = this.GetTeam(this.Owner.SelectedCpuTeamId);
            Owner.UserTeamMatchDayTeam = this.GetTeam(this.Owner.SelectedUserTeamId);

            // call substates
            base.Enter();

            // enable the prematch menu
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.Root.SetActive(true);
            // Me Added
            UIHandler.Instance.CharactersMenu.gameObject.SetActive(false);
            UIHandler.Instance.ActivePlayImage();
        }

        public override void Exit()
        {
            base.Exit();

            // disable the prematch menu
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.Root.SetActive(false);
        }

        private TeamDto GetTeam(int id)
        {
            return Owner.TeamsData[id];
        }
    }
}
