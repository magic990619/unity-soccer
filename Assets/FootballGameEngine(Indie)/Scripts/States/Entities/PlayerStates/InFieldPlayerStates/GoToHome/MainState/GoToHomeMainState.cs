using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.SubStates;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState
{
    public class GoToHomeMainState : BHState
    {
        PickOutThreatMainState _pickOutThreatMainState;
        SupportAttackerMainState _supportAttackerMainState;

        public Player Owner { get => ((InFieldPlayerFSM)SuperMachine).Owner; }

        public override void AddStates()
        {
            base.AddStates();

            //add the states
            AddState<SteerToHome>();
            AddState<WaitAtHome>();

            //set the initial state
            SetInitialState<SteerToHome>();
        }

        public override void Initialize()
        {
            base.Initialize();

            // set the initial states
            _pickOutThreatMainState = SuperMachine.GetState<PickOutThreatMainState>();
            _supportAttackerMainState = SuperMachine.GetState<SupportAttackerMainState>();
        }

        public override void Enter()
        {
            base.Enter();

            // set steering
            Owner.RPGMovement.Speed = Owner.ActualJogSpeed;

            //listen to variaus events
            Owner.OnBecameTheClosestPlayerToBall += Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToReceiveBall += Instance_OnInstructedToReceiveBall;
            Owner.OnGoToSupportSpot += Instance_OnGoToSupportSpot;
            Owner.OnTrackThreat += Instance_OnTrackThreat;
            Owner.OnInstructedToWait += Instance_OnWait;

        }

        public override void Exit()
        {
            base.Exit();

            //stop listening to variaus events
            Owner.OnBecameTheClosestPlayerToBall -= Instance_OnBecameTheClosestPlayerToBall;
            Owner.OnInstructedToReceiveBall -= Instance_OnInstructedToReceiveBall;
            Owner.OnGoToSupportSpot -= Instance_OnGoToSupportSpot;
            Owner.OnTrackThreat -= Instance_OnTrackThreat;
            Owner.OnInstructedToWait -= Instance_OnWait;
        }

        private void Instance_OnBecameTheClosestPlayerToBall()
        {
            Machine.ChangeState<ChaseBallMainState>();
        }

        private void Instance_OnGoToSupportSpot(Player controllingPlayer, SupportSpot supportSpot)
        {
            _supportAttackerMainState.ControllingPlayer = controllingPlayer;
            _supportAttackerMainState.SupportSpot = supportSpot;
            Machine.ChangeState<SupportAttackerMainState>();
        }

        private void Instance_OnInstructedToReceiveBall(float ballTime, Vector3 position)
        {
            //get the receive ball state and init the steering target
            Machine.GetState<ReceiveBallMainState>().SetSteeringTarget(ballTime, position);
            Machine.ChangeState<ReceiveBallMainState>();
        }

        private void Instance_OnTrackThreat(Player threat)
        {
            _pickOutThreatMainState.Threat = threat;
            Machine.ChangeState<PickOutThreatMainState>();
        }

        private void Instance_OnWait()
        {
            Machine.ChangeState<WaitMainState>();
        }

    }
}
