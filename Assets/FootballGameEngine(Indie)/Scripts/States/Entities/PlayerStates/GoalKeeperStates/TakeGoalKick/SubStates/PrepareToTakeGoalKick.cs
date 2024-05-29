using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.SubStates
{
    public class PrepareToTakeGoalKick : BState
    {
        float waitTime = 1f;

        public override void Execute()
        {
            base.Execute();

            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                // if I'm user controlled then go to manual take throw
                Machine.ChangeState<AutomaticTakeGoalKick>();
                //if (Owner.IsUserControlled == true)
                //    Machine.ChangeState<ManualTakeGoalKick>();
                //else
                //    Machine.ChangeState<AutomaticTakeGoalKick>();
            }
        }

        public Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
