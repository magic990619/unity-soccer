using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.MainState
{
    public class HalfTimeState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            // addt states
            AddState<HalfTimeTeamManagementState>();
            AddState<ShowHalfTimeMenuState>();

            // set initial state
            SetInitialState<ShowHalfTimeMenuState>();
        }

        public override void Enter()
        {
            // disanle all children
            GraphicsManager.Instance.GameOnMainMenu
               .HalfTimeMainMenu
               .DisableChildren();

            // enable the half-timemenu
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .Root
                .SetActive(true);
            Debug.Log(" -------------------------- Half Time Panel Show Called Here");
            UIHandler.Instance.ShowloadingAd();
            MyGameManager.Instance.MainController.gameObject.SetActive(false);
            // run the enter
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            // disable the half-timemenu
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .Root
                .SetActive(false);
            Debug.Log(" -------------------------- Half Time Panel Hide Called Here");
            // Ads Hiding Here
            AdsManager.Instance.RequestInterstitial();
            MyGameManager.Instance.MainController.gameObject.SetActive(true);
        }
    }
}
