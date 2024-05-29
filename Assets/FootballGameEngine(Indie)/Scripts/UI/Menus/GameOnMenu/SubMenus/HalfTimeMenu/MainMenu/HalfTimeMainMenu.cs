using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.MatchInPlayMenu.SubMenus;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.GameOnMenu.SubMenus.HalfTimeMenu.MainMenu
{
    [Serializable]
    public class HalfTimeMainMenu : BSubMenu
    {
        [SerializeField]
        HalfTimeInfoMenu _halfTimeInfoMenu;

        [SerializeField]
        PreMatchTeamManagementSubMenu _halfTimeTeamManagementMenu;

        public HalfTimeInfoMenu HalfTimeInfoMenu { get => _halfTimeInfoMenu; set => _halfTimeInfoMenu = value; }
        public PreMatchTeamManagementSubMenu HalfTimeTeamManagementMenu { get => _halfTimeTeamManagementMenu; set => _halfTimeTeamManagementMenu = value; }

        public void DisableChildren()
        {
            _halfTimeInfoMenu.Root.SetActive(false);
            _halfTimeTeamManagementMenu.Root.SetActive(false);
        }

    }
}
