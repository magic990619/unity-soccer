using SmartMenuManagement.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Menus.PrepareForMatchMenu.SubMenus
{
    [Serializable]
    public class SelectTeamsSubMenu : BSubMenu
    {
        [SerializeField]
        TeamInfo cpuControlledTeamInfo;

        [SerializeField]
        TeamInfo userControlledTeamInfo;

        public TeamInfo CpuControlledTeamInfo { get => cpuControlledTeamInfo; set => cpuControlledTeamInfo = value; }
        public TeamInfo UserControlledTeamInfo { get => userControlledTeamInfo; set => userControlledTeamInfo = value; }


        [Serializable]
        public struct TeamInfo
        {
            [SerializeField]
            Button _btnSelectNextTeam;

            [SerializeField]
             Button _btnSelectPrevTeam;

            [SerializeField]
            Image imgTeamIcon;

            [SerializeField]
            Text _txtTeamAttack;

            [SerializeField]
            Text _txtTeamDefence;

            [SerializeField]
            Text _txtTeamFormation;

            [SerializeField]
            Text _txtTeamName;

            public Button BtnSelectNextTeam { get => _btnSelectNextTeam; set => _btnSelectNextTeam = value; }
            public Button BtnSelectPrevTeam { get => _btnSelectPrevTeam; set => _btnSelectPrevTeam = value; }
            public Image ImgTeamIcon { get => imgTeamIcon; set => imgTeamIcon = value; }
            public Text TxtTeamAttack { get => _txtTeamAttack; set => _txtTeamAttack = value; }
            public Text TxtTeamDefence { get => _txtTeamDefence; set => _txtTeamDefence = value; }
            public Text TxtTeamFormation { get => _txtTeamFormation; set => _txtTeamFormation = value; }
            public Text TxtTeamName { get => _txtTeamName; set => _txtTeamName = value; }
        }
    }
}
