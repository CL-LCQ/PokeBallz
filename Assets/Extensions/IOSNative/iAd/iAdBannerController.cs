#define IAD_API

////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Native Plugin for Unity3D 
// @author Osipov Stanislav (Stan's Assets) 
// @support stans.assets@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////



using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
#if (UNITY_IPHONE && !UNITY_EDITOR && IAD_API) || SA_DEBUG_MODE
using System.Runtime.InteropServices;
#endif

public class iAdBannerController : ISN_Singleton<iAdBannerController> {

	#if (UNITY_IPHONE && !UNITY_EDITOR && IAD_API) || SA_DEBUG_MODE
	[DllImport ("__Internal")]
	private static extern void _IADDestroyBanner(int id);


	[DllImport ("__Internal")]
	private static extern void _IADStartInterstitialAd();

	[DllImport ("__Internal")]
	private static extern void _IADLoadInterstitialAd();

	[DllImport ("__Internal")]
	private static extern void _IADShowInterstitialAd();
	

	#endif



	private static int _nextId = 0;
	private static iAdBannerController _instance;
	private Dictionary<int, iAdBanner> _banners; 

	private bool _IsInterstisialsAdReady = false;

	//Actions
	public static event Action InterstitialDidFailWithErrorAction 	= delegate {};
	public static event Action InterstitialAdWillLoadAction 			= delegate {};
	public static event Action InterstitialAdDidLoadAction 			= delegate {};
	public static event Action InterstitialAdDidFinishAction			= delegate {};
	

	
	//--------------------------------------
	// INITIALIZE
	//--------------------------------------

	void Awake() {
		_banners =  new Dictionary<int, iAdBanner>();
		DontDestroyOnLoad(gameObject);

		if (IsEditorTestingEnabled) {
			SA_EditorAd.Instance.SetFillRate(IOSNativeSettings.Instance.EditorFillRate);
		}
	}

	

	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------

