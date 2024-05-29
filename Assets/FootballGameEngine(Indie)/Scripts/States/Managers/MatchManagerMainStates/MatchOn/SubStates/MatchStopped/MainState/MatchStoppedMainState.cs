using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates.MatchStopped.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchStopped.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates.MatchStopped.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchStopped.MainState
{
    public class MatchStoppedMainState : BHState
    {
        public override void AddStates()
        {
            //add the states
            AddState<BroadcastCornerKick>(); 
            AddState<BroadcastGoalKick>();
            AddState<BroadcastGoalScored>();
            AddState<BroadcastHalfTimeStatus>();
            AddState<BroadcastNormalTimeIsOver>();
            AddState<BroadcastThrowIn>();
            AddState<CheckNextMatchStatus>();
            AddState<HalfTime>();
            AddState<NormalTimeOver>();
            AddState<SwitchSides>();
            AddState<TriggerMatchOver>();

            //set the initial state
            SetInitialState<CheckNextMatchStatus>();
        }

        public override void Enter()
        {
            base.Enter();

            ////register the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff += Owner.TeamAway.OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff += Owner.TeamHome.OnMessagedToTakeKickOff;

            ////raise the match stopped event
            ActionUtility.Invoke_Action(Owner.OnStopMatch);
        }

        public override void Exit()
        {
            base.Exit();

            ////deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff -= Owner.TeamAway.OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff -= Owner.TeamHome.OnMessagedToTakeKickOff;
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
