using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.PickOutThreat.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeKickOff.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.DefendCornerKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportCornerKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeCornerKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeThrowIn.MainState;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState
{
    /// <summary>
    /// The player simply waits for an instruction and reacts to that
    /// instruction if it comes
    /// </summary>
    public class WaitMainState : BState
    {
        PickOutThreatMainState _pickOutThreatMainState;
        SupportAttackerMainState _supportAttackerMainState;

        public Player Owner { get => ((InFieldPlayerFSM)SuperMachine).Owner; }

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

            // set the animator
            Owner.Animator.SetTrigger("Idle");

            // stop steering
            Owner.RPGMovement.SetSteeringOff();
            Owner.RPGMovement.SetTrackingOff();

            //listen to variaus events
            Owner.OnInstructedToDefendCornerKick += Instance_OnInstructedToDefendCornerKick;
            Owner.OnInstructedToGoToHome += Instance_OnInstructedToGoToHome;
            Owner.OnGoToSupportSpot += Instance_OnGoToSupportSpot;
            Owner.OnInstructedToReceiveBall += Instance_OnInstructedToReceiveBall;
            Owner.OnInstructedToSupportCornerKick += Instance_OnInstructedToSupportCornerKick;
            Owner.OnInstructedToTakeCornerKick += Instance_OnInstructedToTakeCornerKick;
            Owner.OnInstructedToTakeKickOff += Instance_OnInstructedToTakeKickOff;
            Owner.OnInstructedToTakeThrowIn += Instance_OnInstructedTakeThrowIn;
            Owner.OnTrackThreat += Instance_OnTrackThreat;
        }

        public override void Exit()
        {
            base.Exit();

            // set the animator
            Owner.Animator.ResetTrigger("Idle");

            //stop listening to variaus events
            Owner.OnInstructedToDefendCornerKick -= Instance_OnInstructedToDefendCornerKick;
            Owner.OnInstructedToGoToHome -= Instance_OnInstructedToGoToHome;
            Owner.OnGoToSupportSpot -= Instance_OnGoToSupportSpot;
            Owner.OnInstructedToReceiveBall -= Instance_OnInstructedToReceiveBall;
            Owner.OnInstructedToSupportCornerKick -= Instance_OnInstructedToSupportCornerKick;
            Owner.OnInstructedToTakeCornerKick -= Instance_OnInstructedToTakeCornerKick;
            Owner.OnInstructedToTakeKickOff -= Instance_OnInstructedToTakeKickOff;
            Owner.OnInstructedToTakeThrowIn -= Instance_OnInstructedTakeThrowIn;
            Owner.OnTrackThreat -= Instance_OnTrackThreat;
        }

        private void Instance_OnInstructedToDefendCornerKick()
        {
            Machine.ChangeState<DefendCornerKickMainState>();
        }

        public void Instance_OnInstructedToGoToHome()
        {
            Machine.ChangeState<GoToHomeMainState>();
        }

        private void Instance_OnInstructedToReceiveBall(float ballTime, Vector3 position)
        {
            //get the receive ball state and init the steering target
            Machine.GetState<ReceiveBallMainState>().SetSteeringTarget(ballTime, position);
            Machine.ChangeState<ReceiveBallMainState>();
        }

        private void Instance_OnGoToSupportSpot(Player controllingPlayer, SupportSpot supportSpot)
        {
            _supportAttackerMainState.ControllingPlayer = controllingPlayer;
            _supportAttackerMainState.SupportSpot = supportSpot;
            Machine.ChangeState<SupportAttackerMainState>();
        }

        private void Instance_OnInstructedToSupportCornerKick()
        {
            Machine.ChangeState<SupportCornerKickMainState>();
        }

        private void Instance_OnInstructedToTakeCornerKick()
        {
            Machine.ChangeState<TakeCornerKickMainState>();
        }

        private void Instance_OnInstructedToTakeKickOff()
        {
            Machine.ChangeState<TakeKickOffMainState>();
        }

        private void Instance_OnInstructedTakeThrowIn()
        {
            Machine.ChangeState<TakeThrowInMainState>();
        }

        private void Instance_OnTrackThreat(Player threat)
        {
            _pickOutThreatMainState.Threat = threat;
            Machine.ChangeState<PickOutThreatMainState>();
        }
    }
}