	public iAdBanner CreateAdBanner(TextAnchor anchor)  {

		
		iAdBanner bannner = new iAdBanner(anchor, nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
		
	}
	
	
	public iAdBanner CreateAdBanner(int x, int y)  {

		iAdBanner bannner = new iAdBanner(x, y, nextId);
		_banners.Add(bannner.id, bannner);
		
		return bannner;
	}


	public void DestroyBanner(int id) {
		if(_banners != null) {
			if(_banners.ContainsKey(id)) {
				_banners.Remove(id);
				
				#if (UNITY_IPHONE && !UNITY_EDITOR && IAD_API) || SA_DEBUG_MODE
					_IADDestroyBanner(id);
				#endif
			}
		}
	}

	private bool _ShowOnLoad = false;
	public void StartInterstitialAd() {
		if (IsEditorTestingEnabled) {
			_ShowOnLoad = true;
			InterstitialAdWillLoadAction();
			
			SA_EditorAd.OnInterstitialLoadComplete += HandleOnInterstitialLoadComplete_Editor;
			SA_EditorAd.Instance.LoadInterstitial();
			return;
		}

		#if (UNITY_IPHONE && !UNITY_EDITOR && IAD_API) || SA_DEBUG_MODE

		#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
			if((iPhone.generation.ToString()).IndexOf("iPhone") == -1 && (iPhone.generation.ToString()).IndexOf("iPad") == -1){
					ISN_Logger.Log("Device: " + iPhone.generation.ToString() + " is not supported by iAd");
					interstitialdidFailWithError("");
					return;
			}
		#else

		if(UnityEngine.iOS.Device.generation.ToString().IndexOf("iPhone") == -1 && (UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") == -1) {			
			ISN_Logger.Log("Device: " + UnityEngine.iOS.Device.generation.ToString() + " is not supported by iAd");
			interstitialdidFailWithError("");
			return;
		}
		#endif

		_IADStartInterstitialAd();
		#endif

	}
	
	public void LoadInterstitialAd() {
		if (IsEditorTestingEnabled) {
			InterstitialAdWillLoadAction();

			SA_EditorAd.OnInterstitialLoadComplete += HandleOnInterstitialLoadComplete_Editor;
			SA_EditorAd.Instance.LoadInterstitial();
			return;
		}

		#if (UNITY_IPHONE && !UNITY_EDITOR && IAD_API) || SA_DEBUG_MODE

		#if UNITY_3_5 || UNITY_4_0 || UNITY_4_0_1 || UNITY_4_2 || UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7
			if((iPhone.generation.ToString()).IndexOf("iPhone") == -1 && (iPhone.generation.ToString()).IndexOf("iPad") == -1){
				if(IOSNativeSettings.Instance.EnablePluginLogs) 	
					ISN_Logger.Log("Device: " + iPhone.generation.ToString() + " is not supported by iAd");
				interstitialdidFailWithError("");
				return;
			}
		#else

		if(UnityEngine.iOS.Device.generation.ToString().IndexOf("iPhone") == -1 && (UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") == -1) {	
				ISN_Logger.Log("Device: " + UnityEngine.iOS.Device.generation.ToString() + " is not supported by iAd");				
				interstitialdidFailWithError("");
				return;
			}
		#endif

		_IADLoadInterstitialAd();
		#endif
	}

	void HandleOnInterstitialLoadComplete_Editor (bool success)
	{
		SA_EditorAd.OnInterstitialLoadComplete -= HandleOnInterstitialLoadComplete_Editor;

		if (success) {
			_IsInterstisialsAdReady = true;
			InterstitialAdDidLoadAction();
			if (_ShowOnLoad) {
				_ShowOnLoad = false;
				ShowInterstitialAd();
			}
		} else {
			InterstitialDidFailWithErrorAction();
		}
	}
	
	public void ShowInterstitialAd() {
		if (IsEditorTestingEnabled) {
			_IsInterstisialsAdReady = false;
			SA_EditorAd.OnInterstitialFinished += HandleOnInterstitialFinished_Editor;
			SA_EditorAd.Instance.ShowInterstitial();
			return;
		}

		if(_IsInterstisialsAdReady) {

			Invoke("interstitialAdActionDidFinish", 1f);
			_IsInterstisialsAdReady = false;
			#if (UNITY_IPHONE && !UNITY_EDITOR && IAD_API) || SA_DEBUG_MODE
				_IADShowInterstitialAd();
			#endif
		}
	}

	void HandleOnInterstitialFinished_Editor (bool isRewarded)
	{
		SA_EditorAd.OnInterstitialFinished -= HandleOnInterstitialFinished_Editor;
		InterstitialAdDidFinishAction();
	}

	
	//--------------------------------------
	//  GET/SET
	//--------------------------------------

	public static int nextId {
		get {
			_nextId++;
			return _nextId;
		}
	}

	public bool IsInterstisialsAdReady {
		get {
			return _IsInterstisialsAdReady;
		}
	}
	
	public bool IsEditorTestingEnabled {
		get {
			return SA_EditorTesting.IsInsideEditor && IOSNativeSettings.Instance.IsEditorTestingEnabled;
		}
	}
	
	public iAdBanner GetBanner(int id) {
		if(_banners.ContainsKey(id)) {
			return _banners[id];
		} else {
			if(!IOSNativeSettings.Instance.DisablePluginLogs) 
				ISN_Logger.Log("Banner id: " + id.ToString() + " not found", LogType.Warning);
			return null;
		}
	}
	
	
	
	public List<iAdBanner> banners {
		get {
			
			List<iAdBanner> allBanners =  new List<iAdBanner>();
			if(_banners ==  null) {
				return allBanners;
			}
			
			foreach(KeyValuePair<int, iAdBanner> entry in _banners) {
				allBanners.Add(entry.Value);
			}
			
			return allBanners;
			
			
		}
	}

	//--------------------------------------
	//  EVENTS
	//--------------------------------------

	private void didFailToReceiveAdWithError(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		iAdBanner banner = GetBanner(id) as iAdBanner;
		if(banner != null) {
			banner.didFailToReceiveAdWithError();
		}

	}


	private void bannerViewDidLoadAd(string bannerID) {
		int id = System.Convert.ToInt32(bannerID);
		iAdBanner banner = GetBanner(id) as iAdBanner;
		if(banner != null) {
			banner.bannerViewDidLoadAd();
		}
	}


	private void bannerViewWillLoadAd(string bannerID){
		int id = System.Convert.ToInt32(bannerID);
		iAdBanner banner = GetBanner(id) as iAdBanner;
		if(banner != null) {
			banner.bannerViewWillLoadAd();
		}
	}


	private void bannerViewActionDidFinish(string bannerID){
		int id = System.Convert.ToInt32(bannerID);
		iAdBanner banner = GetBanner(id) as iAdBanner;
		if(banner != null) {
			banner.bannerViewActionDidFinish();
		}
	}

	private void bannerViewActionShouldBegin(string bannerID){
		int id = System.Convert.ToInt32(bannerID);
		iAdBanner banner = GetBanner(id) as iAdBanner;
		if(banner != null) {
			banner.bannerViewActionShouldBegin();
		}
	}



	private void interstitialdidFailWithError(string data) {
		InterstitialDidFailWithErrorAction();
		_IsInterstisialsAdReady = false;
	}

	private void interstitialAdWillLoad(string data) {
		InterstitialAdWillLoadAction();
		_IsInterstisialsAdReady = false;
	}

	private void interstitialAdDidLoad(string data) {
		InterstitialAdDidLoadAction();
		_IsInterstisialsAdReady = true;
	}

	private void interstitialAdActionDidFinish() {
		InterstitialAdDidFinishAction();
	}






	
	//--------------------------------------
	//  PRIVATE METHODS
	//--------------------------------------
	
	//--------------------------------------
	//  DESTROY
	//--------------------------------------

}
