using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.KickBall.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.ChaseBall.SubStates
{
    public class RunDownBall : BState
    {
        public Player Owner { get => ((GoalKeeperFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // set the steering
            Owner.RPGMovement.Speed = Owner.ActualSprintSpeed;
            Owner.RPGMovement.SetSteeringOn();
            Owner.RPGMovement.SetTrackingOn();

            // set animation
            Owner.Animator.SetFloat("Forward", 1f);
            Owner.Animator.SetFloat("Turn", 0f);
            Owner.Animator.SetTrigger("TendGoal");
        }

        public override void Execute()
        {
            base.Execute();

            // update steering target
            Owner.RPGMovement.SetMoveTarget(Ball.Instance.Position);
            Owner.RPGMovement.SetRotateFacePosition(Ball.Instance.Position);

            // if I'm too far from goal 
            //or is no longer closest player to ball
            // go back home
            if (Owner.IsTooFarFromGoalMouth() == true) 
            {
                SuperMachine.ChangeState<GoToHomeMainState>();
            }
            else {

                // if ball is within my range and I'm inside the 18 area, try to get the ball
                // else try to pass or clear it
                if (Owner.IsBallWithinControllableDistance())
                {
                    if (Owner.IsInsideMyTeamEighteenArea() == true)
                    {
                        // go to get ball
                        Machine.ChangeState<GetBall>();
                    }
                    else
                    {
                        // if ball has owner then tackle owner and then get the ball
                        if (Ball.Instance.CurrentOwner != null)
                            Ball.Instance.CurrentOwner.Invoke_OnTackled();

                        // create a pass
                        Pass pass;
                        if (Owner.CanPass(out pass) == true)
                        {
                            Owner.KickDecision = KickDecisions.Pass;
                            Owner.Pass = pass;
                        }
                        else
                        {
                            // create pass
                            Owner.KickDecision = KickDecisions.Clear;
                            Owner.Pass = new Pass()
                            {
                                KickPower = Owner.BallLongPassArriveVelocity * (1 + 1 * Random.value),
                                PassType = PassTypesEnum.Long,
                                ToPosition = Owner.Position + Owner.transform.forward * (Random.value * 40f)
                            };
                        }

                        // go to kick ball state
                        SuperMachine.ChangeState<KickBallMainState>();
                    }
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            //reset stuff
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            // reset the animator
            Owner.Animator.ResetTrigger("TendGoal");
        }
    }
}
