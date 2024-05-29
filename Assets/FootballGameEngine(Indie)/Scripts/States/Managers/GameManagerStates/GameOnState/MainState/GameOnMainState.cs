using RobustFSM.Base;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.Init.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchInPlayState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.MainState;
using System.Diagnostics;
using UnityEngine;
using System;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState
{
    public class GameOnMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // add the states
            AddState<HalfTimeState>();
            AddState<InitMainState>();
            AddState<MatchInPlayMainState>();
            AddState<MatchPausedState>();

            // set the initial state
            SetInitialState<InitMainState>();
        }

        public override void Enter()
        {
            // reset the children
            GraphicsManager.Instance.GameOnMainMenu.DisableChildren();

            // run base enter
            base.Enter();

            // stop theme sound here
          
            SoundManager.Instance.StopAudioClip(1);

            // enable the main menu
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.GameOnMainMenu.ID);

        }

        public override void Exit()
        {
            base.Exit();

            // disable menu
            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.GameOnMainMenu.ID);
        }

        public GameManager Owner
        {
            get
            {
                return ((GameManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
