using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tests.GoalkeeperTests
{
    public class GoalKeeperCounterKickTest : MonoBehaviour
    {
        [SerializeField]
        float _power = 15;

        [SerializeField]
        Player _goalKeeper;

        private void Awake()
        {
            // inti the goalkeeper
            //_goalKeeper.Init(70f,
            //    5f,
            //    0f,
            //    0f,
            //    10,
            //    5f,
            //    1f,
            //    10,
            //    5f,
            //    0f,
            //    3f, 
            //    3f,
            //    3f,
            //    2f,
            //    0.1f,
            //    0.5f,
            //    0.1f,
            //    50f,
            //    0.5f,
            //    3.5f,
            //    null,
            //    new PlayerDto()
            //    {
            //        DiveSpeed = 1f,
            //        GoalKeeping = 1f,
            //        DiveDistance = 2f,
            //        JumpHeight = 0.5f,
            //        Power = 1f,
            //        Reach = 1f,
            //        Speed = 1f
            //    });

        }

        private void Start()
        {
            // set keeper has ball
            _goalKeeper.HasBall = true;
            Ball.Instance.DisablePhysics();

            // enable tje keeper
            _goalKeeper.gameObject.SetActive(true);

            Invoke("OnInstructedToIdle", 1f);
        }

        private void Update()
        {
            //if (Input.GetMouseButtonDown(0))
            //    _goalKeeper.Invoke_OnInstructedToPutBallBackIntoPlay();
        }

        private void OnInstructedToIdle()
        {
            // go to tend goal state
            _goalKeeper.GoalKeeperFSM.ChangeState<IdleMainState>(); 
        }
    }
}
