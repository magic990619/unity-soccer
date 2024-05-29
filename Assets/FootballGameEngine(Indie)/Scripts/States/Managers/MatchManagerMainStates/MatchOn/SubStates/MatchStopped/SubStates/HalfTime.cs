using System;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;
using static Assets.FootballGameEngine_Indie.Scripts.Managers.MatchManager;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates.MatchStopped.SubStates
{
    public class HalfTime : BState
    {
        public override void Enter()
        {
            base.Enter();

            //listen to instructions to go to second half
            Owner.OnContinueToSecondHalf += Instance_OnContinueToSecondHalf;

            //raise the event that I have entered the half-time state
            RaiseTheHalfTimeStartEvent();

        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to instructions to go to second half
            Owner.OnContinueToSecondHalf -= Instance_OnContinueToSecondHalf;

            //raise the event that I have exited the half-time state
            ActionUtility.Invoke_Action(Owner.OnExitHalfTimeState);

        }

        /// <summary>
        /// Raises the half start event
        /// </summary>
        public void RaiseTheHalfTimeStartEvent()
        {
            //prepare an empty string
            string message = string.Format("{0}    {1}-{2}    {3}",
                Owner.TeamAway.TeamData.Name,
                Owner.TeamAway.Goals,
                Owner.TeamHome.Goals,
                Owner.TeamHome.TeamData.Name);

            //raise the event
            ActionUtility.Invoke_Action(message, Owner.OnEnterHalfTimeState);
        }

        private void Instance_OnContinueToSecondHalf()
        {
            Machine.ChangeState<SwitchSides>();
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
