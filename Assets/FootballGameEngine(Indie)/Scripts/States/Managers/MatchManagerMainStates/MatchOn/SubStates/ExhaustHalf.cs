using System;
using System.Linq;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchStopped.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchPaused.MainState;
using RobustFSM.Base;
using RobustFSM.Interfaces;
using UnityEngine;
using static Assets.FootballGameEngine_Indie.Scripts.Managers.MatchManager;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates
{
    public class ExhaustHalf : BState
    {
        Coroutine _executingCoroutine;

        public override void Enter()
        {
            base.Enter();

            //raise the match play start event
            ActionUtility.Invoke_Action(Owner.OnEnterExhaustHalfState);

            // listen to match manager events
            Owner.OnMatchPaused += Instance_OnMatchPaused;

            // register the teams to goal score events
            Owner.TeamAway.Goal.OnCollideWithBall += Owner.TeamAway.OnOppScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall += Owner.TeamHome.OnOppScoredAGoal;

            Owner.TeamAway.Goal.OnCollideWithBall += Owner.TeamHome.OnTeamScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall += Owner.TeamAway.OnTeamScoredAGoal;

            // register to throw in events
            Owner.LeftThrowInTrigger.OnCollidedWithBall += Instance_OnThrowIn;
            Owner.RightThrowInTrigger.OnCollidedWithBall += Instance_OnThrowIn;

            //listen to the OnTick event of the Time Manager
            Owner.TeamAway.Goal.OnCollideWithBall += Instance_OnGoalScored;
            Owner.TeamHome.Goal.OnCollideWithBall += Instance_OnGoalScored;

            Owner.TeamAway.CornerGoalKickTriggers.OnCollidedWithBall += Instance_OnCornerOrGoalKick;
            Owner.TeamHome.CornerGoalKickTriggers.OnCollidedWithBall += Instance_OnCornerOrGoalKick;

            TimeManager.Instance.OnTick += Instance_TimeManagerOnTick;

            //start the counter
            _executingCoroutine = Owner.StartCoroutine(TimeManager.Instance.TickTime());
        }

        public override void Exit()
        {
            base.Exit();

            //raise the match play stop event
            ActionUtility.Invoke_Action(Owner.OnExitExhaustHalfState);

            // listen to match manager events
            Owner.OnMatchPaused -= Instance_OnMatchPaused;

            // deregister the teams to goal score events
            Owner.TeamAway.Goal.OnCollideWithBall -= Owner.TeamAway.OnOppScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall -= Owner.TeamHome.OnOppScoredAGoal;

            // deregister the teams to goal score events
            Owner.TeamAway.Goal.OnCollideWithBall -= Owner.TeamHome.OnTeamScoredAGoal;
            Owner.TeamHome.Goal.OnCollideWithBall -= Owner.TeamAway.OnTeamScoredAGoal;

            // deregister to throw in events
            Owner.LeftThrowInTrigger.OnCollidedWithBall -= Instance_OnThrowIn;
            Owner.RightThrowInTrigger.OnCollidedWithBall -= Instance_OnThrowIn;

            //stop listening to the OnTick event of the Time Manager
            Owner.TeamAway.Goal.OnCollideWithBall -= Instance_OnGoalScored;
            Owner.TeamHome.Goal.OnCollideWithBall -= Instance_OnGoalScored;

            Owner.TeamAway.CornerGoalKickTriggers.OnCollidedWithBall -= Instance_OnCornerOrGoalKick;
            Owner.TeamHome.CornerGoalKickTriggers.OnCollidedWithBall -= Instance_OnCornerOrGoalKick;

            TimeManager.Instance.OnTick -= Instance_TimeManagerOnTick;

            //stop the counter
            Owner.StopCoroutine(_executingCoroutine);
        }

        private void Instance_OnCornerOrGoalKick(Team team, Vector3 position)
        {
            // check if it's corner-kick or goal-kick
            bool isPlayerWithLastTouchTeamPlayer = team.Players.Any(tM => tM.Player == Ball.Instance.OwnerWithLastTouch);
            bool isCornerKick = isPlayerWithLastTouchTeamPlayer == true;

            // cache the ball pos
            Owner.CachedBallPosition = position;

            // set the match status
            if (isCornerKick == true)
            {
                team.Opponent.CachedBallPosition = position;
                Owner.MatchStatus = MatchStatuses.CornerKick;
            }
            else
            {
                team.CachedBallPosition = position;
                Owner.MatchStatus = MatchStatuses.GoalKick;
            }

            // trigger state change to match stopped
            Machine.ChangeState<MatchStoppedMainState>();
        }

        private void Instance_OnGoalScored()
        {
            //prepare the text
            string info = string.Format("{0}    {1}-{2}    {3}", 
                Owner.TeamAway.TeamData.ShortName,
                Owner.TeamAway.Goals, 
                Owner.TeamHome.Goals,
                Owner.TeamHome.TeamData.ShortName);

            //invoke the goal-scored event
            ActionUtility.Invoke_Action(info, Owner.OnGoalScored);

            // set the match status
            Owner.MatchStatus = MatchStatuses.GoalScored;

            // trigger state change
            Machine.ChangeState<MatchStoppedMainState>();
        }

        private void Instance_OnMatchPaused()
        {
            //prepare the text
            string info = string.Format("{0}    {1}-{2}    {3}",
                Owner.TeamAway.TeamData.ShortName,
                Owner.TeamAway.Goals,
                Owner.TeamHome.Goals,
                Owner.TeamHome.TeamData.ShortName);

            //raise the on-match-end-evet
            ActionUtility.Invoke_Action(info, Owner.OnEnterMatchPausedState);

            // trigger state change
            Machine.ChangeState<MatchPausedMainState>();
        }

        private void Instance_OnThrowIn(Vector3 position)
        {
            // cache the ball pos
            Owner.CachedBallPosition = position;

            // set the match status
            Owner.MatchStatus = MatchStatuses.ThrowIn;

            // trigger state change to match stopped
            Machine.ChangeState<MatchStoppedMainState>();
        }

        private void Instance_TimeManagerOnTick(int minutes, int seconds)
        {
            //raise the on tick event of the match manager
            ActionUtility.Invoke_Action(Owner.CurrentHalf, minutes, seconds, Owner.OnTick);

            //compare the next stop time and the current time
            //stop game if current time is now equal to the next stop time
            if (minutes >= Owner.NextStopTime)
            {
                // set the match status
                Owner.MatchStatus = MatchStatuses.HalfExhausted;

                // trigger state change
                Machine.ChangeState<MatchStoppedMainState>();
            }
        }

        /// <summary>
        /// Access the super state machine
        /// </summary>
        public IFSM SuperFSM
        {
            get
            {
                return (MatchManagerFSM)SuperMachine;
            }
        }

        /// <summary>
        /// Access the owner of the state machine
        /// </summary>
        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
