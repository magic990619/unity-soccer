using SmartMenuManagement.Scripts;
using UnityEditor;
using UnityEngine;

namespace SmartMenuManagement.Editors
{
    [CustomEditor(typeof(MenuManager))]
    [CanEditMultipleObjects]
    public class MenuMenuManagerEditor : Editor
    {
        bool showMenus = false;

        //variable to hold the Menu MenuManager from the MenuManagers namespace
       MenuManager menuMenuManager;

        public void OnEnable()
        {
            menuMenuManager = (MenuManager)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Menu MenuManager");

            //show menu label if we have the menus
            if (menuMenuManager.ListMenus.Count > 0)
            {
                showMenus = EditorGUILayout.Foldout(showMenus, "Game Menus");
                EditorGUILayout.Separator();
            }

            //show the menus if set to true
            if (showMenus)
            {
                for (int i = 0; i < menuMenuManager.ListMenus.Count; ++i)
                {
                    if (menuMenuManager.ListMenus[i].value != null)
                        EditorGUILayout.TextField("Menu Name", menuMenuManager.ListMenus[i].value.name);
                    else
                        EditorGUILayout.TextField("Menu Name", "not available yet");

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.IntField("Menu Key", menuMenuManager.ListMenus[i].key);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    menuMenuManager.ListMenus[i].value = EditorGUILayout.ObjectField("Menu Game Object", menuMenuManager.ListMenus[i].value, typeof(GameObject), true) as GameObject;
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("");
                    if (GUILayout.Button("-"))
                        menuMenuManager.ListMenus.RemoveAt(i);
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.Separator();
                }
            }
            
            //button to add a new panel
            if (GUILayout.Button("Add Menu"))
                menuMenuManager.AddPanel();

            EditorGUILayout.EndVertical();
        }
    }
}