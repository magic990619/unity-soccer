using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.PutBallBackIntoPlay.SubStates
{
    public class TakeCounterKick : BState
    {
        bool _isValidPlayerFound;
        bool _isTakeCounterKickAnimationTriggered;
        float _waitTime;

        bool _isMakePassTriggered;
        bool _isRotatingToFaceTarget;

        Pass _pass;
        Transform _handTarget;

        public Pass Pass { get => _pass; set => _pass = value; }
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // set initial data
            _isMakePassTriggered = false;
            _isRotatingToFaceTarget = false;

            // set the wait time
            _waitTime = 0.5f * Random.value;

            // register to some events 
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;
        }

        public override void Execute()
        {
            base.Execute();

            if (_isValidPlayerFound == true)
            {
                Owner.RPGMovement.SetRotateFacePosition(_pass.ToPosition);
                Owner.RPGMovement.SetMoveTarget(_pass.ToPosition);

                // if we are rotating to face target, wait until we have faced target and trigger animations
                if (_isRotatingToFaceTarget == true)
                {
                    // check if ball is now infront of me
                    bool isPositionInFrontOfMe = Owner.IsInfrontOfPlayer(_pass.ToPosition);
                    if (isPositionInFrontOfMe == true)
                    {
                        _isRotatingToFaceTarget = false;
                        DecideWhichAnimationToPlay();
                    }
                }
                else if (_isMakePassTriggered == false)
                {
                    // My Added
                    _isRotatingToFaceTarget = true;
                    // update the ball position to be the position on the hand
                    Ball.Instance.Position = _handTarget.position;
                    Ball.Instance.transform.rotation = _handTarget.rotation;
                }
            }
            else
            {
                // decrement wait time
                _waitTime -= Time.deltaTime;
                
                //todo::fix take counter-kick logic
                // consider pass safety if we still have time to find player
                if (_waitTime <= 0f)
                    _isValidPlayerFound = Owner.CanPass(out _pass, false);
                else
                    _isValidPlayerFound = Owner.CanPass(out _pass);
                
                // if we have a pass then make sure the target is infront of me first
                if(_pass != null)
                {
                    // decide to rotate
                    bool isPositionInFrontOfMe = Owner.IsInfrontOfPlayer(_pass.ToPosition);
                    if (isPositionInFrontOfMe == true)
                    {
                        DecideWhichAnimationToPlay();
                    }
                    else
                    {
                        // set it's rotating to face targer
                        _isRotatingToFaceTarget = true;

                        // set steering
                        Owner.RPGMovement.SetRotateFacePosition(_pass.ToPosition);
                        Owner.RPGMovement.SetTrackingOn();
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // register to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;

            // reset some stuff
            Owner.HasBall = false;

            // reset trigger animation
            Owner.Animator.ResetTrigger("BelowHandThrow");
            Owner.Animator.ResetTrigger("DropBallKick");
            Owner.Animator.ResetTrigger("OverHandThrow");
        }

        private void DecideWhichAnimationToPlay()
        {
            // decide which animation to play
            if (_pass.PassType == PassTypesEnum.Long)
            {
                // get the distance first
                float distanceToTarget = Vector3.Distance(Owner.Position, _pass.ToPosition);

                // set the hand
                _handTarget = distanceToTarget <= 30f ? Owner.BallRightHand : Owner.BallLeftHand;

                // do a pass if the pass is too near
                string animation = distanceToTarget <= 30f ? "OverHandThrow" : "DropBallKick";

                // trigger the animation
                Owner.Animator.SetTrigger(animation);
            }
            else if (_pass.PassType == PassTypesEnum.Short)
            {
                // set the hand
                _handTarget = Owner.BallRightHand;

                // do a pass if the pass is too near
                string animation = "BelowHandThrow";

                // trigger the animation
                Owner.Animator.SetTrigger(animation);
            }
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            // set is take counter kick animation triggered
            _isMakePassTriggered = true;

            // enable ball physics
            Ball.Instance.EnablePhysics();
            Ball.Instance.transform.SetParent(null);

            //make a normal pass to the player
            Owner.MakePass(_pass);

            //go to recover state
            Machine.ChangeState<RecoverFromKick>();

            // raise the put ball back into playe event
            ActionUtility.Invoke_Action(Owner.OnPutBallBackIntoPlay);
        }
    }
}
