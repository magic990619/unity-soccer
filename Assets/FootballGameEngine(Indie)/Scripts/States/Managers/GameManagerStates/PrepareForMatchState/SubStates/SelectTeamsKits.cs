using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates
{
    public class SelectTeamsKits : BState
    {
        public override void Initialize()
        {
            base.Initialize();

            // init 
            Init();
        }

        public override void Enter()
        {
            base.Enter();

            InitializeUtilityMenu();

            OnSelectTeamKit(Owner.SelectedCpuTeamId, ref Owner.SelectedCpuTeamKitId, 0, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.CpuControlledTeamInfo);
            OnSelectTeamKit(Owner.SelectedUserTeamId, ref Owner.SelectedUserTeamKitId, 0, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.UserControlledTeamInfo);

            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.Root.SetActive(true);
            // Me Added
            // Ads Hiding Here
          //  AdsManager.Instance.RequestInterstitialStatic();
            UIHandler.Instance.CharactersMenu.gameObject.SetActive(true);
            UIHandler.Instance.ActiveNextImage();
        }

        public override void Exit()
        {
            base.Exit();

            // disable the select kits root
            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.Root.SetActive(false);
        
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
            InitCpuTeamKitUI();
            InitUserTeamKitUI();
        }

        private void InitCpuTeamKitUI()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.CpuControlledTeamInfo.BtnSelectNextTeam.onClick.AddListener(delegate ()
            {
                OnSelectNextTeamKit(Owner.SelectedCpuTeamId, ref Owner.SelectedCpuTeamKitId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.CpuControlledTeamInfo);
                // My Added
                ShowkitMenuPlayer();
            });

            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.CpuControlledTeamInfo.BtnSelectPrevTeam.onClick.AddListener(delegate ()
            {
                OnSelectPrevTeamKit(Owner.SelectedCpuTeamId, ref Owner.SelectedCpuTeamKitId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.CpuControlledTeamInfo);
                // My Added
                ShowkitMenuPlayer();
            });
        }

        private void InitUserTeamKitUI()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.UserControlledTeamInfo.BtnSelectNextTeam.onClick.AddListener(delegate ()
            {
                OnSelectNextTeamKit(Owner.SelectedUserTeamId, ref Owner.SelectedUserTeamKitId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.UserControlledTeamInfo);
                // My Added
                ShowkitMenuAI();
            });

            GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.UserControlledTeamInfo.BtnSelectPrevTeam.onClick.AddListener(delegate ()
            {
                OnSelectPrevTeamKit(Owner.SelectedCpuTeamId, ref Owner.SelectedCpuTeamKitId, GraphicsManager.Instance.PrepareForMatchMainMenu.SelectKitsSubMenu.UserControlledTeamInfo);
                // My Added
                ShowkitMenuAI();
            });
        }

        private void OnSelectNextTeamKit(int teamIndex, ref int teamKitIndex, SelectTeamsSubMenu.TeamInfo teamInfo)
        {
            // play sound
            Owner.OnButtonClicked();
            OnSelectTeamKit(teamIndex, ref teamKitIndex, 1, teamInfo);
        }

        private void OnSelectPrevTeamKit(int teamIndex, ref int teamKitIndex, SelectTeamsSubMenu.TeamInfo teamInfo)
        {
            // play sound
            Owner.OnButtonClicked();
            OnSelectTeamKit(teamIndex, ref teamKitIndex, -1, teamInfo);
        }

        private void OnSelectTeamKit(int teamIndex, ref int teamKitIndex, int step, SelectTeamsSubMenu.TeamInfo teamInfo)
        {
            // update the index
            teamKitIndex += step;

            // check if team index is below range
            bool isIndexBelowRange = teamKitIndex < 0;

            // wrap index if below range
            if (isIndexBelowRange)
                teamKitIndex = Owner.TeamsData[teamIndex].Kits.Count - 1;
            
            // check if index is above rang
            bool isIndexAboveRange = teamKitIndex > Owner.TeamsData[teamIndex].Kits.Count - 1;

            // wrap if index is above range
            if (isIndexAboveRange)
                teamKitIndex = 0;
            
            // update ui
            KitDto selectedTeam = Owner.TeamsData[teamIndex].Kits[teamKitIndex];
            teamInfo.ImgTeamIcon.sprite = selectedTeam.ImgIcon;
            teamInfo.TxtTeamName.text = selectedTeam.Name;
        }

        private void InitializeUtilityMenu()
        {
            Owner.InitializeUtilityMenu(true, 
                true, 
                "Select Kits", 
                delegate
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Machine.ChangeState<SelectTeams>();
                },
                delegate
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Machine.ChangeState<PreMatch>();
                });
        }

        private void ShowkitMenuPlayer()
        {
            switch (Owner.SelectedCpuTeamId)
            {
                case 0: // Brazil
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]);  // Yellow
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // Blueline
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 1: // France
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]);  // Blue
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // Blueline
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 2: // Argentina
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]);  // BlueLine
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 3: // England
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 4: // Spain
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 5: // Germany
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 6: // Netherlands
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]);  // Yellow
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 7: // Senegal
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 8: // Portugal
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]);  // RedLine
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                    }
                    break;
                case 9: // United States
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 10: // Uruguay
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 11: // Australia
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]);  // Yellow
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 12: // Poland
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 13: // Morocco
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 14: // Croatia
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]);  // Redline
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                    }
                    break;
                case 15: // Japan
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]);  // Blue
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 16: // Switzerland
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 17: // Ghana
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 18: // Qatar
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 19: // Saudi Arabia
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]);  // Greenline
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                    }
                    break;
                case 20: // Italy
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // GreenLine
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 21: // Pakistan
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]);  // GreenLine
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                    }
                    break;
                case 22: // India
                    switch (Owner.SelectedCpuTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greeline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangePlayerKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                    }
                    break;
            }
        }
        private void ShowkitMenuAI()
        {
            switch (Owner.SelectedUserTeamId)
            {
                case 0: // Brazil
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]);  // Yellow
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // Blueline
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 1: // France
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]);  // Blue
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // Blueline
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 2: // Argentina
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]);  // BlueLine
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 3: // England
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 4: // Spain
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 5: // Germany
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 6: // Netherlands
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]);  // Yellow
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 7: // Senegal
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 8: // Portugal
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]);  // RedLine
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                    }
                    break;
                case 9: // United States
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 10: // Uruguay
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 11: // Australia
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]);  // Yellow
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 12: // Poland
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 13: // Morocco
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 14: // Croatia
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]);  // Redline
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                    }
                    break;
                case 15: // Japan
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]);  // Blue
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 16: // Switzerland
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 17: // Ghana
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 18: // Qatar
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]);  // Red
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greenline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                    }
                    break;
                case 19: // Saudi Arabia
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]);  // Greenline
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // Redline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                    }
                    break;
                case 20: // Italy
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // GreenLine
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                    }
                    break;
                case 21: // Pakistan
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]);  // GreenLine
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]); // White
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                    }
                    break;
                case 22: // India
                    switch (Owner.SelectedUserTeamKitId)
                    {
                        case 0:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[3]);  // White
                            break;
                        case 1:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[2]); // Yellow
                            break;
                        case 2:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[5]); // RedLine
                            break;
                        case 3:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[4]); // BlueLine
                            break;
                        case 4:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[0]); // Blue
                            break;
                        case 5:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[6]); // Greeline
                            break;
                        case 6:
                            MyCharacterSystem.Instance.ChangeAIKit(MyCharacterSystem.Instance.CharacterKitsmats[1]); // Red
                            break;
                    }
                    break;
            }
        }

    }
}
