using System;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchStopped.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchPaused.MainState;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.MainState
{
    public class MatchOnMainState : BHState
    {
        public override void AddStates()
        {
            //add the states
            AddState<BroadcastHalfStatus>();
            AddState<BroadcastMatchStatus>();
            AddState<ExhaustHalf>();
            AddState<MatchStoppedMainState>();
            AddState<MatchPausedMainState>();
            AddState<WaitForCornerKickToComplete>();
            AddState<WaitForGoalKickToComplete>();
            AddState<WaitForKickOffToComplete>();
            AddState<WaitForThrowInToComplete>();

            //set the inital state
            SetInitialState<BroadcastMatchStatus>();
        }

        /// <summary>
        /// On enter
        /// </summary>
        public override void Enter()
        {
            base.Enter();

            //configure the teams to listen to some MatchManaher events
            Owner.OnStopMatch += Owner.TeamAway.Invoke_OnMessagedToStop;
            Owner.OnStopMatch += Owner.TeamHome.Invoke_OnMessagedToStop;

        }

        /// <summary>
        /// Returns the owner of this instance
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
