using UnityEngine;
using System.Collections.Generic;

namespace SmartMenuManagement.Scripts
{
    /// <summary>
    /// Handles enabling and disabling of game menus in the scene
    /// </summary>
    public class MenuManager : MonoBehaviour
    {
        [SerializeField]
        [HideInInspector]
        List<Menu> listMenus = new List<Menu>();

        Dictionary<int, GameObject> menus = new Dictionary<int, GameObject>();              //dictionary that holds the different game menus

        #region MonoBehaviourMethods
        private void Awake()
        {
            Init();
        }
        #endregion

        #region MutatorMethods
        /// <summary>
        /// Adds a panel to the menu list
        /// </summary>
        public void AddPanel()
        {
            //generate a random key for the menu to be added
            int key = Random.Range(1, 1000);

            //keep looking for another key if we already have a menu with his key
            while (ListMenus.Find(x => x.key == key) != null)
            {
                key = Random.Range(1, 1000);
            }

            //add a new menu to the menu list
            ListMenus.Add(new Menu(key));
        }

        /// <summary>
        /// Disables the menu identified by the passed key
        /// </summary>
        /// <param name="key">the key of the menu</param>
        public void DisableMenu(int key)
        {
            try
            {
                GetMenu(key).SetActive(false);
            }
            catch { }
        }

        /// <summary>
        /// Disables all the menus
        /// </summary>
        public void DisableAllMenus()
        {
            foreach (KeyValuePair<int, GameObject> keyValuePair in menus)
            {
                DisableMenu(keyValuePair.Key);
            }

        }
        /// <summary>
        /// Enables the menu identified by the passed key
        /// </summary>
        /// <param name="key">the key of the menu</param>
        public void EnableMenu(int key)
        {
            try
            {
                //get the menu
                GameObject menu = GetMenu(key);

                //process the menu
                menu.transform.SetAsLastSibling();
                menu.SetActive(true);
            }
            catch { }
        }

        /// <summary>
        /// Enables all the menus
        /// </summary>
        public void EnableAllMenus()
        {
            foreach (KeyValuePair<int, GameObject> keyValuePair in menus)
            {
                EnableMenu(keyValuePair.Key);
            }

        }

        /// <summary>
        /// Retrieves the menu identified by the passed key
        /// </summary>
        /// <param name="key">the key of the menu</param>
        public GameObject GetMenu(int key)
        {
            try
            {
                return menus[key];
            }
            catch
            {
                Debug.Log("<color=red>Error::cannot retrieve the menu with id '" + key.ToString() + "'.\nCheck if the menu exists</color>");
                return null;
            }
        }

        /// <summary>
        /// Adds the menu's from the list to the dictionary as a pair(key, value)
        /// </summary>
        public void Init()
        {
            menus.Clear();                                          //clear the dictionary
            ListMenus.ForEach(x => menus.Add(x.key, x.value));      //copy the listMenu to the dictionary
            DisableAllMenus();                                      //disable all menus
        }
        #endregion

        #region Properties
        public Dictionary<int, GameObject> Menus
        {
            get
            {
                return menus;
            }

            set
            {
                menus = value;
            }
        }

        public List<Menu> ListMenus
        {
            get
            {
                return listMenus;
            }

            set
            {
                listMenus = value;
            }
        }
        #endregion
    }
}
