using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.Init.SubStates;
using RobustFSM.Base;
using UnityEngine;
namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.Init.MainState
{
    public class InitMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<InitializeSceneWithData>();
            AddState<LoadGameOnScene>();
            AddState<WaitForContinueToMatchOnInstruction>();
            AddState<WaitForSceneToInitialize>();

            // set the initial state
            SetInitialState<LoadGameOnScene>();
        }

        public override void Enter()
        {
            // disable all init menu children
            GraphicsManager.Instance
                .GameOnMainMenu
                .InitMenu
                .DisableChildren();

            // 
            // enable the loading menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .InitMenu
                .Root
                .SetActive(true);

            // disable the background menu
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);

            // run the base
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            // disable the init menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .InitMenu
                .Root
                .SetActive(false);

            // disable the background menu
            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
        }
    }
}
