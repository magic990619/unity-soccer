using Assets.FootballGameEngine_Indie.Scripts.Entities;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerShootBallTester : MonoBehaviour
    {
        public Goal _goal;
        public LineRenderer _lineRenderer001;
        public LineRenderer _lineRenderer002;
        public Player _primaryPlayer;
        public Transform _shotTargetReferralPoint;

        private void Awake()
        {
            _primaryPlayer.PlaceBallInfronOfMe();
        }

        private void Start()
        {
            _shotTargetReferralPoint.position = _goal.ShotTargetReferencePoint;
            Invoke("Init", 1f);
        }

        private void Update()
        {
            _primaryPlayer.PlaceBallInfronOfMe();

            if(ControlFreak2.CF2Input.GetMouseButtonDown(0))
            {
                //if(_primaryPlayer.CanScore())
                //{
                //    Debug.Log("Can score");
                //    _lineRenderer001.gameObject.SetActive(true);
                //    _lineRenderer001.SetPositions(new Vector3[]
                //    {
                //        _primaryPlayer.Position,
                //        (Vector3)_primaryPlayer.KickTarget
                //    });
                //}
                //else
                //{
                //    Debug.Log("Can't score");
                //    _lineRenderer001.gameObject.SetActive(false);
                //}
            }
        }

        void Init()
        {
            //init primary player
            //_primaryPlayer.Init(0.5f,
            //    15f,
            //    5f,
            //    15f,
            //    1f,
            //    5f,
            //    1f,
            //    0.5f,
            //    15f, // max wonder distance
            //    15f,
            //    10f,
            //    20f,
            //    1f,
            //    3f,
            //    4f,
            //    0.5f,
            //    0.6f,
            //    0.1f,
            //    0.5f,
            //    30f,
            //    1f,
            //    5f,
            //    null,
            //    null,
            //    new FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities.InGamePlayerDto()
            //    {
            //        Accuracy = 0.5f,
            //        GoalKeeping = 0.5f,
            //        JumpHeight = 0.5f,
            //        DiveDistance = 0.5f,
            //        DiveSpeed = 0.5f,
            //        Power = 0.5f,
            //        Reach = 0.5F,
            //        Speed = 0.5f,
            //        Tackling = 0.5f
            //    });
            _primaryPlayer.Init();
        }
    }
}
