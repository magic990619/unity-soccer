using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System;
using System.Linq;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.PrepareForMatchState.SubStates.PreMatchState.SubStates
{
    public class PreMatchTeamManagement : BState
    {
        PlayerItem002 _firstSelectedHomeTeamPlayer;
        PlayerItem002 _secondSelectedHomeTeamPlayer;

        public GameManager Owner { get => ((GameManagerFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // initialize the utility menu
            InitializeUtilityMenu();

            // disable children
            GraphicsManager.Instance.PrepareForMatchMainMenu
               .PreMatchSubMenu
               .PreMatchTeamManagementSubMenu
               .CpuTeamManagementWidget
               .DisableChildren();
            GraphicsManager.Instance.PrepareForMatchMainMenu
               .PreMatchSubMenu
               .PreMatchTeamManagementSubMenu
               .UserTeamManagementWidget
               .DisableChildren();

            // init the menus
            GraphicsManager.Instance.PrepareForMatchMainMenu
               .PreMatchSubMenu
               .PreMatchTeamManagementSubMenu
               .CpuTeamManagementWidget
               .Init(Owner.CpuTeamMatchDayTeam,
                FormationManager.Instance.Formations);
            RefresUserTeamUI();

            // enable the squad button
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchTeamManagementSubMenu
                .CpuTeamManagementWidget
                .TglSquad
                .isOn = true;
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchTeamManagementSubMenu
                .UserTeamManagementWidget
                .TglSquad
                .isOn = true;

            // enable the squad root
            GraphicsManager.Instance.PrepareForMatchMainMenu
               .PreMatchSubMenu
               .PreMatchTeamManagementSubMenu
               .CpuTeamManagementWidget
               .SquadRoot
               .SetActive(true);
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchTeamManagementSubMenu
                .UserTeamManagementWidget
                .SquadRoot
                .SetActive(true);

            // enable the prematch team-management menu
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchTeamManagementSubMenu
                .Root
                .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // disable the prematch team-management menu
            GraphicsManager.Instance.PrepareForMatchMainMenu
                .PreMatchSubMenu
                .PreMatchTeamManagementSubMenu
                .Root
                .SetActive(false);
        }

        private void InitializeUtilityMenu()
        {
            Owner.InitializeUtilityMenu(true,
                false,
                "Team Management",
                delegate
                {
                    // play sound
                    Owner.OnButtonClicked();
                    Machine.GoToPreviousState();
                });
        }

        public void RefresUserTeamUI()
        {
            GraphicsManager.Instance.PrepareForMatchMainMenu
            .PreMatchSubMenu
            .PreMatchTeamManagementSubMenu
            .UserTeamManagementWidget
           .Init(Owner.UserTeamMatchDayTeam,
              FormationManager.Instance.Formations,
              OnSelectUserTeamFormation,
              OnSelectUserTeamFormationManagement,
              OnSelectUserTeamSquadManagement,
              OnSelectUserTeamTacticManagement,
              OnSelectNextAttackTactic,
              OnSelectNextDefenceTactic,
              OnSelectPreviousAttackTactic,
              OnSelectPreviousDefendTactic,
              OnSelectHomeTeamPlayer,
              OnClickSwapHomeTeamPlayers);
        }

        #region FormationManagement

        public void OnSelectUserTeamFormationManagement()
        {
            //refresh the squad list
            // refresh ui
            RefresUserTeamUI();

            // enable and disable appropriate ui elements
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .SquadRoot.SetActive(false);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .FormationRoot.SetActive(true);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .TacticsRoot.SetActive(false);
        }

        public void OnSelectUserTeamSquadManagement()
        {
            //refresh the squad list
            // refresh ui
            RefresUserTeamUI();

            // enable and disable appropriate ui elements
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .SquadRoot.SetActive(true);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .FormationRoot.SetActive(false);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .TacticsRoot.SetActive(false);
        }

        public void OnSelectUserTeamTacticManagement()
        {
            //refresh the squad list
            // refresh ui
            RefresUserTeamUI();

            // enable and disable appropriate ui elements
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .SquadRoot.SetActive(false);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .FormationRoot.SetActive(false);
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
            .TacticsRoot.SetActive(true);
        }

        public void OnSelectUserTeamFormation(string formationType)
        {
            // set the team formations to be the selected formation
            FormationTypeEnum formationTypeEnum = (FormationTypeEnum)Enum.Parse(typeof(FormationTypeEnum), formationType);
            Owner.UserTeamMatchDayTeam.FormationType = formationTypeEnum;

            // get formation
            Formation formation = FormationManager.Instance.GetFormation(formationTypeEnum);

            // set the formation name image on the ui
            GraphicsManager.Instance.PrepareForMatchMainMenu.PreMatchSubMenu.PreMatchTeamManagementSubMenu.UserTeamManagementWidget
                .FormationManagementWidget
                .Init(formation.FormationName, formation.Icon);
        }

        #endregion

        #region SquadManagement

        private void OnClickSwapHomeTeamPlayers()
        {
            //copy the first player into a temp variable
            int playerOneID = int.Parse(_firstSelectedHomeTeamPlayer.Id.text.ToString());
            int playerTwoID = int.Parse(_secondSelectedHomeTeamPlayer.Id.text.ToString());

            //temporarily store the first player here
            PlayerDto tempPlayer = new PlayerDto(Owner.UserTeamMatchDayTeam.Players.Find(tM => tM.Id == playerOneID));

            // get the indexes of the selected players
            int playerOneIndex = Owner.UserTeamMatchDayTeam.Players.IndexOf(Owner.UserTeamMatchDayTeam.Players.Find(x => x.Id == playerOneID));
            int playerTwoIndex = Owner.UserTeamMatchDayTeam.Players.IndexOf(Owner.UserTeamMatchDayTeam.Players.Find(x => x.Id == playerTwoID));

            // swap here
            Owner.UserTeamMatchDayTeam.Players[playerOneIndex] = Owner.UserTeamMatchDayTeam.Players.Find(x => x.Id == playerTwoID);
            Owner.UserTeamMatchDayTeam.Players[playerTwoIndex] = tempPlayer;

            // refresh ui
            RefresUserTeamUI();
        }

        private void OnSelectHomeTeamPlayer(PlayerItem002 playerItem)
        {
            if (_firstSelectedHomeTeamPlayer == null)
            {
                _firstSelectedHomeTeamPlayer = playerItem;
                _firstSelectedHomeTeamPlayer.transform.Find("CheckMark").gameObject.SetActive(true);
            }
            else if (_firstSelectedHomeTeamPlayer != null)
            {
                if (_firstSelectedHomeTeamPlayer == playerItem)
                {
                    _firstSelectedHomeTeamPlayer.transform.Find("CheckMark").gameObject.SetActive(false);
                    _firstSelectedHomeTeamPlayer = null;

                }
                else if (_secondSelectedHomeTeamPlayer == null)
                {
                    _secondSelectedHomeTeamPlayer = playerItem;
                    _secondSelectedHomeTeamPlayer.transform.Find("CheckMark").gameObject.SetActive(true);
                }
                else if (_secondSelectedHomeTeamPlayer != null)
                {
                    if (_secondSelectedHomeTeamPlayer == playerItem)
                    {
                        _secondSelectedHomeTeamPlayer.transform.Find("CheckMark").gameObject.SetActive(false);
                        _secondSelectedHomeTeamPlayer = null;
                    }
                    else
                    {
                        _secondSelectedHomeTeamPlayer.transform.Find("CheckMark").gameObject.SetActive(false);
                        _secondSelectedHomeTeamPlayer = playerItem;
                        _secondSelectedHomeTeamPlayer.transform.Find("CheckMark").gameObject.SetActive(true);
                    }
                }
            }

            if (_firstSelectedHomeTeamPlayer != null && _secondSelectedHomeTeamPlayer != null)
            { 
                GraphicsManager.Instance.PrepareForMatchMainMenu
                  .PreMatchSubMenu
                  .PreMatchTeamManagementSubMenu
                  .UserTeamManagementWidget
                  .SquadManagementWidget
                  .SwapButton
                  .gameObject
                  .SetActive(true);
            }
            else
            {
                GraphicsManager.Instance.PrepareForMatchMainMenu
                  .PreMatchSubMenu
                  .PreMatchTeamManagementSubMenu
                  .UserTeamManagementWidget
                  .SquadManagementWidget
                  .SwapButton
                  .gameObject
                  .SetActive(false);
            }
        }

        #endregion

        #region TacticManagement

        public void OnSelectPreviousAttackTactic()
        {
            // convert the team attack tactics enum to array
            var attackTactics = Enum.GetValues(typeof(AttackTypeEnum)).Cast<AttackTypeEnum>().ToList();
            var currTeamAttackTacticIndex = attackTactics.IndexOf(Owner.UserTeamMatchDayTeam.AttackType);

            // get index of the curr attack tactic
            int nextTacticIndex;

            // check the if we are
            if (currTeamAttackTacticIndex == 0)
                nextTacticIndex = attackTactics.Count - 1;
            else
                nextTacticIndex = currTeamAttackTacticIndex - 1;

            // set the team attack tatctic to be the next attack tactic
            Owner.UserTeamMatchDayTeam.AttackType = attackTactics[nextTacticIndex];

            // update ui
            RefresUserTeamUI();
        }

        public void OnSelectNextAttackTactic()
        {
            // convert the team attack tactics enum to array
            var attackTactics = Enum.GetValues(typeof(AttackTypeEnum)).Cast<AttackTypeEnum>().ToList();
            var currTeamAttackTacticIndex = attackTactics.IndexOf(Owner.UserTeamMatchDayTeam.AttackType);

            // get index of the curr attack tactic
            int nextTacticIndex;

            // check the if we are at max index
            if (currTeamAttackTacticIndex == attackTactics.Count - 1)
                nextTacticIndex = 0;
            else
                nextTacticIndex = 1 + currTeamAttackTacticIndex;

            // set the team attack tatctic to be the next attack tactic
            Owner.UserTeamMatchDayTeam.AttackType = attackTactics[nextTacticIndex];

            // update ui
            RefresUserTeamUI();
        }

        public void OnSelectPreviousDefendTactic()
        {
            // convert the team attack tactics enum to array
            var defenceTactics = Enum.GetValues(typeof(DefenceTypeEnum)).Cast<DefenceTypeEnum>().ToList();
            var currTeamDefenceTacticIndex = defenceTactics.IndexOf(Owner.UserTeamMatchDayTeam.DefenceType);

            // get index of the curr attack tactic
            int nextTacticIndex;

            // check the if we are
            if (currTeamDefenceTacticIndex == 0)
                nextTacticIndex = defenceTactics.Count - 1;
            else
                nextTacticIndex = currTeamDefenceTacticIndex - 1;

            // set the team attack tatctic to be the next attack tactic
            Owner.UserTeamMatchDayTeam.DefenceType = defenceTactics[nextTacticIndex];

            // update ui
            RefresUserTeamUI();
        }

        public void OnSelectNextDefenceTactic()
        {
            // convert the team attack tactics enum to array
            var defenceTactics = Enum.GetValues(typeof(DefenceTypeEnum)).Cast<DefenceTypeEnum>().ToList();
            var currTeamDefenceTacticIndex = defenceTactics.IndexOf(Owner.UserTeamMatchDayTeam.DefenceType);

            // get index of the curr attack tactic
            int nextTacticIndex;

            // check the if we are at max index
            if (currTeamDefenceTacticIndex == defenceTactics.Count - 1)
                nextTacticIndex = 0;
            else
                nextTacticIndex = 1 + currTeamDefenceTacticIndex;

            // set the team attack tatctic to be the next attack tactic
            Owner.UserTeamMatchDayTeam.DefenceType = defenceTactics[nextTacticIndex];

            // update ui
            RefresUserTeamUI();
        }
        #endregion
    }
}
