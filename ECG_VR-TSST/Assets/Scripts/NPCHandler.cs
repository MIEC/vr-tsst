using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHandler : MonoBehaviour {

    public GameObject secondNPC;
    public GameObject thirdNPC;

    /// <summary>
    /// If only one auditorr is seet in the settings, the two other auditors will be deactivated
    /// </summary>
    /// <param name="setting"></param>
    public void setNPCs(Setting setting)
    {
        if (setting.oneAuditor)
        {
            secondNPC.SetActive(false);
            thirdNPC.SetActive(false);
        }
    }
}
