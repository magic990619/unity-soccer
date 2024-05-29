using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerPickOutThreatTest : MonoBehaviour
    {
        public Player _primaryPlayer;
        public Player _secondaryPlayer;

        private void Awake()
        {
            //Ball.Instance.Owner = _secondaryPlayer;

            //_secondaryPlayer.PlaceBallInfronOfMe();
            //_secondaryPlayer.OnTackled += Instance_OnTackled;
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }

        private void Init()
        {
            ////init primary player
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

            //_secondaryPlayer.Init(0.5f,
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
            _secondaryPlayer.Init();

            //set the state to supprt attacker
            _primaryPlayer.InFieldPlayerFSM.GetState<PickOutThreatMainState>().Threat = _secondaryPlayer;
            _primaryPlayer.InFieldPlayerFSM.ChangeState<PickOutThreatMainState>();
        }
    }
}
