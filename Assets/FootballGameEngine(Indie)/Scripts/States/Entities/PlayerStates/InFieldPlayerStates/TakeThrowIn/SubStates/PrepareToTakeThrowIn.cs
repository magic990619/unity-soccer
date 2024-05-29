using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeThrowIn.SubStates
{
    public class PrepareToTakeThrowIn : BState
    {
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            // set wait time
            waitTime = 1f;

            // go to throw-in
            Owner.Animator.SetBool("IsDoubleHandThrow", true);
            Owner.Animator.SetTrigger("ThrowBall");

            // place ball at position
            Ball.Instance.Trap();
            Ball.Instance.CurrentOwner = Owner;
            Ball.Instance.Rigidbody.isKinematic = true;
            Ball.Instance.Position = Owner.BallTopPosition.position;
            Ball.Instance.transform.rotation = Owner.BallTopPosition.transform.rotation;
        }

        public override void Execute()
        {
            base.Execute();

            waitTime -= Time.deltaTime;
            if(waitTime <= 0f)
            {
                // if I'm user controlled then go to manual take throw
                Machine.ChangeState<AutomaticTakeThrow>();

                // I commented this part out simply for uniformity
                // Every setpiece is taken by the AI
                // This is an example of how you can take over from the AI if
                // the team is user controlled
                //if (Owner.IsUserControlled == true)
                //    Machine.ChangeState<ManualTakeThrow>();
                //else
                //    Machine.ChangeState<AutomaticTakeThrow>();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset
            Owner.Animator.SetBool("IsDoubleHandThrow", false);
            Owner.Animator.ResetTrigger("ThrowBall");
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
