using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ProtectGoal.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeCornerKick.MainState;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState
{
    public class WaitMainState : BState
    {
        public override void Enter()
        {
            base.Enter();

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            // stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            //listen to variaus events
            Owner.OnInstructedToDefendCornerKick += Instance_OnInstructedToDefendCornerKick;
            Owner.OnInstructedToGoToHome += Instance_OnInstructedToGoToHome;
            Owner.OnInstructedToTakeGoalKick += Instance_OnInstructedToTakeGoalKick;
        }

        public override void Exit()
        {
            base.Exit();

            // reset the animator
            Owner.Animator.ResetTrigger("Idle");

            //stop listening to variaus events
            Owner.OnInstructedToDefendCornerKick -= Instance_OnInstructedToDefendCornerKick;
            Owner.OnInstructedToGoToHome -= Instance_OnInstructedToGoToHome;
            Owner.OnInstructedToTakeGoalKick -= Instance_OnInstructedToTakeGoalKick;
        }

        private void Instance_OnInstructedToDefendCornerKick()
        {
            Machine.ChangeState<ProtectGoalMainState>();
        }

        public void Instance_OnInstructedToGoToHome()
        {
            Machine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnInstructedToTakeGoalKick()
        {
            Machine.ChangeState<TakeGoalKickMainState>();
        }

        public Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
