using Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.MainMenu
{
    [Serializable]
    public class PrepareForMatchMainMenu : BMenu
    {
        [SerializeField]
        PreMatchSubMenu _preMatchSubMenu;

        [SerializeField]
        SelectTeamsSubMenu _selectKitsSubMenu;

        [SerializeField]
        SelectTeamsSubMenu _selectTeamSubMenu;

        public SelectTeamsSubMenu SelectKitsSubMenu { get => _selectKitsSubMenu; set => _selectKitsSubMenu = value; }
        public SelectTeamsSubMenu SelectTeamSubMenu { get => _selectTeamSubMenu; set => _selectTeamSubMenu = value; }
        public PreMatchSubMenu PreMatchSubMenu { get => _preMatchSubMenu; set => _preMatchSubMenu = value; }

        public void DisableAllSubMenus()
        {
            _preMatchSubMenu.Root.SetActive(false);
            _selectKitsSubMenu.Root.SetActive(false);
            _selectTeamSubMenu.Root.SetActive(false);
        }
    }
}
