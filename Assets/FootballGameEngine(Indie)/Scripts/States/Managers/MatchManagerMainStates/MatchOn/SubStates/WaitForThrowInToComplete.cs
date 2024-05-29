using Assets.FootballGameEngine_Indie.Scripts.Controllers;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;
using static Assets.FootballGameEngine_Indie.Scripts.Managers.MatchManager;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates
{
    /// <summary>
    /// Waits for the kick-off event to be raised by either of the teams
    /// If the kickoff event is raised then it triggers a state change to
    /// the exhaust first half state
    /// </summary>
    public class WaitForThrowInToComplete : BState
    {
        bool hasInvokedTakeThrowInEvent;
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            // set hasn't invoked kick off event
            hasInvokedTakeThrowInEvent = false;
            waitTime = 1f;
            CameraController.Instance.CanAutoFollowTarget = false;

            //register the teams to listen to the take-off events
            Owner.OnBroadcastTakeThrowIn += Owner.TeamAway.Invoke_OnMessagedToTakeKThrowIn;
            Owner.OnBroadcastTakeThrowIn += Owner.TeamHome.Invoke_OnMessagedToTakeKThrowIn;

            //listen to team OnTakeKickOff events
            Owner.TeamAway.OnTakeThrowIn += Instance_OnTeamTakeThrowIn;
            Owner.TeamHome.OnTakeThrowIn += Instance_OnTeamTakeThrowIn;
        }

        public override void Execute()
        {
            base.Execute();

            if(hasInvokedTakeThrowInEvent == false)
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0) 
                {
                    var temp = Owner.OnBroadcastTakeThrowIn;
                    if (temp != null)
                    {
                        // raise the throw in event
                        hasInvokedTakeThrowInEvent = true;
                        ActionUtility.Invoke_Action(Owner.CachedBallPosition, Owner.OnBroadcastTakeThrowIn);

                        // calculate the place to place the camera
                        Vector3 cameraPosition = CameraController.Instance.CalculateNextPosition(Owner.CachedBallPosition);
                        CameraController.Instance.Position = cameraPosition;
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset sm stuff
            CameraController.Instance.CanAutoFollowTarget = true;

            //deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeThrowIn -= Owner.TeamAway.Invoke_OnMessagedToTakeKThrowIn;
            Owner.OnBroadcastTakeThrowIn -= Owner.TeamHome.Invoke_OnMessagedToTakeKThrowIn;

            //stop listening to team OnInit events
            Owner.TeamAway.OnTakeThrowIn -= Instance_OnTeamTakeThrowIn;
            Owner.TeamHome.OnTakeThrowIn -= Instance_OnTeamTakeThrowIn;
        }

        private void Instance_OnTeamTakeThrowIn()
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
