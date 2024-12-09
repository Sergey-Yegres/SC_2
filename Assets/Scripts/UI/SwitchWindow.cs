using UnityEngine;
using UnityEngine.UI;

public class SwitchWindow : MonoBehaviour
{
    [SerializeField] private GameObject closeThisWindow;
    [SerializeField] private GameObject openThisWindow;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if(closeThisWindow != null)
                closeThisWindow.SetActive(false);
            if (openThisWindow != null)
                openThisWindow.SetActive(true);
        });
    }

}