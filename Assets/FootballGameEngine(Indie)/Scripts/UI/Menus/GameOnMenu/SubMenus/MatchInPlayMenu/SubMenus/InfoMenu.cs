using SmartMenuManagement.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus
{
    [Serializable]
    public class InfoMenu : BSubMenu
    {
        [SerializeField]
        Text _txtInfo;

        public Text TxtInfo { get => _txtInfo; set => _txtInfo = value; }
    }
}
