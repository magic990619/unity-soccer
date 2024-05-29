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

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.TrophyState.MainState
{
    public class TrophyMainState : BState
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
  
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);

            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.TrophyPanel .ID);
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
                .DisableMenu(GraphicsManager.Instance.TrophyPanel.ID);
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
            this.InitCancelTrophy();
        }

        private void InitCancelTrophy()
        {
            GraphicsManager.Instance.TrophyPanel.BtnCancelTrophy.onClick.AddListener(() =>
            {
                Owner.OnButtonClicked();
                SuperMachine.ChangeState<HomeMainState>();
       
            });
        }
    }
}
