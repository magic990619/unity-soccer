using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ReceiveBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates
{
    /// <summary>
    /// Handles team behaviour if when it's the one taking the throw-in
    /// </summary>
    public class TakeThrowIn : BState
    {
        bool _isExecuted;
        float _waitTime = 1f;

        Action InstructPlayerToTakeThrowIn;
        Vector3 _defaultHomePosition;

        public override void Enter()
        {
            base.Enter();

            // set to unexecuted
            _isExecuted = false;

            // set up controlling player position
            Owner.ControllingPlayer.OnTakeThrowIn += Instance_OnPlayerTakeThrowIn;
            InstructPlayerToTakeThrowIn += Owner.ControllingPlayer.Invoke_OnInstructedToTakeThrowIn;
        }

        public override void Execute()
        {
            base.Execute();

            // if not executed then run logic
            if (!_isExecuted)
            {
                // decrement time
                _waitTime -= Time.deltaTime;

                if (_waitTime <= 0)
                {
                    // set to executed
                    _isExecuted = true;

                    // trigger player to take throw-in
                    ActionUtility.Invoke_Action(InstructPlayerToTakeThrowIn);
                }
            }
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            // trigger players to support attacker
            Owner.TriggerPlayersToSupportAttacker();
        }

        public override void Exit()
        {
            base.Exit();

            // deregister player from listening to take-kickoff action
            Owner.ControllingPlayer.OnTakeThrowIn -= Instance_OnPlayerTakeThrowIn;
            InstructPlayerToTakeThrowIn -= Owner.ControllingPlayer.Invoke_OnInstructedToTakeThrowIn;
        }

        private void Instance_OnPlayerTakeThrowIn()
        {
            // change the position of the 
            // tell every player that it's no longer a setpiece
            Owner.Players
                   .Where(p => p.Player.PlayerType == PlayerTypes.InFieldPlayer
                    && p.Player.InFieldPlayerFSM.IsCurrentState<ReceiveBallMainState>() == false)
                   .ToList()
                   .ForEach(p =>
                   {
                       // set some stuff
                       p.Player.InFieldPlayerFSM.ChangeState<GoToHomeMainState>();
                   });

            // trigger state change to attack
            SuperMachine.ChangeState<AttackMainState>();

            //simply raise that I have taken the kick-off
            ActionUtility.Invoke_Action(Owner.OnTakeThrowIn);
        }

        public FootballGameEngine_Indie.Scripts.Entities.Team Owner
        {
            get
            {
                return ((TeamFSM)SuperMachine).Owner;
            }
        }
    }
}
