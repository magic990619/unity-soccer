using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.SubStates
{
    public class WaitAtDefendCornerKickPoint : BState
    {
        float _waitTime;

        public override void Enter()
        {
            base.Enter();

            // set wait time
            _waitTime = Random.Range(0.1f, 1f);

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            // stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.Position);
        }

        public override void Execute()
        {
            base.Execute();

            _waitTime -= Time.deltaTime;
            if (_waitTime <= 0f)
                Machine.ChangeState<FindRandomPointToDefendCornerKick>();
        }

        public override void Exit()
        {
            base.Exit();

            // set the animator
            Owner.Animator.ResetTrigger("Idle");
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
