using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.Wait.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.SubStates;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState
{
    public class IdleMainState : BHState
    {
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<CheckIfHasBall>();
            AddState<CheckIfIHaveToTendGoal>();
            AddState<HoldBall>();

            // set the initial state
            SetInitialState<CheckIfHasBall>();
        }

        public override void Enter()
        {
            base.Enter();

            //listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;

            //set the components
            Owner.Animator.SetBool("HasBall", Owner.HasBall);
            Owner.Animator.SetTrigger("Idle");
            Owner.RPGMovement.SetSteeringOff();
        }

        public override void Exit()
        {
            base.Exit();

            //listen to some events
            Owner.OnInstructedToWait -= Instance_OnWait;

            //reset the animator
            Owner.Animator.ResetTrigger("Idle");
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }
    }
}
