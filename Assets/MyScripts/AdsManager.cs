using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using GoogleMobileAds.Api;
using UnityEngine.Advertisements;
using UnityEngine.UIElements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static AdsManager Instance;

    public bool enableTestMode = true;


    [Header("Admob Ads")]
    public string appID = "ca-app-pub-2492371397237739~3222301428";
    [Space(5)]
    public string interstitialID = "ca-app-pub-2492371397237739/2138689588";
    [Space(5)]
    public string staticInterstitialID = "ca-app-pub-2492371397237739/6491913275";
 //   [Space(5)]
//    public string largeBannerID = "ca-app-pub-2492371397237739/3055754230";
    [Space(5)]
    public string bannerID = "ca-app-pub-2492371397237739/3055754230";
 //   [Space(5)]
   // public string rewardedAdID = "ca-app-pub-6458658565513365/9544087648";

    [HideInInspector] public BannerView bannerView, medBannerView;
    [HideInInspector] public InterstitialAd interstitial;
    [HideInInspector] public InterstitialAd interstitialStatic;
    [HideInInspector] public RewardedAd rewardedAd;

    [Header("Unity Ads")]
    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] string _videoId = "Interstitial_Android";
    [SerializeField] string _rewardedId = "Rewarded_Android";
    public bool isVideoReady = false;
    public bool isRewardedReady = false;
    string _gameId;



    public void Awake()
    {
        MakeSingleton();
    }

    private void Start()
    {


        if (enableTestMode)
        {
            bannerID  = "ca-app-pub-3940256099942544/6300978111";
            interstitialID = staticInterstitialID = "ca-app-pub-3940256099942544/1033173712";
         //   rewardedAdID = "ca-app-pub-3940256099942544/5224354917";
        }

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        RequestBanner();
      //  RequestMediumBanner();
        RequestInterstitial();
        RequestInterstitialStatic();

        // Unity Ads
        InitializeAds();



    }

    private void MakeSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    //======================================== Banner AD =============================================================//

    public void RequestBanner()
    {
        if (this.bannerView == null)
        {
            this.bannerView = new BannerView(bannerID, AdSize.Banner, AdPosition.Top);
        }

        AdRequest request = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request);
        HideBanner();
    }

    public void ShowBanner()
    {
        this.bannerView.Show();
    }

    public void HideBanner()
    {
        this.bannerView.Hide();
    }

    //======================================== Medium Banner AD =============================================================//
/*
    public void RequestMediumBanner()
    {
        if (this.medBannerView == null)
        {
            this.medBannerView = new BannerView(largeBannerID, AdSize.MediumRectangle, AdPosition.BottomLeft);
        }

        AdRequest request = new AdRequest.Builder().Build();
        this.medBannerView.LoadAd(request);
        HideMediumBanner();
    }

    public void ShowMediumBanner()
    {
        this.medBannerView.Show();
    }

    public void HideMediumBanner()
    {
        this.medBannerView.Hide();
    }
    */
    //======================================== Interstitial AD =================ff======================================//

    public void RequestInterstitial()
    {
        if (this.interstitial == null)
        {
            if (SystemInfo.systemMemorySize > 2000)
                this.interstitial = new InterstitialAd(interstitialID);
            else
                this.interstitial = new InterstitialAd(staticInterstitialID);
        }
        else if (this.interstitial.IsLoaded())
            return;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void ShowInterstitial()
    {
        if (this.interstitial.IsLoaded())
        {
            this.interstitial.Show();
        }
    }
    // Static Only
    public void RequestInterstitialStatic()
    {
        if (this.interstitialStatic == null)
        {
            this.interstitialStatic = new InterstitialAd(staticInterstitialID);
        }
        else if (this.interstitialStatic.IsLoaded())
            return;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialStatic.LoadAd(request);
    }
    public void ShowInterstitialStatic()
    {
        if (this.interstitialStatic.IsLoaded())
        {
            this.interstitialStatic.Show();
        }
    }
    //======================================== Rewarded AD =======================================================//
    /*
    public void RequestRewardedAd()
    {
        if (this.rewardedAd == null)
            this.rewardedAd = new RewardedAd(rewardedAdID);
        else if (this.rewardedAd.IsLoaded())
            return;


        this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd.LoadAd(request);
    }

    public void ShowRewardedAd()
    {
        if (this.rewardedAd.IsLoaded())
        {
            this.rewardedAd.Show();
        }
    }
    public void HandleUserEarnedReward(object sender, Reward args)
    {
        Debug.Log("Rewarded Ads Completd");
    }
    */
    /// <summary>
    /// //////////////////////////////////////////////// Unity Ads ////////////////////////////////////////////////////////////
    /// </summary>
     #region 
    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        Advertisement.Initialize(_gameId, enableTestMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization completed");
        LoadInerstitialAd();
        LoadRewardedAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    public void LoadInerstitialAd()
    {
        Advertisement.Load(_videoId, this);
    }

    public void LoadRewardedAd()
    {
        Advertisement.Load(_rewardedId, this);
    }

    public void OnUnityAdsAdLoaded(string _androidGameId)
    {
        if (_androidGameId.Equals(_videoId))
        {
            Debug.Log("My Ad Loaded" + _androidGameId);
            isVideoReady = true;
        }
        else if (_androidGameId.Equals(_rewardedId))
        {
            Debug.Log("My Ad Loaded" + _androidGameId);
            isRewardedReady = true;
        }

    }
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {placementId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.Log("OnUnityAdsShowFailure");
        if (placementId.Equals(_videoId))
        {
            LoadInerstitialAd();
        }
        else if (placementId.Equals(_rewardedId))
        {
            LoadRewardedAd();
        }
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("OnUnityAdsShowStart");
        if (_androidGameId.Equals(_videoId))
        {
            Debug.Log("My Ad Showing" + _androidGameId);
            isVideoReady = false;
        }
        else if (_androidGameId.Equals(_rewardedId))
        {
            Debug.Log("My Ad Showing" + _androidGameId);
            isRewardedReady = false;
        }
        //   Time.timeScale = 0;
        //   Advertisement.Banner.Hide();            //   Hiding Unity Banner
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("OnUnityAdsShowClick");
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        Debug.Log("OnUnityAdsShowComplete " + showCompletionState);
        if (placementId.Equals(_videoId) && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            LoadInerstitialAd();
        }
        else if (placementId.Equals(_rewardedId) && UnityAdsShowCompletionState.COMPLETED.Equals(showCompletionState))
        {
            Debug.Log("rewared Player");
            LoadRewardedAd();

        }

    }

    /*
    public void LoadBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Load("Banner_Android",
            new BannerLoadOptions
            {
                loadCallback = OnBannerLoaded,
                errorCallback = OnBannerError
            }
            );
    }

    void OnBannerLoaded()
    {
        Advertisement.Banner.Show("Banner_Android");
    }

    void OnBannerError(string message)
    {

    }
    */

    ///////////////////
    ///
    public void ShowUnityRewardedVideoAd()
    {
        Advertisement.Show(_rewardedId, this);
    }
    public void ShowUnityVideoAd()
    {
        if (isVideoReady)
        {
            Advertisement.Show(_videoId, this);
        }
        else
        {
            LoadInerstitialAd();
        }
    }
    #endregion
}
