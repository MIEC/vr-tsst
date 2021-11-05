using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;

public class AdditiveSceneLoader : MonoBehaviour {
	
	
	[SerializeField]
	private string _sceneName;
	
	private void Start() {
		SteamVR_Fade.Start(Color.black,0f);
		LoadScene();
	}
	
	[ContextMenu("LoadScene")]
	private void LoadScene() {
		SceneManager.LoadSceneAsync(_sceneName,LoadSceneMode.Additive);		
	}
	
}
