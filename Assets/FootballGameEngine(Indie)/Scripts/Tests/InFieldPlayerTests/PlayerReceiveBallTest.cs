using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    /// <summary>
    /// Tests the player logic to receive the ball 
    /// 
    /// </summary>
    public class PlayerReceiveBallTest : MonoBehaviour
    {
        public Ball ball;
        public Transform refPoint;
        public Transform homePosition;
        public Player PlayerControlling;

        public Action InstructToWait;
        public Action InstructLostControl;

        private void Awake()
        {
            Invoke("Init", 1f);
        }

        private void Update()
        {
            //invoke wait
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.W))
                ActionUtility.Invoke_Action(InstructToWait);

            //invoke team lost control
            if (ControlFreak2.CF2Input.GetKeyDown(KeyCode.L))
                ActionUtility.Invoke_Action(InstructLostControl);

            if (ControlFreak2.CF2Input.GetMouseButtonDown(0))
            {
                //set-up the rays
                Ray ray = Camera.main.ScreenPointToRay(ControlFreak2.CF2Input.mousePosition);
                RaycastHit hitInfo;

                //check for hits
                if (Physics.Raycast(ray, out hitInfo))
                {
                    //raise event if the hit target is the ground
                    if(hitInfo.transform.gameObject.CompareTag("Ground"))
                    {
                        refPoint.position = hitInfo.point;
                        PlayerControlling.OnInstructedToReceiveBall.Invoke(1f, hitInfo.point);

                        //move ball to target
                        StartCoroutine(MoveBallToPoint(hitInfo.point));
                    }
                }

            }
        }

        public IEnumerator MoveBallToPoint(Vector3 position)
        {
            while(true)
            {
                ball.transform.position = Vector3.MoveTowards(ball.NormalizedPosition,
                    position,
                    3f * Time.deltaTime);

                yield return new WaitForSeconds(0f);
            }
        }

        private void Init()
        {
            InstructToWait += PlayerControlling.Invoke_OnInstructedToWait;
            InstructLostControl += PlayerControlling.Invoke_OnTeamLostControl;

            //PlayerControlling.Init(15f, 5f, 15f, 5f, 10f, 5f);
            PlayerControlling.Init();
        }
    }
}
