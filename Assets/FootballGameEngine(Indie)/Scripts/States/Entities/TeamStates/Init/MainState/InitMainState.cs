using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.KickOff.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Init.MainState
{
    public class InitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();
            
            //listen to some ball events
            Ball.Instance.OnBallLaunched += Owner.Invoke_OnBallLaunched;

            //listen to some team events
            Owner.OnMessagedToTakeKickOff += Instance_OnMessagedSwitchToTakeKickOff;
            Owner.OnOppFinishedInit += Instance_OnOppFinishedInit;
            Owner.Opponent.OnGainPossession += Owner.Invoke_OnLostPossession;

            //init the team
            Init();

            //raise the on init event
            ActionUtility.Invoke_Action(Owner.OnInit);
        }

        public override void Exit()
        {
            base.Exit();

            // register to some stuff
            RegisterTeamToGoalKeeperEvents();

            // instruct every player to go to wait
            ActionUtility.Invoke_Action(Owner.OnInstructPlayersToWait);

            //stop listening to some team events
            Owner.OnMessagedToTakeKickOff -= Instance_OnMessagedSwitchToTakeKickOff;
            Owner.OnOppFinishedInit -= Instance_OnOppFinishedInit;
        }

        public void Init()
        {
            // get some prerequisite data
            InitTeamFormation();

            // create players
            CreateTeamPlayers();

            // init players
            InitTeamMemberOppGoal();
            InitTeamMemberPlayerSupportSpots();
            InitTeamMemberTeamGoal();
            InitTeamPlayerTeamMembers();

            // register team members to team events
            RegisterTeamMemberToOnDefendCornerKickEvent();
            RegisterTeamMemberToOnGainPossessionEvent();
            RegisterTeamMemberToOnLoosePossessionEvent();
            RegisterTeamMemberToOnSupportCornerKickEvent();
            RegisterTeamMemberToPlayerOnInstructedToWaitEvent();

            // register team to team player events
            RegisterGoalKeeperToOnBallLaunchedEvent();
            RegisterTeamToTeamPlayerOnChaseBallEvent();
            RegisterTeamToTeamPlayerOnGainPossessionEvent();

            // set some player variables
            SetPlayerControl();
        }

        public void CreateTeamPlayers()
        {
            CreateTheFirstElevenPlayers();
            CreateTheSubstitutes();
        }

        public void CreateTheFirstElevenPlayers()
        {
            //create eleven team players
            for (int i = 0; i <= 10; i++)
            {
                //get the index
                Player player = Owner.RootPlayers.GetChild(i).GetComponent<Player>();
                Transform attackOffTransfrom = Owner.Formation.PositionsAttackingRoot.GetChild(i);
                Transform defendTransfrom = Owner.Formation.PositionsDefendingRoot.GetChild(i);
                Transform currentHomeTransfrom = Owner.Formation.PositionsCurrentHomeRoot.GetChild(i);
                Transform kickOffTransfrom = Owner.Formation.PositionsKickOffRoot.GetChild(i);

                //create the team player
                TeamPlayer newPlayer = new TeamPlayer(
                    player,
                    Owner,
                    attackOffTransfrom,
                    defendTransfrom,
                    currentHomeTransfrom,
                    kickOffTransfrom,
                    Owner.AiUpdateFrequency,
                    Owner.DistancePassMax,
                    Owner.DistancePassMin,
                    Owner.DistanceShotValidMax,
                    Owner.DistanceTendGoal,
                    Owner.DistanceThreatMax,
                    Owner.DistanceThreatMin,
                    Owner.DistanceThreatTrack,
                    Owner.DistanceWonderMax,
                    Owner.VelocityLongPassArrive,
                    Owner.VelocityShortPassArrive,
                    Owner.VelocityShotArrive,
                    Owner.DiveDistance,
                    Owner.DiveSpeed,
                    Owner.JogSpeed,
                    Owner.JumpHeight,
                    Owner.Power,
                    Owner.Reach,
                    Owner.Speed,
                    i,
                    Owner.PitchRegions,
                    Owner.TeamData);

                //add it to list
                Owner.Players.Add(newPlayer);

                // enable player
                newPlayer.Player.gameObject.SetActive(true);
            }

        }

        public void CreateTheSubstitutes()
        {
            // get the other players
            Owner.Substitues.AddRange(
                Owner.TeamData
                .Players
                .Where(p => Owner.TeamData.Players.IndexOf(p) > 10)
                .ToList()
            );
        }

        public void InitTeamFormation()
        {
            // get the team formation
            Formation formation = GameObject.Instantiate(Owner.TeamData.Formation, Vector3.zero, Owner.RootFormations.rotation, Owner.RootFormations);

            // initialize the team with the formation
            formation.gameObject.SetActive(true);
            Owner.Formation = formation;
        }

        public void InitTeamMemberOppGoal()
        {
            // init the opponent goal of each player
            Owner.Players.ForEach(tM => tM.Player.OppGoal = Owner.Opponent.Goal);
        }

        public void InitTeamMemberOppTeamMembers()
        {
            // get the full list of the team's members
            Owner.Players.ForEach(tM => tM.Player.OppositionMembers = Owner.Opponent.Players
            .Select(tMS => tMS.Player)
            .ToList());
        }

        public void InitTeamPlayerTeamMembers()
        {
            // get the full list of the team's members
            Owner.Players.ForEach(tM => tM.Player.TeamMembers = Owner.Players
            .Select(tMS => tMS.Player)
            .ToList());
        }

        public void InitTeamMemberPlayerSupportSpots()
        {
            //set the positions
            Owner.Players.ForEach(tM => tM.Player.PlayerSupportSpots = Owner.PlayerSupportSpots
            .GetComponentsInChildren<SupportSpot>()
            .ToList());
        }

        public void InitTeamMemberTeamGoal()
        {
            // init the opponent goal of each player
            Owner.Players.ForEach(tM => tM.Player.TeamGoal = Owner.Goal);
        }

        public void RegisterGoalKeeperToOnBallLaunchedEvent()
        {
            // get the goalkeeper
            TeamPlayer goalKeeper = Owner.Players
                .Where(tM => tM.Player.PlayerType == PlayerTypes.Goalkeeper)
                .FirstOrDefault();

            Ball.Instance.OnBallShot += goalKeeper.Player.Invoke_OnShotTaken;
        }

        public void RegisterTeamMemberToOnDefendCornerKickEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnDefendCornerKick += tM.Player.Invoke_OnInstructedToDefendCornerKick);
        }

        public void RegisterTeamMemberToOnGainPossessionEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnGainPossession += tM.Player.Invoke_OnTeamGainedPossession);
        }

        public void RegisterTeamMemberToOnSupportCornerKickEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnAttackCornerKick += tM.Player.Invoke_OnInstructedToSupportCornerKick);
        }

        public void RegisterTeamToGoalKeeperEvents()
        {
            Owner.GoalKeeper.OnGoalKeeperGainedControlOfBall += Owner.Invoke_OnGoalKeeperGainedBallControl;
            Owner.Opponent.GoalKeeper.OnGoalKeeperGainedControlOfBall += Owner.Invoke_OnGoalKeeperGainedBallControl;
        }

        public void RegisterTeamMemberToOnLoosePossessionEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnLostPossession += tM.Player.Invoke_OnTeamLostControl);
        }

        public void RegisterTeamToTeamPlayerOnChaseBallEvent()
        {
            Owner.Players.ForEach(tM => tM.Player.OnChaseBall += Owner.Invoke_OnPlayerChaseBall);
        }

        public void RegisterTeamToTeamPlayerOnGainPossessionEvent()
        {
            Owner.Players.ForEach(tM => tM.Player.OnControlBall += Owner.Invoke_OnGainPossession);
        }

        public void RegisterTeamMemberToPlayerOnInstructedToWaitEvent()
        {
            Owner.Players.ForEach(tM => Owner.OnInstructPlayersToWait += tM.Player.Invoke_OnInstructedToWait);
        }

        public void SetPlayerControl()
        {
            Owner.Players.ForEach(tM => tM.Player.IsUserControlled = Owner.IsUserControlled);
        }

        private void Instance_OnMessagedSwitchToTakeKickOff()
        {
            Machine.ChangeState<KickOffMainState>();
        }

        private void Instance_OnOppFinishedInit()
        {
            InitTeamMemberOppTeamMembers();
        }

        public Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
