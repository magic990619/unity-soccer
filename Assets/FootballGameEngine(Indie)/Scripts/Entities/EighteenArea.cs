using Assets.FootballGameEngine_Indie.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.Entities
{
    public class EighteenArea : MonoBehaviour
    {
        [SerializeField]
        Goal _goal;

        public Goal Goal { get => _goal; set => _goal = value; }
    }
}
