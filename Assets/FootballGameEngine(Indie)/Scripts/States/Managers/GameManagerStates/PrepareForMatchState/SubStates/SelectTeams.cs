using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.HomeState.MainState;
using RobustFSM.Base;
using static Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.SelectTeamsSubMenu;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates
{
    public class SelectTeams : BState
    {
        public override void Initialize()
        {
            base.Initialize();

            // initialize stuff
            Init();
        }

        public override void Enter()
        {
            base.Enter();

            InitializeUtilityMenu();

            // get the selected items
            OnSelectTeam(ref Owner.SelectedCpuTeamId, 0, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.CpuControlledTeamInfo);
            OnSelectTeam(ref Owner.SelectedUserTeamId, 0, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.UserControlledTeamInfo);
            
            // enable the select teams submenu
            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.Root.SetActive(true);
            // Me Added
            UIHandler.Instance.CharactersMenu.gameObject.SetActive(true);
            UIHandler.Instance.ActiveNextImage();
            // My Added
            ShowplayerKitsMenu();
            ShowAIKitsMenu();

        }

        public override void Exit()
        {
            base.Exit();

            // disable the select teams submenu
            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.Root.SetActive(false);
    
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
            InitCpuTeamInfoUI();
            InitUserTeamInfoUI();
        }

        private void InitCpuTeamInfoUI()
        {
            GraphicsManager.Instance
                .PrepareForMatchMainMenu
                .SelectTeamSubMenu
                .CpuControlledTeamInfo
                .BtnSelectNextTeam
                .onClick
                .AddListener(delegate ()
            {
                OnSelectNextTeam(ref Owner.SelectedCpuTeamId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.CpuControlledTeamInfo);
                // My Added
                ShowplayerKitsMenu();
            });

            GraphicsManager.Instance
                .PrepareForMatchMainMenu
                .SelectTeamSubMenu
                .CpuControlledTeamInfo
                .BtnSelectPrevTeam
                .onClick
                .AddListener(delegate ()
            {
                OnSelectPrevTeam(ref Owner.SelectedCpuTeamId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.CpuControlledTeamInfo);
                // My Added
                ShowplayerKitsMenu();
            });
        }

        private void InitUserTeamInfoUI()
        {
            GraphicsManager.Instance
                .PrepareForMatchMainMenu
                .SelectTeamSubMenu
                .UserControlledTeamInfo
                .BtnSelectNextTeam
                .onClick
                .AddListener(delegate ()
            {
                OnSelectNextTeam(ref Owner.SelectedUserTeamId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.UserControlledTeamInfo);
                // My Added
                ShowAIKitsMenu();
            });

            GraphicsManager.Instance
                .PrepareForMatchMainMenu
                .SelectTeamSubMenu
                .UserControlledTeamInfo
                .BtnSelectPrevTeam
                .onClick
                .AddListener(delegate ()
            {
                OnSelectPrevTeam(ref Owner.SelectedUserTeamId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectTeamSubMenu.UserControlledTeamInfo);
                // My Added
                ShowAIKitsMenu();
            });
        }

        private void InitializeUtilityMenu()
        {
            Owner.InitializeUtilityMenu(true, true, "Select Teams", 
            delegate
            {
                // play sound
                Owner.OnButtonClicked();
                SuperMachine.ChangeState<HomeMainState>();
            }, 
            delegate
            {
                // play sound
                Owner.OnButtonClicked();
                Machine.ChangeState<SelectTeamsKits>();
            });
        }

        private void OnSelectNextTeam(ref int teamIndex, TeamInfo teamInfo)
        {
            // play sound
            Owner.OnButtonClicked();
            OnSelectTeam(ref teamIndex, 1, teamInfo);
        }

        private void OnSelectPrevTeam(ref int teamIndex, TeamInfo teamInfo)
        {
            // play sound
            Owner.OnButtonClicked();
            OnSelectTeam(ref teamIndex, -1, teamInfo);
        }

        private void OnSelectTeam(ref int teamIndex, int step, TeamInfo teamInfo)
        {
            // update the team index by step
            teamIndex += step;

            // check if team index is now below 0
            bool isIndexBelowRange = teamIndex < 0;

            // wrap team index if below zero
            if (isIndexBelowRange)
                teamIndex = Owner.TeamsData.Count - 1;
            
            // check if index has overshoot range
            bool isIndexAboveRange = teamIndex > Owner.TeamsData.Count - 1;

            // wrap index if it has overshoot range
            if (isIndexAboveRange)
                teamIndex = 0;
            
            // get the team dto
            TeamDto selectedTeam = Owner.TeamsData[teamIndex];

            // update the ui
            teamInfo.ImgTeamIcon.sprite = selectedTeam.Icon;
            teamInfo.TxtTeamName.text = selectedTeam.Name;

            teamInfo.TxtTeamFormation.text = FormationManager.Instance.GetFormation(selectedTeam.FormationType).FormationName;

            teamInfo.TxtTeamAttack.text = Owner.GetTeamAttackTactic(selectedTeam.AttackType).Name;
            teamInfo.TxtTeamDefence.text = Owner.GetTeamDefenceTactic(selectedTeam.DefenceType).Name;
        }

        private void ShowplayerKitsMenu()
        {
            switch (Owner.SelectedCpuTeamId)
            {
                case 0:
                    MyCharacterSystem.Instance.BrazilPlayerActive();
                    break;
                case 1:
                    MyCharacterSystem.Instance.FrancePlayerActive();
                    break;
                case 2:
                    MyCharacterSystem.Instance.ArgentinaPlayerActive();
                    break;
                case 3:
                    MyCharacterSystem.Instance.EnglandPlayerActive();
                    break;
                case 4:
                    MyCharacterSystem.Instance.SpainPlayerActive();
                    break;
                case 5:
                    MyCharacterSystem.Instance.GermanyPlayerActive();
                    break;
                case 6:
                    MyCharacterSystem.Instance.NetherlandsPlayerActive();
                    break;
                case 7:
                    MyCharacterSystem.Instance.SenegalPlayerActive();
                    break;
                case 8:
                    MyCharacterSystem.Instance.PortugalPlayerActive();
                    break;
                case 9:
                    MyCharacterSystem.Instance.USAPlayerActive();
                    break;
                case 10:
                    MyCharacterSystem.Instance.UruguayPlayerActive();
                    break;
                case 11:
                    MyCharacterSystem.Instance.AustraliaPlayerActive();
                    break;
                case 12:
                    MyCharacterSystem.Instance.PolandPlayerActive();
                    break;
                case 13:
                    MyCharacterSystem.Instance.MoroccoPlayerActive();
                    break;
                case 14:
                    MyCharacterSystem.Instance.CroatiaPlayerActive();
                    break;
                case 15:
                    MyCharacterSystem.Instance.JapanPlayerActive();
                    break;
                case 16:
                    MyCharacterSystem.Instance.SwitzerlandPlayerActive();
                    break;
                case 17:
                    MyCharacterSystem.Instance.GhanaPlayerActive();
                    break;
                case 18:
                    MyCharacterSystem.Instance.QatarPlayerActive();
                    break;
                case 19:
                    MyCharacterSystem.Instance.SaudiPlayerActive();
                    break;
                case 20:
                    MyCharacterSystem.Instance.ItalyPlayerActive();
                    break;
                case 21:
                    MyCharacterSystem.Instance.PakistanPlayerActive();
                    break;
                case 22:
                    MyCharacterSystem.Instance.IndiaPlayerActive();
                    break;
            }
        }
        private void ShowAIKitsMenu()
        {
            switch (Owner.SelectedUserTeamId)
            {
                case 0:
                    MyCharacterSystem.Instance.BrazilAIActive();
                    break;
                case 1:
                    MyCharacterSystem.Instance.FranceAIActive();
                    break;
                case 2:
                    MyCharacterSystem.Instance.ArgentinaAIActive();
                    break;
                case 3:
                    MyCharacterSystem.Instance.EnglandAIActive();
                    break;
                case 4:
                    MyCharacterSystem.Instance.SpainAIActive();
                    break;
                case 5:
                    MyCharacterSystem.Instance.GermanyAIActive();
                    break;
                case 6:
                    MyCharacterSystem.Instance.NetherlandsAIActive();
                    break;
                case 7:
                    MyCharacterSystem.Instance.SenegalAIActive();
                    break;
                case 8:
                    MyCharacterSystem.Instance.PortugalAIActive();
                    break;
                case 9:
                    MyCharacterSystem.Instance.USAAIActive();
                    break;
                case 10:
                    MyCharacterSystem.Instance.UruguayAIActive();
                    break;
                case 11:
                    MyCharacterSystem.Instance.AustraliaAIActive();
                    break;
                case 12:
                    MyCharacterSystem.Instance.PolandAIActive();
                    break;
                case 13:
                    MyCharacterSystem.Instance.MoroccoAIActive();
                    break;
                case 14:
                    MyCharacterSystem.Instance.CroatiaAIActive();
                    break;
                case 15:
                    MyCharacterSystem.Instance.JapanAIActive();
                    break;
                case 16:
                    MyCharacterSystem.Instance.SwitzerlandAIActive();
                    break;
                case 17:
                    MyCharacterSystem.Instance.GhanaAIActive();
                    break;
                case 18:
                    MyCharacterSystem.Instance.QatarAIActive();
                    break;
                case 19:
                    MyCharacterSystem.Instance.SaudiAIActive();
                    break;
                case 20:
                    MyCharacterSystem.Instance.ItalyAIActive();
                    break;
                case 21:
                    MyCharacterSystem.Instance.PakistanAIActive();
                    break;
                case 22:
                    MyCharacterSystem.Instance.IndiaAIActive();
                    break;
            }
        }
    }
}
