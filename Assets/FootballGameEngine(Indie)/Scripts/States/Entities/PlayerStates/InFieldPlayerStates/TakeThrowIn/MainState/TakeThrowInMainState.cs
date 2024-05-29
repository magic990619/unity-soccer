using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeThrowIn.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using RobustFSM.Base;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.TakeThrowIn.MainState
{
    public class TakeThrowInMainState : BHState
    {
        float _cachedMaxWonderDistance;

        public override void AddStates()
        {
            base.AddStates();

            // add states
            AddState<AutomaticTakeThrow>();
            AddState<ManualTakeThrow>();
            AddState<PrepareToTakeThrowIn>();

            // set initial state
            SetInitialState<PrepareToTakeThrowIn>();
        }

        public override void Enter()
        {
            base.Enter();

            _cachedMaxWonderDistance = Owner.DistanceMaxWonder;
        }

        public override void Exit()
        {
            base.Exit();

            Owner.DistanceMaxWonder = _cachedMaxWonderDistance;
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
