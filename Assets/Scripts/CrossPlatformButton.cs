using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class CrossPlatformButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {
    public InputKey key;

    public void OnPointerDown(PointerEventData eventData)
    {
        CrossPlatformInputManager.instance.SetStatus(key, true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        CrossPlatformInputManager.instance.SetStatus(key, false);
    }
}
