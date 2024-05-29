using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.MainState;
using RobustFSM.Base;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.GoalKick.SubStates
{
    public class TakeGoalKick : BState
    {
        bool executed;
        float waitTime = 1f;

        Action InstructPlayerToTakeGoalKick;

        public TeamPlayer ControllingPlayer { get; set; }

        public override void Enter()
        {
            base.Enter();

            // set to unexecuted
            executed = false;

            // set up controlling player position
            ControllingPlayer.Player.OnTakeGoalKick += Instance_OnPlayerTakeGoalKick;
            InstructPlayerToTakeGoalKick += ControllingPlayer.Player.Invoke_OnInstructedToTakeGoalKick;
        }

        public override void Execute()
        {
            base.Execute();

            // if not executed then run logic
            if (!executed)
            {
                // decrement time
                waitTime -= Time.deltaTime;

                if (waitTime <= 0)
                {
                    // set to executed
                    executed = true;

                    // trigger player to take throw-in
                    ControllingPlayer.Player.GoalKeeperFSM.ChangeState<TakeGoalKickMainState>();
                    //ActionUtility.Invoke_Action(InstructPlayerToTakeGoalKick);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // deregister player from listening to take-kickoff action
            ControllingPlayer.Player.OnTakeGoalKick -= Instance_OnPlayerTakeGoalKick;
            InstructPlayerToTakeGoalKick -= ControllingPlayer.Player.Invoke_OnInstructedToTakeGoalKick;
        }

        private void Instance_OnPlayerTakeGoalKick()
        {
            // trigger state change to attack
            SuperMachine.ChangeState<AttackMainState>();

            //simply raise that I have taken the kick-off
            ActionUtility.Invoke_Action(Owner.OnTakeGoalKick);
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
