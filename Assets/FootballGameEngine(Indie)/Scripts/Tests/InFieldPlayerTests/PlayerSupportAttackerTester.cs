using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.GoToHome.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.SupportAttacker.MainState;
using Assets.FootballGameEngine_Indie.Scripts.Utilities.Enums;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Tests.InFieldPlayerTests
{
    public class PlayerSupportAttackerTester : MonoBehaviour
    {
        public Player _controllingPlayer;

        public List<Player> _opponents;
        public List<Player> _players;
        public List<SupportSpot> _supportSpots;

        public Player _secondaryPlayer;
        public Transform _pitcPoints;

        private void Awake()
        {
            //set the positions
            //_controllingPlayer.PlayerSupportSpots = _pitcPoints
            //    .GetComponentsInChildren<SupportSpot>()
            //    .ToList();

            //set the positions
           _supportSpots = _pitcPoints
                .GetComponentsInChildren<SupportSpot>()
                .ToList();
        }

        private void Start()
        {
            Invoke("Init", 1f);
        }

        private void Update()
        {
           _controllingPlayer.PlaceBallInfronOfMe();
        }

        void Init()
        {
            //init primary player
            //_players.ForEach(p =>
            //{
            //    p.Init(0.5f,
            //        15f,
            //        5f,
            //        15f,
            //        1f,
            //        5f,
            //        1f,
            //        0.5f,
            //        15f, // max wonder distance
            //        15f,
            //        10f,
            //        20f,
            //        1f,
            //        3f,
            //        4f,
            //        0.5f,
            //        0.6f,
            //        0.1f,
            //        0.5f,
            //        30f,
            //        1f,
            //        5f,
            //        null,
            //        null,
            //        new FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities.InGamePlayerDto()
            //        {
            //            Accuracy = 0.5f,
            //            GoalKeeping = 0.5f,
            //            JumpHeight = 0.5f,
            //            DiveDistance = 0.5f,
            //            DiveSpeed = 0.5f,
            //            Power = 0.5f,
            //            Reach = 0.5F,
            //            Speed = 0.5f,
            //            Tackling = 0.5f
            //        });
            //    p.Init();

            //    if(p != _controllingPlayer)
            //        p.InFieldPlayerFSM.ChangeState<GoToHomeMainState>();
            //});

            //_opponents.ForEach(p =>
            //{
            //    p.Init(0.5f,
            //        15f,
            //        5f,
            //        15f,
            //        1f,
            //        5f,
            //        1f,
            //        0.5f,
            //        15f,
            //        15f,
            //        10f,
            //        20f,
            //        1f,
            //        3f,
            //        4f,
            //        0.5f,
            //        0.3f,
            //        0.1f,
            //        0.5f,
            //        20f,
            //        1f,
            //        5f,
            //        null,
            //        null,
            //        new FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities.InGamePlayerDto()
            //        {
            //            Accuracy = 0.5f,
            //            GoalKeeping = 0.5f,
            //            JumpHeight = 0.5f,
            //            DiveDistance = 0.5f,
            //            DiveSpeed = 0.5f,
            //            Power = 0.5f,
            //            Reach = 0.5F,
            //            Speed = 0.5f,
            //            Tackling = 0.5f
            //        });
            //    p.Init();
            //});

            //set the state to supprt attacker
            InvokeRepeating("TriggerPlayersToSupportAttacker", 0.01f, 0.01f);
        }

        void TriggerPlayersToSupportAttacker()
        {
            // loop through each team players and try to find a support spot for this player to go
            _players
                .Where(tP => tP.PlayerType == PlayerTypes.InFieldPlayer
                    && _controllingPlayer != tP)
                .ToList()
                .ForEach(tP =>
                {
                    if (tP.IsPositionWithinDistance(Ball.Instance.Position, tP.HomePosition.position, tP.DistancePassMax))
                    {
                        // find the support spot for this player
                        SupportSpot supportSpot = _supportSpots
                        .ToList()
                        .Where(sS => sS.IsPickedOut(tP) == false
                            && tP.IsPositionWithinWanderRadius(sS.transform.position)
                            && tP.IsTeamMemberWithinMinPassDistance(sS.transform.position) == false
                            && tP.CanMakeShortPass(Ball.Instance.Position, sS.Position) == true)// todo::consider long passes...decide between numerous support spots
                        .OrderBy(sS => Vector3.Distance(sS.Position, tP.OppGoal.Position))
                        .FirstOrDefault();

                        // if we have a support spot for this player then tell him to go to this spot
                        if (supportSpot != null) tP.OnGoToSupportSpot?.Invoke(_controllingPlayer, supportSpot);
                    }
                    else
                    {
                        // tell the other player that no support spot for him wa found
                        tP.OnNoSupportSpotFound?.Invoke();
                    }
                });
        }
    }
}
