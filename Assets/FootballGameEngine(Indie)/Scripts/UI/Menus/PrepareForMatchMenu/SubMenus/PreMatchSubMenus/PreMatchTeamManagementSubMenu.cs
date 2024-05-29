using Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.TeamWidgets;
using SmartMenuManagement.Scripts;
using System;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus.PreMatchSubMenus
{
    [Serializable]
    public class PreMatchTeamManagementSubMenu : BSubMenu
    {
        [SerializeField]
        TeamManagementWidget _cpuTeamManagementWidget;

        [SerializeField]
        TeamManagementWidget _userTeamManagementWidget;

        public TeamManagementWidget CpuTeamManagementWidget { get => _cpuTeamManagementWidget; set => _cpuTeamManagementWidget = value; }
        public TeamManagementWidget UserTeamManagementWidget { get => _userTeamManagementWidget; set => _userTeamManagementWidget = value; }
    }
}
