using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities
{
    [Serializable]
    public class InGamePlayerDto
    {
        [SerializeField]
        [Range(0.1f, 1f)]
        private float _accuracy;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _diveSpeed;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _goalKeeping;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _jumpDistance;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _jumpHeight;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _power;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _reach;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _speed;

        [SerializeField]
        [Range(0.1f, 1f)]
        private float _tackling;


        [SerializeField]
        private string _firstname;

        [SerializeField]
        private string _lastname;

        [SerializeField]
        private string _kitName;

        [SerializeField]
        private string _kitNumber;

        [Header("Tactics Attributes")]

        [SerializeField]
        bool _canJoinCornerKick;


        public bool CanJoinCornerKick { get => _canJoinCornerKick; set => _canJoinCornerKick = value; }


        public float Accuracy { get => _accuracy; set => _accuracy = value; }
        public float GoalKeeping { get => _goalKeeping; set => _goalKeeping = value; }
        public float Power { get => _power; set => _power = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public float Tackling { get => _tackling; set => _tackling = value; }
        public float DiveDistance { get => _jumpDistance; set => _jumpDistance = value; }
        public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }
        public float DiveSpeed { get => _diveSpeed; set => _diveSpeed = value; }
        public float Reach { get => _reach; set => _reach = value; }


        public string Firstname { get => _firstname; set => _firstname = value; }
        public string Lastname { get => _lastname; set => _lastname = value; }
        public string KitName { get => _kitName; set => _kitName = value; }
        public string KitNumber { get => _kitNumber; set => _kitNumber = value; }
    }
}
