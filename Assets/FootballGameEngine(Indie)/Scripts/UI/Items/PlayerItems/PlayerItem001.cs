using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems
{
    public class PlayerItem001 : MonoBehaviour
    {
        [SerializeField]
        private Text _name;

        [SerializeField]
        private Text _role;

        public Text Name { get => _name; set => _name = value; }
        public Text Role { get => _role; set => _role = value; }

        public void Init(string name, string role)
        {
            _name.text = name;
            _role.text = role;
        }
    }
}
