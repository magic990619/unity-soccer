using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tests.GoalkeeperTests
{
    public class GoalKeeperFullTest : MonoBehaviour
    {
        [SerializeField]
        float _power = 15;

        [SerializeField]
        Player _goalKeeper;

        private void Awake()
        {
            //// inti the goalkeeper
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
            //    3f,
            //    3f,
            //    4f,
            //    0.5f,
            //    0.6f, 
            //    0.1f,
            //    0.5f,
            //    30f,
            //    0.1f,
            //    5f,
            //    null,
            //    null,
            //    new Data.Dtos.InGame.Entities.InGamePlayerDto()
            //    {
            //        Accuracy = 1f,
            //        GoalKeeping = 1f,
            //        JumpHeight = 1f,
            //        DiveDistance = 1f,
            //        DiveSpeed = 1f,
            //        Power = 1f,
            //        Reach = 1f,
            //        Speed = 1f,
            //        Tackling = 1f
            //    });

            // enable tje keeper
            _goalKeeper.gameObject.SetActive(true);

            Invoke("OnInstructedToWait", 1f);
        }

        private void Update()
        {
            if(ControlFreak2.CF2Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition);
                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo))
                {
                    float flightTime = Vector3.Distance(Ball.Instance.Position,
                        hitInfo.point)
                        / _power;

                    Ball.Instance.LaunchToPoint(hitInfo.point,
                        _power);

                    Shot shot = new Shot()
                    {
                        BallTimeToTarget = flightTime,
                        KickPower = _power,
                        FromPosition = Ball.Instance.Position,
                        ToPosition = hitInfo.point
                    };

                    _goalKeeper.Invoke_OnShotTaken(shot);

                    //_goalKeeper.Invoke_OnShotTaken(flightTime,
                    //    _power,
                    //    Ball.Instance.Position,
                    //    hitInfo.point);

                    //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    //obj.transform.position = hitInfo.point;
                    //GameObject.Destroy(obj, 1f);
                }
            }

            //if (Input.GetKeyDown(KeyCode.P))
            //    _goalKeeper.Invoke_OnInstructedToPutBallBackIntoPlay();
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
