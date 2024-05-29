using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ChaseBall.SubStates
{
    public class GetBall : BState
    {
        bool hasGrabbedBall;

        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // reset has grabbed the ball
            hasGrabbedBall = false;

            // register to some events 
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;

            // if ball has owner then tackle owner and then get the ball
            if (Ball.Instance.CurrentOwner != null)
                Ball.Instance.CurrentOwner.Invoke_OnTackled();

            // set ball 
            Ball.Instance.DisablePhysics();
            Ball.Instance.transform.rotation = Owner.Rotation;

            // set the owner
            Owner.HasBall = true;

            // set the animators
            Owner.Animator.SetFloat("Forward", 0f);
            Owner.Animator.SetFloat("Turn", 0f);
            Owner.Animator.SetTrigger("ScoopBall");
        }

        public override void Execute()
        {
            base.Execute();

            // attach ball to bone
            if (hasGrabbedBall == true)
            {
                //go to home after state
                if (Owner.Animator.GetCurrentAnimatorStateInfo(2).IsName("New State"))
                    SuperMachine.ChangeState<IdleMainState>();

                // update ball position to hand position
                Ball.Instance.Position = Owner.BallRightHand.position;
                Ball.Instance.transform.rotation = Owner.BallRightHand.rotation;
            }
        }

        public override void Exit()
        {
            base.Exit();

            // register to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;
        }

        public override void OnAnimatorExecuteIK(int layerIndex)
        {
            base.OnAnimatorExecuteIK(layerIndex);

            if (hasGrabbedBall == false)
            {
                // get the hand weights
                float leftHandWeight = Owner.Animator.GetFloat("WeightHandLeft");
                float rightHandWeight = Owner.Animator.GetFloat("WeightHandRight");

                // move the hand towards the ball
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

                Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, Ball.Instance.BallIkTargets.IkTargetLeft.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, Ball.Instance.BallIkTargets.IkTargetRight.position);
            }
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            // set has grabbed ball
            hasGrabbedBall = true;
        }
    }
}
