using System;
using UnityEngine;
using UnityEngine.UI;

public class PopUp : MonoBehaviour
{
    [SerializeField] private Button actionButton;

    public void Init(Action action)
    {
        if (action == null)
            throw new Exception("Action in popup is null");
        else
            actionButton.onClick.AddListener(() =>
            {
                if (action == null)
                    throw new Exception("Action in popup is null");
                else
                    action.Invoke();
            });
    }
}
