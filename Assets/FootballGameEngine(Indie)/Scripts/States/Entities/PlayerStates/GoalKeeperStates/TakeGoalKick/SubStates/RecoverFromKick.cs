using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.TakeGoalKick.SubStates
{
    public class RecoverFromKick : BState
    {
        float waitTime;

        public override void Enter()
        {
            base.Enter();

            //set the wait time 
            waitTime = 0.5f;
        }

        public override void Execute()
        {
            base.Execute();

            //decrement time
            waitTime -= Time.deltaTime;

            //go to home after state
            if (waitTime <= 0f)
                SuperMachine.ChangeState<GoToHomeMainState>();
        }
    }
}
