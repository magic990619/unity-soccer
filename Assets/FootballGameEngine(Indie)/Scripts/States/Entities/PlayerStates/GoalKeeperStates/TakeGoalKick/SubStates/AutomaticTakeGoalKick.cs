using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.SubStates
{
    public class AutomaticTakeGoalKick : BState
    {
        bool _isValidPlayerFound;
        bool _isTakeGoalKickAnimationTriggered;
        float _waitTime;

        Pass _pass;

        public override void Enter()
        {
            base.Enter();

            // set the wait time
            _waitTime = 0.5f * Random.value;

            // register to some events 
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;

            // set is take counter kick animation triggered
            _isTakeGoalKickAnimationTriggered = false;
            _isValidPlayerFound = false;
        }

        public override void Execute()
        {
            base.Execute();

            if (_isValidPlayerFound)
            {
                if (_isTakeGoalKickAnimationTriggered == false)
                {
                    // set is take counter kick animation triggered
                    _isTakeGoalKickAnimationTriggered = true;

                    // trigger animation
                    Owner.Animator.SetTrigger("TakeGoalKick");
                }
            }
            else
            {
                // decrement wait time
                _waitTime -= Time.deltaTime;

                // consider pass safety if we still have time to find player
                //todo::fix automatic take goalkick
                if (_waitTime <= 0f)
                    _isValidPlayerFound = Owner.CanPass(out _pass, false);
                else
                    _isValidPlayerFound = Owner.CanPass(out _pass);
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset the ball owner
            // Ball.Instance.Owner = null;

            // register to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;

            // reset trigger animation
            Owner.Animator.ResetTrigger("TakeGoalKick");
        }

        public override void OnAnimatorExecuteIK(int layerIndex)
        {
            base.OnAnimatorExecuteIK(layerIndex);

            // run this if counter kick animation has been triggered
            if (_isTakeGoalKickAnimationTriggered == true)
            {
                // get the weights
                float weightLeftHand = Owner.Animator.GetFloat("WeightHandLeft");
                float weightLeftFoot = Owner.Animator.GetFloat("WeightFootLeft");
                float weightRightHand = Owner.Animator.GetFloat("WeightHandRight");
                float weightRightFoot = Owner.Animator.GetFloat("WeightFootRight");

                // update the weights
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, weightLeftFoot);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, weightLeftHand);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, weightRightFoot);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weightRightHand);

                // set the positions
                Owner.Animator.SetIKPosition(AvatarIKGoal.LeftFoot, Ball.Instance.BallIkTargets.IkTargetBack.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, Ball.Instance.BallIkTargets.IkTargetBack.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.RightFoot, Ball.Instance.BallIkTargets.IkTargetBack.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, Ball.Instance.BallIkTargets.IkTargetBack.position);
            }
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            // enable ball physics
            Ball.Instance.EnablePhysics();

            // set the prev pass receiver
            Owner.PrevPassReceiver = Owner.PassReceiver;

            //make a normal pass to the player
            Owner.MakePass(_pass);

            // broad cast that I have taken goal kick
            ActionUtility.Invoke_Action(Owner.OnTakeGoalKick);

            //go to recover state
            Machine.ChangeState<RecoverFromKick>();
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
