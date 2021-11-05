using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearAllSettings : MonoBehaviour {

    [SerializeField]
    private Dropdown numberOfNPCs;

    [SerializeField]
    private Dropdown language;

    [SerializeField]
    private Dropdown secondRoundAnnouncement;

    [SerializeField]
    private Image[] allTasks;

    [SerializeField]
    private Toggle t;

    /// <summary>
    /// Set back every UI Element to its standard state
    /// </summary>
    public void onButtonClicked()
    {
        numberOfNPCs.value = 0;
        language.value = 0;
        secondRoundAnnouncement.value = 0;
        t.isOn = false;
        foreach(Image i in allTasks)
        {
            i.color = new Color(1, 1, 1, 0);
            i.sprite = null;
        }
    }
}
