using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Items.PlayerItems;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.TeamWidgets
{
    [Serializable]
    public class TeamManagementWidget
    {
        [SerializeField]
        Toggle _tglFormation;

        [SerializeField]
        Toggle _tglSquad;

        [SerializeField]
        Toggle _tglTactics;

        [SerializeField]
        Text _teamName;

        [SerializeField]
        PlayerItem002 _playerItemPrefab;

        [SerializeField]
        GameObject _formationRoot;

        [SerializeField]
        GameObject _squadRoot;

        [SerializeField]
        GameObject _tacticsRoot;

        [SerializeField]
        FormationManagementWidget _formationManagementWidget;

        [SerializeField]
        SquadManagementWidget _squadManagementWidget;

        [SerializeField]
        TacticsManagementWidget _tacticsManagementWidget;

        public GameObject FormationRoot { get => _formationRoot; set => _formationRoot = value; }
        public GameObject SquadRoot { get => _squadRoot; set => _squadRoot = value; }
        public GameObject TacticsRoot { get => _tacticsRoot; set => _tacticsRoot = value; }
        public SquadManagementWidget SquadManagementWidget { get => _squadManagementWidget; set => _squadManagementWidget = value; }
        public FormationManagementWidget FormationManagementWidget { get => _formationManagementWidget; set => _formationManagementWidget = value; }
        public Toggle TglFormation { get => _tglFormation; set => _tglFormation = value; }
        public Toggle TglSquad { get => _tglSquad; set => _tglSquad = value; }
        public Toggle TglTactics { get => _tglTactics; set => _tglTactics = value; }

        public void DisableChildren()
        {
            _formationRoot.SetActive(false);
            _squadRoot.SetActive(false);
            _tacticsRoot.SetActive(false);

            _tglFormation.isOn = false;
            _tglSquad.isOn = false;
            _tglTactics.isOn = false;
        }

        public void Init(TeamDto teamData, 
            List<Formation> formations, 
            UnityAction<string> onClickFormationItem = null, 
            UnityAction onClickFormationTgl = null, 
            UnityAction onClickSquadTgl = null, 
            UnityAction onClickTacticsTgl = null, 
            UnityAction onClickNextAttackTactic = null,
            UnityAction onClickNextDefenceTactic = null,
            UnityAction onClickPrevAttackTactic = null,
            UnityAction onClickPrevDefenceTactic = null,
            UnityAction<PlayerItem002> onClickPlayerItem = null, 
            UnityAction onClickSwapButton = null)
        {
            _teamName.text = teamData.Name;

            // reset some stuff
            _tglFormation.onValueChanged.RemoveAllListeners();
            _tglSquad.onValueChanged.RemoveAllListeners();
            _tglTactics.onValueChanged.RemoveAllListeners();

            // init buttons
            _tglFormation.onValueChanged.AddListener((value) => { 
                if(value == true)
                {
                    onClickFormationTgl?.Invoke();
                }
            });
            _tglSquad.onValueChanged.AddListener((value) => {
                if(value == true)
                {
                    onClickSquadTgl?.Invoke();
                }
            });
            _tglTactics.onValueChanged.AddListener((value) => {
                if (value == true)
                {
                    onClickTacticsTgl?.Invoke();
                }
            });

            // init stuff
            _formationManagementWidget.Init(teamData, formations, onClickFormationItem);
            _squadManagementWidget.Init(teamData, onClickPlayerItem, onClickSwapButton);
            _tacticsManagementWidget.Init(teamData, onClickNextAttackTactic, onClickNextDefenceTactic, onClickPrevAttackTactic, onClickPrevDefenceTactic);
        }
    }
}
