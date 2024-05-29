using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.ExitState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.SettingsState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.TrophyState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.TrophyPanel.PanelMenu;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState
{
    public class HomeMainState : BState
    {
        public override void Initialize()
        {
            base.Initialize();

            // initialize this instance
            Init();
        }

        public override void Enter()
        {
            base.Enter();
            // Ads here On Main Menu
            // Ads Here
            
            AdsManager.Instance.RequestInterstitial();
            AdsManager.Instance.RequestInterstitialStatic();
         //   AdsManager.Instance.RequestRewardedAd();
            AdsManager.Instance.bannerView.SetPosition(GoogleMobileAds.Api.AdPosition.Top);
          //  AdsManager.Instance.medBannerView.SetPosition(GoogleMobileAds.Api.AdPosition.BottomLeft);

          //  AdsManager.Instance.HideMediumBanner();
            AdsManager.Instance.ShowBanner();
            if (!AdsManager.Instance.isVideoReady)
            {
                AdsManager.Instance.LoadInerstitialAd();
            }
           
            
            
            //
            // play theme sound here
            AudioSource audioSource = SoundManager.Instance.GetAudioSource(1).Item2;
            if (audioSource.isPlaying == false)
                audioSource.Play();

            // enable the home menu
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.HomeMainMenu.ID);
        }

        public override void Exit()
        {
            base.Exit();

            // disable the home menu
            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.HomeMainMenu.ID);
        }

        public GameManager Owner
        {
            get
            {
                return ((GameManagerFSM)SuperMachine).Owner;
            }
        }

        private void Init()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            GraphicsManager.Instance.HomeMainMenu.BtnExit.onClick.AddListener(() =>
            {
                OnClickExitButton();
            });

            GraphicsManager.Instance.HomeMainMenu.BtnKickOff.onClick.AddListener(() =>
            {
                OnClickKickOffButton();
            });

            GraphicsManager.Instance.HomeMainMenu.BtnKickOff2.onClick.AddListener(() =>
            {
                OnClickKickOffButton();
            });

            GraphicsManager.Instance.HomeMainMenu.BtnSettings.onClick.AddListener(() =>
            {
                OnClickSettingsButton();
            });

            GraphicsManager.Instance.HomeMainMenu.BtnTrophy.onClick.AddListener(() =>
            {
                OnClickTrophyButton();
            });
        }

        private void OnClickExitButton()
        {
            Owner.OnButtonClicked();
            SuperMachine.ChangeState<ExitMainState>();
        }

        private void OnClickKickOffButton()
        {
            Owner.OnButtonClicked();
            SuperMachine.ChangeState<PrepareForMatchMainState>();
        }

        private void OnClickSettingsButton()
        {
            Owner.OnButtonClicked();
            SuperMachine.ChangeState<SettingsMainState>();
        }
        private void OnClickTrophyButton()
        {
            Owner.OnButtonClicked();
            SuperMachine.ChangeState<TrophyMainState >();
        }
    }
}
