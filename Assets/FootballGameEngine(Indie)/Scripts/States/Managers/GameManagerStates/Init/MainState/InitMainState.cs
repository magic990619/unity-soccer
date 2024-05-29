using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.SubStates;
using RobustFSM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.MainState
{
    public class InitMainState : BHState
    {
        SettingsDto _settings;
        List<TeamDto> _teamsData;

        public SettingsDto Settings { get => _settings; set => _settings = value; }
        public List<TeamDto> TeamsData { get => _teamsData; set => _teamsData = value; }

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<InitializeDataDependentEntities>();
            AddState<LoadDataFromSource>();
            AddState<WaitForContinueInstruction>();

            // set initial state
            SetInitialState<LoadDataFromSource>();
        }

        public override void Enter()
        {
            // disable all the menus
            GraphicsManager.Instance.MenuManager.DisableAllMenus();

            // disable all submenues
            GraphicsManager.Instance.InitMainMenu.DisableAll();

            //enable the init main menu and background menu
            GraphicsManager.Instance.MenuManager.EnableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
            GraphicsManager.Instance.MenuManager.EnableMenu(GraphicsManager.Instance.InitMainMenu.ID);

            // run the base class
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            // disable the init main menu
            GraphicsManager.Instance.MenuManager.DisableMenu(GraphicsManager.Instance.BackgroundMainMenu.ID);
            GraphicsManager.Instance.MenuManager.DisableMenu(GraphicsManager.Instance.InitMainMenu.ID);
        }
    }
}
