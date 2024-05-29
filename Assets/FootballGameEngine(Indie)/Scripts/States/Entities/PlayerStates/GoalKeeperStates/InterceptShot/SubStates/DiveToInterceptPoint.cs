using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.References;
using Assets.FootballGameEngine_Indie_.Scripts.Shared;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.SubStates
{
    public class DiveToInterceptPoint : BState
    {
        bool _hasGrabbedBall;
        bool _hasSavedBall;
        bool _isBallCatchable;

        float _animatorHeightValue;
        float _initDistOfBallToBallInterceptPoint;
        float _initHandWeight;
        float _playerDiveDistance;
        float _timeOfBallToInitialBallInterceptPoint;
        float _turn;
        float _waitTime;
        float _weightMultiplier;

        Vector3 _dirOfPlayerToInitialBallInterceptPoint;
        Vector3 _finalBallInterceptPoint;
        Vector3 _initialBallInterceptPoint;
        Vector3 _normalizedBallPosition;
        Vector3 _relativeBallPositionAtInitialBallInterceptPoint;
        Vector3 _relativeDirectionOfPlayerToBallInterceptPoint;

        Transform _leftHandTargetPosition;
        Transform _rightHandTargetPosition;

        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();
            
            // reset has grabbed the ball
            _hasGrabbedBall = false;
            _hasSavedBall = false;
            _initHandWeight = 1f;

            // set some data
            _waitTime = _timeOfBallToInitialBallInterceptPoint + 0.1f;

            // calculate player dive distance
            _playerDiveDistance = Mathf.Clamp(_dirOfPlayerToInitialBallInterceptPoint.magnitude, 0f, Owner.ActuaDiveDistance);

            // calculate if ball is catchable
            _isBallCatchable = CalculateIfBallIsCatchable(Owner.Shot.KickPower,
                _dirOfPlayerToInitialBallInterceptPoint.magnitude,
                _playerDiveDistance);

            //find the player intercept point
            _finalBallInterceptPoint = CalculateBallFinalInterceptPoint(_playerDiveDistance,
                                                                        _dirOfPlayerToInitialBallInterceptPoint);

            // calculate the speed to intercept point
            float rawDiveSpeed = Vector3.Distance(Owner.Position, _finalBallInterceptPoint) / _timeOfBallToInitialBallInterceptPoint;
            float diveSpeed = Mathf.Clamp(rawDiveSpeed, 0f, Owner.ActualDiveSpeed);

            // calculate animator values
            _animatorHeightValue = Mathf.Clamp(_relativeBallPositionAtInitialBallInterceptPoint.y / (0.75f * (Owner.ActuaJumpHeight + Owner.Height)), 0f, 1f);
            _relativeDirectionOfPlayerToBallInterceptPoint = Owner.transform.InverseTransformDirection(_dirOfPlayerToInitialBallInterceptPoint);
            _turn = _relativeDirectionOfPlayerToBallInterceptPoint.x > 0 ? 1 : -1;

            // set the hand ik targets
            CalculateHandIkTargets(_turn,
                Owner.BallReference,
                out _leftHandTargetPosition,
                out _rightHandTargetPosition);

            // set animation data
            Owner.Animator.SetBool("IsBallCatchable", _isBallCatchable);
            Owner.Animator.SetFloat("Height", _animatorHeightValue);
            Owner.Animator.SetFloat("Turn", _turn);
            Owner.Animator.SetTrigger("Dive");

        }

        public override void Execute()
        {
            base.Execute();

            // set ball reference position
            CalculateBallReferencePosition(Ball.Instance.Position, Owner.transform.rotation);

            // move towards target
            Owner.Position = Vector3.MoveTowards(Owner.Position, _finalBallInterceptPoint, Owner.RPGMovement.Speed * Time.deltaTime);

            // decrement wait time
            _waitTime -= Time.deltaTime;
            bool isBallInteractable = CalculateIfBallIsInteractable();
           
            // if we have exhausted time then go to exit
            // if ball is interactable, interact with it
            if (_waitTime <= 0f && _hasGrabbedBall == false && _hasSavedBall == false)
            {
                //set the animator to exit the dive state
                Owner.Animator.SetTrigger("Exit");
            }
            else if (isBallInteractable == true && _hasGrabbedBall == false && _hasSavedBall == false)
            {
                // if ball is catchabe then catch it else save it
                if (_isBallCatchable)
                {
                    // set has grabbed the ball
                    _hasGrabbedBall = true;
                    Owner.HasBall = true;

                    // set some data
                    Ball.Instance.DisablePhysics();
                    Ball.Instance.Trap();

                    // raise that I now have control of the ball
                    ActionUtility.Invoke_Action(Owner.OnGoalKeeperGainedControlOfBall);

                    //set the animator to exit the dive state
                    Owner.Animator.SetTrigger("Exit");
                }
                else
                {
                    // set has saved ball
                    _hasSavedBall = true;

                    //calculate the punch direction
                    Vector3 relativeDirectionOfPlayerToBallInterceptPoint = Owner.transform.InverseTransformDirection(_dirOfPlayerToInitialBallInterceptPoint);
                    Vector3 ballPunchDirection = Vector3.zero;

                    //detemine the punch direction
                    //todo::change punch direction...if the ball is to high the player punches up else he saves to the side
                    if (Mathf.Abs(relativeDirectionOfPlayerToBallInterceptPoint.x) > 0.1f)
                    {
                        //simply punch it to the side
                        ballPunchDirection = new Vector3(relativeDirectionOfPlayerToBallInterceptPoint.x, 0f, 0f);
                    }
                    else
                    {
                        //if it's less than my height then punch it infront of me else punch up
                        if (relativeDirectionOfPlayerToBallInterceptPoint.y <= Owner.Height)
                            ballPunchDirection = new Vector3(0f, 0f, 1f);
                        else
                            ballPunchDirection = new Vector3(0f, 1f, -1f);
                    }

                    //punch the ball
                    ballPunchDirection = Owner.transform.TransformDirection(ballPunchDirection);
                    Ball.Instance.KickInDirection(ballPunchDirection,
                        0.5f * Ball.Instance.Rigidbody.velocity.magnitude);

                    //set the animator to exit the dive state
                    Owner.Animator.SetTrigger("Exit");

                    //raise the punch ball event
                    Owner.Invoke_OnPunchBall();
                    Owner.Invoke_OnControlBall();
                }
            }

            // attach ball to bone
            if (_waitTime <= 0f && _hasGrabbedBall == false && _hasSavedBall == false)
            {
                //go to home after state
                if (Owner.Animator.GetCurrentAnimatorStateInfo(3).IsName("New State"))
                    SuperMachine.ChangeState<IdleMainState>();
            }
            else if (_hasGrabbedBall == true)
            {
                //go to home after state
                if (Owner.Animator.GetCurrentAnimatorStateInfo(3).IsName("New State"))
                    SuperMachine.ChangeState<IdleMainState>();
                
                // update ball position to hand position
                Ball.Instance.Position = Owner.BallRightHand.position;
                Ball.Instance.transform.rotation = Owner.BallRightHand.rotation;
            }
            else if (_hasSavedBall == true)
            {
                //go to home after state
                if (Owner.Animator.GetCurrentAnimatorStateInfo(3).IsName("New State"))
                    SuperMachine.ChangeState<IdleMainState>();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
        }

        public override void OnAnimatorExecuteIK(int layerIndex)
        {
            base.OnAnimatorExecuteIK(layerIndex);

            if (_waitTime <= 0f && _hasGrabbedBall == false && _hasSavedBall == false)
            {
                // get I.K hand weights
                float leftHandWeight = Owner.Animator.GetIKPositionWeight(AvatarIKGoal.LeftHand);
                float rightHandWeight = Owner.Animator.GetIKPositionWeight(AvatarIKGoal.RightHand);

                // process weights 
                leftHandWeight -= Time.deltaTime * 0.25f * 0.5f;
                rightHandWeight -= Time.deltaTime * 0.25f * 0.5f;

                // set ik position weight
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            }
            else if (_hasGrabbedBall == true || _hasSavedBall == true)
            {
                // decrement hand weight
                _initHandWeight -= Time.deltaTime * 0.25f;
               
                // set ik weights
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _initHandWeight);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, _initHandWeight);

                // set ik position
                Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, Owner.BallReference.BallIkTargets.IkTargetLeft.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, Owner.BallReference.BallIkTargets.IkTargetRight.position);
            }
            else if (_hasGrabbedBall == false || _hasSavedBall == false)
            {
                // declare the weight variables
                float leftHandWeight = 0f;
                float rightHandWeight = 0f;
                float lookAtWeight = 0f;

                // calculate the hand and look at weights
                CalculateHandAndLookWeights(out leftHandWeight,
                                            out rightHandWeight,
                                            out lookAtWeight);

                // set ik weights
                Owner.Animator.SetLookAtWeight(lookAtWeight);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
                Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);

                // set ik position
                Owner.Animator.SetLookAtPosition(Ball.Instance.Position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, Owner.BallReference.BallIkTargets.IkTargetLeft.position);
                Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, Owner.BallReference.BallIkTargets.IkTargetRight.position);
            }
        }

        public Vector3 CalculateBallFinalInterceptPoint(float maxMovementDistanceOfPlayer, Vector3 dirToInitialBallInterceptPoint)
        {
            //find the player intercept point
            Vector3 finalBallInterceptPoint = Owner.Position
                + dirToInitialBallInterceptPoint.normalized
                * maxMovementDistanceOfPlayer;

            // return result
            return finalBallInterceptPoint;
        }

        /// <summary>
        /// Set the ball reference
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        public void CalculateBallReferencePosition(Vector3 position, Quaternion rotation)
        {
            // set the ball reference position and rotation
            Owner.BallReference.transform.position = position;
            Owner.BallReference.transform.rotation = rotation;
            Owner.BallReference.transform.SetParent(null);
        }

        public void CalculateHandAndLookWeights(out float leftHandWeight, out float rightHandWeight, out float lookAtWeight)
        {
            //initialize the weight parameters
            leftHandWeight = 0f;
            rightHandWeight = 0f;
            lookAtWeight = 0f;

            //calculate the weight multiplier depending on the remaining distance to target
            _normalizedBallPosition = Ball.Instance.NormalizedPosition;

            //find the distance of ball to orthogonal point
            float distanceOfBallToTarget = Vector3.Distance(_normalizedBallPosition, _initialBallInterceptPoint);

            if (distanceOfBallToTarget <= 5f)
            {
                // calculate the weight depending on the traveled distance
                float rawWeightMultiplier = distanceOfBallToTarget / _initDistOfBallToBallInterceptPoint;
                if (rawWeightMultiplier <= 0.01f) rawWeightMultiplier = 0f;
                rawWeightMultiplier = 1 - rawWeightMultiplier;
                _weightMultiplier = Mathf.Clamp(rawWeightMultiplier, 0f, 1f);

                // set the weights
                leftHandWeight = _weightMultiplier;
                rightHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;
            }
        }

        public bool CalculateIfBallIsCatchable(float ballVelocity, float initPlayerDistanceToFinalBallInterceptPoint, float initPlayerDistanceToInitBallInterceptPoint)
        {
            return true;
            // if the distance between the initial ball-intercept-point and the final-ball-intercept-point is less than my reach
            // then check ball velocity to make sure we can catch the ball
            // else the ball is not catchable
            if (Mathf.Abs(initPlayerDistanceToInitBallInterceptPoint - initPlayerDistanceToFinalBallInterceptPoint) <= Owner.ActualReach)
            {
                // check if ball velocity is okay
                bool isBallVelocityWithinCatchableVelocity = ballVelocity <= Globals.MaxBallCatchableVelocity ? true : false;

                // if ball velocity is okay we can catch the ball

                // return result
                return isBallVelocityWithinCatchableVelocity == true ? true : false;
            }
            else
            {
                // return
                return false;
            }
        }

        public bool CalculateIfBallIsInteractable()
        {
            // check if ball is now within interactable/controllable distance 
            // and also if I'm within distance to catch the ball
            bool isBallWithinControllableDistance = Owner.IsBallWithinControllableDistance(Ball.Instance.NormalizedPosition, _initialBallInterceptPoint);
            bool isInterceptPointWithinControllableDistance = Owner.IsBallWithinControllableDistance(Owner.Position, _finalBallInterceptPoint);

            // return result
            return isBallWithinControllableDistance && isInterceptPointWithinControllableDistance; 
        }

        public void Init(float initDistOfBallToBallInterceptPoint, float playerDiveDistance, float timeOfBallToInitialBallInterceptPoint, Vector3 dirOfPlayerToInitialBallInterceptPoint, Vector3 initialBallInterceptPoint, Vector3 relativeBallPositionAtInitialBallInterceptPoint)
        {
            _playerDiveDistance = playerDiveDistance;
            _timeOfBallToInitialBallInterceptPoint = timeOfBallToInitialBallInterceptPoint;

            _dirOfPlayerToInitialBallInterceptPoint = dirOfPlayerToInitialBallInterceptPoint;
            _initialBallInterceptPoint = initialBallInterceptPoint;
            _initDistOfBallToBallInterceptPoint = initDistOfBallToBallInterceptPoint;
            _relativeBallPositionAtInitialBallInterceptPoint = relativeBallPositionAtInitialBallInterceptPoint;
        }

        /// <summary>
        /// Sets the turn and the hands Ik targets
        /// </summary>
        /// <param name="turn"></param>
        public void CalculateHandIkTargets(float turn, BallReference ballReference, out Transform leftHandTargetPosition, out Transform rightHandTargetPosition)
        {
            // init some stuff
            leftHandTargetPosition = null;
            rightHandTargetPosition = null;

            // if going left, left hand goes below right hand goes up
            // if going right, right hand goes below, left hand goes up
            if (turn > 0)// going right
            {
                // if going right, right hand is below
                // and left hand is above
                leftHandTargetPosition = ballReference.BallIkTargets.IkTargetTop;
                rightHandTargetPosition = ballReference.BallIkTargets.IkTargetBottom;
            }
            else if (turn < 0) //going left
            {
                // if going left, right hand is above
                // and left hand is below
                leftHandTargetPosition = ballReference.BallIkTargets.IkTargetBottom;
                rightHandTargetPosition = ballReference.BallIkTargets.IkTargetTop;
            }
        }
    }
}
