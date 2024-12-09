using TMPro;
using UnityEngine;

public class ChangeVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI version;
    private void Awake()
    {
        version.text = Application.version;
    }
}