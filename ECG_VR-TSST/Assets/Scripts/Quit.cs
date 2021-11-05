using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

    /// <summary>
    /// On button click, terminate the application
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
