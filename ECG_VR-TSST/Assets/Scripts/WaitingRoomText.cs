using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class to hold all three texts shown on the wall in th waiting room prior to each round
/// </summary>
public class WaitingRoomText{

    public string initialText;
    public string surveillanceText;
    public string readyText;

    public WaitingRoomText()
    {

    }

    public WaitingRoomText(string initialText, string surveillanceText, string readyText)
    {
        this.initialText = initialText;
        this.surveillanceText = surveillanceText;
        this.readyText = readyText;
    }
}
