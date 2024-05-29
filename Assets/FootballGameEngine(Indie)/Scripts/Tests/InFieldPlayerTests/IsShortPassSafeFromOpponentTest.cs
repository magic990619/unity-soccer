using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    public class IsShortPassSafeFromOpponentTest : MonoBehaviour
    {
        public Player _primaryPlayer;
        public Player _secondaryPlayer;
        public Player _oppositionPlayer;

        public GameObject _passSafetyHighlight;
        public Transform _passTarget;

        private void Awake()
        {
            ////init primary player
            //_primaryPlayer.Init(0.5f,
            //    25f,
            //    5f,
            //    15f,
            //    1f,
            //    5f,
            //    1f,
            //    0.5f,
            //    15f, // max wonder distance
            //    25f,
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
            //    new InGamePlayerDto()
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
        }

        void Update()
        {
            if(ControlFreak2.CF2Input.GetKeyDown(KeyCode.P))
            {
                bool canPass = _primaryPlayer.CanMakeShortPass(_primaryPlayer.Position,
                    _secondaryPlayer.Position,
                    _secondaryPlayer);

                //bool canPass = _primaryPlayer.IsShortBallKickSafeFromOpponent(_primaryPlayer.Position,
                //    _secondaryPlayer.Position,
                //    _oppositionPlayer.Position,
                //    _secondaryPlayer.Position,
                //    20f);

                if (canPass)
                {
                    _passSafetyHighlight.GetComponent<LineRenderer>().SetPosition(0, _primaryPlayer.Position);
                    _passSafetyHighlight.GetComponent<LineRenderer>().SetPosition(1, _secondaryPlayer.Position);

                    _passSafetyHighlight.GetComponent<LineRenderer>().material.color = Color.green;

                    Debug.Log(string.Format("<color=green>Can pass: {0}</color>", canPass));
                }
                else
                {
                    _passSafetyHighlight.GetComponent<LineRenderer>().SetPosition(0, _primaryPlayer.Position);
                    _passSafetyHighlight.GetComponent<LineRenderer>().SetPosition(1, _secondaryPlayer.Position);

                    _passSafetyHighlight.GetComponent<LineRenderer>().material.color = Color.red;

                    Debug.Log(string.Format("<color=red>Can pass: {0}</color>", canPass));
                }
            }
        }
    }
}
