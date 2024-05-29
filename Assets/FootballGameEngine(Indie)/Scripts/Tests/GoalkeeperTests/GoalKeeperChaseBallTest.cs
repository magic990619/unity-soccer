using Assets.FootballGameEngine_Indie.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tests.GoalkeeperTests
{
    public class GoalKeeperChaseBallTest : MonoBehaviour
    {
        [SerializeField]
        Player _goalKeeper;

        [SerializeField]
        Player _inFieldPlayer;

        private void Awake()
        {
            //Ball.Instance.CurrentOwner = _inFieldPlayer;

            // inti the goalkeeper
            //_goalKeeper.Init(0.5f,
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
            //    8f,
            //    null,
            //    null,
            //    new Data.Dtos.InGame.Entities.InGamePlayerDto()
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

            // enable tje keeper
            _goalKeeper.gameObject.SetActive(true);

            Invoke("OnInstructedToWait", 1f);
        }

        private void Update()
        {
            if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
            {
                _goalKeeper.Invoke_OnBecameTheClosestPlayerToBall();
            }
        }

        private void OnInstructedToWait()
        {
            // go to tend goal state
            _goalKeeper.Invoke_OnInstructedToWait();

            Invoke("OnInstructedGoToHome", 1f);
        }

        private void OnInstructedGoToHome()
        {
            // go to tend goal state
            _goalKeeper.Invoke_OnInstructedGoToHome();
        }
    }
}
