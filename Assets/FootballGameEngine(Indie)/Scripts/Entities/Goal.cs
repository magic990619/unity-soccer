using Assets.FootballGameEngine_Indie.Scripts.Triggers;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    public class Goal : MonoBehaviour
    {
        [SerializeField]
        EighteenArea _eighteenArea;

        [SerializeField]
        GoalMouth _goalMouth;

        [SerializeField]
        GoalTrigger _goalTrigger;

        [SerializeField]
        Transform _goalLineReference;

        [SerializeField]
        Transform _shotTargetReferencePoint;

        float _goalHeight;

        /// <summary>
        /// Action raised when goal collides with the ball
        /// </summary>
        public Action OnCollideWithBall;

        public float GoalHeight { get => _goalHeight; }

        ///ToDo::Speak about why you put them here as an initialization
        public Vector3 BottomLeftRelativePosition { get; set; }
        public Vector3 BottomRightRelativePosition { get; set; }
        public Vector3 Position { get => transform.position; }
        public Vector3 ShotTargetReferencePoint { get => _shotTargetReferencePoint.position; }
        public Transform GoalLineReference { get => _goalLineReference; set => _goalLineReference = value; }
        public EighteenArea EighteenArea { get => _eighteenArea; set => _eighteenArea = value; }

        private void Awake()
        {
            //init some data 
            BottomLeftRelativePosition = _goalLineReference.InverseTransformPoint(_goalMouth._pointBottomLeft.position);
            BottomRightRelativePosition = _goalLineReference.InverseTransformPoint(_goalMouth._pointBottomRight.position);

            // set stuff
            _goalTrigger.Goal = this;
            _goalHeight = Mathf.Abs(_goalMouth._pointTopLeft.localPosition.y - _goalMouth._pointBottomLeft.localPosition.y);
           
            //listen to the goal-trigger events
            _goalTrigger.OnCollidedWithBall += Instance_OnCollidedWithBall;
        }

        private void Instance_OnCollidedWithBall()
        {
            //raise the on collision with ball event
            Action temp = OnCollideWithBall;
            if (temp != null)
                temp.Invoke();
        }

        public bool IsPositionWithinGoalMouthFrustrum(Vector3 position)
        {
            //find the relative position to goal
            Vector3 relativePosition = transform.InverseTransformPoint(position);

            //find the relative position of each goal mouth
            Vector3 pointBottomLeftRelativePosition = transform.InverseTransformPoint(_goalMouth._pointBottomLeft.position);
            Vector3 pointBottomRightRelativePosition = transform.InverseTransformPoint(_goalMouth._pointBottomRight.position);
            Vector3 pointTopLeftRelativePosition = transform.InverseTransformPoint(_goalMouth._pointTopLeft.position);

            //check if the x- coordinate of the relative position lies within the goal mouth
            bool isPositionWithTheXCoordinates = relativePosition.x > pointBottomLeftRelativePosition.x && relativePosition.x < pointBottomRightRelativePosition.x;
            bool isPositionWithTheYCoordinates = relativePosition.y > pointBottomLeftRelativePosition.y && relativePosition.y < pointTopLeftRelativePosition.y;

            //the result is the combination of the two tests
            return isPositionWithTheXCoordinates && isPositionWithTheYCoordinates;
        }

    }

    [Serializable]
    public struct GoalMouth
    {
        public Transform _pointBottomLeft;
        public Transform _pointBottomRight;
        public Transform _pointTopLeft;
        public Transform _pointTopRight;
    }
}
