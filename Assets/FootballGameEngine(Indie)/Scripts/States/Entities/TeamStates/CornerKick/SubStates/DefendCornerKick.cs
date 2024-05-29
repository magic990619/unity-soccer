using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.StateMachines.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.GoalKeeperStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using RobustFSM.Base;
using System;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.States.Entities.Team.CornerKick.SubStates
{
    public class DefendCornerKick : BState
    {
        bool _hasMessagedPlayers;

        float _waitTime;

        Vector3[] _oppTeamcornerKickAttackMeshVertices;

        public override void Initialize()
        {
            base.Initialize();

            // get vertices
            _oppTeamcornerKickAttackMeshVertices = Owner.Opponent.PitchRegions.CornerKickSupportRegion.GetComponent<MeshFilter>().mesh.vertices;
        }

        public override void Enter()
        {
            base.Enter();

            _hasMessagedPlayers = false;
            _waitTime = 0.1f;

            // update players with certain data
            PlacePlayersAtDefendCornerKickPositions();

            // listen to opp ontakegoalkick event
            Owner.Opponent.OnTakeCornerKick += Instance_OnOpponentTakeCornerKick;
            Owner.Opponent.ControllingPlayer.OnTakeCornerKick += Instance_OnOpponentTakeCornerKick;
        }

        public override void Execute()
        {
            base.Execute();

            if (_hasMessagedPlayers == false)
            {
                _waitTime -= Time.deltaTime;
                if (_waitTime <= 0f)
                {
                    // raise the defend corner kick
                    ActionUtility.Invoke_Action(Owner.OnDefendCornerKick);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            // delisten to opp ontakegoalkick event
            Owner.Opponent.OnTakeCornerKick -= Instance_OnOpponentTakeCornerKick;
            Owner.Opponent.ControllingPlayer.OnTakeCornerKick -= Instance_OnOpponentTakeCornerKick;
        }

        private void Instance_OnOpponentTakeCornerKick(float ballTime, Vector3? position, Player receiver)
        {
            // get the current closest player to ball
            TeamPlayer currClosestPlayerToPoint = Owner.GetClosestPlayerToPoint((Vector3)position);
            if (currClosestPlayerToPoint != null)
                currClosestPlayerToPoint.Player.Invoke_OnBecameTheClosestPlayerToBall();
        }

        private void Instance_OnOpponentTakeCornerKick()
        {
            SuperMachine.ChangeState<DefendMainState>();
        }

        void PlacePlayersAtDefendCornerKickPositions()
        {
            // control loop
            int loopControl = 11;

            // place defending players at pos
            Owner.Players
            .ToList()
            .ForEach(tM =>
            {
                // set that the team isn't in control
                tM.Player.IsTeamInControl = false;

                // update player positions
                if (tM.Player.PlayerType == PlayerTypes.Goalkeeper)
                {
                    tM.Player.Position = tM.CurrentHomePosition.position;
                }
                else
                {
                    // find a random position on the corner-kick support spot
                    Vector3 position;
                    do
                    {
                        // decrement
                        --loopControl;

                        // get the pos
                        int randomIndex = UnityEngine.Random.Range(0, _oppTeamcornerKickAttackMeshVertices.Length);
                        position = Owner.Opponent.PitchRegions.CornerKickSupportRegion.transform.TransformPoint(_oppTeamcornerKickAttackMeshVertices[randomIndex]);

                    } while (tM.Player.IsTeamMemberWithinDistance(5f, position) == true && loopControl <= 0);

                    // place the players at pos
                    tM.CurrentHomePosition.position = position;
                    tM.Player.Position = tM.CurrentHomePosition.position;
                    tM.Player.RPGMovement.SetMoveTarget(tM.CurrentHomePosition.position);
                    tM.Player.transform.LookAt(Ball.Instance.Position);
                }
            });
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
