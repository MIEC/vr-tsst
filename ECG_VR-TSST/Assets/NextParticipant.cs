using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextParticipant : MonoBehaviour{

    [SerializeField]
    private InputField inputField;


    /// <summary>
    /// Function for the Next button in the main Menu
    /// Increments the participant number by one
    /// </summary>
    public void OnButtonClicked()
    {
        string text = inputField.text;
        int number = int.Parse(text);
        text = (number + 1).ToString();
        inputField.text = text;
    }
}
