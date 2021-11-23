using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private void Awake() {
        GetComponent<RectTransform>().anchorMin = new Vector2(Screen.safeArea.position.x / Screen.width, Screen.safeArea.position.y / Screen.height);
        GetComponent<RectTransform>().anchorMax = new Vector2((Screen.safeArea.position.x + Screen.safeArea.size.x) / Screen.width, (Screen.safeArea.position.y + Screen.safeArea.size.y) / Screen.height);
    }
}
