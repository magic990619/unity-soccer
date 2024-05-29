using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.GameManagerStates.GameOnState.SubStates.HalfTime.SubStates
{
    public class HalfTimeTeamManagementState : BState
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
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .CpuTeamManagementWidget
                .DisableChildren();
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .UserTeamManagementWidget
                .DisableChildren();

            // init the menus
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .CpuTeamManagementWidget
                .Init(Owner.CpuTeamMatchDayTeam,
                FormationManager.Instance.Formations);
            RefresUserTeamUI();

            // enable the squad button
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .CpuTeamManagementWidget
                .TglSquad
                .isOn = true;
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .UserTeamManagementWidget
                .TglSquad
                .isOn = true;

            // enable the squad root
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .CpuTeamManagementWidget
                .SquadRoot
                .SetActive(true);
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .UserTeamManagementWidget
                .SquadRoot
                .SetActive(true);

            // enable the team-management menu
            GraphicsManager.Instance
                .MenuManager
                .EnableMenu(GraphicsManager.Instance.UtilityMainMenu.ID);

            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
                .Root
                .SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // update team with changes
            UpdateTeamWithChanges();

            // disable team-management menu
            GraphicsManager.Instance
                .MenuManager
                .DisableMenu(GraphicsManager.Instance.UtilityMainMenu.ID);

            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
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
                    Machine.ChangeState<ShowHalfTimeMenuState>();
                });
        }

        public void RefresUserTeamUI()
        {
            GraphicsManager.Instance.GameOnMainMenu
                .HalfTimeMainMenu
                .HalfTimeTeamManagementMenu
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

        public void UpdateTeamWithChanges()
        {
            // delete old formation
            GameObject.Destroy(MatchManager.Instance.TeamHome
                .RootFormations
                .GetChild(0)
                .GetComponent<Formation>()
                .gameObject);

            // get new formation
            Formation newFormation = FormationManager.Instance.GetFormation(Owner.UserTeamMatchDayTeam.FormationType);
            MatchManager.Instance.TeamHome.Formation = GameObject.Instantiate(newFormation, Vector3.zero, MatchManager.Instance.TeamHome.RootFormations.rotation, MatchManager.Instance.TeamHome.RootFormations);

            //// set the tactics
            MatchManager.Instance.TeamHome.TeamData.AttackTactic = Owner.GetTeamAttackTactic(Owner.UserTeamMatchDayTeam.AttackType);
            MatchManager.Instance.TeamHome.TeamData.DefendTactic = Owner.GetTeamDefenceTactic(Owner.UserTeamMatchDayTeam.DefenceType);


            //// update the first 11
            MatchManager.Instance.TeamHome
                .Players
                .ForEach(p =>
                {
                    //get index
                    int index = MatchManager.Instance.TeamHome.Players.IndexOf(p);

                    // get the formation
                    Transform attackPosition = MatchManager.Instance.TeamHome.Formation.PositionsAttackingRoot.GetChild(index).transform;
                    Transform currHomePosition = MatchManager.Instance.TeamHome.Formation.PositionsCurrentHomeRoot.GetChild(index).transform;
                    Transform defendPosition = MatchManager.Instance.TeamHome.Formation.PositionsDefendingRoot.GetChild(index).transform;
                    Transform kickOffPosition = MatchManager.Instance.TeamHome.Formation.PositionsKickOffRoot.GetChild(index).transform;

                    // update player formation root
                    p.AttackingHomePosition = attackPosition;
                    p.CurrentHomePosition = currHomePosition;
                    p.DefendingHomePosition = defendPosition;
                    p.KickOffHomePosition = kickOffPosition;

                    p.Player.HomePosition = currHomePosition;

                    // update player attributes
                    p.Player.Accuracy = Owner.UserTeamMatchDayTeam.Players[index].Accuracy;
                    p.Player.CanJoinCornerKick = Owner.UserTeamMatchDayTeam.Players[index].CanJoinCornerKick;
                    p.Player.DiveDistance = Owner.UserTeamMatchDayTeam.Players[index].JumpDistance;
                    p.Player.DiveSpeed = Owner.UserTeamMatchDayTeam.Players[index].DiveSpeed;
                    p.Player.GoalKeeping = Owner.UserTeamMatchDayTeam.Players[index].GoalKeeping;
                    p.Player.JumpHeight = Owner.UserTeamMatchDayTeam.Players[index].JumpHeight;
                    p.Player.Power = Owner.UserTeamMatchDayTeam.Players[index].Power;
                    p.Player.Reach = Owner.UserTeamMatchDayTeam.Players[index].Reach;
                    p.Player.Speed = Owner.UserTeamMatchDayTeam.Players[index].Speed;
                    p.Player.Tackling = Owner.UserTeamMatchDayTeam.Players[index].Tackling;

                    // update actual stuff
                    p.Player.ActuaDiveDistance = MatchManager.Instance.TeamHome.DiveDistance * Owner.UserTeamMatchDayTeam.Players[index].JumpDistance;
                    p.Player.ActualDiveSpeed = MatchManager.Instance.TeamHome.DiveSpeed * Owner.UserTeamMatchDayTeam.Players[index].DiveSpeed;
                    p.Player.ActuaJumpHeight = MatchManager.Instance.TeamHome.JumpHeight * Owner.UserTeamMatchDayTeam.Players[index].JumpHeight;
                    p.Player.ActualPower = MatchManager.Instance.TeamHome.Power * Owner.UserTeamMatchDayTeam.Players[index].Power;
                    p.Player.ActualReach = MatchManager.Instance.TeamHome.Reach * Owner.UserTeamMatchDayTeam.Players[index].Reach;
                    p.Player.ActualRotationSpeed = p.Player.RotationSpeed * Owner.UserTeamMatchDayTeam.Players[index].Speed;
                    p.Player.ActualSprintSpeed = MatchManager.Instance.TeamHome.JogSpeed + (MatchManager.Instance.TeamHome.Speed - MatchManager.Instance.TeamHome.JogSpeed) * Owner.UserTeamMatchDayTeam.Players[index].Speed;

                });

            // replace the substitues
            MatchManager.Instance.TeamHome.Substitues = Owner.UserTeamMatchDayTeam.Players
                .Where(p => Owner.UserTeamMatchDayTeam.Players.IndexOf(p) > 10)
                .Select(p => new InGamePlayerDto()
                {
                    Accuracy = p.Accuracy,
                    CanJoinCornerKick = p.CanJoinCornerKick,
                    DiveSpeed = p.DiveSpeed,
                    GoalKeeping = p.GoalKeeping,
                    DiveDistance = p.JumpDistance,
                    JumpHeight = p.JumpHeight,
                    Power = p.Power,
                    Reach = p.Reach,
                    Speed = p.Speed,
                    Tackling = p.Tackling
                })
                .ToList();
        }

        #region FormationManagement

        public void OnSelectUserTeamFormationManagement()
        {
            //refresh the squad list
            // refresh ui
            RefresUserTeamUI();

            // enable and disable appropriate ui elements
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.SquadRoot.SetActive(false);
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.FormationRoot.SetActive(true);
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.TacticsRoot.SetActive(false);
        }

        public void OnSelectUserTeamSquadManagement()
        {
            //refresh the squad list
            // refresh ui
            RefresUserTeamUI();

            // enable and disable appropriate ui elements
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.SquadRoot.SetActive(true);
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.FormationRoot.SetActive(false);
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.TacticsRoot.SetActive(false);
        }

        public void OnSelectUserTeamTacticManagement()
        {
            //refresh the squad list
            // refresh ui
            RefresUserTeamUI();

            // enable and disable appropriate ui elements
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.SquadRoot.SetActive(false);
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.FormationRoot.SetActive(false);
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget.TacticsRoot.SetActive(true);
        }

        public void OnSelectUserTeamFormation(string formationType)
        {
            // set the team formations to be the selected formation
            FormationTypeEnum formationTypeEnum = (FormationTypeEnum)Enum.Parse(typeof(FormationTypeEnum), formationType);
            Owner.UserTeamMatchDayTeam.FormationType = formationTypeEnum;

            // get formation
            Formation formation = FormationManager.Instance.GetFormation(formationTypeEnum);

            // set the formation name image on the ui
            GraphicsManager.Instance.GameOnMainMenu.HalfTimeMainMenu.HalfTimeTeamManagementMenu.UserTeamManagementWidget
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
                GraphicsManager.Instance.GameOnMainMenu
                    .HalfTimeMainMenu
                    .HalfTimeTeamManagementMenu
                    .UserTeamManagementWidget
                    .SquadManagementWidget
                    .SwapButton
                    .gameObject
                    .SetActive(true);
            }
            else
            {
                GraphicsManager.Instance.GameOnMainMenu
                    .HalfTimeMainMenu
                    .HalfTimeTeamManagementMenu
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
