using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Tackled.MainState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerChaseBallTester : MonoBehaviour
    {
        public Player _primaryPlayer;
        public Player _secondaryPlayer;

        private void Awake()
        {
            Ball.Instance.CurrentOwner = _secondaryPlayer;

            _secondaryPlayer.PlaceBallInfronOfMe();
            _secondaryPlayer.OnTackled += Instance_OnTackled;

            _secondaryPlayer.RPGMovement.CharacterController.detectCollisions = false;
        }

        private void Instance_OnTackled()
        {
            _secondaryPlayer.InFieldPlayerFSM.ChangeState<TackledMainState>();
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }

        void Init()
        {
            //init primary player
            //_primaryPlayer.Init(15f, 5f, 15f, 1f, 10f, 5f, 1f, 15f, 8f, 30f, 2f, 0.1f, 0.1f, 3f, 3f, 3f, 1f, 25f, 1f, 5f, null, new FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities.PlayerDto()
            //{
            //    Accuracy = 1f,
            //    Speed = 1f,
            //});

            //_secondaryPlayer.Init(15f, 5f, 15f, 1f, 10f, 5f, 1f, 15f, 8f, 30f, 2f, 0.1f, 0.1f, 3f, 3f, 3f, 1f, 25f, 1f, 5f, null, new FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities.PlayerDto()
            //{
            //    Accuracy = 1f,
            //    Speed = 1f,
            //});

            _primaryPlayer.Init();
            _secondaryPlayer.Init();

            Ball.Instance.CurrentOwner = _secondaryPlayer;

            //set the state to chaseball
            _primaryPlayer.InFieldPlayerFSM.ChangeState<ChaseBallMainState>();
        }
    }
}
