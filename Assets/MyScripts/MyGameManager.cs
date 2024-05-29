using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.GridLayoutGroup;
using Assets.FootballGameEngine_Indie.Scripts.Managers;

public class MyGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    private static MyGameManager instance;
    public static MyGameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<MyGameManager>();
            }
            return instance;
        }
    }

    [Header("My Panels")]
    public GameObject SkipButton;
    public GameObject MainController;
    public GameObject StartscenePanel;
    public GameObject RandomTeamDetails;

    [Header("StartScenes Objects")]
    public Camera MainCam;
    public bool isSceneplaying = true;
    [SerializeField ] private float stepscount = 0;
    public GameObject MainScene;  // Main Scene Folder
    public GameObject Scene1;  // Showflags
    public GameObject Scene2;  // ShowWalkPlayer
    public GameObject Scene3;  // ShowIdlePlayer
    public GameObject Scene4;  // ShowTeamDetail

    [Header("Scene Player Manager")]
    public int myPlayerindex = 0;
    public int myPlayerKits = 0;
    public int myAIindex = 0;
    public int myAIKits = 0;
    public Renderer[] playerKitmesh;
    public Renderer[] AIKitmesh;
    public Material[] CharacterKitsmats;

    [Header("Gameplay Flags Manager")]
    private GameObject nothing;
    public Renderer[] PlayerFlagsMeshes;
    public Renderer[] AIFlagsMeshes;
    public Material[] AllFlags;


    private void Start()
    {
  
    }
    private void Update()
    {
    }
    #region // StartCutscene Manager
    public void StratCutscenefrom1() // Showflags
    {
        StartCoroutine(start1());
    }
    IEnumerator start1()
    {
        SoundManager.Instance.PlayAudioClip(7);
        isSceneplaying = true;
        MainCam.enabled = false;
        stepscount = 1;
        MainScene.gameObject.SetActive(true);
        StartscenePanel.gameObject.SetActive (true);
        RandomTeamDetails.gameObject.SetActive(false);
        ActiveSceneSteps(Scene1);
        yield return new WaitForSeconds(5f);
        SkipButton.gameObject.SetActive(true);
        stepscount = 2;
        ActiveSceneSteps(Scene2);
        yield return new WaitForSeconds(5f);
        stepscount = 3;
        ActiveSceneSteps(Scene3);
        yield return new WaitForSeconds(5f);
        stepscount = 4;
        ActiveSceneSteps(Scene4);
        RandomTeamDetails.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        stepscount = 5;
        StartscenePanel.gameObject.SetActive(false);
        RandomTeamDetails.gameObject.SetActive(false);
        MainScene.gameObject.SetActive(false);
        MainController.gameObject.SetActive(true);
        MainCam.enabled = true ;
        isSceneplaying = false;
    }
    public void StratCutscenefrom2() // ShowWalkPlayer
    {
        StopAllCoroutines();
        StartCoroutine(start2());
    }
    IEnumerator start2()
    {
        SkipButton.gameObject.SetActive(true);
        stepscount = 2;
        RandomTeamDetails.gameObject.SetActive(false);
        ActiveSceneSteps(Scene2);
        yield return new WaitForSeconds(5f);
        stepscount = 3;
        ActiveSceneSteps(Scene3);
        yield return new WaitForSeconds(5f);
        stepscount = 4;
        ActiveSceneSteps(Scene4);
        RandomTeamDetails.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        stepscount = 5;
        StartscenePanel.gameObject.SetActive(false);
        RandomTeamDetails.gameObject.SetActive(false);
        MainScene.gameObject.SetActive(false);
        MainController.gameObject.SetActive(true);
        MainCam.enabled = true;
        isSceneplaying = false;
    }
    public void StratCutscenefrom3() // ShowIdlePlayer
    {
        StopAllCoroutines();
        StartCoroutine(start3());
    }
    IEnumerator start3()
    {
        stepscount = 3;
        RandomTeamDetails.gameObject.SetActive(false);
        ActiveSceneSteps(Scene3);
        yield return new WaitForSeconds(5f);
        stepscount = 4;
        ActiveSceneSteps(Scene4);
        RandomTeamDetails.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        stepscount = 5;
        StartscenePanel.gameObject.SetActive(false);
        RandomTeamDetails.gameObject.SetActive(false);
        MainScene.gameObject.SetActive(false);
        MainController.gameObject.SetActive(true);
        MainCam.enabled = true;
        isSceneplaying = false;
    }
    public void StratCutscenefrom4() // ShowTeamDetail
    {
        StopAllCoroutines();
        StartCoroutine(start4());
    }
    IEnumerator start4()
    {
        stepscount = 4;
        ActiveSceneSteps(Scene4);
        RandomTeamDetails.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        stepscount = 5;
        StartscenePanel.gameObject.SetActive(false);
        RandomTeamDetails.gameObject.SetActive(false);
        MainScene.gameObject.SetActive(false);
        MainController.gameObject.SetActive(true);
        MainCam.enabled = true;
        isSceneplaying = false;
    }

    public void EndCutscene() // ShowTeamDetail
    {
        StopAllCoroutines();
        stepscount = 5;
        StartscenePanel.gameObject.SetActive(false);
        RandomTeamDetails.gameObject.SetActive(false);
        MainScene.gameObject.SetActive(false);
        MainController.gameObject.SetActive(true);
        MainCam.enabled = true;
        isSceneplaying = false;
    }
    void ActiveSceneSteps(GameObject active)
    {
        Scene1.gameObject.SetActive(false);
        Scene2.gameObject.SetActive(false);
        Scene3.gameObject.SetActive(false);
        Scene4.gameObject.SetActive(false);

        active.gameObject.SetActive(true);
    }

    public void SkipButtonPressed()
    {
        if (stepscount== 0)
        {

        }
        else if (stepscount== 1)
        {
            StratCutscenefrom2();
        }
        else if (stepscount == 2)
        {
            StratCutscenefrom3();
        }
        else if (stepscount == 3)
        {
            StratCutscenefrom4();
        }
        else if (stepscount == 4)
        {
            EndCutscene();
        }
    }
    #endregion

    #region // Scene Player Kit Manager
    // Changing AI Kit
     void ChangePlayerKitS(Material selectMat)
    {
        for (int i = 0; i < playerKitmesh.Length; i++)
        {
            Material Newkits = playerKitmesh[i].material;
            Newkits = selectMat;
            playerKitmesh[i].material = Newkits;
        }

    }
     void ChangeAIKitS(Material selectMat)
    {
        for (int i = 0; i<AIKitmesh.Length; i++)
        {
            Material Newkits = AIKitmesh[i].material;
            Newkits = selectMat;
            AIKitmesh[i].material = Newkits;
        }
       
    }
    void ChangePlayerFlags(Material selectMat)
    {
        for (int i = 0; i < PlayerFlagsMeshes.Length; i++)
        {
            Material Newkits = PlayerFlagsMeshes[i].material;
            Newkits = selectMat;
            PlayerFlagsMeshes[i].material = Newkits;
        }

    }
    void ChangeAIFlags(Material selectMat)
    {
        for (int i = 0; i < AIFlagsMeshes.Length; i++)
        {
            Material Newkits = AIFlagsMeshes[i].material;
            Newkits = selectMat;
            AIFlagsMeshes[i].material = Newkits;
        }

    }
    public void ShowkitMenuPlayer()
    {
        switch (PlayerPrefs.GetInt("myplayerindex"))
        {
            case 0: // Brazil
                switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[2]);  // Yellow
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[4]); // Blueline
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[0]);
                break;
            case 1: // France
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[0]);  // Blue
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[4]); // Blueline
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[1]);
                break;
            case 2: // Argentina
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[4]);  // BlueLine
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[2]);
                break;
            case 3: // England
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[3]);
                break;
            case 4: // Spain
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[4]);
                break;
            case 5: // Germany
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[5]);
                break;
            case 6: // Netherlands
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[2]);  // Yellow
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[6]);
                break;
            case 7: // Senegal
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[7]);
                break;
            case 8: // Portugal
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[5]);  // RedLine
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                }
                ChangePlayerFlags(AllFlags[8]);
                break;
            case 9: // United States
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[9]);
                break;
            case 10: // Uruguay
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[10]);
                break;
            case 11: // Australia
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[2]);  // Yellow
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[11]);
                break;
            case 12: // Poland
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[12]);
                break;
            case 13: // Morocco
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[13]);
                break;
            case 14: // Croatia
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[5]);  // Redline
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                }
                ChangePlayerFlags(AllFlags[14]);
                break;
            case 15: // Japan
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[0]);  // Blue
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangePlayerFlags(AllFlags[15]);
                break;
            case 16: // Switzerland
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangePlayerFlags(AllFlags[16]);
                break;
            case 17: // Ghana
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangePlayerFlags(AllFlags[17]);
                break;
            case 18: // Qatar
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangePlayerFlags(AllFlags[18]);
                break;
            case 19: // Saudi Arabia
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[6]);  // Greenline
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[5]); // Redline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                }
                ChangePlayerFlags(AllFlags[19]);
                break;
            case 20: // Italy
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // GreenLine
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangePlayerFlags(AllFlags[20]);
                break;
            case 21: // Pakistan
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[6]);  // GreenLine
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                }
                ChangePlayerFlags(AllFlags[21]);
                break;
            case 22: // India
                   switch (PlayerPrefs.GetInt("myplayerkits"))
                {
                    case 0:
                        ChangePlayerKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                        ChangePlayerKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                        ChangePlayerKitS( CharacterKitsmats[5]); // RedLine
                        break;
                    case 3:
                        ChangePlayerKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                        ChangePlayerKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                        ChangePlayerKitS( CharacterKitsmats[6]); // Greeline
                        break;
                    case 6:
                        ChangePlayerKitS( CharacterKitsmats[1]); // Red
                        break;
                }
                ChangePlayerFlags(AllFlags[22]);
                break;
        }
    }
    public void ShowkitMenuAI()
    {
        switch (PlayerPrefs.GetInt("myAiIndex"))
        {
            case 0: // Brazil
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[2]);  // Yellow
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[4]); // Blueline
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[0]);
                break;
            case 1: // France
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[0]);  // Blue
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[4]); // Blueline
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[1]);
                break;
            case 2: // Argentina
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[4]);  // BlueLine
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[2]);
                break;
            case 3: // England
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[3]);
                break;
            case 4: // Spain
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[4]);
                break;
            case 5: // Germany
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[5]);
                break;
            case 6: // Netherlands
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[2]);  // Yellow
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[6]);
                break;
            case 7: // Senegal
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[7]);
                break;
            case 8: // Portugal
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[5]);  // RedLine
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                }
                ChangeAIFlags(AllFlags[8]);
                break;
            case 9: // United States
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[9]);
                break;
            case 10: // Uruguay
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[10]);
                break;
            case 11: // Australia
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[2]);  // Yellow
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[11]);
                break;
            case 12: // Poland
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[12]);
                break;
            case 13: // Morocco
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[13]);
                break;
            case 14: // Croatia
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[5]);  // Redline
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                }
                ChangeAIFlags(AllFlags[14]);
                break;
            case 15: // Japan
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[0]);  // Blue
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangeAIFlags(AllFlags[15]);
                break;
            case 16: // Switzerland
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangeAIFlags(AllFlags[16]);
                break;
            case 17: // Ghana
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangeAIFlags(AllFlags[17]);
                break;
            case 18: // Qatar
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[1]);  // Red
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greenline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // Redline
                        break;
                }
                ChangeAIFlags(AllFlags[18]);
                break;
            case 19: // Saudi Arabia
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[6]);  // Greenline
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[5]); // Redline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                }
                ChangeAIFlags(AllFlags[19]);
                break;
            case 20: // Italy
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // GreenLine
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                }
                ChangeAIFlags(AllFlags[20]);
                break;
            case 21: // Pakistan
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[6]);  // GreenLine
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[3]); // White
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                }
                ChangeAIFlags(AllFlags[21]);
                break;
            case 22: // India
                switch (PlayerPrefs.GetInt("myAiKits"))
                {
                    case 0:
                         ChangeAIKitS( CharacterKitsmats[3]);  // White
                        break;
                    case 1:
                         ChangeAIKitS( CharacterKitsmats[2]); // Yellow
                        break;
                    case 2:
                         ChangeAIKitS( CharacterKitsmats[5]); // RedLine
                        break;
                    case 3:
                         ChangeAIKitS( CharacterKitsmats[4]); // BlueLine
                        break;
                    case 4:
                         ChangeAIKitS( CharacterKitsmats[0]); // Blue
                        break;
                    case 5:
                         ChangeAIKitS( CharacterKitsmats[6]); // Greeline
                        break;
                    case 6:
                         ChangeAIKitS( CharacterKitsmats[1]); // Red
                        break;
                }
                ChangeAIFlags(AllFlags[22]);
                break;
        }
    }
    #endregion

}
