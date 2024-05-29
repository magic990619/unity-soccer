using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.SubStates
{
    public class CheckIfIShouldDiveOrMoveToInterceptPoint : BState
    {
        float _initDistOfBallToBallInterceptPoint;
        float _playerDiveDistance;
        float _timeOfBallToInitialBallInterceptPoint;

        Vector3 _ballPositionAtInitialBallInterceptPoint;
        Vector3 _dirOfPlayerToInitialBallInterceptPoint;
        Vector3 _initialBallInterceptPoint;
        Vector3 _normalizedBallInitialPosition;
        Vector3 _normalizedPlayerPosition;
        Vector3 _normalizedShotTarget;
        Vector3 _relativeBallPositionAtInitialBallInterceptPoint;

        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // calculate the basic data needed to intercept the ball
            CalculatePlayerInterceptBallData(Owner.Shot.KickPower,
                Owner.Shot.FromPosition,
                Owner.Shot.ToPosition,
                Owner.Position,
                out _initDistOfBallToBallInterceptPoint,
                out _timeOfBallToInitialBallInterceptPoint,
                out _ballPositionAtInitialBallInterceptPoint,
                out _dirOfPlayerToInitialBallInterceptPoint,
                out _initialBallInterceptPoint,
                out _relativeBallPositionAtInitialBallInterceptPoint);

            // calculate time of player to inital ball intercept point
            float timeOfPlayerToInterceptBallInitialPoint = Owner.TimeToTarget(Owner.Position, _initialBallInterceptPoint, Owner.TendGoalSpeed);

            // I can move to intercept point if my time to intercept point is smaller than time of ball to intercept point
            // I can dive to intercept point if my time to intercept point is higher than time of ball to intercept point
            if (timeOfPlayerToInterceptBallInitialPoint + 0.5f < _timeOfBallToInitialBallInterceptPoint)
            {
                // init dive-to-intercept-point state
                Machine.GetState<DiveToInterceptPoint>().Init(
                    _initDistOfBallToBallInterceptPoint,
                    _playerDiveDistance,
                    _timeOfBallToInitialBallInterceptPoint,
                    _dirOfPlayerToInitialBallInterceptPoint,
                    _initialBallInterceptPoint,
                    _relativeBallPositionAtInitialBallInterceptPoint);

                // change state
                Machine.ChangeState<DiveToInterceptPoint>();
            }
            else
            {
                // init move-to-intercept-point state
                Machine.GetState<MoveToInterceptPoint>().Init(_dirOfPlayerToInitialBallInterceptPoint,
                    _initialBallInterceptPoint,
                    _relativeBallPositionAtInitialBallInterceptPoint);

                // change state
                Machine.ChangeState<MoveToInterceptPoint>();
            }
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
        /// <param name="ballPositionAtInitialBallInterceptPoint">the actual ball position at the intercept point</param>
        /// <param name="initialBallInterceptPoint">the ball intercept point on the ground</param>
        /// <param name="relativeBallPositionAtInitialBallInterceptPoint">the actual ball position at ball intercept point relative to the player's rotation</param>
        public void CalculatePlayerInterceptBallData(float ballInitialVelocity, Vector3 ballPosition, Vector3 shotTarget, Vector3 playerPosition, out float initDistOfBallToBallInterceptPoint, out float timeOfBallToInitialBallInterceptPoint, out Vector3 ballPositionAtInitialBallInterceptPoint, out Vector3 dirOfPlayerToInitBallInterceptPoint, out Vector3 initialBallInterceptPoint, out Vector3 relativeBallPositionAtInitialBallInterceptPoint)
        {
            // get the player ball intercept point
            initialBallInterceptPoint = GetPlayerBallInterceptPoint(ballPosition,
                shotTarget,
                playerPosition);

            //calculate some data depended on the orthogonal point
            initDistOfBallToBallInterceptPoint = Vector3.Distance(initialBallInterceptPoint,
                _normalizedBallInitialPosition);

            // calculate time of ball to intercept point
            timeOfBallToInitialBallInterceptPoint = Owner.TimeToTarget(_normalizedBallInitialPosition,
               initialBallInterceptPoint,
               ballInitialVelocity);

            //get the ball's future position at intercept point
            ballPositionAtInitialBallInterceptPoint = Ball.Instance.FuturePosition(timeOfBallToInitialBallInterceptPoint);
            //ballPositionAtInitialBallInterceptPoint.y -= Ball.Instance.SphereCollider.radius;   // ToDo::Fix logic that has to do with finding ball future position
            relativeBallPositionAtInitialBallInterceptPoint = Owner.transform.InverseTransformPoint(ballPositionAtInitialBallInterceptPoint);

            // calculate the direction of the player to the ball intercept point
            dirOfPlayerToInitBallInterceptPoint = initialBallInterceptPoint - playerPosition;
            dirOfPlayerToInitBallInterceptPoint.y = 0.0f;
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
    }
}
