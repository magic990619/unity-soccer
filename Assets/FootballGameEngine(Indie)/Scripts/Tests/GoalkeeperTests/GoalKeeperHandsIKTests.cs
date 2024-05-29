using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tests.GoalkeeperTests
{
    public class GoalKeeperHandsIKTests : MonoBehaviour
    {
        [SerializeField]
        Player _goalKeeper;

        private void Awake()
        {
            // inti the goalkeeper
            //_goalKeeper.Init(0f,
            //    0f,
            //    0f,
            //    0f,
            //    10,
            //    5f,
            //    1f,
            //    10,
            //    0f,
            //    0f,
            //    3f, 
            //    3f,
            //    3f,
            //    2f,
            //    0.1f,
            //    0.5f,
            //    0.1f,
            //    0f,
            //    0.5f,
            //    3.5f,
            //    null,
            //    new PlayerDto()
            //    {
            //        DiveSpeed = 1f,
            //        GoalKeeping = 1f,
            //        DiveDistance = 2f,
            //        JumpHeight = 0.5f,
            //        Reach = 1f,
            //        Speed = 1f
            //    });

            // enable the keeper
            _goalKeeper.gameObject.SetActive(true);
        }

    }
}
