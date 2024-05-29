using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Triggers;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Entities
{
    public class CornerGoalKickTriggers : MonoBehaviour
    {
        [SerializeField]
        CornerGoalKickTrigger _centralCornerGoalKickTrigger;

        [SerializeField]
        CornerGoalKickTrigger _leftCornerGoalKickTrigger;

        [SerializeField]
        CornerGoalKickTrigger _rightCornerGoalKickTrigger;

        [SerializeField]
        Team _owner;

        public delegate void CollidedWithBall(Team team, Vector3 position);
        public CollidedWithBall OnCollidedWithBall;

        public CornerGoalKickTrigger CentralCornerGoalKickTrigger { get => _centralCornerGoalKickTrigger; set => _centralCornerGoalKickTrigger = value; }
        public CornerGoalKickTrigger LeftCornerGoalKickTrigger { get => _leftCornerGoalKickTrigger; set => _leftCornerGoalKickTrigger = value; }
        public CornerGoalKickTrigger RightCornerGoalKickTrigger { get => _rightCornerGoalKickTrigger; set => _rightCornerGoalKickTrigger = value; }

        void Awake()
        {
            _centralCornerGoalKickTrigger.OnCollidedWithBall += Instance_OnCollidedWithBall;
            _leftCornerGoalKickTrigger.OnCollidedWithBall += Instance_OnCollidedWithBall;
            _rightCornerGoalKickTrigger.OnCollidedWithBall += Instance_OnCollidedWithBall;
        }

        private void Instance_OnCollidedWithBall(Vector3 position)
        {
            CollidedWithBall temp = OnCollidedWithBall;
            if (temp != null) temp.Invoke(_owner, position);
        }
    }
}
