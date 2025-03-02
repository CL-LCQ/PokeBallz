﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2 
#else
using UnityEngine.SceneManagement;
#endif

public class customInterstitialAd : MonoBehaviour {

	// --------------------------------------
	// Unity Events
	// --------------------------------------

	void Awake() {


		if(!GoogleMobileAd.IsInited) {
			GoogleMobileAd.Init();
		}



	}

	void Start() {

		GoogleMobileAd.LoadInterstitialAd ();
		//ShowBanner();
	}




	// --------------------------------------
	// PUBLIC METHODS
	// --------------------------------------

	public void ShowBanner() {
		GoogleMobileAd.StartInterstitialAd();

	}



	// --------------------------------------
	// GET / SET
	// --------------------------------------



	public string sceneBannerId {
		get {
			#if UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			return Application.loadedLevelName + "_" + this.gameObject.name;
			#else
			return SceneManager.GetActiveScene().name + "_" + this.gameObject.name;
			#endif
		}
	}


}