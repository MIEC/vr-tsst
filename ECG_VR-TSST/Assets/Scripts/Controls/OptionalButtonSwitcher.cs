using UnityEngine;

public class OptionalButtonSwitcher : MonoBehaviour
{

    private static OptionalButtonSwitcher s_instance;
    public static OptionalButtonSwitcher Instance
    {

        get
        {
            if (s_instance == null) Debug.LogError("[OptionalButtonSwitcher] Instance accessed but is null.");
            return s_instance;
        }
    }

    [SerializeField]
    private GenericButton[] _allOptionalButtons;
	private GenericButton[] _enabledButtons;

    [ContextMenu("Find GenericButtons in children")]
    private void Reset()
    {
        _allOptionalButtons = transform.GetComponentsInChildren<GenericButton>();
        Debug.Log("[OptionalButtonSwitcher] Found " + _allOptionalButtons.Length + " buttons.");
    }

    private void Awake()
    {
        if (s_instance == null) s_instance = this;
        else
        {
            Debug.LogError("[OptionalButtonSwitcher] Instance already set. Destroying.");
            Destroy(gameObject);
        }
    }

    public void DisableAll()
    {
        foreach (var button in _allOptionalButtons)
        {
            button.enabled = false;
        }
    }

    public void ReenableButtons()
	{
		if (_enabledButtons == null) return;
        foreach (var button in _enabledButtons)
        {
            button.enabled = true;
        }
    }

    public void EnableButtons(GenericButton[] buttons)
    {
        _enabledButtons = buttons;
        DisableAll();
        ReenableButtons();
    }

}
