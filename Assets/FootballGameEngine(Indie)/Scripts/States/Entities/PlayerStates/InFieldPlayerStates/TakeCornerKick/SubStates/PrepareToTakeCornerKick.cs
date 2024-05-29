using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeCornerKick.SubStates
{
    public class PrepareToTakeCornerKick : BState
    {
        float waitTime = 1f;

        public override void Execute()
        {
            base.Execute();

            waitTime -= Time.deltaTime;
            if (waitTime <= 0f)
            {
                // if I'm user controlled then go to manual take throw
                Machine.ChangeState<AutomaticTakeCornerKick>();
                //if (Owner.IsUserControlled == true)
                //    Machine.ChangeState<ManualTakeCornerKick>();
                //else
                //    Machine.ChangeState<AutomaticTakeCornerKick>();
            }
        }

        public Player Owner
        {
            get
            {
                return ((InFieldPlayerFSM)SuperMachine).Owner;
            }
        }
    }
}
