using Assets.FootballGameEngine_Indie.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.References
{
    public class BallReference : MonoBehaviour
    {
        [SerializeField]
        IKTargets _ballIkTargets;

        /// <summary>
        /// A reference to the IK targets
        /// </summary>
        public IKTargets BallIkTargets { get => _ballIkTargets; set => _ballIkTargets = value; }
    }
}
