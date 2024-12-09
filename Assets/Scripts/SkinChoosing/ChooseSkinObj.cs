using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseSkinObj : MonoBehaviour
{
    [SerializeField] private Image skinImage;
    [SerializeField] private TMP_Text skinName;

    [SerializeField] private GameObject choosedObject;
    [SerializeField] private GameObject boughtObj;

    [SerializeField] private TMP_Text costText;
    [SerializeField] private GameObject blockedObj;
    public Skin skin;
    public int skinNum;
    public Sprite sprite;
    public void Init(Skin skin, Sprite sprite, Action<ChooseSkinObj, int> action, int skinNum)
    {
        this.skin = skin;
        this.skinNum = skinNum;
        this.sprite = sprite;
        skinImage.sprite = sprite;
        skinName.text = skin.Name;
        choosedObject.SetActive(skin.Choosed);
        boughtObj.SetActive(skin.buyed);
        costText.gameObject.SetActive(!skin.buyed);
        costText.text = skin.Cost.ToString();
        blockedObj.SetActive(!skin.buyed);
        GetComponent<Button>().onClick.AddListener(() =>
        {
            action?.Invoke(this, skinNum);
        });
    }
}
