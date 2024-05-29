using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Settings;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.MainState;
using RobustFSM.Base;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.Init.SubStates
{
    public class LoadDataFromSource : BState
    {
        public override void Enter()
        {
            base.Enter();

            // enable the initmainmenu loading submenu
            GraphicsManager.Instance.InitMainMenu.EnableLoadSubMenu();

            // load the data
            RetrieveData();

            // go to initialize data dependents
            Machine.ChangeState<InitializeDataDependentEntities>();
        }

        void RetrieveData()
        {
            RetrievePlayerData();
            RetrieveSettingsData();
        }

        void RetrievePlayerData()
        {
            // get the team data
            //SuperMachine.GetState<InitMainState>().TeamsData = DataManager
            //    .Instance
            //    .GetData<List<TeamDto>>(GameManager.Instance.TeamsDataSaveFileName);

            SuperMachine.GetState<InitMainState>().TeamsData = DataManager.Instance.Teams;
        }

        void RetrieveSettingsData()
        {
            // get the settings data
            SettingsDto settings = DataManager
                .Instance
                .GetData<SettingsDto>(GameManager.Instance.SettingsSaveFileName);

            // get the settings flag
            bool isSettingsAvailable = settings != null;
            
            // set the settings depending on the flag
            SuperMachine.GetState<InitMainState>().Settings = isSettingsAvailable ? settings : GameManager.Instance.DefaultSettings;
        }
    }
}
