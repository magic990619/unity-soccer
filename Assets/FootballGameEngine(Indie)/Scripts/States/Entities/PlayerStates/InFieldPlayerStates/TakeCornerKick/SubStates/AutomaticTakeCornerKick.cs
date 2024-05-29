using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeCornerKick.SubStates
{
    public class AutomaticTakeCornerKick : BState
    {
        bool _isValidPlayerFound;
        bool _isTakeGoalKickAnimationTriggered;
        float _waitTime;

        Pass _pass;

        public override void Enter()
        {
            base.Enter();

            // set the wait time
            _waitTime = 0.5f;

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

                    //// set the animator
                    bool isLongPass = Owner.PassType == PassTypesEnum.Long;
                    bool isShortPass = Owner.PassType == PassTypesEnum.Short;
                    Owner.Animator.SetBool("IsLongPass", isLongPass);
                    Owner.Animator.SetBool("IsShortPass", isShortPass);
                    Owner.Animator.SetTrigger("PassBall");
                }
            }
            else
            {
                // decrement wait time
                _waitTime -= Time.deltaTime;
                
                // consider pass safety if we still have time to find player
                if (_waitTime <= 0f)
                    _isValidPlayerFound = Owner.CanMakeCornerKick(out _pass);
                //else
                //    _isValidPlayerFound = Owner.CanPass();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset the ball owner
            Ball.Instance.CurrentOwner = null;

            // register to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;

            // reset animator
            Owner.Animator.SetBool("IsLongPass", false);
            Owner.Animator.SetBool("IsShortPass", false);
            Owner.Animator.ResetTrigger("PassBall");
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            // enable ball physics
            Ball.Instance.EnablePhysics();

            // set the prev pass receiver
            Owner.PrevPassReceiver = Owner.PassReceiver;

            //make a normal pass to the player
            Owner.MakePass(_pass);

            // broad cast that I have taken corner kick
            if (Owner.OnTakeCornerKick != null)
                Owner.OnTakeCornerKick.Invoke(_pass.BallTimeToTarget, _pass.ToPosition, _pass.Receiver);

            //go to recover state
            Machine.ChangeState<RecoverFromKick>();
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
