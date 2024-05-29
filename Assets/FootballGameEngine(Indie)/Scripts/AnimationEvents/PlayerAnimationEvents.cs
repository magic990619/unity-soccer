using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.MainState;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.AnimationEvents
{
    public class PlayerAnimationEvents : MonoBehaviour
    {
        public Player Owner;

        public void GoToIdleState()
        {
            if(Owner.PlayerType == PlayerTypes.Goalkeeper)
                Owner.GoalKeeperFSM.ChangeState<IdleMainState>();
        }

        public void Invoke_InteractWithBall()
        {
            Owner.Invoke_OnInstructedToInteractWithBall();
        }

        public void OnAnimatorIK(int layerIndex)
        {
            if (Owner.PlayerType == PlayerTypes.Goalkeeper)
                Owner.GoalKeeperFSM.OnAnimatorExecuteIK(layerIndex);
            else
                Owner.InFieldPlayerFSM.OnAnimatorExecuteIK(layerIndex);
        }

        private void OnAnimatorMove()
        {
            if (Owner.PlayerType == PlayerTypes.Goalkeeper)
                Owner.GoalKeeperFSM.OnAnimatorExecuteMove();
            else
                Owner.InFieldPlayerFSM.OnAnimatorExecuteMove();
        }
    }
}
