using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerReturnToHomeTest : MonoBehaviour
    {
        public Player PlayerControlling;

        private void Start()
        {
            Invoke("Init", 1f);
        }

        void Init()
        {
            PlayerControlling.InFieldPlayerFSM.SetCurrentState<GoToHomeMainState>();
            PlayerControlling.InFieldPlayerFSM.GetState<GoToHomeMainState>().Enter();
            //PlayerControlling.Init(15f, 5f, 15f, 5f, 10f, 5f);
            PlayerControlling.Init();
        }
    }
}
