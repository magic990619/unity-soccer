using Assets.FootballGameEngine_Indie.Scripts.Controllers;
using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using RobustFSM.Base;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.SubStates
{
    public class TakeCornerKick : BState
    {
        bool isExecuted;
        bool isCornerKickTaken;

        float waitTime = 1f;

        Action InstructPlayerToTakeCornerKick;

        public override void Enter()
        {
            base.Enter();

            // update players with certain data
            Owner.Players.ForEach(tM => 
            { 
                // tell players to support corner kick
                if (tM.Player != Owner.ControllingPlayer)
                    tM.Player.Invoke_OnInstructedToSupportCornerKick();
            });

            // set to unexecuted
            isCornerKickTaken = false;
            isExecuted = false;

            // set up controlling player position
            Owner.ControllingPlayer.OnTakeCornerKick += Instance_OnPlayerTakeCornerKick;
            InstructPlayerToTakeCornerKick += Owner.ControllingPlayer.Invoke_OnInstructedToTakeCornerKick;
        }

        public override void Execute()
        {
            base.Execute();

            // if not executed then run logic
            if (isExecuted == false)
            {
                // decrement time
                waitTime -= Time.deltaTime;

                if (waitTime <= 0)
                {
                    // set to executed
                    isExecuted = true;

                    // trigger player to take throw-in
                    ActionUtility.Invoke_Action(InstructPlayerToTakeCornerKick);
                }
            }
            else if(isCornerKickTaken)
            {
                waitTime -= Time.deltaTime;
                if(waitTime <= 0f)
                {
                    // trigger state change to attack
                    SuperMachine.ChangeState<DefendMainState>();

                    //simply raise that I have taken the kick-off
                    ActionUtility.Invoke_Action(Owner.OnTakeCornerKick);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // track the ball again
            CameraController.Instance.Target = Ball.Instance.transform;

            // deregister player from listening to take-kickoff action
            Owner.ControllingPlayer.OnTakeCornerKick -= Instance_OnPlayerTakeCornerKick;
            InstructPlayerToTakeCornerKick -= Owner.ControllingPlayer.Invoke_OnInstructedToTakeGoalKick;
        }

        private void Instance_OnPlayerTakeCornerKick(float ballTime, Vector3? position, Player receiver)
        {
            isCornerKickTaken = true;
            waitTime = ballTime + UnityEngine.Random.value * 0.05f;
        }

        public FootballGameEngine_Indie.Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
