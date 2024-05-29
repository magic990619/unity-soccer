using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ProtectGoal.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.SubStates
{
    public class MoveToInterceptPoint : BState
    {
        bool _hasGrabbedBall;
        bool _isCountingDown;

        float _animatorHeightValue;
        float _waitTime;
        float _turn;

        Vector3 _dirOfPlayerToInitialBallInterceptPoint;
        Vector3 _initialBallInterceptPoint;
        Vector3 _relativeBallPositionAtInitialBallInterceptPoint;
        Vector3 _relativeDirectionOfPlayerToBallInterceptPoint;

        public float AnimatorHeightValue { get => _animatorHeightValue; set => _animatorHeightValue = value; }
        public float Turn { get => _turn; set => _turn = value; }
        public Vector3 InitialBallInterceptPoint { get => _initialBallInterceptPoint; set => _initialBallInterceptPoint = value; }
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // reset wait time
            _waitTime = 0.5f;

            // reset has grabbed the ball
            _hasGrabbedBall = false;
            _isCountingDown = false;
            Owner.HasBall = false;

            // register to some events 
            Owner.OnInstructedToInteractWithBall += Instance_OnInstructedToInteractWithBall;

            // calculate animator values
            _animatorHeightValue = Mathf.Clamp(_relativeBallPositionAtInitialBallInterceptPoint.y / (0.75f * (Owner.ActuaJumpHeight + Owner.Height)), 0f, 1f);
            _relativeDirectionOfPlayerToBallInterceptPoint = Owner.transform.InverseTransformDirection(_dirOfPlayerToInitialBallInterceptPoint);
            _turn = _relativeDirectionOfPlayerToBallInterceptPoint.x > 0 ? 1 : -1;

            // set the animator
            Owner.Animator.SetFloat("Turn", _turn);
        }

        public override void Execute()
        {
            base.Execute();

            // move towards target
            Owner.Position = Vector3.MoveTowards(Owner.Position, _initialBallInterceptPoint, Owner.TendGoalSpeed * Time.deltaTime);

            // if I'm at target then stop
            if (Owner.HasBall == false)
            {
                // calculate if at target
                float distance = Vector3.Distance(Owner.Position, _initialBallInterceptPoint);
                bool isAtTarget = distance <= 0.3;

                if (isAtTarget == true && _isCountingDown == false)
                {
                    // set is counting down
                    _isCountingDown = true;

                    // reset the animator
                    Owner.Animator.SetFloat("Turn", 0);
                }
                else if (_isCountingDown == true)
                {
                    // decrement wait time
                    _waitTime -= Time.deltaTime;

                    // if we have exhausted time then go to protect goal main state
                    if (_waitTime <= 0)
                        SuperMachine.ChangeState<ProtectGoalMainState>();
                }
            }

            // here we try to catch the ball depending on it's height etc
            if (Owner.HasBall == false && Owner.IsBallWithinControllableDistance())
            {
                // stop steering
                if (Owner.RPGMovement.Steer == true)
                    Owner.RPGMovement.SetSteeringOff();

                // set ball 
                Ball.Instance.DisablePhysics();
                Ball.Instance.transform.rotation = Owner.Rotation;

                // set the owner
                Owner.HasBall = true;

                // set the animation parameters
                Owner.Animator.SetFloat("Height", _animatorHeightValue);
                Owner.Animator.SetTrigger("CatchBall");
            }

            // attach ball to bone
            if (_hasGrabbedBall == true)
            {
                //go to home after state
                if (Owner.Animator.GetCurrentAnimatorStateInfo(1).IsName("New State"))
                    SuperMachine.ChangeState<IdleMainState>();

                // update ball position to hand position
                Ball.Instance.Position = Owner.BallRightHand.position;
                Ball.Instance.transform.rotation = Owner.BallRightHand.rotation;
            }
        }

        public override void Exit()
        {
            base.Exit();

            // deregister to some events 
            Owner.OnInstructedToInteractWithBall -= Instance_OnInstructedToInteractWithBall;

            // reset steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
        }

        public override void OnAnimatorExecuteIK(int layerIndex)
        {
            base.OnAnimatorExecuteIK(layerIndex);

            if(_hasGrabbedBall == false)
            {
                // get hand weights from animation
                float leftHandIkWeight = Owner.Animator.GetFloat("WeightHandLeft");
                float rightHandIkWeight = Owner.Animator.GetFloat("WeightHandRight");

                // set ik hand weights
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandIkWeight);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandIkWeight);

                // set ik position
                Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, Ball.Instance.BallIkTargets.IkTargetLeft.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, Ball.Instance.BallIkTargets.IkTargetRight.position);
            }
        }

        public void Init(Vector3 dirOfPlayerToInitialBallInterceptPoint, Vector3 initialBallInterceptPoint, Vector3 relativeBallPositionAtInitialBallInterceptPoint)
        {
            _dirOfPlayerToInitialBallInterceptPoint = dirOfPlayerToInitialBallInterceptPoint;
            _initialBallInterceptPoint = initialBallInterceptPoint;
            _relativeBallPositionAtInitialBallInterceptPoint = relativeBallPositionAtInitialBallInterceptPoint;
        }

        private void Instance_OnInstructedToInteractWithBall()
        {
            // set has grabbed ball
            _hasGrabbedBall = true;
            Owner.HasBall = true;
        }
    }
}
