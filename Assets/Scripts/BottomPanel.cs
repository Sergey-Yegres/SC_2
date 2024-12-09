using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BottomPanel : MonoBehaviour
{
    [Serializable]
    private struct buttonsAndWindows
    {
        public Button button;
        public GameObject window;
    }
    [SerializeField]
    private buttonsAndWindows[] BAW;
    private void Awake()
    {
        foreach (var obj in BAW)
        {
            obj.button.onClick.AddListener(() =>
            OnClick(obj.window,obj.button));
        }
    }
    private void OnClick(GameObject window,Button bttn)
    {
        foreach(var obj in BAW)
        {
            obj.window.SetActive(false);
            obj.button.interactable = true;
        }
        window.SetActive(true);
        bttn.interactable = false;
    }

    public void ChangeWindow(int windowNumber)
    {
        var obj = BAW[windowNumber];
        OnClick(obj.window,obj.button);
    }
}
