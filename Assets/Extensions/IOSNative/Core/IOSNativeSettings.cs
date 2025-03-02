﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
[InitializeOnLoad]
#endif

public class IOSNativeSettings : ScriptableObject {

	public const string VERSION_NUMBER = "8.5.1";

	public string AppleId = "XXXXXXXXX";


	public int ToolbarIndex = 0;
	
	public bool SendFakeEventsInEditor = true;
	public List<string> DefaultStoreProductsView = new List<string>();


	public List<IOSProductTemplate> InAppProducts =  new List<IOSProductTemplate>();
	public List<GK_Leaderboard> Leaderboards =  new List<GK_Leaderboard>();
	public List<GK_AchievementTemplate> Achievements =  new List<GK_AchievementTemplate>();



	public bool checkInternetBeforeLoadRequest = false;
	public bool ShowStoreKitProducts = true;
	public bool ShowLeaderboards = true;
	public bool ShowAchievementsParams = true;
	public bool ShowUsersParams = false;
	public bool ShowOtherParams = true;
	public bool ShowRPKParams = false;


	public bool ExpandAPISettings = true;


	public bool EnableGameCenterAPI = true;
	public bool EnableInAppsAPI = true;
	public bool EnableCameraAPI = true;
	public bool EnablePickerAPI = false;
	public bool EnableSocialSharingAPI = true;
	public bool EnableMediaPlayerAPI = true;
	public bool EnableiAdAPI = true;
	public bool EnableReplayKit = false;
	public bool EnableCloudKit = false;
	public bool EnableSoomla = false;

	public bool EnableGestureAPI = false;

	public bool EnablePushNotificationsAPI = false;

	public int EditorFillRateIndex = 4;
	public int EditorFillRate = 100;
	public bool IsEditorTestingEnabled = true;

	public bool DisablePluginLogs = false;

	public bool UseGCRequestCaching = false;
	public bool UsePPForAchievements = false;


	public bool AutoLoadUsersSmallImages = true;
	public bool AutoLoadUsersBigImages = false;


	public int  MaxImageLoadSize = 512;
	public float JPegCompressionRate = 0.8f;
	public IOSGalleryLoadImageFormat GalleryImageFormat = IOSGalleryLoadImageFormat.JPEG;


	public int RPK_iPadViewType = 0;



	//Soomla
	public string SoomlaDownloadLink = "http://goo.gl/7LYwuj";
	public string SoomlaDocsLink = "https://goo.gl/JFkpNa";
	public string SoomlaGameKey = "" ;
	public string SoomlaEnvKey = "" ;

	//One Signal
	public bool OneSignalEnabled = false;
	public string OneSignalDocsLink = "https://goo.gl/Royty6";


	private const string ISNSettingsAssetName = "IOSNativeSettings";
	private const string ISNSettingsAssetExtension = ".asset";

	private static IOSNativeSettings instance = null;

	public static IOSNativeSettings Instance {

		
		get {
			if (instance == null) {
				instance = Resources.Load(ISNSettingsAssetName) as IOSNativeSettings;
				
				if (instance == null) {
					
					// If not found, autocreate the asset object.
					instance = CreateInstance<IOSNativeSettings>();
					#if UNITY_EDITOR

					SA.Util.Files.CreateFolder(SA.Config.SettingsPath);
				
					/*string properPath = Path.Combine(Application.dataPath, ISNSettingsPath);
					if (!Directory.Exists(properPath)) {
						AssetDatabase.CreateFolder("Extensions/", "IOSNative");
						AssetDatabase.CreateFolder("Extensions/IOSNative", "Resources");
					}
					*/
					
					string fullPath = Path.Combine(Path.Combine("Assets", SA.Config.SettingsPath),
					                               ISNSettingsAssetName + ISNSettingsAssetExtension
					                               );
					
					AssetDatabase.CreateAsset(instance, fullPath);
					#endif
				}
			}
			return instance;
		}
	}

}
