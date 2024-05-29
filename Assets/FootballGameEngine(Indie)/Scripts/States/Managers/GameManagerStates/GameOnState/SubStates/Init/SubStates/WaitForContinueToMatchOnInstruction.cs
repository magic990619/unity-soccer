using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.MatchInPlayState.MainState;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.Init.SubStates
{
    public class WaitForContinueToMatchOnInstruction : BState
    {
        Action _onMessageSwitchToMatchOn;

        public override void Enter()
        {
            base.Enter();

            // register the match manager to listen to some events
            _onMessageSwitchToMatchOn += MatchManager.Instance.OnMesssagedToSwitchToMatchOn;

            // listen to match manager events
            MatchManager.Instance.OnExitWaitForMatchOnInstruction += Instance_OnMatchManagerExitWaitForMatchOnInstruction;

            // enable the loading menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .InitMenu
                .WaitForContinueInstructionMenu
                .Root.
                gameObject
                .SetActive(true);
        }

        public override void Execute()
        {
            base.Execute();

            // message the match manager to start the match
#if UNITY_EDITOR || UNITY_STANDALONE  || UNITY_WEBGL
            if (Input.GetKeyDown("space"))
            {
                ActionUtility.Invoke_Action(_onMessageSwitchToMatchOn);
                // Ads Here
                
                AdsManager.Instance.RequestInterstitial();
                AdsManager.Instance.RequestInterstitialStatic();
                AdsManager.Instance.HideBanner();
                if (!AdsManager.Instance.isVideoReady)
                {
                    AdsManager.Instance.LoadInerstitialAd();
                }
                MyGameManager.Instance.StratCutscenefrom1();
                Debug.Log(" -------------------------- Press Any Key To Start Game Called Here");
                // Players
                PlayerPrefs.SetInt("myplayerindex", Owner.SelectedCpuTeamId);
                PlayerPrefs.SetInt("myAiIndex", Owner.SelectedUserTeamId);
                // Kits
                PlayerPrefs.SetInt("myplayerkits", Owner.SelectedCpuTeamKitId);
                PlayerPrefs.SetInt("myAiKits", Owner.SelectedUserTeamKitId);
                // Check Player
                MyGameManager.Instance.ShowkitMenuPlayer();
                MyGameManager.Instance.ShowkitMenuAI ();
              


            }

#else
            if(Input.touchCount > 0)
            {
                ActionUtility.Invoke_Action(_onMessageSwitchToMatchOn);
               // Ads Here
               
               AdsManager.Instance.RequestInterstitial();
               AdsManager.Instance.RequestInterstitialStatic();
                AdsManager.Instance.HideBanner();
                 if (!AdsManager.Instance.isVideoReady)
            {
                AdsManager.Instance.LoadInerstitialAd();
            }
                MyGameManager.Instance.StratCutscenefrom1();
                Debug.Log(" -------------------------- Press Any Key To Start Game Called Here");
                     // Players
                PlayerPrefs.SetInt("myplayerindex", Owner.SelectedCpuTeamId);
                PlayerPrefs.SetInt("myAiIndex", Owner.SelectedUserTeamId);
                // Kits
                PlayerPrefs.SetInt("myplayerkits", Owner.SelectedCpuTeamKitId);
                PlayerPrefs.SetInt("myAiKits", Owner.SelectedUserTeamKitId);
                // Check Player
                MyGameManager.Instance.ShowkitMenuPlayer();
                MyGameManager.Instance.ShowkitMenuAI ();
            }
#endif
        }

        public override void Exit()
        {
            base.Exit();

            // deregister the match manager to listen to some events
            _onMessageSwitchToMatchOn -= MatchManager.Instance.OnMesssagedToSwitchToMatchOn;

            // deregister from listening to some of the match manager events
            MatchManager.Instance.OnExitWaitForMatchOnInstruction -= Instance_OnMatchManagerExitWaitForMatchOnInstruction;

            // disable the loading menu
            GraphicsManager.Instance
                .GameOnMainMenu
                .InitMenu
                .WaitForContinueInstructionMenu
                .Root.
                gameObject
                .SetActive(false);

            // disable the background
            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
        }

        public GameManager Owner
        {
            get
            {
                return ((GameManagerFSM)SuperMachine).Owner;
            }
        }

        void Instance_OnMatchManagerExitWaitForMatchOnInstruction()
        {
            // play sound
            Owner.OnButtonClicked();

            // go to match in play main state
            SuperMachine.GetState<GameOnMainState>()
                .ChangeState<MatchInPlayMainState>();
        }
    }
}
