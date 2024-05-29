using Assets.FootballGameEngine_Indie.Scripts.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Entities;
using Assets.FootballGameEngine_Indie_.Scripts.Managers;
using Assets.FootballGameEngine_Indie_.Scripts.UI.Items.FormationItems;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.FootballGameEngine_Indie_.Scripts.UI.Widgets.TeamWidgets
{
    [Serializable]
    public class FormationManagementWidget
    {
        [SerializeField]
        FormationItem001 _formationItemPrefab;

        [SerializeField]
        Image _currFormationIcon;

        [SerializeField]
        Text _currFormationNameText;

        [SerializeField]
        ToggleGroup _formationItemsToggleGroup;

        [SerializeField]
        Transform _formationsRoot;

        public void Init(string formationName, Sprite formationIcon)
        {
            _currFormationNameText.text = formationName;
            _currFormationIcon.sprite = formationIcon;
        }

        public void Init(TeamDto teamData, List<Formation> formations, UnityAction<string> unityAction)
        {
            // remove current formations
            _formationsRoot.GetComponentsInChildren<FormationItem001>().ToList()
                .ForEach(fI => GameObject.Destroy(fI.gameObject));

            // create formation items
            formations.ForEach(f =>
            {
                // create formation item
                var formationItem = GameObject.Instantiate(_formationItemPrefab, _formationsRoot);
                var isOn = f.FormationType == teamData.FormationType;
                if(isOn == true)
                {
                    // Get the formation
                    Formation formation = FormationManager.Instance.GetFormation(teamData.FormationType);

                    // set some stuff
                    _currFormationIcon.sprite = formation.Icon;
                    _currFormationNameText.text = formation.name;
                }

                formationItem.Init(isOn, f.FormationType.ToString(), f.name, _formationItemsToggleGroup, unityAction);

            });
        }
    }
}
