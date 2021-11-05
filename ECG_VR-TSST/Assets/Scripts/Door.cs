using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour {

    private Animator _anim;

	public static Door Instance {
		get; private set;
	}

    private void Awake()
    {
	    Instance = this;
	    _anim = GetComponent<Animator>();
    }

    [ContextMenu("Open")]
	public void Open()
    {
        _anim.SetTrigger("openDoor");
    }

    [ContextMenu("Close")]
	public void Close()
    {
        _anim.SetTrigger("closeDoor");
    }
	
}
