using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;

public class TruncateText : MonoBehaviour
{
    private TextMeshProUGUI text;
    public int maxLength = 20;
    public void Truncate()
    {
        text = GetComponent<TextMeshProUGUI>();
        if(text.text.Length > maxLength)
            text.text = text.text.Truncate(maxLength,"...");
    }
}
