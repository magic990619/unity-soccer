using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.Init.SubStates
{
    public class Initialize : BState
    {
        int _finishedInitializedTeamCount;

        public override void Enter()
        {
            base.Enter();

            //reset the count
            _finishedInitializedTeamCount = 0;

            // initialize ball
            Ball.Instance.IsAutoRotatingBall = true;

            //listen to team OnInit events
            Owner.TeamAway.OnInit += Instance_OnTeamInit;
            Owner.TeamHome.OnInit += Instance_OnTeamInit;

            //set some team data
            InitializeTeam(Owner.TeamAway, Owner.AwayTeamData);
            InitializeTeam(Owner.TeamHome, Owner.HomeTeamData);

            //set the team with the initial kick-off
            if (Random.value <= 0.5f)
                Owner.TeamAway.HasInitialKickOff = true;
            else
                Owner.TeamHome.HasInitialKickOff = true;

            //set some variables
            Owner.CurrentHalf = 1;
            Owner.NextStopTime = Owner.NormalTimeHalfLength;

            //calculate some variables
            TimeManager.Instance.TimeUpdateFrequency = Owner.ActualHalfLength / Owner.NormalTimeHalfLength;

            //enable the teams
            Owner.TeamAway.gameObject.SetActive(true);
            Owner.TeamHome.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            // notify the two teams that the oppossition has finished initializing
            Owner.TeamAway.Invoke_OnOppFinishedInit();
            Owner.TeamHome.Invoke_OnOppFinishedInit();

            //stop listening to team OnInit events
            Owner.TeamAway.OnInit -= Instance_OnTeamInit;
            Owner.TeamHome.OnInit -= Instance_OnTeamInit;
        }

        void InitializeTeam(Team team, FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities.InGameTeamDto teamData)
        {
            if(team.IsUserControlled == true)
            {
                team.Init(Owner.UserTeamParams.AiUpdateFrequency,
                    Owner.UserTeamParams.TightPressFrequency,
                    Owner.UserTeamParams.DistancePassMax,
                    Owner.UserTeamParams.DistancePassMin,
                    Owner.UserTeamParams.DistanceShotValidMax,
                    Owner.UserTeamParams.DistanceTendGoal,
                    Owner.UserTeamParams.DistanceThreatMax,
                    Owner.UserTeamParams.DistanceThreatMin,
                    Owner.UserTeamParams.DistanceThreatTrack,
                    Owner.UserTeamParams.DistanceWonderMax,
                    Owner.UserTeamParams.VelocityLongPassArrive,
                    Owner.UserTeamParams.VelocityShortPassArrive,
                    Owner.UserTeamParams.VelocityShotArrive,
                    Owner.UserTeamParams.DiveSpeed,
                    Owner.UserTeamParams.JogSpeed,
                    Owner.UserTeamParams.DiveDistance,
                    Owner.UserTeamParams.JumpHeight,
                    Owner.UserTeamParams.Power,
                    Owner.UserTeamParams.Reach,
                    Owner.UserTeamParams.Speed,
                    teamData);
            }
            else
            {
                team.Init(Owner.CpuTeamParams.AiUpdateFrequency,
                    Owner.CpuTeamParams.TightPressFrequency,
                    Owner.CpuTeamParams.DistancePassMax,
                    Owner.CpuTeamParams.DistancePassMin,
                    Owner.CpuTeamParams.DistanceShotValidMax,
                    Owner.CpuTeamParams.DistanceTendGoal,
                    Owner.CpuTeamParams.DistanceThreatMax,
                    Owner.CpuTeamParams.DistanceThreatMin,
                    Owner.CpuTeamParams.DistanceThreatTrack,
                    Owner.CpuTeamParams.DistanceWonderMax,
                    Owner.CpuTeamParams.VelocityLongPassArrive,
                    Owner.CpuTeamParams.VelocityShortPassArrive,
                    Owner.CpuTeamParams.VelocityShotArrive,
                    Owner.CpuTeamParams.DiveSpeed,
                    Owner.CpuTeamParams.JogSpeed,
                    Owner.CpuTeamParams.DiveDistance,
                    Owner.CpuTeamParams.JumpHeight,
                    Owner.CpuTeamParams.Power,
                    Owner.CpuTeamParams.Reach,
                    Owner.CpuTeamParams.Speed,
                    teamData);
            }
        }

        private void Instance_OnTeamInit()
        {
            ++_finishedInitializedTeamCount;

            if (_finishedInitializedTeamCount == 2)
                Machine.ChangeState<WaitForMatchOnInstruction>();
        }

        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
