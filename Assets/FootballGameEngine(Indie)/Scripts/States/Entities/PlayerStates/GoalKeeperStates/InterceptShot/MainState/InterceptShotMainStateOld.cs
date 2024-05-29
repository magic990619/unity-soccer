using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.References;
using Assets.FootballGameEngine_Indie_.Scripts.Shared;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.MainState
{
    public class InterceptShotMainStateOld : BState
    {
        bool _isBallCatchable;
        bool _isBallInteractable;
        bool _isTwoHandsUsable;
        float _height;
        float _initDistOfBallToBallInterceptPoint;
        float _playerDiveDistance;
        float _playerDiveSpeed;
        float _playerJumpHeight;
        float _playerJumpSpeed;
        float _timeOfBallToInitialBallInterceptPoint;
        float _turn;
        float _weightMultiplier;

        Vector3 _ballPositionAtInitialBallInterceptPoint;
        Vector3 _ballReferenceInitialPosition;
        Vector3 _dirToInitialBallInterceptPoint;
        Vector3 _finalBallInterceptPoint;
        Vector3 _initialBallInterceptPoint;
        Vector3 _normalizedBallInitialPosition;
        Vector3 _normalizedBallPosition;
        Vector3 _normalizedShotTarget;
        Vector3 _normalizedPlayerPosition;
        Vector3 _relativeBallPositionAtInitialBallInterceptPoint;

        Transform _leftHandTargetPosition;
        Transform _rightHandTargetPosition;

        public bool IsBallCatchable { get => _isBallCatchable; set => _isBallCatchable = value; }
        public bool IsTwoHandsUsable { get => _isTwoHandsUsable; set => _isTwoHandsUsable = value; }

        public float BallInitialVelocity { get; set; }
        public float RequiredJumpHeight { get => _playerJumpHeight; set => _playerJumpHeight = value; }
        public float PlayerJumpSpeed { get => _playerJumpSpeed; set => _playerJumpSpeed = value; }
        public float Turn { get => _turn; set => _turn = value; }

        public Vector3 BallInitialPosition { get; set; }
        public Vector3 ShotTarget { get; set; }

        public Transform LeftHandTargetPosition { get => _leftHandTargetPosition; set => _leftHandTargetPosition = value; }
        public Transform RightHandTargetPosition { get => _rightHandTargetPosition; set => _rightHandTargetPosition = value; }

        public override void Initialize()
        {
            base.Initialize();

            // cache the ball reference initial position
            _ballReferenceInitialPosition = Owner.BallReference.transform.localPosition;
        }

        public override void Enter()
        {
            base.Enter();
            
            // calculate the basic data needed to intercept the ball
            CalculatePlayerInterceptBallData(BallInitialVelocity,
                BallInitialPosition,
                ShotTarget,
                Owner.Position,
                out _initDistOfBallToBallInterceptPoint,
                out _timeOfBallToInitialBallInterceptPoint,
                out _ballPositionAtInitialBallInterceptPoint,
                out _initialBallInterceptPoint,
                out _relativeBallPositionAtInitialBallInterceptPoint);

            //AdjustBallInterceptPointData(_relativeBallPositionAtInitialBallInterceptPoint.x,
            //    _initialBallInterceptPoint,
            //    _normalizedBallInitialPosition,
            //    out _initDistOfBallToBallInterceptPoint,
            //    out _timeOfBallToInitialBallInterceptPointCached,
            //    out _timeOfBallToPlayerInitialSteeringTarget,
            //    out _ballPositionAtInitialBallInterceptPoint,
            //    out _initialBallInterceptPoint,
            //    out _relativeBallPositionAtInitialBallInterceptPoint);

            /**Calculate player control variables**/

            // calculate the dive parameters
            CalculatePlayerDiveParameters(_timeOfBallToInitialBallInterceptPoint,
                _initialBallInterceptPoint,
                _normalizedPlayerPosition,
                out _height,
                out _playerDiveDistance,
                out _playerDiveSpeed,
                out _playerJumpHeight,
                out _playerJumpSpeed,
                out _dirToInitialBallInterceptPoint);                                                      //clamp the jump height
           
            //find the player intercept point
            _finalBallInterceptPoint = CalculateBallFinalInterceptPoint(_playerDiveDistance,
                                                                        _dirToInitialBallInterceptPoint);

            // check if the ball is catchable or not
            _isTwoHandsUsable = CheckIfTwoHandsAreUsable(BallInitialVelocity,
                                                     _dirToInitialBallInterceptPoint.magnitude,
                                                     _initialBallInterceptPoint,
                                                     _finalBallInterceptPoint,
                                                     out _isBallCatchable);
            // set the steering 
            SetPlayerSteering(_playerDiveSpeed,
                              _finalBallInterceptPoint,
                              BallInitialPosition);

            // set the ball reference
            SetBallReference(_ballPositionAtInitialBallInterceptPoint,
                             Owner.transform.rotation);
           
            // set the turn and hand ik targets
            SetTurnAndHandIkTargets(_relativeBallPositionAtInitialBallInterceptPoint.x,
                                    Owner.BallReference,
                                    out _turn,
                                    out _leftHandTargetPosition,
                                    out _rightHandTargetPosition);

            // set the animator parameters
            SetTheAnimatorParameters(_isBallCatchable,
                                     _isTwoHandsUsable,
                                     _height,
                                     _turn);
        }

        public override void Execute()
        {
            base.Execute();


            //normalize stuff here
            _normalizedBallPosition = new Vector3(Ball.Instance.Position.x, 
                Ball.Instance.Position.y + Ball.Instance.SphereCollider.radius, 
                Ball.Instance.Position.z);

            _normalizedPlayerPosition = new Vector3(Owner.Position.x, 
                0f, 
                Owner.Position.z);

            // check if I should stop steering
            if (Vector3.Distance(_finalBallInterceptPoint, _normalizedPlayerPosition) <= 0.05f)
            {
                if (Owner.RPGMovement.Steer == true)
                    Owner.RPGMovement.SetSteeringOff();
            }

            // decrement ball time
            _timeOfBallToInitialBallInterceptPoint -= Time.deltaTime;

            // check if the keeper can now intercat with the ball
            bool isPlayerSupposedToInteractWithBall = CheckIfPlayerShouldInteractWithBall(IsTwoHandsUsable,
                                                                       _timeOfBallToInitialBallInterceptPoint,
                                                                       out _isBallInteractable);

            // intercat with the ball
            if (isPlayerSupposedToInteractWithBall)
                Machine.ChangeState<InteractWithBallMainState>();
        }

        public override void Exit()
        {
            base.Exit();

            // reset the animators
            ReSetTheAnimatorParameters();

            // reset steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();
        }

        public override void OnAnimatorExecuteIK(int layerIndex)
        {
            base.OnAnimatorExecuteIK(layerIndex);

            // declare the weight variables
            float leftHandWeight = 0f;
            float rightHandWeight = 0f;
            float lookAtWeight = 0f;

            // calculate the hand and look at weights
            CalculateHandAndLookWeights(_isTwoHandsUsable,
                                        _turn,
                                        out leftHandWeight,
                                        out rightHandWeight,
                                        out lookAtWeight);

            // move the hands to ik target
            MoveHandsToIkTarget(leftHandWeight,
                                rightHandWeight,
                                lookAtWeight,
                                _leftHandTargetPosition.position,
                                _rightHandTargetPosition.position);
        }

        public override void OnAnimatorExecuteMove()
        {
            base.OnAnimatorExecuteMove();

            // move the model root
            MoveModelRoot(Owner.ModelRoot.localPosition.y,
                          RequiredJumpHeight,
                          PlayerJumpSpeed);
        }

        public Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }

        public bool IsBallInteractable { get => _isBallInteractable; set => _isBallInteractable = value; }

        /// <summary>
        /// Checks whether the goalkeeper should use two hands when diving
        /// catchable speed
        /// </summary>
        /// <param name="ballVelocity"></param>
        /// <param name="playerDistanceToInitialSteeringTarget"></param>
        /// <param name="maxMovementDistanceOfPlayer"></param>
        /// <returns></returns>
        public bool CheckIfTwoHandsAreUsable(float ballVelocity, float playerDistanceToInitialSteeringTarget, Vector3 initialBallInterceptPoint, Vector3 finalBallInterceptPoint, out bool isBallCatchable)
        {
            // set is catchable to false
            isBallCatchable = false;
            bool isTwoHandsUsable = false;

            //if intercept point is within player reach check ball velocity
            //if not check if the distance between initial ball intercept point and final ball intercept point is greater than player reach
            //if this distance is greater than player reach then the ball is not catchable
            //else check ball velocity
            if(playerDistanceToInitialSteeringTarget <= Owner.ActualReach)
            {
                isTwoHandsUsable = true;
                isBallCatchable = ballVelocity <= Globals.MaxBallCatchableVelocity ? true : false;  // ball is catchable if ball velocity is within the catchable velocity
            }
            else
            {
                // find the distance between the initial ball intercept point and the final ball intercept point
                float distBetweenInitialAndFinalBallInterceptPoint = Vector3.Distance(initialBallInterceptPoint, finalBallInterceptPoint);

                //find the distance that the player can jump and still reach the ball
                if (distBetweenInitialAndFinalBallInterceptPoint > Owner.ActualReach)
                {
                    isBallCatchable = false;
                    isTwoHandsUsable = false;    // set is ball catchable to false   
                }
                else
                {
                    isTwoHandsUsable = true;
                    isBallCatchable = ballVelocity <= Globals.MaxBallCatchableVelocity ? true : false;  // ball is catchable if ball velocity is within the catchable velocity
                }
            }

            // return result
            return isTwoHandsUsable;
        }

        public bool CheckIfPlayerShouldInteractWithBall(bool isBallCatchable, float remaingBallTimeToBallInterceptPoint, out bool isBallInteractable)
        {
            isBallInteractable = true;

            // create the can intercat with ball
            bool canInteractWithBall = false;

            // if time is exhausted then go to tend goal
            // if ball within control distance te deflect its path
            if (remaingBallTimeToBallInterceptPoint <= -0.1f)
            {
                canInteractWithBall = true;
                isBallInteractable = false;
            }
            else
            {
                // calculate the distance between the ball and reference point
                float distBetweenBallAndBallReference = Vector3.Distance(Ball.Instance.Position, Owner.BallReference.transform.position);
                
                // if this distance is now so small, we then check if we can interact with the ball
                if (distBetweenBallAndBallReference <= 0.5f)    //ToDo::Fix logic to use Mathf.Episilon
                {
                    // get the distance travelled by the ball in a single frame
                    float distanceOfBallTravelledInSingleFrame = GetDistanceTravelledInSingleFrame(Ball.Instance.Rigidbody.velocity);

                    if (isBallCatchable)
                    {
                        canInteractWithBall = GetDistOfBoneToPosition(HumanBodyBones.LeftHand, _leftHandTargetPosition.position) <= distanceOfBallTravelledInSingleFrame
                            && GetDistOfBoneToPosition(HumanBodyBones.RightHand, _rightHandTargetPosition.position) <= distanceOfBallTravelledInSingleFrame;
                    }
                    else
                    {
                        //punch the ball
                        if (_turn == 0f)
                        {
                            //check if the left or right hand can punch the ball
                            canInteractWithBall = GetDistOfBoneToPosition(HumanBodyBones.RightHand,
                                _normalizedBallPosition) <= distanceOfBallTravelledInSingleFrame
                                || GetDistOfBoneToPosition(HumanBodyBones.LeftHand,
                                _normalizedBallPosition) <= distanceOfBallTravelledInSingleFrame;
                        }
                        else if (_turn == 1)
                        {
                            // check if the right hand can interact with the ball
                            canInteractWithBall = GetDistOfBoneToPosition(HumanBodyBones.RightHand, _rightHandTargetPosition.position) <= distanceOfBallTravelledInSingleFrame;
                        }
                        else if (_turn == -1)
                        {
                            // check if the right hand can interact with the ball
                            canInteractWithBall = GetDistOfBoneToPosition(HumanBodyBones.LeftHand, _leftHandTargetPosition.position) <= distanceOfBallTravelledInSingleFrame;
                        }
                    }
                }
            }

            // return result
            return canInteractWithBall;
        }

        public float GetDistOfBoneToPosition(HumanBodyBones bone, Vector3 position)
        {
            //find the distance between the bone and the target
            return Vector3.Distance(Owner.Animator.GetBoneTransform(bone).transform.position, position);
        }

        public float GetDistanceTravelledInSingleFrame(Vector3 velocity)
        {
            return velocity.magnitude * Time.deltaTime;
        }

        public Vector3 GetPlayerBallInterceptPoint(Vector3 ballInitialPosition, Vector3 shotTarget, Vector3 playerPosition)
        {
            //normalize stuff
            _normalizedBallInitialPosition = new Vector3(ballInitialPosition.x, 0f, ballInitialPosition.z);
            _normalizedShotTarget = new Vector3(shotTarget.x, 0f, shotTarget.z);
            _normalizedPlayerPosition = new Vector3(playerPosition.x, 0f, playerPosition.z);

            //find the point on the ball path to target that is orthogonal to player position
            Vector3 initialBallInterceptPoint = Owner.GetPointOrthogonalToLine(_normalizedBallInitialPosition,
               _normalizedShotTarget,
               _normalizedPlayerPosition);

            // return result
            return initialBallInterceptPoint;
        }

        public void AdjustBallInterceptPointData(float xOffset,
            Vector3 oldInitalBallInterceptPoint,
            Vector3 normalizedBallInitialPosition,
            out float initDistOfBallToBallInterceptPoint,
            out float timeOfBallToInitialBallInterceptPointCached,
            out float timeOfBallToPlayerInitialSteeringTarget,
            out Vector3 ballPositionAtInitialBallInterceptPoint,
            out Vector3 newInitalBallInterceptPoint, 
            out Vector3 relativeBallPositionAtInitialBallInterceptPoint)
        {
            // initialize some data
            initDistOfBallToBallInterceptPoint = 0f;
            timeOfBallToInitialBallInterceptPointCached = 0f;
            timeOfBallToPlayerInitialSteeringTarget = 0f;

            ballPositionAtInitialBallInterceptPoint = Vector3.zero;
            newInitalBallInterceptPoint = Vector3.zero;
            relativeBallPositionAtInitialBallInterceptPoint = Vector3.zero;

            //if I'm already at target, I target somewhere in front of me
            if (Mathf.Abs(xOffset) < Owner.ActualReach)
            {
                //update the ball intercept point
                newInitalBallInterceptPoint = oldInitalBallInterceptPoint + Owner.transform.forward * (Owner.ActualReach + Owner.Radius);
                initDistOfBallToBallInterceptPoint = Vector3.Distance(newInitalBallInterceptPoint, normalizedBallInitialPosition);

                //we get the future ball target at steering target
                timeOfBallToInitialBallInterceptPointCached
                    = timeOfBallToPlayerInitialSteeringTarget
                    = Owner.TimeToTarget(normalizedBallInitialPosition,
                    newInitalBallInterceptPoint,
                    BallInitialVelocity,
                    Ball.Instance.Friction);

                ballPositionAtInitialBallInterceptPoint = Ball.Instance.FuturePosition(timeOfBallToPlayerInitialSteeringTarget);

                //find the future position of the ball relative to me
                relativeBallPositionAtInitialBallInterceptPoint = Owner.transform.InverseTransformPoint(ballPositionAtInitialBallInterceptPoint);
            }
        }

        public void CalculateHandAndLookWeights(bool isTwoHandsUsable, float turn, out float leftHandWeight, out float rightHandWeight, out float lookAtWeight)
        {
            //initialize the weight parameters
            leftHandWeight = 0f;
            rightHandWeight = 0f;
            lookAtWeight = 0f;

            //calculate the weight multiplier depending on the remaining distance to target
            _normalizedBallPosition = Ball.Instance.NormalizedPosition;

            //find the distance of ball to orthogonal point
            float distanceOfBallToTarget = Vector3.Distance(_normalizedBallPosition, _initialBallInterceptPoint);

            // calculate the weight depending on the traveled distance
            float rawWeightMultiplier = distanceOfBallToTarget / _initDistOfBallToBallInterceptPoint;
            if (rawWeightMultiplier <= 0.01f) rawWeightMultiplier = 0f;
            rawWeightMultiplier = 1 - rawWeightMultiplier;
            _weightMultiplier = Mathf.Clamp(rawWeightMultiplier, 0f, 1f);
            
            //choose which hands to effect
            if (turn == 0f)
            {
                leftHandWeight = _weightMultiplier;
                rightHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;
            }
            else if (turn == -1)
            {
                // run basic weight multiplier
                leftHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;

                // update right hand multiplier
                rightHandWeight = isTwoHandsUsable ? _weightMultiplier : 0f;
            }
            else if (turn == 1)
            {
                // run basic weight multiplier
                rightHandWeight = _weightMultiplier;
                lookAtWeight = _weightMultiplier;

                // update left hand multiplier
                leftHandWeight = isTwoHandsUsable ? _weightMultiplier : 0f;
            }
        }

        public void CalculatePlayerDiveParameters(float timeOfBallToPlayerBallInterceptPoint, Vector3 ballInterceptPoint, Vector3 playerPosition, out float height, out float playerDiveDistance, out float playerDiveSpeed, out float requiredJumpHeight, out float requiredJumpSpeed, out Vector3 dirToInitialBallInterceptPoint)
        {
            // prepare some data
            dirToInitialBallInterceptPoint = ballInterceptPoint - playerPosition;    //calculate the direction of player to intercept point
            dirToInitialBallInterceptPoint.y = 0.0f;

            // set the player dive distance
            playerDiveDistance = Mathf.Clamp(dirToInitialBallInterceptPoint.magnitude, 0f, Owner.ActuaDiveDistance);

            // no need to dive the entire distance, just dive to a point I can still reach the ball
            if (playerDiveDistance > Owner.ActualReach) playerDiveDistance -= Owner.ActualReach;

            //calculate the required player speed to reach the steering target at the same time as the ball
            float playerRawDiveSpeed = playerDiveDistance / timeOfBallToPlayerBallInterceptPoint;                                   //find the raw dive speed
            playerDiveSpeed = Mathf.Clamp(playerRawDiveSpeed, 0f, Owner.ActualDiveSpeed);                                           //clamp the player dive speed

            //calculate the jump height required to reach the ball
            float playerRawJumpHeight = _relativeBallPositionAtInitialBallInterceptPoint.y - Owner.Height;                          //find the jump height to reach ball
            requiredJumpHeight = Mathf.Clamp(playerRawJumpHeight, 0f, Owner.ActuaJumpHeight);                                       //clamp the jump height

            // calculate the jump up speed
            float playerRawJumpSpeed = requiredJumpHeight / timeOfBallToPlayerBallInterceptPoint;
            requiredJumpSpeed = Mathf.Clamp(playerRawJumpSpeed, 0f, Owner.ActualDiveSpeed);

            //set the height animator value
            height = Mathf.Clamp(_relativeBallPositionAtInitialBallInterceptPoint.y / (0.75f * (Owner.ActuaJumpHeight + Owner.Height)), 0f, 1f);
        }

        /// <summary>
        /// Calculates the point to intercept the ball from the ball's initial position to it's target
        /// </summary>
        /// <param name="ballInitialVelocity">inital velocity of the ball</param>
        /// <param name="ballPosition">the initial ball position</param>
        /// <param name="shotTarget">the ball destination</param>
        /// <param name="playerPosition">the player position</param>
        /// <param name="initDistOfBallToBallInterceptPoint">the distance between the ball initial position and the intercept point</param>
        /// <param name="timeOfBallToInitialBallInterceptPointCached">the cached time of ball to travel from the the ball position to the intercept point</param>
        /// <param name="timeOfBallToInitialBallInterceptPoint">the time of ball to travel from the the ball position to the intercept point</param>
        /// <param name="ballPositionAtInitialBallInterceptPoint">the cached time of ball to travel from the the ball position to the intercept point</param>
        /// <param name="initialBallInterceptPoint">the ball intercept point on the ground</param>
        /// <param name="relativeBallPositionAtInitialBallInterceptPoint">the actual ball position at ball intercept point relative to the player's rotation</param>
        public void CalculatePlayerInterceptBallData(float ballInitialVelocity, Vector3 ballPosition, Vector3 shotTarget, Vector3 playerPosition, out float initDistOfBallToBallInterceptPoint, out float timeOfBallToInitialBallInterceptPoint, out Vector3 ballPositionAtInitialBallInterceptPoint, out Vector3 initialBallInterceptPoint, out Vector3 relativeBallPositionAtInitialBallInterceptPoint)
        {
            // get the player ball intercept point
            initialBallInterceptPoint = GetPlayerBallInterceptPoint(ballPosition,
                shotTarget,
                playerPosition);

            //calculate some data depended on the orthogonal point
            initDistOfBallToBallInterceptPoint = Vector3.Distance(initialBallInterceptPoint,
                _normalizedBallInitialPosition);

            //ToDo::Implement logic to calculate ball time for different types of shots

            //// calculate time of ball to intercept point
            //_timeOfBallToInitialBallInterceptPointCached = timeOfBallToInitialBallInterceptPoint = Owner.TimeToTarget(_normalizedBallInitialPosition,
            //    _initialBallInterceptPoint,
            //    BallInitialVelocity,
            //    Ball.Instance.Friction);

            timeOfBallToInitialBallInterceptPoint = Owner.TimeToTarget(_normalizedBallInitialPosition,
               initialBallInterceptPoint,
               ballInitialVelocity);

            //get the ball's future position at intercept point
            ballPositionAtInitialBallInterceptPoint = Ball.Instance.FuturePosition(timeOfBallToInitialBallInterceptPoint);
            //ballPositionAtInitialBallInterceptPoint.y -= Ball.Instance.SphereCollider.radius;   // ToDo::Fix logic that has to do with finding ball future position
            relativeBallPositionAtInitialBallInterceptPoint = Owner.transform.InverseTransformPoint(ballPositionAtInitialBallInterceptPoint);
        }

        public Vector3 CalculateBallFinalInterceptPoint(float maxMovementDistanceOfPlayer, Vector3 dirToInitialBallInterceptPoint)
        {
            //find the player intercept point
            Vector3 finalBallInterceptPoint = _normalizedPlayerPosition
                + dirToInitialBallInterceptPoint.normalized
                * maxMovementDistanceOfPlayer;

            // return result
            return finalBallInterceptPoint;
        }

        public void MoveHandsToIkTarget(float leftHandWeight, float rightHandWeight, float lookAtWeight, Vector3 leftHandPosition, Vector3 rightHandPosition)
        {
            //set the animations weights
            Owner.Animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            Owner.Animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            //Owner.Animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            //Owner.Animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
            Owner.Animator.SetLookAtWeight(lookAtWeight);

            //set the animations positions
            Owner.Animator.SetLookAtPosition(Ball.Instance.Position);
            Owner.Animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandPosition);
            Owner.Animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandPosition);
            //Owner.Animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTargetPosition.rotation);
            //Owner.Animator.SetIKRotation(AvatarIKGoal.RightHand, RightHandTargetPosition.rotation);

        }

        public void MoveModelRoot(float initial, float final, float speed)
        {
            // move towards the jump height
            float positionY = Mathf.MoveTowards(initial, final, speed * Time.deltaTime);

            // set the movement position for the model root
            Vector3 localPosition = new Vector3(Owner.ModelRoot.transform.localPosition.x,
                positionY,
                Owner.ModelRoot.transform.localPosition.z);

            // move the model root
            Owner.ModelRoot.transform.localPosition = localPosition;
        }

        public void ResetBallReference()
        {
            // reset the ball reference
            Owner.BallReference.transform.SetParent(Owner.transform);
            Owner.BallReference.transform.localPosition =_ballReferenceInitialPosition;
            Owner.BallReference.transform.rotation = Owner.transform.rotation;
        }

        public void ReSetTheAnimatorParameters()
        {
            //reset the animator
            Owner.Animator.ResetTrigger("Dive");
        }

        /// <summary>
        /// Set the ball reference
        /// </summary>
        /// <param name="ballPositionAtInitialBallInterceptPoint"></param>
        /// <param name="rotation"></param>
        public void SetBallReference(Vector3 ballPositionAtInitialBallInterceptPoint, Quaternion rotation)
        {
            // set the ball reference position and rotation
            Owner.BallReference.transform.position = ballPositionAtInitialBallInterceptPoint;
            Owner.BallReference.transform.rotation = rotation;
            Owner.BallReference.transform.SetParent(null);
        }

        /// <summary>
        /// Set the player steering point
        /// </summary>
        /// <param name="playerDiveSpeed"></param>
        /// <param name="finalBallInterceptPoint"></param>
        /// <param name="ballInitialPosition"></param>
        public void SetPlayerSteering(float playerDiveSpeed, Vector3 finalBallInterceptPoint, Vector3 ballInitialPosition)
        {
            Owner.RPGMovement.Speed = playerDiveSpeed;
            Owner.RPGMovement.SetMoveTarget(finalBallInterceptPoint);
            Owner.RPGMovement.SetRotateFacePosition(ballInitialPosition);
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();
        }

        /// <summary>
        /// Set the animator parameters
        /// </summary>
        public void SetTheAnimatorParameters(bool isBallCatchable, bool isTwoHandsUsable, float height, float turn)
        {
            //set the dive animation to play
            Owner.Animator.SetBool("IsBallCatchable", isBallCatchable);
            Owner.Animator.SetBool("IsTwoHandsUsable", isTwoHandsUsable);
            Owner.Animator.SetFloat("Height", height);
            Owner.Animator.SetFloat("Turn", turn);
            Owner.Animator.SetTrigger("Dive");
        }

        /// <summary>
        /// Sets the turn and the hands Ik targets
        /// </summary>
        /// <param name="xOffset"></param>
        public void SetTurnAndHandIkTargets(float xOffset, BallReference ballReference, out float turn, out Transform leftHandTargetPosition, out Transform rightHandTargetPosition)
        {
            // init some stuff
            turn = 0f;
            leftHandTargetPosition = null;
            rightHandTargetPosition = null;

            //set the turn animator value
            if (Mathf.Abs(xOffset) < Owner.ActualReach)
            {
                // set turn
                turn = 0f;

                // if not moving then left hand is on the left side
                // and right is on the right side of the ball
                leftHandTargetPosition = ballReference.BallIkTargets.IkTargetLeft;
                rightHandTargetPosition = ballReference.BallIkTargets.IkTargetRight;
            }
            else if (xOffset > 0)
            {
                // set turn
                turn = 1f;

                // if going right, right hand is below
                // and left hand is above
                leftHandTargetPosition = ballReference.BallIkTargets.IkTargetTop;
                rightHandTargetPosition = ballReference.BallIkTargets.IkTargetBottom;
            }
            else if (xOffset < 0)
            {
                // set turn
                turn = -1f;

                // if going left, right hand is above
                // and left hand is below
                leftHandTargetPosition = ballReference.BallIkTargets.IkTargetBottom;
                rightHandTargetPosition = ballReference.BallIkTargets.IkTargetTop;
            }
        }
    }
}
