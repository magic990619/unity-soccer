using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeThrowIn.SubStates
{
    /// <summary>
    /// Player logic to autiomatically take a throw-in
    /// </summary>
    public class AutomaticTakeThrow : BState
    {
        bool _canControlBallPosition;
        bool _hasTriggeredThrowBall;
        bool _isBallFarEnough;
        bool _isRotatingToFaceTarget;

        float waitTime;

        Pass _pass;

        public override void Enter()
        {
            base.Enter();

            // set some initial data
            _canControlBallPosition = true;
            _hasTriggeredThrowBall = false;
            _isRotatingToFaceTarget = false;

            // reset time
            waitTime = Random.value * 3f;

            // register to interact with ball event
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;
        }

        public override void Execute()
        {
            base.Execute();

            if(_isRotatingToFaceTarget)
            {
                // check the current angle
                bool isTArgetInFrontOfMe = Vector3.Angle(Owner.transform.forward, _pass.ToPosition - Owner.Position) <= 5f;
                if(isTArgetInFrontOfMe == true)
                {
                    Owner.RPGMovement.SetTrackingOff();
                    Owner.Animator.SetBool("CanContinueWithThrow", true);
                }
            }

            // decrement time
            waitTime -= Time.deltaTime;
            if (waitTime <= 0f 
                && _hasTriggeredThrowBall == false)
            {
                _hasTriggeredThrowBall = true;

                // check if I can make a throw
                bool canIMakeThrow = Owner.CanMakeThrowIn(false, Owner.transform.forward, out _pass);

                // if I can't make  throw, just find a random player
                // to throw to
                if(canIMakeThrow == false)
                {
                    // pass type is short pass
                    PassTypesEnum passType = PassTypesEnum.Long;

                    //get a player to pass to
                    Player receiver = Owner.GetTeamMemberClosestToPosition(Owner.Position);
                    Vector3 kickTarget = receiver.Position;

                    //find the power to target
                    float power = Owner.FindPower(Ball.Instance.NormalizedPosition,
                        kickTarget,
                        Owner.BallLongPassArriveVelocity,
                        0f);

                    //clamp the power
                    power = Mathf.Clamp(power, 0f, Owner.ActualPower);
                    float ballTime = Owner.TimeToTarget(Ball.Instance.NormalizedPosition,
                        receiver.Position,
                        power);

                    // create pass object
                    _pass = new Pass()
                    {
                        BallTimeToTarget = ballTime,
                        KickPower = power,

                        PassType = passType,

                        FromPosition = Owner.Position,
                        ToPosition = kickTarget,

                        Receiver = receiver
                    };
                }

                // check if target is intront of me
                bool isTargetInFronOfMe = Vector3.Angle(Owner.transform.forward, _pass.ToPosition - Owner.Position) <= 15f;
                if(isTargetInFronOfMe == true)
                {
                    Owner.Animator.SetBool("CanContinueWithThrow", true);
                }
                else
                {
                    // set rotate to face target
                    _isRotatingToFaceTarget = true;

                    // set the steering
                    Owner.RPGMovement.SetTrackingOn();
                    Owner.RPGMovement.SetRotateFacePosition(_pass.ToPosition);
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
            if(_hasTriggeredThrowBall == true)
            {
                _isBallFarEnough = Vector3.Distance(Owner.Position, Ball.Instance.Position) >= 5f;
                if(_isBallFarEnough == true)
                {
                    ////broadcast that I have taken kick-off
                    ActionUtility.Invoke_Action(Owner.OnTakeThrowIn);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset some stuff
            _hasTriggeredThrowBall = false;

            // reset the animator
            Owner.Animator.SetBool("CanContinueWithThrow", false);

            // deregister to interact with ball event
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;
        }

        void Instance_OnInstructedToInteractWithBall()
        {
            // unset some stuff
            _canControlBallPosition = false;
            Ball.Instance.Rigidbody.isKinematic = false;

            // make pass
            Owner.MakePass(_pass);

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
