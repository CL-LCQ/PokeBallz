﻿using System;
using UnityEngine;
using System.Collections;

public class UM_Camera : SA.Pattern.Singleton<UM_Camera> {

	//Actions
	public event Action<UM_ImagePickResult> OnImagePicked = delegate{};
	public event Action<UM_ImageSaveResult> OnImageSaved = delegate{};
	public event Action<UM_ImagesPickResult> OnImagesPicked = delegate {};

	void Awake() {
		DontDestroyOnLoad(gameObject);

		AndroidCamera.instance.OnImagePicked += OnAndroidImagePicked;
		IOSCamera.OnImagePicked += OnIOSImagePicked;

		AndroidCamera.instance.OnImageSaved += OnAndroidImageSaved;
		IOSCamera.OnImageSaved += OnIOSImageSaved;

		AndroidCamera.Instance.OnImagesPicked += HandleOnImagesPicked;
	}

	public void SaveImageToGalalry(Texture2D image) {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.instance.SaveImageToGallery(image);
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.instance.SaveTextureToCameraRoll(image);
				break;
		}
		
	}
	
	
	public void SaveScreenshotToGallery() {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.instance.SaveScreenshotToGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.instance.SaveScreenshotToCameraRoll();
				break;
		}
	}
	
	
	public void GetImageFromGallery() {
		switch(Application.platform) {
			case RuntimePlatform.Android:
				AndroidCamera.instance.GetImageFromGallery();
				break;
			case RuntimePlatform.IPhonePlayer:
				IOSCamera.instance.PickImage(ISN_ImageSource.Library);
				break;
		}
	}
	
	public void GetImagesFromGallery() {
		switch (Application.platform) {
		case RuntimePlatform.Android:
			AndroidCamera.Instance.GetImagesFromGallery();
			break;
		case RuntimePlatform.IPhonePlayer:
			break;
		}
	}
	
	public void GetImageFromCamera() {
		switch(Application.platform) {
		case RuntimePlatform.Android:
			AndroidCamera.instance.GetImageFromCamera();
			break;
		case RuntimePlatform.IPhonePlayer:
			IOSCamera.instance.PickImage(ISN_ImageSource.Camera);
			break;
		}
	}
	
	void HandleOnImagesPicked (AndroidImagesPickResult result)
	{
		OnImagesPicked(new UM_ImagesPickResult(result.IsSucceeded, result.Images));
	}

	void OnAndroidImagePicked (AndroidImagePickResult obj) {
		UM_ImagePickResult result = new UM_ImagePickResult(obj.Image);
		OnImagePicked(result);
	}

	void OnIOSImagePicked (IOSImagePickResult obj) {
		UM_ImagePickResult result = new UM_ImagePickResult(obj.Image);
		OnImagePicked(result);
	}

	void OnAndroidImageSaved (GallerySaveResult res) {
		UM_ImageSaveResult result = new UM_ImageSaveResult(res.imagePath, res.IsSucceeded);
		OnImageSaved(result);
	}
	

	void OnIOSImageSaved (ISN_Result res) {
		UM_ImageSaveResult result = new UM_ImageSaveResult(string.Empty, res.IsSucceeded);
		OnImageSaved(result);
	}
}
