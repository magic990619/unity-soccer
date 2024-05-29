using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.PlayerStates.InFieldPlayerStates.Wait.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Attack.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Defend.MainState;
using Assets.FootballGameEngine_Indie.Scripts.States.Entities.TeamStates.Init.MainState;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Tests.TeamTests
{
    public class AttackDefendFormationTest : MonoBehaviour
    {
        [SerializeField]
        Team _awayTeam;

        [SerializeField]
        Team _homeTeam;

        int _finishedInitializedTeamCount;

        private void Awake()
        {
            _awayTeam.OnInit += Instance_OnTeamInit;
            _homeTeam.OnInit += Instance_OnTeamInit;

            _awayTeam.gameObject.SetActive(true);
            _homeTeam.gameObject.SetActive(true);
        }

        private void Update()
        {
            if (_awayTeam.ControllingPlayer != null)
                _awayTeam.ControllingPlayer.PlaceBallInfronOfMe();
        }

        private void Instance_OnTeamInit()
        {
            ++_finishedInitializedTeamCount;

            if (_finishedInitializedTeamCount == 2)
                Invoke("InitTeams", 1f);
        }

        private void InitTeams()
        {
            _awayTeam.ControllingPlayer = _awayTeam.Players[_awayTeam.Players.Count - 1].Player;

            _awayTeam.FSM.ChangeState<AttackMainState>();
            _homeTeam.FSM.ChangeState<DefendMainState>();

            _awayTeam.ControllingPlayer.InFieldPlayerFSM.ChangeState<WaitMainState>();
        }
    }
}
