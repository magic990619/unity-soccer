using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchInPlayState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState;
using RobustFSM.Base;
using RobustFSM.Interfaces;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchPaused.SubStates
{
    public class MatchPausedInfoState : BState
    {
        string _message;

        Action OnResumeMatch;

        public string Message { get => _message; set => _message = value; }

        public override void Initialize()
        {
            base.Initialize();

            // register to match paused buttons' clicks
            GraphicsManager.Instance.GameOnMainMenu
                .MatchPausedMenu
                .MatchPausedInfoMenu
                .BtnQuit
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();

                    // go to home
                    Time.timeScale = 1f;
                    SuperMachine.ChangeState<HomeMainState>();
                });

            GraphicsManager.Instance.GameOnMainMenu
                .MatchPausedMenu
                .MatchPausedInfoMenu
                .BtnRestart
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();

                    // restart match
                    Time.timeScale = 1f;
                    // Ads Hiding Here
                 //   AdsManager.Instance.HideMediumBanner();
                    SuperMachine.ChangeState<GameOnMainState>();
                });

            GraphicsManager.Instance.GameOnMainMenu
                .MatchPausedMenu
                .MatchPausedInfoMenu
                .BtnResume
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();

                    // resume match
                    ActionUtility.Invoke_Action(OnResumeMatch);
                    // My Manager
                    MyGameManager.Instance.MainController.gameObject.SetActive(true);
                    Debug.Log(" -------------------------- Resume Game Called Here");
                    // Ads Here
                    AdsManager.Instance.RequestInterstitial();
                //  AdsManager.Instance.HideMediumBanner();
                    GraphicsManager.Instance
             .GameOnMainMenu
             .MatchInPlayMainMenu
             .MatchPlayMenu
             .TxtScores
             .text = _message;
                });

            GraphicsManager.Instance.GameOnMainMenu
               .MatchPausedMenu
               .MatchPausedInfoMenu
               .BtnTeamManagement
               .onClick
               .AddListener(() =>
               {
                   // play sound
                   GameManager.Instance.OnButtonClicked();
                   Machine.ChangeState<MatchTeamManagement>();
               });
        }

        public override void Enter()
        {
            base.Enter();

            // set the Match Manager to listen to some GameManager events
            OnResumeMatch += MatchManager.Instance.Instance_ExitMatchPaused;
            MatchManager.Instance.OnExitMatchPaused += Instance_OnExitMatchPausedState;

            // initialize the match over menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchPausedMenu
                .MatchPausedInfoMenu
                .TxtInfo
                .text = _message;

            // enable the match over menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchPausedMenu
                .MatchPausedInfoMenu
                .Root
                .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // deregister the Match Manager to listen to some GameManager events
            OnResumeMatch -= MatchManager.Instance.Instance_ExitMatchPaused;
            MatchManager.Instance.OnExitMatchPausedState -= Instance_OnExitMatchPausedState;

            // deactivate menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchPausedMenu
                .MatchPausedInfoMenu
                .Root
                .SetActive(false);
        }

        private void Instance_OnExitMatchPausedState()
        {
            ((IState)Machine).Machine.ChangeState<MatchInPlayMainState>();
        }
    }
}
