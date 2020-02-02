using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using GoogleMobileAds.Api;

public class Admanager : MonoBehaviour {

	public static Admanager Instance;

	void Awake(){
		if (Instance) {
			DestroyImmediate (gameObject);
		} else {
			UnityAdRequest ();
		}
	}

	private void UnityAdRequest(){

		string gameId = "";
		#if UNITY_ANDROID
		gameId = Const.UNITYADS_ANDROID_ID;
		#elif UNITY_IPHONE
		gameId = Const.UNITYADS_IOS_ID;
		#endif

		Advertisement.Initialize (gameId);
	}
}
