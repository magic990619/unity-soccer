using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus
{
    [Serializable]
    public class PreMatchSubMenu : BSubMenu
    {
        [SerializeField]
        PreMatchMatchInfoSubMenu _preMatchMatchInfoSubMenu;

        [SerializeField]
        PreMatchMatchSettingsSubMenu _preMatchMatchSettingsSubMenu;

        [SerializeField]
        PreMatchTeamManagementSubMenu _preMatchTeamManagementSubMenu;

        public PreMatchMatchInfoSubMenu PreMatchMatchInfoSubMenu { get => _preMatchMatchInfoSubMenu; set => _preMatchMatchInfoSubMenu = value; }
        public PreMatchTeamManagementSubMenu PreMatchTeamManagementSubMenu { get => _preMatchTeamManagementSubMenu; set => _preMatchTeamManagementSubMenu = value; }
        public PreMatchMatchSettingsSubMenu PreMatchMatchSettingsSubMenu { get => _preMatchMatchSettingsSubMenu; set => _preMatchMatchSettingsSubMenu = value; }

        public void DisableAllSubMenus()
        {
            _preMatchMatchInfoSubMenu.Root.SetActive(false);
            _preMatchMatchSettingsSubMenu.Root.SetActive(false);
            _preMatchTeamManagementSubMenu.Root.SetActive(false);
        }
    }
}
