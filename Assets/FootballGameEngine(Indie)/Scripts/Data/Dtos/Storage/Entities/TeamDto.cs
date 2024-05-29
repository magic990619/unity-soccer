using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities
{
    [Serializable]
    public class TeamDto
    {
        [SerializeField]
        int _leftSideCornerKickTaker;

        [SerializeField]
        int _leftSideThrowInTaker;

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
        AttackTypeEnum _attackType;

        [SerializeField]
        DefenceTypeEnum _defendType;

        [SerializeField]
        FormationTypeEnum _formationType;

        [SerializeField]
        List<KitDto> _kits;

        [SerializeField]
        List<PlayerDto> _players;

        public string Name { get => _name; set => _name = value; }
        public string ShortName { get => _shortName; set => _shortName = value; }
        public Sprite Icon { get => _icon; set => _icon = value; }

        public List<KitDto> Kits { get => _kits; set => _kits = value; }
        public List<PlayerDto> Players { get => _players; set => _players = value; }
        public FormationTypeEnum FormationType { get => _formationType; set => _formationType = value; }
        public AttackTypeEnum AttackType { get => _attackType; set => _attackType = value; }
        public int LeftSideCornerKickTaker { get => _leftSideCornerKickTaker; set => _leftSideCornerKickTaker = value; }
        public int RightSideCornerKickTaker { get => _rightSideCornerKickTaker; set => _rightSideCornerKickTaker = value; }
        public int LeftSideThrowInTake { get => _leftSideThrowInTaker; set => _leftSideThrowInTaker = value; }
        public int RightSideThrowInTake { get => _rightSideThrowInTake; set => _rightSideThrowInTake = value; }
        public DefenceTypeEnum DefenceType { get => _defendType; set => _defendType = value; }
    }
}
