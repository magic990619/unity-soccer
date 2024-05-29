using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeThrowIn.SubStates
{
    public class ManualTakeThrow : BState
    {
        bool _canControlBallPosition;
        bool _hasTriggeredThrowBall;
        bool _isBallFarEnough;

        float _waitTime;

        Pass _pass;

        public override void Enter()
        {
            base.Enter();

            // set some initial data
            _canControlBallPosition = true;
            _hasTriggeredThrowBall = false;
            _waitTime = 15f;

            // register to interact with ball event
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;
        }

        public override void Execute()
        {
            base.Execute();

            if (_hasTriggeredThrowBall == false)
            {
                // decrement wait time
                _waitTime -= Time.deltaTime;

                // check some conditions
                bool isTimeExhausted = _waitTime <= 0;
                bool isPassButtonPressed = ControlFreak2.CF2Input.GetButtonDown("ShortPass/Press");

                // trigger throw in ball if one of the above conditions is true
                if (isPassButtonPressed || isTimeExhausted)
                {
                    // set has pressed pass button
                    _hasTriggeredThrowBall = true;
                    Owner.Animator.SetBool("CanContinueWithThrow", true);
                }
                else
                {
                    // find the rotation
                    float turnSpeed = 1.5f * Time.deltaTime;
                    float rotate = ControlFreak2.CF2Input.GetAxis("Horizontal") * turnSpeed;

                    // calculate the direction
                    Vector3 dir = Camera.main.transform.forward * rotate;

                    // rotate player depending on user input
                    Owner.RPGMovement.Rotate(dir);
                }
            }

            // update ball position
            if (_canControlBallPosition == true)
            {
                // calculate the new ball position
                Vector3 ballPosition = Vector3.Lerp(Owner.Animator.GetBoneTransform(HumanBodyBones.LeftHand).position,
                    Owner.Animator.GetBoneTransform(HumanBodyBones.RightHand).position,
                    0.5f);

                // set ball pos
                Ball.Instance.Position = ballPosition;
            }
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // check if I should raise the taken throw-in event
            if (_hasTriggeredThrowBall == true)
            {
                _isBallFarEnough = Vector3.Distance(Owner.Position, Ball.Instance.Position) >= 5f;
                if (_isBallFarEnough == true)
                {
                    ////broadcast that I have taken kick-off
                    ActionUtility.Invoke_Action(Owner.OnTakeThrowIn);

                    //go to home state
                    SuperMachine.ChangeState<GoToHomeMainState>();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset some stuff
            _hasTriggeredThrowBall = false;
            Owner.Animator.SetBool("CanContinueWithThrow", false);
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            // unset some stuff
            _canControlBallPosition = false;

            Ball.Instance.Rigidbody.isKinematic = false;
            Owner.PassType = PassTypesEnum.Long;

            // check if I can make a throw
            bool canIMakeThrow = Owner.CanMakeThrowIn(true, Owner.transform.forward, out _pass);

            // make throwi-in if possible
            if(canIMakeThrow)
            {
                //make a normal pass to the player
                Owner.MakePass(_pass);
            }
            else
            {
                // find target
                Owner.PassType = PassTypesEnum.Long;
                Vector3 kickTarget = Owner.transform.position + Owner.transform.forward * 15f;
                Player receiver = Owner.GetClosestTeamPlayerToPosition(kickTarget);
                float time = Vector3.Distance(Owner.Position, kickTarget) / Owner.BallLongPassArriveVelocity;

                //make a normal pass to the player
                Owner.MakePass(Ball.Instance.Position,
                    kickTarget,
                    receiver,
                    Owner.BallLongPassArriveVelocity,
                    time);
            }

            // reset can throw ball
            Owner.Animator.SetBool("CanContinueWithThrow", false);
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
