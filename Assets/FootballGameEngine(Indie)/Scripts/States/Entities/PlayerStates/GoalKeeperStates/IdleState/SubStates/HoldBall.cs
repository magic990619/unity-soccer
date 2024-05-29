using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.PutBallBackIntoPlay.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.IdleState.SubStates
{
    public class HoldBall : BState
    {
        private bool _isSteering;
        Vector3 _steeringTarget;

        Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // register to the OnInstructedToPutBallBackIntoPlay event
            Owner.OnInstructedToPutBallBackIntoPlay += Instance_OnInstructedToPutBallBackIntoPlay;
        }

        public override void Execute()
        {
            base.Execute();

            // update ball position to right ball ik
            Ball.Instance.Position = Owner.BallRightHand.position;
            Ball.Instance.transform.rotation = Owner.BallLeftHand.rotation;

            // move higher up the pitch if I'm too close to goal
            float distanceToGoalZ = Owner.TeamGoal.transform.InverseTransformPoint(Owner.Position).z;
            float distanceToGoal = Vector3.Distance(Owner.Position, Owner.TeamGoal.Position);

            // move player upfield if too close to goal
            if (distanceToGoalZ <= 5f && distanceToGoal <= 4.15f)
            {
                //todo::Movement logic should utilize the rpgmovement class. Unfortunately if I try to move the character using this logic it does't work. Research on this one.
                if (_isSteering == false)
                {
                    // set movement
                    _isSteering = true;
                    _steeringTarget = Owner.Position + Owner.TeamGoal.transform.forward * 5f;

                    Owner.Animator.SetFloat("Forward", 0.95f);
                    Owner.Animator.SetFloat("Turn", 0f);

                    Owner.Animator.ResetTrigger("Idle");
                    Owner.Animator.SetTrigger("TendGoal");
                }
                else
                {
                    // move to target
                    Owner.transform.position = Vector3.MoveTowards(Owner.Position,
                        _steeringTarget,
                        Time.deltaTime * Owner.ActualJogSpeed);
                }
            }
            else
            {
                // If I'm steering then stop
                if(_isSteering == true)
                {
                    //_isSteering = false;
                    Owner.Animator.ResetTrigger("TendGoal");
                    Owner.Animator.SetTrigger("Idle");
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // deregister to the OnInstructedToPutBallBackIntoPlay event
            Owner.OnInstructedToPutBallBackIntoPlay -= Instance_OnInstructedToPutBallBackIntoPlay;
        }

        private void Instance_OnInstructedToPutBallBackIntoPlay()
        {
            SuperMachine.ChangeState<PutBallBackIntoPlayMainState>();
        }
    }
}
