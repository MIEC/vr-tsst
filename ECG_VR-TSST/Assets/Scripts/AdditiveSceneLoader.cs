﻿using UnityEngine;
using UnityEngine.SceneManagement;


public class AdditiveSceneLoader : MonoBehaviour {
	
	
	[SerializeField]
	private string _sceneName;
	
	private void Start() {
		ScreenFader.Fade(Color.black,0f);
		LoadScene();
	}
	
	[ContextMenu("LoadScene")]
	private void LoadScene() {
		SceneManager.LoadSceneAsync(_sceneName,LoadSceneMode.Additive);		
	}
	
}
