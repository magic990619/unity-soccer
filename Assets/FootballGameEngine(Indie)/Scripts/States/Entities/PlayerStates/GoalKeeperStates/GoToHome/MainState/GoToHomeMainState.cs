using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ProtectGoal.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ChaseBall.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState
{
    public class GoToHomeMainState : BHState
    {
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void AddStates()
        {
            base.AddStates();

            //add the states
            AddState<SteerToHome>();
            AddState<WaitAtHome>();

            //set the initial state
            SetInitialState<SteerToHome>();
        }

        public override void Enter()
        {
            base.Enter();

            //listen to some events
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToWait += Instance_OnWait;

            //set the animator
            Owner.Animator.SetTrigger("TendGoal");
        }

        public override void Execute()
        {
            base.Execute();

            // get the velocity direction
            bool isSteering = Owner.RPGMovement.Steer;
            Vector3 velocity = Owner.RPGMovement.MovementDirection;

            // find local direction
            Vector3 localVelocity = Owner.transform.InverseTransformDirection(velocity);
            localVelocity.Normalize();

            // set the forward an turns
            float forward = isSteering ? localVelocity.z > 0 ? 1f : localVelocity.z < 0 ? -1f : 0f : 0f;
            float turn = isSteering ? localVelocity.x > 0 ? 1f : localVelocity.x < 0 ? -1f : 0f : 0f;

            // update animator
            Owner.Animator.SetFloat("Forward", forward, 0.1f, 0.1f);
            Owner.Animator.SetFloat("Turn", turn, 0.1f, 0.1f);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // run logic depending on whether team is in control or not
            if (Owner.IsTeamInControl == false)
                SuperMachine.ChangeState<ProtectGoalMainState>();
        }

        public override void Exit()
        {
            base.Exit();

            //listen to some events
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToWait -= Instance_OnWait;

            // set the animator
            Owner.Animator.ResetTrigger("TendGoal");
        }

        private void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }
    }
}
