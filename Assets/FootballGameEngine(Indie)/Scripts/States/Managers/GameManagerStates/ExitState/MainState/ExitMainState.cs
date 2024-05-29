using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState;
using RobustFSM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.ExitState.MainState
{
    public class ExitMainState : BState
    {
        public override void Initialize()
        {
            base.Initialize();

            // init this instance
            this.Init();
        }

        public override void Enter()
        {
            base.Enter();
            //Ads
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
            
                
            // enable exit menu
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.ExitMainMenu.ID);
        }

        public override void Exit()
        {
            base.Exit();

            // disable exit menu

            GraphicsManager.Instance
               .MenuManager
               .DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.ExitMainMenu.ID);
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
            this.InitButtons();
        }

        private void InitButtons()
        {
            this.InitCancelExitButton();
            this.InitConfirmExitButton();
        }

        private void InitCancelExitButton()
        {
            GraphicsManager.Instance.ExitMainMenu.BtnCancelQuit.onClick.AddListener(() =>
            {
                Owner.OnButtonClicked();
                SuperMachine.ChangeState<HomeMainState>();

                // Ads  Here
                
                AdsManager.Instance.RequestInterstitial();
             //   AdsManager.Instance.HideMediumBanner();
                

            });
        }

        private void InitConfirmExitButton()
        {
            GraphicsManager.Instance.ExitMainMenu.BtnConfirmQuit.onClick.AddListener(() =>
            {
                // play sound
                Owner.OnButtonClicked();

                // get the various settings
                Owner.DefaultSettings.SoundSettings = new SoundSettingsDto(SoundManager.Instance.IsSoundEnabled, 
                    SoundManager.Instance.MusicVolume, 
                    SoundManager.Instance.SoundVolume);

                // write to file
                DataManager.Instance.SaveData(Owner.DefaultSettings, 
                    GameManager.Instance.SettingsSaveFileName);

                // quit application
                Application.Quit();
            });
        }
    }
}
