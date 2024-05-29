using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates;
using RobustFSM.Base;
using System.Diagnostics;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.MainState
{
    public class PrepareForMatchMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<PreMatch>();
            AddState<SelectTeams>();
            AddState<SelectTeamsKits>();

            // set the initial state
            SetInitialState<SelectTeams>();
        }

        public override void Enter()
        {
            // reset the menus
            GraphicsManager.Instance.PrepareForMatchMainMenu.DisableAllSubMenus();
            // Ads Here
           // UIHandler.Instance.ShowloadingAdStatic();
            // set the menus
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
            GraphicsManager.Instance
              .MenuManager
              .EnableMenu(GraphicsManager.Instance.PrepareForMatchMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.UtilityMainMenu.ID);

            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            // set the menus
            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
            GraphicsManager.Instance
               .MenuManager
               .DisableMenu(GraphicsManager.Instance.PrepareForMatchMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.UtilityMainMenu.ID);


        }
    }
}
