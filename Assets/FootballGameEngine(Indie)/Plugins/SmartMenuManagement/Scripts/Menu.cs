using System;
using UnityEngine;

namespace SmartMenuManagement.Scripts
{
    [Serializable]
    public class Menu
    {
        [SerializeField]
        public int key;

        [SerializeField]
        public string name;

        [SerializeField]
        public GameObject value;

        public Menu(int key)
        {
            this.key = key;
        }
    }
}
