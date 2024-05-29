using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.KickBall.MainState
{
    public class KickBallMainState : BHState
    {
        public override void AddStates()
        {
            base.AddStates();

            //add states
            AddState<CheckKickType>();
            AddState<PassBall>();
            AddState<RecoverFromKick>();
            AddState<RotateToFaceTarget>();
            AddState<ShootBall>();

            //set initial state
            SetInitialState<RotateToFaceTarget>();
        }

        public override void Enter()
        {
            // set the Ball
            Ball.Instance.Trap();

            //listen to some events
            Owner.OnInstructedToWait += Instance_OnWait;

            // set controlling player
            Owner.Team.ControllingPlayer = Owner;

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            // run enter
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            //listen to some events
            Owner.OnInstructedToWait -= Instance_OnWait;

            // reset controlling player
            Owner.Team.ControllingPlayer = null;

            // reset the animator
            Owner.Animator.ResetTrigger("Idle");
            Owner.Animator.ResetTrigger("PassBall");
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
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
