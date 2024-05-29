using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SmartMenuManagement.Scripts
{
    public class BSubMenu
    {
        [SerializeField]
        GameObject _root;

        public GameObject Root { get => _root; set => _root = value; }
    }
}
