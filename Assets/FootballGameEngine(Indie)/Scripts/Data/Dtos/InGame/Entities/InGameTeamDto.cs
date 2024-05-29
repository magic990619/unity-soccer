using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Tactics;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.InGame.Entities
{
    [Serializable]
    public class InGameTeamDto
    {
        [SerializeField]
        int _leftSideCornerKickTaker;

        [SerializeField]
        int _leftSideThrowInTake;

        [SerializeField]
        int _rightSideCornerKickTaker;

        [SerializeField]
        int _rightSideThrowInTake;

        [SerializeField]
        string _name;

        [SerializeField]
        string _shortName;

        [SerializeField]
        Sprite _icon;

        [SerializeField]
        AttackTactic _attackTactic;

        [SerializeField]
        DefendTactic _defendTactic;

        [SerializeField]
        Formation _formation;

        [SerializeField]
        InGameKitDto _kit;

        [SerializeField]
        List<InGamePlayerDto> _players;

        public InGameTeamDto(AttackTactic attackTactic, DefendTactic defendTactic, Formation formation, KitDto kitDto, TeamDto teamDto)
        {
            // copy basic team info
            _name = teamDto.Name;
            _shortName = teamDto.ShortName;
            _icon = teamDto.Icon;
            _leftSideCornerKickTaker = teamDto.LeftSideCornerKickTaker;
            _leftSideThrowInTake = teamDto.LeftSideThrowInTake;
            _rightSideCornerKickTaker = teamDto.RightSideCornerKickTaker;
            _rightSideThrowInTake = teamDto.RightSideThrowInTake;

            // tacticts
            _attackTactic = attackTactic;
            _defendTactic = defendTactic;

            // get the formation
            _formation = formation;

            // copy kit info
            _kit = new InGameKitDto(kitDto);

            // copy the players
            _players = teamDto.Players.Select(p => new InGamePlayerDto()
            {
                Accuracy = p.Accuracy,
                CanJoinCornerKick = p.CanJoinCornerKick,
                DiveSpeed = p.DiveSpeed,
                GoalKeeping = p.GoalKeeping,
                DiveDistance = p.JumpDistance,
                JumpHeight = p.JumpHeight,
                Power = p.Power,
                Reach = p.Reach,
                Speed = p.Speed,
                Tackling = p.Tackling,

                Firstname = p.Firstname,
                Lastname = p.Lastname,
                KitName = p.KitName,
                KitNumber = p.KitNumber
            })
            .ToList();
        }

        public int LeftSideThrowInTake { get => _leftSideThrowInTake; set => _leftSideThrowInTake = value; }
        public int RightSideThrowInTake { get => _rightSideThrowInTake; set => _rightSideThrowInTake = value; }
        public int LeftSideCornerKickTaker { get => _leftSideCornerKickTaker; set => _leftSideCornerKickTaker = value; }
        public int RightSideCornerKickTaker { get => _rightSideCornerKickTaker; set => _rightSideCornerKickTaker = value; }

        public string Name { get => _name; set => _name = value; }
        public string ShortName { get => _shortName; set => _shortName = value; }

        public Sprite Icon { get => _icon; set => _icon = value; }
        public InGameKitDto Kit { get => _kit; set => _kit = value; }
        public List<InGamePlayerDto> Players { get => _players; set => _players = value; }

        public AttackTactic AttackTactic { get => _attackTactic; set => _attackTactic = value; }
        public DefendTactic DefendTactic { get => _defendTactic; set => _defendTactic = value; }

        public Formation Formation { get => _formation; set => _formation = value; }
    }
}
