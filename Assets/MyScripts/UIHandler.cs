using Assets.FootballGameEngine_Indie.Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    // Start is called before the first frame update
    private static UIHandler instance;
    public static UIHandler Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<UIHandler>();
            }
            return instance;
        }
    }
    [Header ("Panels")]
    public GameObject ExitPanel;
    public GameObject NextImage;
    public GameObject PlayImage;
    public GameObject TrophyPanel;
    [Header("GameObject")]
    public GameObject CharactersMenu;
    public GameObject[] LoadingAd;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    public void Privacy()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Application.OpenURL("");
        }
    }
    public void RateUs()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            Application.OpenURL("");
        }
    }
    public void ShowloadingAdStatic()
    {

        // Ads Here
        
        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            if (AdsManager.Instance.interstitialStatic.IsLoaded())
            {
                StartCoroutine(playadStatic());
            }
        }
        

    }
    void HidePanels()
    {
        for (int i = 0; i < LoadingAd.Length; i++)
        {
            LoadingAd[i].SetActive(false);
        }
    }
    void activePanels()
    {
        for (int i = 0; i <LoadingAd.Length; i++)
        {
            LoadingAd[i].SetActive(true);
        }
    }
    IEnumerator playadStatic()
    {
       
        
        activePanels();
         yield return new WaitForSeconds(2f);
        // Ads Here
        
        if (AdsManager.Instance.interstitialStatic.IsLoaded())
        {
            AdsManager.Instance.ShowInterstitialStatic();
        }
        
        yield return new WaitForSeconds(0.1f);
        HidePanels();
        
    }

    public void ShowloadingAd()
    {

        if (Application.internetReachability != NetworkReachability.NotReachable)
        {
            // Ads Here

            if (AdsManager.Instance.interstitial.IsLoaded())
            {
                StartCoroutine(playad());
            
            }
            else if (AdsManager.Instance.isVideoReady)
            {
                StartCoroutine(playad());
            }
                
            
        }

    }
    IEnumerator playad()
    {
        activePanels();
        yield return new WaitForSeconds(2f);
        // Ads Here
        
        if (AdsManager.Instance.interstitial.IsLoaded())
        {
            AdsManager.Instance.ShowInterstitial();
        }
        else if (AdsManager.Instance.isVideoReady) 
        {
            AdsManager.Instance.ShowUnityVideoAd();
        }
      
        yield return new WaitForSeconds(0.1f);
        HidePanels();

    }

    public void ActiveNextImage()
    {
        NextImage.gameObject.SetActive(true);
        PlayImage.gameObject.SetActive(false);
    }
    public void ActivePlayImage()
    {
        PlayImage.gameObject.SetActive(true);
        NextImage.gameObject.SetActive(false);
    }
    public void ShowTrophyPanel()
    {
        // play statdium audio clip
        SoundManager.Instance.PlayAudioClip(5);
        TrophyPanel.SetActive(true);
    }
    public void HidetrophyPanel()
    {
        // play statdium audio clip
        SoundManager.Instance.PlayAudioClip(5);
        TrophyPanel.SetActive(false);
    }
}
