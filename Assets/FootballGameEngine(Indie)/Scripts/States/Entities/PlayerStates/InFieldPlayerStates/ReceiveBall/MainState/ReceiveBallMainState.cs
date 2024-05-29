using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState
{
    /// <summary>
    /// The player steers to the pass target and waits for the ball
    /// there. If ball comes within range the player controls the ball. If
    /// the player receives a message that the team has lost control he
    /// goes back to home
    /// </summary>
    public class ReceiveBallMainState : BHState
    {
        float _ballTime;

        public override void AddStates()
        {
            base.AddStates();

            //// set this to be the ball owner
            //Ball.Instance.CurrentOwner = Owner;
            //Owner.Team.ReceivingPlayer = Owner;

            //add the state
            AddState<SteerToReceiveTarget>();
            AddState<WaitForBallAtReceiveTarget>();

            //set the initial state
            SetInitialState<SteerToReceiveTarget>();
        }

        public override void Enter()
        {
            base.Enter();

            // set this to be the ball owner
            Ball.Instance.CurrentOwner = Owner;
            Owner.Team.ReceivingPlayer = Owner;

            //register to some player events
            Owner.OnTeamLostControl += Instance_OnTeamLostControl;
            Owner.OnInstructedToWait += Instance_OnWait;
        }

        public override void Execute()
        {
            base.Execute();

            // decrement ball trap time
            if (_ballTime > 0)
            {
                _ballTime -= Time.deltaTime;
                GetState<SteerToReceiveTarget>().BallTime = _ballTime;
            }

            //trap the ball if it is now in a trapping distance
             if (Owner.IsBallWithinControllableDistance())
                 Machine.ChangeState<ControlBallMainState>();
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // if we have exhausted ball time, chase down ball
            if (_ballTime <= 0f)
                Machine.ChangeState<ChaseBallMainState>();
        }

        public override void Exit()
        {
            base.Exit();
            
            // set some stuff
            Owner.Team.ReceivingPlayer = null;

            //stop listing to some player events
            Owner.OnTeamLostControl -= Instance_OnTeamLostControl;
            Owner.OnInstructedToWait -= Instance_OnWait;
        }

        private void Instance_OnTeamLostControl()
        {
            SuperMachine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }

        public void SetSteeringTarget(float ballTime, Vector3 position)
        {
            // get ball-time
            _ballTime = ballTime + 5;

            // calculate steering speed
            float distanceToTarget = Vector3.Distance(Owner.Position, position);
            float requiredSpeed = distanceToTarget / ballTime;
            float actualSpeed = Mathf.Clamp(requiredSpeed, Owner.ActualJogSpeed, Owner.ActualSprintSpeed);
            Owner.RPGMovement.Speed = actualSpeed;

            // set the steering target
            GetState<SteerToReceiveTarget>().BallTime = _ballTime;
            GetState<SteerToReceiveTarget>().SteeringTarget = position;
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
