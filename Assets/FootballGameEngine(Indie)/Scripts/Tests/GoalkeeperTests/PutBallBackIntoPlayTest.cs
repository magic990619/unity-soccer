using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tests.GoalkeeperTests
{
    public class PutBallBackIntoPlayTest : MonoBehaviour
    {
        public float maxShortPassDistance = 20f;

        public GameObject _passHighlight;
        public Player _goalKeeper;

        bool isReady;

        private void Start()
        {
            Invoke("ResetStuff", 1f);
        }

        private void Update()
        {

            if(ControlFreak2.CF2Input.GetMouseButtonDown(0) && isReady == true)
            {
                isReady = false;

                RaycastHit raycastHit;
                Ray ray = Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition);

                if(Physics.Raycast(ray, out raycastHit))
                {
                    _passHighlight.transform.position = raycastHit.point;

                    float distanceToTarget = Vector3.Distance(_goalKeeper.Position, raycastHit.point);
                    Pass pass = new Pass()
                    {
                        ToPosition = _passHighlight.transform.position,
                        PassType = distanceToTarget <= maxShortPassDistance ? PassTypesEnum.Short : PassTypesEnum.Long,
                        KickPower = 15f
                    };

                    _goalKeeper.Invoke_OnInstructedToPutBallBackIntoPlay();

                    // cool down
                    Invoke("ResetStuff", 5f);
                }
            }
        }

        private void ResetStuff()
        {
            isReady = true;

            Ball.Instance.DisablePhysics();

            _goalKeeper.HasBall = true;

            _goalKeeper.RPGMovement.Acceleration = 1f;
            _goalKeeper.RPGMovement.RotationSpeed = 6f;
            _goalKeeper.RPGMovement.Speed = 3f;

            _goalKeeper.GoalKeeperFSM.ChangeState<IdleMainState>();
        }
    }
}
