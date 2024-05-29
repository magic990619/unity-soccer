using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Storage.MatchDifficulties
{
    [Serializable]
    public class MatchDifficultyTeamParam
    {

        [Header("Global Control Variables")]

        [Range(0.01f, 0.1f)]
        [SerializeField]
        float _aiUpdateFrequency = 0.5f;
        [Range(0.1f, 1f)]
        [SerializeField]
        float _tightPressFrequency = 0.5f;

        [SerializeField]
        float _distancePassMax = 15f;
        [SerializeField]
        float _distancePassMin = 5f;
        [SerializeField]
        float _distanceShotValidMax = 30f;
        [SerializeField]
        float _distanceTendGoal = 3f;
        [SerializeField]
        float _distanceThreatMax = 1f;
        [SerializeField]
        float _distanceThreatMin = 5f;
        [SerializeField]
        float _distanceThreatTrack = 1f;
        [SerializeField]
        float _distanceWonderMax = 15f;
        [SerializeField]
        float _velocityLongPassArrive = 15f;
        [SerializeField]
        float _velocityShortPassArrive = 15f;
        [SerializeField]
        float _velocityShotArrive = 30f;

        [Header("Player Control Variables")]

        [SerializeField]
        float _diveDistance = 1f;
        [SerializeField]
        float _diveSpeed = 4f;
        [SerializeField]
        float _jogSpeed = 4f;
        [SerializeField]
        float _jumpHeight = 0.5f;
        [SerializeField]
        float _power = 30f;
        [SerializeField]
        float _reach = 0.5f;
        [SerializeField]
        float _speed = 3.5f;

        public float DistancePassMax { get => _distancePassMax; set => _distancePassMax = value; }
        public float DistancePassMin { get => _distancePassMin; set => _distancePassMin = value; }
        public float DistanceShotValidMax { get => _distanceShotValidMax; set => _distanceShotValidMax = value; }
        public float DistanceThreatMax { get => _distanceThreatMax; set => _distanceThreatMax = value; }
        public float DistanceThreatMin { get => _distanceThreatMin; set => _distanceThreatMin = value; }
        public float DistanceThreatTrack { get => _distanceThreatTrack; set => _distanceThreatTrack = value; }
        public float DistanceWonderMax { get => _distanceWonderMax; set => _distanceWonderMax = value; }
        public float VelocityLongPassArrive { get => _velocityLongPassArrive; set => _velocityLongPassArrive = value; }
        public float VelocityShortPassArrive { get => _velocityShortPassArrive; set => _velocityShortPassArrive = value; }
        public float VelocityShotArrive { get => _velocityShotArrive; set => _velocityShotArrive = value; }
        public float DiveDistance { get => _diveDistance; set => _diveDistance = value; }
        public float DiveSpeed { get => _diveSpeed; set => _diveSpeed = value; }
        public float JogSpeed { get => _jogSpeed; set => _jogSpeed = value; }
        public float JumpHeight { get => _jumpHeight; set => _jumpHeight = value; }
        public float Power { get => _power; set => _power = value; }
        public float Reach { get => _reach; set => _reach = value; }
        public float Speed { get => _speed; set => _speed = value; }
        public float DistanceTendGoal { get => _distanceTendGoal; set => _distanceTendGoal = value; }
        public float AiUpdateFrequency { get => _aiUpdateFrequency; set => _aiUpdateFrequency = value; }
        public float TightPressFrequency { get => _tightPressFrequency; set => _tightPressFrequency = value; }
    }
}
