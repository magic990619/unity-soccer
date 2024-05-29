using RobustFSM.Base;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using static Assets.FootballGameEngine_Indie.Scripts.Managers.MatchManager;
using System;
using UnityEngine;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Random = UnityEngine.Random;
using RobustFSM.Interfaces;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates.MatchStopped.SubStates
{
    public class NormalTimeOver : BState
    {
        public override void Enter()
        {
            //listen to instructions to go to the next phase
            Owner.OnContinueToNormalTime += Instance_OnContinueToNormalTime;

            //raise the event that I have entered the half-time state
            RaiseTheOnEnterNormalTimeIsOverStateEvent();
        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to instructions to go to the next phase
            Owner.OnContinueToNormalTime -= Instance_OnContinueToNormalTime;

            //raise the event that I have exited the half-time state
            ActionUtility.Invoke_Action(Owner.OnExitNormalTimeIsOverState);
        }

        /// <summary>
        /// Raises the half start event
        /// </summary>
        public void RaiseTheOnEnterNormalTimeIsOverStateEvent()
        {
            //prepare an empty string
            string message = string.Format("{0}    {1}-{2}    {3}",
                Owner.TeamAway.TeamData.Name,
                Owner.TeamAway.Goals,
                Owner.TeamHome.Goals,
                Owner.TeamHome.TeamData.Name);

            //raise the event
            Action<string> temp = Owner.OnEnterNormalTimeIsOverState;
            if (temp != null) temp.Invoke(message);
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

        private void Instance_OnContinueToNormalTime()
        {
            // set match to be extra time
            Owner.CurrGameTime = GameTimeEnum.ExtraTime;
            Owner.TeamAway.HasInitialKickOff = false;
            Owner.TeamHome.HasInitialKickOff = false;

            //set the team with the initial kick-off
            if (Random.value <= 0.5f)
                Owner.TeamAway.HasInitialKickOff = true;
            else
                Owner.TeamHome.HasInitialKickOff = true;

            //set some variables
            Owner.CurrentHalf = 1;
            Owner.NextStopTime += Owner.ExtraTimeHalfLength;

            // go to switch sides
            ((IState)Machine).Machine.ChangeState<BroadcastMatchStatus>();
        }
    }
}