using Assets.FootballGameEngine_Indie.Scripts.Controllers;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Managers;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Managers;
using Assets.FootballGameEngine_Indie.Scripts.States.Managers.MatchManagerMainStates.MatchOn.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Managers.MatchManagerMainState.MatchOn.SubStates
{
    public class WaitForGoalKickToComplete : BState
    {
        bool hasInvokedTakeGoalKickEvent;
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            // set hasn't invoked kick off event
            hasInvokedTakeGoalKickEvent = false;
            waitTime = 1f;
            CameraController.Instance.CanAutoFollowTarget = false;

            //register the teams to listen to the take-off events
            Owner.OnBroadcastTakeGoalKick += Owner.TeamAway.Invoke_OnMessagedToTakeGoalKick;
            Owner.OnBroadcastTakeGoalKick += Owner.TeamHome.Invoke_OnMessagedToTakeGoalKick;

            //listen to team OnTakeKickOff events
            Owner.TeamAway.OnTakeGoalKick += Instance_OnTeamTakeGoalKick;
            Owner.TeamHome.OnTakeGoalKick += Instance_OnTeamTakeGoalKick;
        }

        public override void Execute()
        {
            base.Execute();

            if (hasInvokedTakeGoalKickEvent == false)
            {
                waitTime -= Time.deltaTime;
                if (waitTime < 0)
                {
                    // invoke take goal-kick
                    hasInvokedTakeGoalKickEvent = true;
                    ActionUtility.Invoke_Action(Owner.OnBroadcastTakeGoalKick);

                    // calculate the place to place the camera
                    Vector3 cameraPosition = CameraController.Instance.CalculateNextPosition(Ball.Instance.NormalizedPosition);
                    CameraController.Instance.Position = cameraPosition;
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset some stuff
            CameraController.Instance.CanAutoFollowTarget = true;

            //deregister the teams to listen to the take-off events
            Owner.OnBroadcastTakeGoalKick -= Owner.TeamAway.Invoke_OnMessagedToTakeGoalKick;
            Owner.OnBroadcastTakeGoalKick -= Owner.TeamHome.Invoke_OnMessagedToTakeGoalKick;

            //delisten to team OnTakeKickOff events
            Owner.TeamAway.OnTakeGoalKick -= Instance_OnTeamTakeGoalKick;
            Owner.TeamHome.OnTakeGoalKick -= Instance_OnTeamTakeGoalKick;
        }

        private void Instance_OnTeamTakeGoalKick()
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
