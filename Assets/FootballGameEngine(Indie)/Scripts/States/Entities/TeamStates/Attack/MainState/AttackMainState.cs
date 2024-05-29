using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ChaseBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.ControlBall.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Wait.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CounterKick.MainState;
using Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.ThrowIn.SubStates;
using RobustFSM.Base;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState
{
    /// <summary>
    /// The team finds positions higher up the pitch and instructs it's players to
    /// move up the pitch into goal scoring positions
    /// </summary>
    public class AttackMainState : BState
    {
        bool _canAttack;

        float _waitTime;
        float _pitchLength;

        Vector3 _originalFormationRootPosition;

        public Team Owner { get => ((TeamFSM)SuperMachine).Owner; }

        public override void Enter()
        {
            base.Enter();

            // pick some wait time
            _pitchLength = Pitch.Instance.Length;
            _originalFormationRootPosition = Owner.Formation.transform.position;
            _waitTime = Mathf.Clamp(Random.value, 0.1f, Owner.TeamData.AttackTactic.TransistIntoAttackSpeed);

            // enable the support spots root
            Owner.PlayerSupportSpots.gameObject.SetActive(true);

            //listen to some team events
            Owner.OnGoalKeeperGainedBallControl += Instance_OnGoalKeeperGainedBallControl;
            Owner.OnLostPossession += Instance_OnLoosePossession;
            Owner.OnMessagedToStop += Instance_OnMessagedToStop;
            Owner.OnMessagedToTakeThrowIn += Instance_OnMessagedToTakeThrowIn;

            // init the players home positions
            Owner.Players.ForEach(tM => ActionUtility.Invoke_Action(tM.Player.OnInstructedToGoToHome));
        }

        public override void Execute()
        {
            base.Execute();

            if (_canAttack == false)
            {
                _waitTime -= Time.deltaTime;
                if (_waitTime < 0) _canAttack = true;
            }
        }

        public override void ManualExecute()
        {
            base.ManualExecute();

            if (Owner.ControllingPlayer == null
                && Owner.ReceivingPlayer == null)
            {
                TeamPlayer closestPlayerToBall = Owner.GetClosestPlayerToPoint(Ball.Instance.Position);
                Owner.ControllingPlayer = closestPlayerToBall.Player;
                closestPlayerToBall.Player.InFieldPlayerFSM.ChangeState<ChaseBallMainState>();
            }

            // attack
            if (_canAttack)
            {
                //Owner.TriggerPlayersToSupportAttacker();
                Owner.MovePlayersUpField();
            }
        }

        public override void Exit()
        {
            base.Exit();

            // reset stuff
            Owner.Formation.transform.position = _originalFormationRootPosition;
            Owner.PlayerSupportSpots.gameObject.SetActive(false);

            //stop listening to some team events
            Owner.OnGoalKeeperGainedBallControl -= Instance_OnGoalKeeperGainedBallControl;
            Owner.OnLostPossession -= Instance_OnLoosePossession;
            Owner.OnMessagedToStop -= Instance_OnMessagedToStop;
            Owner.OnMessagedToTakeThrowIn -= Instance_OnMessagedToTakeThrowIn;
        }

        private void Instance_OnGoalKeeperGainedBallControl()
        {
            // change state
            SuperMachine.GetState<CounterKickMainState>().GoalKeeperHasBall = false;
            SuperMachine.ChangeState<CounterKickMainState>();
        }

        private void Instance_OnLoosePossession()
        {
            Machine.ChangeState<DefendMainState>();
        }

        private void Instance_OnMessagedToStop()
        {
            SuperMachine.ChangeState<WaitMainState>();
        }

        private void Instance_OnMessagedToTakeThrowIn()
        {
            SuperMachine.ChangeState<TakeThrowIn>();
        }
    }
}
