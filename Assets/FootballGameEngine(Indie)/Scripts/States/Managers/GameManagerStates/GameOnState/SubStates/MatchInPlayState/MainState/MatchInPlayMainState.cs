using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchInPlayState.MainState
{
    public class MatchInPlayMainState : BState
    {
        // normal time is over menu
        Action OnContinueToNormalTimeEvent;

        public override void Initialize()
        {
            base.Initialize();

            // register to match inplay buttons' clicks
            GraphicsManager.Instance.GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchPlayMenu
                .BtnPauseMatch
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Machine.ChangeState<MatchPausedState>();
                    MyGameManager.Instance.MainController.gameObject.SetActive(false);
                    Debug.Log(" -------------------------- Pause Game Called Here");
                    // Ads Here
                    
                    if (AdsManager.Instance.interstitial.IsLoaded())
                    {
                        AdsManager.Instance.ShowInterstitial();
                    }
                    else if (AdsManager.Instance.isVideoReady)
                    {
                        AdsManager.Instance.ShowUnityVideoAd();
                    }
                  //  AdsManager.Instance.ShowMediumBanner();
                    
                });

            // register to match over buttons' clicks
            GraphicsManager.Instance.GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchOverMenu
                .BtnQuit
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Quit();
                });

            GraphicsManager.Instance.GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchOverMenu
                .BtnRestart
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Restart();
                });

            #region RegisterToNormalTimeIsOverMenuButtons

            GraphicsManager.Instance.GameOnMainMenu
                .MatchInPlayMainMenu
                .NormalTimeOverMenu
                .BtnContinueToNormalTime
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Invoke_OnContinueToNormalTime();
                });

            GraphicsManager.Instance.GameOnMainMenu
                .MatchInPlayMainMenu
                .NormalTimeOverMenu
                .BtnRestart
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Restart();
                });

            GraphicsManager.Instance.GameOnMainMenu
                .MatchInPlayMainMenu
                .NormalTimeOverMenu
                .BtnQuit
                .onClick
                .AddListener(() =>
                {
                    // play sound
                    GameManager.Instance.OnButtonClicked();
                    Quit();
                });

            #endregion
        }

        public override void Enter()
        {
            base.Enter();

            // play statdium audio clip
            SoundManager.Instance.PlayAudioClip(2);

            // set the Match Manager to listen to some GameManager events
            OnContinueToNormalTimeEvent += MatchManager.Instance.Invoke_OnContinueToNormalTime;

            // register to ball events
            Ball.Instance.OnBallLaunched += Instance_OnBallLaunched;
            Ball.Instance.OnBallShot += Instance_OnBallShot;

            // register to match manager broadcast message events
            MatchManager.Instance.OnStartBroadcastMessage += Instance_OnStartBroadcastMessage;
            MatchManager.Instance.OnFinishBroadcastMessage += Instance_OnFinishBroadcastMessage;

            // register to on entering and exiting  various match manager states
            MatchManager.Instance.OnEnterExhaustHalfState += Instance_OnEnterExhaustHalfState;
            MatchManager.Instance.OnExitExhaustHalfState += Instance_OnExitExhaustHalfState;

            MatchManager.Instance.OnEnterHalfTimeState += Instance_OnEnterHalfTimeState;

            MatchManager.Instance.OnEnterNormalTimeIsOverState += Instance_OnEnterNormalTimeIsOverState;
            MatchManager.Instance.OnExitNormalTimeIsOverState += Instance_OnExitNormalTimeIsOverState;

            MatchManager.Instance.OnEnterMatchOverState += Instance_OnEnterMatchOverState;
            MatchManager.Instance.OnExitMatchOverState += Instance_OnExitMatchOverState;

            MatchManager.Instance.OnGoalScored += Instance_OnGoalScored;
            MatchManager.Instance.OnTick += Instance_OnTick;

            // disable the match in play menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .DisableChildren();

            // initialize the matchplay menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchPlayMenu
                .Init(GameManager.Instance.MatchDayMatchSettings.IsRadarOn,
                0,
                0,
                MatchManager.Instance.AwayTeamData.ShortName,
                MatchManager.Instance.HomeTeamData.ShortName,
                "00:00");

            // enable the match in play menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .Root
                .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // stop statdium audio clip
            SoundManager.Instance.StopAudioClip(2);

            // deregister to ball events
            Ball.Instance.OnBallLaunched -= Instance_OnBallLaunched;
            Ball.Instance.OnBallShot -= Instance_OnBallShot;

            // deregister the Match Manager to listen to some GameManager events
            OnContinueToNormalTimeEvent -= MatchManager.Instance.Invoke_OnContinueToNormalTime;

            // deregister to broadcast message broadcast message events
            MatchManager.Instance.OnEnterExhaustHalfState -= Instance_OnEnterExhaustHalfState;
            MatchManager.Instance.OnExitExhaustHalfState -= Instance_OnExitExhaustHalfState;

            MatchManager.Instance.OnEnterHalfTimeState -= Instance_OnEnterHalfTimeState;

            MatchManager.Instance.OnEnterNormalTimeIsOverState -= Instance_OnEnterNormalTimeIsOverState;
            MatchManager.Instance.OnExitNormalTimeIsOverState -= Instance_OnExitNormalTimeIsOverState;

            MatchManager.Instance.OnEnterMatchOverState -= Instance_OnEnterMatchOverState;
            MatchManager.Instance.OnExitMatchOverState -= Instance_OnExitMatchOverState;

            MatchManager.Instance.OnGoalScored -= Instance_OnGoalScored;
            MatchManager.Instance.OnTick -= Instance_OnTick;

            // disable the  match in play menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .Root
                .SetActive(false);
        }

        private void HideInfoPanel()
        {
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .InfoMenu
                .Root
                .SetActive(false);
        }
        private void ShowInfoPanel(string message)
        {
            // set the message
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .InfoMenu
                .TxtInfo
                .text = message;

            // enable the menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .InfoMenu
                .Root
                .SetActive(true);
        }

        #region Button Events

        private void Invoke_OnContinueToNormalTime()
        {
            ActionUtility.Invoke_Action(OnContinueToNormalTimeEvent);
        }

        private void Quit()
        {
            Time.timeScale = 1f;
            SuperMachine.ChangeState<HomeMainState>();
        }

        private void Restart()
        {
            Time.timeScale = 1f;
            SuperMachine.ChangeState<GameOnMainState>();
        }

        #endregion

        #region Match Manager Enter & Exit State Events

        private void Instance_OnEnterExhaustHalfState()
        {
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchPlayMenu
                .Root
                .SetActive(true);
        }
        private void Instance_OnExitExhaustHalfState()
        {
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchPlayMenu
                .Root
                .SetActive(false);
        }

        private void Instance_OnEnterHalfTimeState(string message)
        {
            // set the message and switch to half-time state
            Machine.GetState<HalfTimeState>().GetState<ShowHalfTimeMenuState>().Message = message;
            Machine.ChangeState<HalfTimeState>();
        }

        private void Instance_OnEnterMatchOverState(string message)
        {
            // initialize the match over menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchOverMenu
                .TextInfo
                .text = message;

            // enable the match over menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchOverMenu
                .Root
                .SetActive(true);
            Debug.Log(" -------------------------- Time'sUp Game Over Panel Called Here");
            MyGameManager.Instance.MainController.gameObject.SetActive(false);
            UIHandler.Instance.ShowloadingAd();

        }

        private void Instance_OnExitMatchOverState()
        {
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchOverMenu
                .Root
                .SetActive(false);
              // Ads Hiding Here
             //  AdsManager.Instance.RequestInterstitial();
        }

        private void Instance_OnEnterNormalTimeIsOverState(string message)
        {

            // update the normal-time is over menu
            GraphicsManager.Instance
               .GameOnMainMenu
               .MatchInPlayMainMenu
               .NormalTimeOverMenu
               .TxtInfo
               .text = message;

            // enable the normal-time is over menu
            GraphicsManager.Instance
               .GameOnMainMenu
               .MatchInPlayMainMenu
               .NormalTimeOverMenu
               .Root
               .SetActive(true);
        }
        private void Instance_OnExitNormalTimeIsOverState()
        {
            // deactivate the normal time is over menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .NormalTimeOverMenu
                .Root
                .SetActive(false);
        }

        #endregion

        #region Other Match Manager Events

        private void Instance_OnBallShot(Shot shot)
        {
            // play sound
            GameManager.Instance.OnBallKicked();
        }

        private void Instance_OnBallLaunched(float flightTime, float velocity, Vector3 initial, Vector3 target)
        {
            // play sound
            GameManager.Instance.OnBallKicked();
        }

        private void Instance_OnGoalScored(string message)
        {
            // update the ui
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchPlayMenu
                .TxtScores
                .text = message;

            //message the sound manager
            GameManager.Instance.OnGoalScored();
        }

        private void Instance_OnStartBroadcastMessage(string message)
        {
            ShowInfoPanel(message);
        }
        private void Instance_OnFinishBroadcastMessage()
        {
            HideInfoPanel();
        }

        private void Instance_OnTick(int half, int minutes, int seconds)
        {
            //declare the string
            string timeInfo = string.Empty;

            //prepare the message
            string infoHalf = half == 1 ? "1st" : "2nd";

            timeInfo = string.Format("{0} {1}:{2}",
                infoHalf,
                minutes.ToString("00"),
                seconds.ToString("00"));

            //set the ui
            GraphicsManager.Instance
                .GameOnMainMenu
                .MatchInPlayMainMenu
                .MatchPlayMenu
                .TxtTime
                .text = timeInfo;
        }

        #endregion
    }
}
