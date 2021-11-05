using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownListener : MonoBehaviour
{

    public Toggle Vortrag;
    public Toggle Rueckwaerts;
    public Toggle Reihen;
    public Toggle Kopfrechnen;
    public Dropdown Drop;
    private bool toggleActive;
    private void Update()
    {

        if (Drop.value.Equals(0))
        {
            Vortrag.interactable = false;
            Rueckwaerts.interactable = false;
            Reihen.interactable = false;
            Kopfrechnen.interactable = false;
        }
        else
        {
            Vortrag.interactable = true;
            Rueckwaerts.interactable = true;
            Reihen.interactable = true;
            Kopfrechnen.interactable = true;
        }

    }

}
