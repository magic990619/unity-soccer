using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchInPlayState.MainState;
using RobustFSM.Base;
using RobustFSM.Interfaces;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.SubStates
{
    public class ShowHalfTimeMenuState : BState
    {
        string _message;

        // match paused menu
        Action OnMessageContinueToSecondHalf;

        public string Message { get => _message; set => _message = value; }

        public override void Initialize()
        {
            base.Initialize();

            // register to half time menu buttons' clicks
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeInfoMenu
                .BtnContinueToSecondHalf
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Invoke_OnMessageContinueToSecondHalf();
                });

            GraphicsManager.Instance.GameOnMainMenu
               .HalfTimeMainMenu
                .HalfTimeInfoMenu
                .BtnRestart
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();

                    // restart match
                    Time.timeScale = 1f;
                    SuperMachine.ChangeState<GameOnMainState>();
                });

            GraphicsManager.Instance.GameOnMainMenu
               .HalfTimeMainMenu
                .HalfTimeInfoMenu
                .BtnTeamManagement
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Machine.ChangeState<HalfTimeTeamManagementState>();

                });
        }

        public override void Enter()
        {
            base.Enter();

            // set the Match Manager to listen to some GameManager events
            OnMessageContinueToSecondHalf += MatchManager.Instance.Instance_OnContinueToSecondHalf;
            MatchManager.Instance.OnExitHalfTimeState += Instance_OnExitHalfTimeState;

            // update the halftime menu
            GraphicsManager.Instance.GameOnMainMenu
               .HalfTimeMainMenu
               .HalfTimeInfoMenu
               .TxtInfo
               .text = _message;

            // enable the half-time menu
            GraphicsManager.Instance.GameOnMainMenu
               .HalfTimeMainMenu
               .HalfTimeInfoMenu
               .Root
               .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // deregister the Match Manager to listen to some GameManager events
            OnMessageContinueToSecondHalf -= MatchManager.Instance.Instance_OnContinueToSecondHalf;
            MatchManager.Instance.OnExitHalfTimeState -= Instance_OnExitHalfTimeState;

            // disable the half-time menu
            GraphicsManager.Instance.GameOnMainMenu
               .HalfTimeMainMenu
               .HalfTimeInfoMenu
               .Root
               .SetActive(false);
        }

        private void Instance_OnExitHalfTimeState()
        {
            Time.timeScale = 1f;
            ((IState)Machine).Machine.ChangeState<MatchInPlayMainState>();
        }

        private void Invoke_OnMessageContinueToSecondHalf()
        {
            ActionUtility.Invoke_Action(OnMessageContinueToSecondHalf);
        }


    }
}
