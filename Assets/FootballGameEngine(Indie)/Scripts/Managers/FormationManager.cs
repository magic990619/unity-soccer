using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using Patterns.Singleton;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Managers
{
    public class FormationManager : Singleton<FormationManager>
    {
        [SerializeField]
        List<Formation> _formations;

        public List<Formation> Formations { get => _formations; set => _formations = value; }

        public Formation GetFormation(FormationTypeEnum formationType)
        {
            // get the formation
            Formation formation = _formations
                .Where(f => f.FormationType == formationType)
                .FirstOrDefault();

            // return result
            return formation;
        }
    }
}
