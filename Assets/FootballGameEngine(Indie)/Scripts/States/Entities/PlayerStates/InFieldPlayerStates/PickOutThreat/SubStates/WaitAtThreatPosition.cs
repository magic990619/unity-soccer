using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.SubStates
{
    public class WaitAtThreatPosition : BState
    {
        Vector3 _steeringTarget;

        public Vector3 SteeringTarget { get => _steeringTarget; set => _steeringTarget = value; }
        public Player Owner { get => ((InFieldPlayerFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            //stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOn();

            // set the animator
            Owner.Animator.SetTrigger("Idle");
        }

        public override void Execute()
        {
            base.Execute();

            //update the track position
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.NormalizedPosition);
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // if it's a setpiece then don't stand too close to the ball
            if (Owner.IsAtTarget(_steeringTarget) == false)
                Machine.ChangeState<SteerToThreat>();
        }

        public override void Exit()
        {
            base.Exit();

            // set the animator
            Owner.Animator.ResetTrigger("Idle");
        }

    }
}
