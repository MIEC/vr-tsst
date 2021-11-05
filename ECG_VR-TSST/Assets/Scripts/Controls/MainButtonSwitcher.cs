using UnityEngine;

public class MainButtonSwitcher : MonoBehaviour {
	
	private static MainButtonSwitcher s_instance;
	public static MainButtonSwitcher Instance {
		get { return s_instance;}	
	}
	
	[SerializeField]
	private GenericButton _next;

	[SerializeField]
	private GenericButton _repeat;
	
	private void Awake() {
		if (s_instance == null) s_instance = this;
		else {
			Debug.LogError("[MainButtonSwitcher] Instance set already. Destroying.");
			Destroy(gameObject);
		}
	}
	
	public void Next(bool enabled) {
		_next.enabled = enabled;
	}

	public void Repeat(bool enabled) {
		_repeat.enabled = enabled;
	}
	
	public void All(bool enabled) {
		_next.enabled = enabled;
		_repeat.enabled =enabled;
	}

}
