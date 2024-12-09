using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class InputFieldChanger : MonoBehaviour
{
    private TMP_InputField InputField;
    [HideInInspector] public string text;
    [SerializeField]
    private int MaxLength = 20;

    private GameObject textObj;
    private void Awake()
    {
        InputField = GetComponent<TMP_InputField>();
        InputField.onSelect.AddListener((string t) => { OnSelect(); });
        InputField.onDeselect.AddListener((string t) => { OnDeselect(); });
        InputField.onValueChanged.AddListener(OnValueChange);
        textObj = InputField.textComponent.gameObject;
    }
    private void OnValueChange(string str)
    {
        text = InputField.text;
    }
    private void OnSelect()
    {
        InputField.text = text;
    }
    private void OnDeselect()
    {
        InputField.onValueChanged.RemoveAllListeners();
        text = InputField.text.ToString();
        if (InputField.text.Length > MaxLength) 
        {
            InputField.text = InputField.text.Truncate(MaxLength) + ".";
        }
        textObj.transform.localPosition = Vector3.zero;
        InputField.onValueChanged.AddListener(OnValueChange);
    }

    public void ChangeText(string t)
    {
        text = t;
        if(InputField == null)
            InputField = GetComponent<TMP_InputField>();
        InputField.text = t;
    }
}
