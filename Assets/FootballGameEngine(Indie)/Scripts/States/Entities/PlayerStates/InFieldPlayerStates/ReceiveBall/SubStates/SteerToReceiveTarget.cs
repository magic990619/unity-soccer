using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.SubStates
{
    /// <summary>
    /// Player steers to pass target. He switches to wait
    /// for ball if he reaches the target
    /// </summary>
    public class SteerToReceiveTarget : BState
    {
        bool _isNoLongeSteeringToTarget;
        bool _isSprinting;

        float _ballTime;
        float _distance;
        float _speed;
        float _targetSpeed;
        float _timeToTarget;

        Vector3 _steeringTarget;
        Vector3 _targetFuturePosition;
        Vector3 _targetPosition;
        Vector3 _targetVelocity;

        public float BallTime { get => _ballTime; set => _ballTime = value; }

        public Vector3 SteeringTarget { get => _steeringTarget; set => _steeringTarget = value; }


        public override void Enter()
        {
            base.Enter();

            // calculate our speed 
            _timeToTarget = Owner.TimeToTarget(Owner.Position, _steeringTarget, Owner.ActualJogSpeed);

            // check if I should sprint
            _isSprinting = _ballTime <= _timeToTarget;

            // find the speed
            if (_isSprinting == true)
            {
                // find the speed to reach the spot before the ball
                _speed = Vector3.Distance(Owner.Position, _steeringTarget) / (_ballTime * Random.value);
                _speed = Mathf.Clamp(_speed, Owner.ActualJogSpeed, Owner.ActualSprintSpeed);

                // Check if I can still reach point before ball, if not then I have to steer to
                // the ball estimate position
                _timeToTarget = Owner.TimeToTarget(Owner.Position, _steeringTarget, _speed);
                _isNoLongeSteeringToTarget = _ballTime < _timeToTarget;
                
                // set the speed
                if (_isNoLongeSteeringToTarget == false)
                    Owner.RPGMovement.Speed = _speed;
            }

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(_steeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();

            // set the animator
            Owner.Animator.SetTrigger("Move");
        }

        public override void Execute()
        {
            base.Execute();

#if UNITY_EDITOR
            Debug.DrawLine(Owner.Position + Vector3.up, _steeringTarget, Color.blue);
#endif

            //check if now at target and switch to wait for ball
            if (Owner.IsAtTarget(_steeringTarget))
                Machine.ChangeState<WaitForBallAtReceiveTarget>();

            // update the animator
            if(_isSprinting)
                Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f);
            else
                Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // recalculate steering target
            if (_isNoLongeSteeringToTarget == true)
            {
                // get the data
                _targetPosition = Ball.Instance.Position;
                _targetSpeed = Ball.Instance.Rigidbody.velocity.magnitude;
                _targetVelocity = Ball.Instance.Rigidbody.velocity;
                _timeToTarget = Owner.TimeToTarget(Owner.Position, Ball.Instance.NormalizedPosition, _speed);

                // find the future position
                _distance = _targetSpeed * _timeToTarget * 0.5f;
                _targetFuturePosition = _targetPosition + _targetVelocity.normalized * _distance;

                //update the steering target
                _steeringTarget = _targetFuturePosition;

                //set the steering to on
                Owner.RPGMovement.SetMoveTarget(_steeringTarget);
                Owner.RPGMovement.SetRotateFacePosition(_steeringTarget);
            }
            else
            {
                // check if I can still reach the ball else refactor my speed
                if (_isSprinting == true)
                {
                    // check if I should steer to the new target
                    _timeToTarget = Owner.TimeToTarget(Owner.Position, _steeringTarget, _speed);
                    _isNoLongeSteeringToTarget = _ballTime < _timeToTarget;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            //stop any steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            // reset the animator
            Owner.Animator.SetFloat("Forward", 0f);
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
