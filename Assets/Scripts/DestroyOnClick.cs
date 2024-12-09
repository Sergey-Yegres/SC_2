using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyOnClick : MonoBehaviour
{
    public GameObject gm;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => Destroy(gm));
    }
}
