﻿using UnityEngine;
using System;
using System.Collections;


namespace SA.Util {

	public static class Loader  {

		/// <summary>
		/// Asynchronously loads image data and converts it to the Texture2D object
		/// 
		/// <param name="url">Texture web url</param>
		/// <param name="callback">Texture load callback</param>
		/// </summary>
		public static void LoadWebTexture(string url,  Action<Texture2D> callback) {
			var loader = SA.Models.WWWTextureLoader.Create();

			loader.OnLoad += callback;
			loader.LoadTexture(url);
		}


		/// <summary>
		/// Asynchronously loads local prefab  and converts it to the GameObject
		/// 
		/// <param name="localPath">Local prefab path</param>
		/// <param name="callback">Prefab load callback</param>
		/// </summary>
		public static void LoadPrefab(string localPath,  Action<GameObject> callback) {
			var loader = SA.Models.PrefabAsyncLoader.Create();

			loader.ObjectLoadedAction += callback;
			loader.LoadAsync(localPath);
		}

	}

}