using Patterns.Singleton;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    public class Pitch : Singleton<Pitch>
    {
        [SerializeField]
        Team _awayTeam;

        [SerializeField]
        Team _homeTeam;

        [SerializeField]
        Transform _centerSpot;

        float _length;

        public float Length { get => _length; set => _length = value; }
        public Transform CenterSpot { get => _centerSpot; set => _centerSpot = value; }

        public override void Awake()
        {
            base.Awake();

            _length = Mathf.Abs(_awayTeam.Goal.Position.z - _homeTeam.Goal.Position.z);
        }
    }
}
