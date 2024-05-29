using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InterceptShot.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.MainState;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.SubStates
{
    public class PunchBall : BState
    {
        float _time;
        float _turn;
        Vector3 _leftHandTargetPosition;
        Vector3 _rightHandTargetPosition;

        public override void Enter()
        {
            base.Enter();

            _time = 0f;

            ////get some important data
            //_leftHandTargetPosition =((InteractWithBallMainState)Machine).InterceptShotMainState.LeftHandTargetPosition.position;
            //_rightHandTargetPosition =((InteractWithBallMainState)Machine).InterceptShotMainState.RightHandTargetPosition.position;
            //_turn =((InteractWithBallMainState)Machine).InterceptShotMainState.Turn;

            //calculate the punch direction
            Vector3 ballRelativePosition = Owner.transform.InverseTransformPoint(Ball.Instance.Position);
            Vector3 ballPunchDirection = Vector3.zero;

            //detemine the punch direction
            if (Mathf.Abs(ballRelativePosition.x) > 0.1f)
            {
                //simply punch it to the side
                ballPunchDirection = new Vector3(ballRelativePosition.x, 0f, 0f);
            }
            else
            {
                //if it's less than my height then punch it infront of me else punch up
                if (ballRelativePosition.y <= Owner.Height)
                    ballPunchDirection = new Vector3(0f, 0f, 1f);
                else
                    ballPunchDirection = new Vector3(0f, 1f, -1f);
            }

            //punch the ball
            ballPunchDirection = Owner.transform.TransformDirection(ballPunchDirection);
            Ball.Instance.KickInDirection(ballPunchDirection,
                0.5f * Ball.Instance.Rigidbody.velocity.magnitude);

            //set the animator to exit the dive state
            Owner.Animator.SetTrigger("Exit");

            //raise the punch ball event
            Owner.Invoke_OnPunchBall();
            Owner.Invoke_OnControlBall();
        }

        Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
