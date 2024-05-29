using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TacklePlayer.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.SubStates
{
    public class ManualChase : BState
    {
        bool _updateLogic;

        float _distance;
        float _targetSpeed;
        float _timeToTarget;

        Vector3 _targetPosition;
        Vector3 _targetVelocity;
        Vector3 _targetFuturePosition;

        /// <summary>
        /// The steering target
        /// </summary>
        public Vector3 SteeringTarget { get; set; }

        public override void Enter()
        {
            base.Enter();

            // enable the user controlled icon
            Owner.PlayerControlInfoWidget.Root.SetActive(true);

            // set update logic
            _updateLogic = false;

            //get the steering target
            SteeringTarget = Ball.Instance.NormalizedPosition;

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(SteeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
        }

        public override void Execute()
        {
            base.Execute();

            // update logic
            if(_updateLogic)
            {
                // take control of ball
                TakeControlOfBall();

                //// calculate the right pos to steer to
                //_timeToTarget = Owner.TimeToTarget(Owner.Position, Ball.Instance.NormalizedPosition, Owner.ActualJogSpeed);
                //if (Ball.Instance.CurrentOwner == null)
                //{
                //    _targetPosition = Ball.Instance.Position;
                //    _targetSpeed = Ball.Instance.Rigidbody.velocity.magnitude;
                //    _targetVelocity = Ball.Instance.Rigidbody.velocity;
                //}
                //else
                //{
                //    _targetPosition = Ball.Instance.CurrentOwner.Position;
                //    _targetSpeed = Ball.Instance.CurrentOwner.RPGMovement.Speed;
                //    _targetVelocity = Ball.Instance.CurrentOwner.RPGMovement.MovementDirection;
                //}

                //_distance = _targetSpeed * _timeToTarget * 0.5f;
                //_targetFuturePosition = _targetPosition + _targetVelocity.normalized * _distance;

                //get the steering target
                SteeringTarget = Ball.Instance.NormalizedPosition;// _targetFuturePosition;

                //set the steering to on
                Owner.RPGMovement.SetMoveTarget(SteeringTarget);
                Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
            }
            else
            {
                // listen to move commands
                float horizontalRot = ControlFreak2.CF2Input.GetAxisRaw("Horizontal");
                float verticalRot = -ControlFreak2.CF2Input.GetAxisRaw("Vertical");

                //calculate the direction to rotate to
                Vector3 input = new Vector3(verticalRot, 0f, horizontalRot);

                // calculate camera relative direction to move:
                Vector3 Movement = input;

                //process if any key down
                if (input == Vector3.zero)
                {
                    if (Owner.RPGMovement.Steer == true)
                        Owner.RPGMovement.SetSteeringOff();

                    if (Owner.RPGMovement.Track == true)
                        Owner.RPGMovement.SetTrackingOff();
                }
                else
                {
                    // set the movement
                    Vector3 moveDirection = Movement == Vector3.zero ? Vector3.zero : Owner.transform.forward;
                    Owner.RPGMovement.SetMoveDirection(moveDirection);
                    Owner.RPGMovement.SetRotateFaceDirection(Movement);

                    // check sprinting
                    bool isSprinting = ControlFreak2.CF2Input.GetButton("Sprint");
                    Owner.RPGMovement.Speed = isSprinting == true ? Owner.ActualSprintSpeed : Owner.ActualJogSpeed;

                    // set the steering to on
                    if (Owner.RPGMovement.Steer == false)
                        Owner.RPGMovement.SetSteeringOn();

                    if (Owner.RPGMovement.Track == false)
                        Owner.RPGMovement.SetTrackingOn();

                    // update animator
                    if (isSprinting == true)
                        Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f * Time.deltaTime);
                    else
                        Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f * Time.deltaTime);

                    // take control of ball
                    TakeControlOfBall();
                }
            }

            // listen to key events
            if(ControlFreak2.CF2Input.GetButton("ShortPass/Press"))
            {
                // set update logic
                if(_updateLogic == false)
                    _updateLogic = true;

                // check sprinting
                bool isSprinting = ControlFreak2.CF2Input.GetButton("Sprint");
                Owner.RPGMovement.Speed = isSprinting == true ? Owner.ActualSprintSpeed : Owner.ActualJogSpeed;

                // set steering
                if (Owner.RPGMovement.Steer == false)
                    Owner.RPGMovement.SetSteeringOn();
                if(Owner.RPGMovement.Track == false)
                    Owner.RPGMovement.SetTrackingOn();

                // update animator
                if (isSprinting == true)
                    Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f * Time.deltaTime);
                else
                    Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f * Time.deltaTime);
            }
            else if(ControlFreak2.CF2Input.GetButtonUp("ShortPass/Press"))
            {
                // set update logic
                _updateLogic = false;

                // set steering
                Owner.RPGMovement.SetSteeringOff();
                Owner.RPGMovement.SetTrackingOff();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // disable the user controlled icon
            Owner.PlayerControlInfoWidget.Root.SetActive(false);

            //set the steering to on
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            // reset the animator
            Owner.Animator.SetFloat("Forward", 0f);
        }

        public void TakeControlOfBall()
        {
            if (Ball.Instance.CurrentOwner != null
                    && Ball.Instance.CurrentOwner.IsBallWithinControllableDistance()
                    && Owner.IsBallWithinControllableDistance())
            {
                //tackle player
                SuperMachine.ChangeState<TackleMainState>();
            }
            else if (Owner.IsBallWithinControllableDistance())
            {
                // control ball
                SuperMachine.ChangeState<ControlBallMainState>();
            }
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
