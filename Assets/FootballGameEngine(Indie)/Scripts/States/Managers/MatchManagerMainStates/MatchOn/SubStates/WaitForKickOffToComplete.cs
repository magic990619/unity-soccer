using System;
using Assets.FootballGameEngine_Indie.Scripts.Controllers;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates
{
    /// <summary>
    /// Waits for the kick-off event to be raised by either of the teams
    /// If the kickoff event is raised then it triggers a state change to
    /// the exhaust first half state
    /// </summary>
    public class WaitForKickOffToComplete : BState
    {
        bool hasInvokedKickOffEvent;
        float waitTime;


        public override void Enter()
        {
            base.Enter();

            // set hasn't invoked kick off event
            hasInvokedKickOffEvent = false;
            //Me Added
            once = false;
            if (MyGameManager.Instance.isSceneplaying)
            {
                waitTime = 19f;
            }else
            {
                once = true;
                waitTime = 1f;
            }
          
            CameraController.Instance.CanAutoFollowTarget = false;

            // put the ball at the kick-off position
            Ball.Instance.Trap();
            Ball.Instance.Position = Owner.TransformCentreSpot.position;

            //register the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff += Owner.TeamAway.Invoke_OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff += Owner.TeamHome.Invoke_OnMessagedToTakeKickOff;

            //listen to team OnTakeKickOff events
            Owner.TeamAway.OnTakeKickOff += Instance_OnTeamTakeKickOff;
            Owner.TeamHome.OnTakeKickOff += Instance_OnTeamTakeKickOff;
        }
        bool once;
        public override void Execute()
        {
            base.Execute();

            if(hasInvokedKickOffEvent == false)
            {
                waitTime -= Time.deltaTime;
                Debug.Log("Cutscene is playing Now ..................");
                if (!MyGameManager.Instance.isSceneplaying)
                {
                    if (!once)
                    {
                        Debug.Log("Once = fals ..................");
                        waitTime = 0f;
                        once = true;
                        SoundManager.Instance.StopAudioClip(7);
                        SoundManager.Instance.PlayAudioClip(6);
                        SoundManager.Instance.PlayAudioClip(8);

                    } 
                }
                if (waitTime < 0)
                {
                    // Me Hide
                    // raise the take kick-off event
                    hasInvokedKickOffEvent = true;
                    ActionUtility.Invoke_Action(Owner.OnBroadcastTakeKickOff);

                    // calculate the place to place the camera
                    Vector3 cameraPosition = CameraController.Instance.CalculateNextPosition(Ball.Instance.Position);
                    CameraController.Instance.Position = cameraPosition;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset sm stuff
            CameraController.Instance.CanAutoFollowTarget = true;

            //deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeKickOff -= Owner.TeamAway.Invoke_OnMessagedToTakeKickOff;
            Owner.OnBroadcastTakeKickOff -= Owner.TeamHome.Invoke_OnMessagedToTakeKickOff;

            //stop listening to team OnInit events
            Owner.TeamAway.OnTakeKickOff -= Instance_OnTeamTakeKickOff;
            Owner.TeamHome.OnTakeKickOff -= Instance_OnTeamTakeKickOff;
        }

        private void Instance_OnTeamTakeKickOff()
        {
            Machine.ChangeState<ExhaustHalf>();
        }

        public MatchManager Owner
        {
            get
            {
                return ((MatchManagerFSM)SuperMachine).Owner;
            }
        }
    }
}
