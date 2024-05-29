using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Entities
{
    public class Formation : MonoBehaviour
    {
        [SerializeField]
        string _formationName;

        [SerializeField]
        FormationTypeEnum _formationType;

        [SerializeField]
        Sprite _icon;

        [SerializeField]
        Transform _positionsAttackingRoot;

        [SerializeField]
        Transform _positionsDefendingRoot;

        [SerializeField]
        Transform _positionCurrentHomeRoot;

        [SerializeField]
        Transform _positionsKickOffRoot;

        public string GetPosition(int index)
        {
            return _positionsKickOffRoot.GetChild(index).gameObject.name;
        }

        public string FormationName { get => _formationName; set => _formationName = value; }


        public FormationTypeEnum FormationType { get => _formationType; set => _formationType = value; }

        public Sprite Icon { get => _icon; set => _icon = value; }

        public Transform PositionsAttackingRoot { get => _positionsAttackingRoot; set => _positionsAttackingRoot = value; }
        public Transform PositionsDefendingRoot { get => _positionsDefendingRoot; set => _positionsDefendingRoot = value; }
        public Transform PositionsCurrentHomeRoot { get => _positionCurrentHomeRoot; set => _positionCurrentHomeRoot = value; }
        public Transform PositionsKickOffRoot { get => _positionsKickOffRoot; set => _positionsKickOffRoot = value; }
    }
}
