using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Objects;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.SubStates
{
    public class AutomaticControl : BState
    {
        bool _isSprinting;

        int _layerMask;
        int _maxNumOfTries;

        float _lengthPitch = 90f;
        float _maxPassTime;

        Range _rangePassTime = new Range(0.5f, 5f);

        Pass _pass;
        RaycastHit _hit;
        Shot _shot;

        public override void Initialize()
        {
            base.Initialize();

            _layerMask = 1 << LayerMask.NameToLayer("Player");
        }

        public override void Enter()
        {
            base.Enter();

            // set speed
            Owner.RPGMovement.Speed = Owner.ActualJogSpeed;

            //set some prerequisites
            _isSprinting = false;
            _maxNumOfTries = Random.Range(5, 10);
            _maxPassTime = Random.Range(_rangePassTime.Min, _rangePassTime.Max);

            //set the steering
            Owner.RPGMovement.SetMoveTarget(Owner.OppGoal.transform.position);
            Owner.RPGMovement.SetRotateFacePosition(Owner.OppGoal.transform.position);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();

            // set the animator
            Owner.Animator.SetTrigger("Move");
        }

        public override void Execute()
        {
            base.Execute();

            //decrement time
            if(_maxPassTime > 0)
                _maxPassTime -= Time.deltaTime;

            // update animator
            if(_isSprinting == true)
                Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f * Time.deltaTime);
            else
                Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f * Time.deltaTime);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            //set the steering
            Owner.RPGMovement.SetMoveTarget(Owner.OppGoal.transform.position);
            Owner.RPGMovement.SetRotateFacePosition(Owner.OppGoal.transform.position);

            // check if I can sprint, I can't sprint if there is a player 
            // infront of me
            Vector3 p1 = Owner.Position + Owner.RPGMovement.CharacterController.center + Vector3.up * -Owner.RPGMovement.CharacterController.height * 0.5f;
            Vector3 p2 = Owner.Position + Vector3.up * Owner.RPGMovement.CharacterController.height;

            // cast to check if we will hit someone
            if(Physics.CapsuleCast(p1, p2, Random.value * 5f * Owner.Radius, Owner.RPGMovement.MovementDirection, out _hit, 10f, _layerMask))
            {
                // check if I'm facing movement direction
                float dot = Vector3.Dot(Owner.RPGMovement.MovementDirection.normalized, Owner.transform.forward);
               
                // sprint if facing movement direction
                if(dot > 0.75f)
                {
                    // if I'm not sprinting then check with my tactics
                    if(_isSprinting == false)
                    {
                        // calculate how far the ball has moved upfiled
                        Vector3 ballGoalLocalPosition = Owner.TeamGoal.transform.InverseTransformPoint(Ball.Instance.transform.position);
                        float ballMoveUpfieldRAtio = Mathf.Clamp01((ballGoalLocalPosition.z / _lengthPitch));

                        // check if I should sprint 
                        bool _shouldISprint = Random.value <= (Owner.ForwardRunFrequency + ballMoveUpfieldRAtio) / 2f;

                        // if I can sprint the sprint
                        if(_shouldISprint == true)
                        {
                            _isSprinting = true;
                            Owner.RPGMovement.Speed = Owner.ActualSprintSpeed;
                        }
                        else
                        {
                            _isSprinting = false;
                            Owner.RPGMovement.Speed = Owner.ActualJogSpeed;
                        }
                    }
                }
            }

            // if within shoot distance
            bool isWithinShootDistance = Owner.IsPositionWithinDistance(Owner.OppGoal.Position, Owner.Position,
                                                                Owner.DistanceShotMaxValid);

            // If I'm within range, then force me to shoot at one point
            if (isWithinShootDistance)
            {
                if (Owner.CanScore(out _shot))
                {
                    // set the data
                    Owner.KickDecision = KickDecisions.Shot;
                    Owner.Shot = _shot;

                    //go to kick-ball state
                    SuperMachine.ChangeState<KickBallMainState>();
                }
                else
                {
                    // decrement max num of tries
                    if (_maxNumOfTries > 0)
                        --_maxNumOfTries;
                }

                // if max num of tries have been exhausted the shoot
                if (_maxNumOfTries <= 0)
                {
                    if (Owner.CanScore(out _shot, true, false))
                    {
                        // set the data
                        Owner.KickDecision = KickDecisions.Shot;
                        Owner.Shot = _shot;

                        //go to kick-ball state
                        SuperMachine.ChangeState<KickBallMainState>();
                    }
                }
            }
            else if (_maxPassTime <= 0 || Owner.IsThreatened())  //try passing if threatened or depleted wait time
            {
                // if I can pass, then pass the ball
                if (Owner.CanPass(out _pass))
                {
                    // set the data
                    Owner.KickDecision = KickDecisions.Pass;
                    Owner.Pass = _pass;

                    // tell the player to receive ball to go to idle state
                    Owner.Pass.Receiver.Invoke_OnInstructedToWait();

                    //go to kick-ball state
                    SuperMachine.ChangeState<KickBallMainState>();
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            //stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            // set the animator
            Owner.Animator.ResetTrigger("Move");
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
