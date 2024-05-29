using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TacklePlayer.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.SubStates
{
    public class AutomaticChase : BState
    {
        bool _isBewteenBallAndTeamGoal;
        bool _shouldSprint;

        float _distance;
        float _targetSpeed;
        float _timeToTarget;
        float _timeOfTargetToPoint;

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

            //get the steering target
            SteeringTarget = Ball.Instance.NormalizedPosition;

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(SteeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
        }

        public override void Execute()
        {
            base.Execute();

            //check if ball is within control distance
            if (Ball.Instance.CurrentOwner != null
                && Ball.Instance.CurrentOwner.IsBallWithinControllableDistance()
                && Owner.IsBallTacklable())
            {
                //tackle player
                SuperMachine.ChangeState<TackleMainState>();
            }
            else if (Owner.IsBallContrallable())
            {
                // control ball
                SuperMachine.ChangeState<ControlBallMainState>();
            } 

            // calculate the right pos to steer to
            _timeToTarget = Owner.TimeToTarget(Owner.Position, Ball.Instance.NormalizedPosition, Owner.ActualJogSpeed);
            if (Ball.Instance.CurrentOwner == null) 
            {
                _targetPosition = Ball.Instance.Position;
                _targetSpeed = Ball.Instance.Rigidbody.velocity.magnitude;
                _targetVelocity = Ball.Instance.Rigidbody.velocity;
            }
            else 
            {
                _targetPosition = Ball.Instance.CurrentOwner.Position;
                _targetSpeed = Ball.Instance.CurrentOwner.RPGMovement.Speed;
                _targetVelocity = Ball.Instance.CurrentOwner.RPGMovement.MovementDirection;
            }

            _distance = _targetSpeed * _timeToTarget * 0.5f;
            _targetFuturePosition = _targetPosition + _targetVelocity.normalized * _distance;

            //get the steering target
            SteeringTarget = _targetFuturePosition;

            // should I sprint or wat
            // sprint if ball is now ahead of me towards goal
            _isBewteenBallAndTeamGoal = Owner.IsPositionBetweenTwoPoints(Ball.Instance.Position, 
                Owner.TeamGoal.Position, 
                Owner.Position);

            if(_isBewteenBallAndTeamGoal)
            {
                // sprint if ball can reach point before me
                _timeOfTargetToPoint = Owner.TimeToTarget(_targetPosition, _targetFuturePosition, _targetSpeed);

                if (_timeToTarget < _timeOfTargetToPoint)
                    _shouldSprint = true;
                else
                    _shouldSprint = false;
            }
            else
            {
                _shouldSprint = true;
            }

            if(_shouldSprint == true)
            {
                // set speed to sprint
                Owner.RPGMovement.Speed = Owner.ActualSprintSpeed; 
                Owner.Animator.SetFloat("Forward", Owner.SprintAnimatorValue, 0.1f, 0.1f * Time.deltaTime);
            }
            else
            {
                // set speed to sprint
                Owner.RPGMovement.Speed = Owner.ActualJogSpeed;
                Owner.Animator.SetFloat("Forward", 0.5f, 0.1f, 0.1f * Time.deltaTime);
            }

            //set the steering to on
            Owner.RPGMovement.SetMoveTarget(SteeringTarget);
            Owner.RPGMovement.SetRotateFacePosition(SteeringTarget);
        }

        public override void Exit()
        {
            base.Exit();

            //set the steering to on
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            // reset the animator
            Owner.Animator.SetFloat("Forward", 0f);
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
