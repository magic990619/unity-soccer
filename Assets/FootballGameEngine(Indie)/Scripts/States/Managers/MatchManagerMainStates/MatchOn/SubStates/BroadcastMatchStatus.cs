using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;
using static Assets.FootballGameEngine_Indie.Scripts.Managers.MatchManager;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates
{
    /// <summary>
    /// Broadcasts the match status just before the match status is executed
    /// </summary>
    public class BroadcastMatchStatus : BState
    {
        /// <summary>
        /// A reference to the wait time
        /// </summary>
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            //set the wait time
            waitTime = 1f;

            //raise the half-start event
            RaiseTheMatchStartEvent();
        }

        public override void Execute()
        {
            base.Execute();

            //decrement the time
            waitTime -= Time.deltaTime;

            //go to wait-for-kick-to-complete if time is less than 0
            if (waitTime <= 0f) Machine.ChangeState<BroadcastHalfStatus>();
        }

        public override void Exit()
        {
            base.Exit();

            //raise the event that I finished broadcasting the start
            //of the first half
            ActionUtility.Invoke_Action(Owner.OnFinishBroadcastMessage);
        }

        /// <summary>
        /// Raises the half start event
        /// </summary>
        public void RaiseTheMatchStartEvent()
        {
            //prepare an empty string
            string message = Owner.CurrGameTime == GameTimeEnum.NormalTime ? "Normal Time" : "Extra Time";

            //raise the event
            ActionUtility.Invoke_Action(message, Owner.OnStartBroadcastMessage);
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
