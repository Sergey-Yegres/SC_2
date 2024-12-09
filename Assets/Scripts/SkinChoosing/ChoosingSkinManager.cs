using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoosingSkinManager : MonoBehaviour
{
    [SerializeField] private ChooseSkinObj choosingSkinPrefab;
    private List<ChooseSkinObj> chooseSkinObjs = new List<ChooseSkinObj>();
    [SerializeField] private Transform spawnPlace;

    [SerializeField] private Button buyButton;
    [SerializeField] private TMP_Text costTMP;
    [SerializeField] private GameObject costHolder;
    [SerializeField] private Image choosedSprite;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private TMP_Text skinName;

    private void ChangeMoneyText()
    {
        moneyText.text = ((int)DataProcessor.Instance.allData.money).ToString();
    }
    private void Awake()
    {
        DataProcessor.Instance.onMoneyChange += ChangeMoneyText;
        ChangeMoneyText();
    }
    public void Init(List<Skin> skins, List<SkinsAndNames> skinsAndNames)
    {
        for(int i = 0; i < skins.Count; i++) 
        {
            var obj = Instantiate(choosingSkinPrefab, spawnPlace);
            obj.Init(skins[i], skinsAndNames[i].Skin, ChooseSkin, i);
            chooseSkinObjs.Add(obj);
        }
        if (isSpaceflight)
        {
            choosedSprite.sprite = DataProcessor.Instance.SpaceFlightSkinsHolder[DataProcessor.Instance.allData.CurrentFlightSkin].Skin;

            skinName.text = DataProcessor.Instance.SpaceFlightSkinsHolder[DataProcessor.Instance.allData.CurrentFlightSkin].Name;

        }
        else
        {
            choosedSprite.sprite = DataProcessor.Instance.LightOffSkinsHolder[DataProcessor.Instance.allData.CurrentTurnOffSkin].Skin;

            skinName.text = DataProcessor.Instance.LightOffSkinsHolder[DataProcessor.Instance.allData.CurrentTurnOffSkin].Name;

        }
    }
    [SerializeField] private bool isSpaceflight;
    public void BuySkin()
    {
        var data = DataProcessor.Instance.allData;
        Skin skin;
        if (isSpaceflight)
        {
            skin = DataProcessor.Instance.BuySpaceFlightSkin(currentSkin.skin.Name);

        }
        else
            skin = DataProcessor.Instance.BuyLightOffSkin(currentSkin.skin.Name);

        currentSkin.Init(skin, currentSkin.sprite, ChooseSkin, currentSkin.skinNum);
        ChooseSkin(currentSkin, currentSkin.skinNum);
        if(currentSkin.skin.buyed)
        {
            buyButton.gameObject.SetActive(false);
            costHolder.gameObject.SetActive(false);
        }

    }
    private ChooseSkinObj currentSkin;
    private void ChooseSkin(ChooseSkinObj skin, int skinNum)
    {
        currentSkin = skin;
        skinName.text = currentSkin.skin.Name;
        buyButton.gameObject.SetActive(!skin.skin.buyed);
        buyButton.interactable = skin.skin.Cost <= DataProcessor.Instance.allData.money;
        costHolder.gameObject.SetActive(!skin.skin.buyed);
        costTMP.text = skin.skin.Cost.ToString();
        if(isSpaceflight)
            choosedSprite.sprite = DataProcessor.Instance.SpaceFlightSkinsHolder[skinNum].Skin;
        else
            choosedSprite.sprite = DataProcessor.Instance.LightOffSkinsHolder[skinNum].Skin;

        if (skin.skin.buyed)
        {
            var data = DataProcessor.Instance.allData;
            if (isSpaceflight)
            {
                data.CurrentFlightSkin = skinNum;
                Parser.StartSave();
                skin.Init(data.SpaceFlightSkins[skinNum], DataProcessor.Instance.SpaceFlightSkinsHolder[skinNum].Skin, ChooseSkin, skinNum);

            }
            else
            {
                data.CurrentTurnOffSkin = skinNum;
                Parser.StartSave();
                skin.Init(data.LightOffSkins[skinNum], DataProcessor.Instance.LightOffSkinsHolder[skinNum].Skin, ChooseSkin, skinNum);
            }
        }
    }
}
