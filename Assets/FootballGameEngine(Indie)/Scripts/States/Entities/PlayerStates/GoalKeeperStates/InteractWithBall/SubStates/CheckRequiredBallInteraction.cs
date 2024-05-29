using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.MainState;
using RobustFSM.Base;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.GoalKeeperStates.InteractWithBall.SubStates
{
    public class CheckRequiredBallInteraction : BState
    {
        bool _hasResetPunchBallState;
        bool _isBallCatchable;
        bool _isBallInteractable;

        public bool IsBallCatchable { get => _isBallCatchable; set => _isBallCatchable = value; }
        public bool IsBallInteractable { get => _isBallInteractable; set => _isBallInteractable = value; }

        public override void Enter()
        {
            base.Enter();

            // set some variables
            _hasResetPunchBallState = false;

            // if ball is catchable then catch the ball else punch it
            if (_isBallInteractable)
            {
                if (_isBallCatchable)
                    Machine.ChangeState<CatchBall>();
                else
                    Machine.ChangeState<PunchBall>();
            }
            else
            {
                //set the animator to exit the dive state
                Owner.Animator.SetTrigger("Exit");
            }
        }


        public override void Execute()
        {
            base.Execute();

            //if (_hasResetPunchBallState == false)
            //{
            //    if (Owner.Animator.GetCurrentAnimatorStateInfo(0).IsName("PunchBall"))
            //    {
            //        _hasResetPunchBallState = true;

            //        Owner.Animator.SetTrigger("Exit");
            //    }
            //}
        }

        Player Owner
        {
            get
            {
                return ((GoalKeeperFSM)SuperMachine).Owner;
            }
        }
    }
}
