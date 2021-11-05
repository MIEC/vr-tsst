using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardInputManager : MonoBehaviour {

    [SerializeField]
    private Button weiter;
    [SerializeField]
    private Button erneut;
    [SerializeField]
    private Button zeitNichtUm;
    [SerializeField]
    private Button nichtVerstanden;
    [SerializeField]
    private Button lautUndDeutlich;
    [SerializeField]
    private Button vonVorne;
    [SerializeField]
    private Button falsch;
    [SerializeField]
    private Button richtig;
    [SerializeField]
    private Button ergebnis;

    /// <summary>
    /// Listen for the number keys one through nine and initiate button presses accordingly
    /// </summary>
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weiter.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            erneut.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            zeitNichtUm.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            nichtVerstanden.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            lautUndDeutlich.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            vonVorne.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            falsch.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            richtig.onClick.Invoke();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            ergebnis.onClick.Invoke();
        }
    }
}
