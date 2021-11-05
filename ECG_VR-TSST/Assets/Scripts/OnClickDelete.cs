using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickDelete : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Delete a task from the drop panel on click with the mouse
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        Image i = this.gameObject.GetComponent<Image>();
        i.color = new Color(1, 1, 1, 0);
        i.sprite = null;
    }

}

