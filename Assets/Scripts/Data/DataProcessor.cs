using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataProcessor : MonoBehaviour
{
    public Root allData;
    public static DataProcessor Instance;
    public List<SkinsAndNames> SpaceFlightSkinsHolder;
    public List<SkinsAndNames> LightOffSkinsHolder;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private ChoosingSkinManager FlightSkinManager;
    [SerializeField] private ChoosingSkinManager LightOffSkinManager;
    public void ChangeAudio(bool disabled)
    {
        allData._isAudioDisabled = disabled;
    }
    private void Awake()
    {
        Instance = this;
    }
    [SerializeField] private DailyGiftManager dailyGiftManager;
    [SerializeField] private MainMenu mainMenu;
    public void LoadData(Root data)
    {
        allData = data;
        FlightSkinManager.Init(data.SpaceFlightSkins, SpaceFlightSkinsHolder);
        LightOffSkinManager.Init(data.LightOffSkins, LightOffSkinsHolder);
        dailyGiftManager.CheckEveryDay();
        mainMenu.Init();
        if (allData._isAudioDisabled)
            audioManager.SwitchAudioEvent();
    }

    public Skin BuySpaceFlightSkin(string skinName)
    {
        Skin skin = allData.SpaceFlightSkins.FirstOrDefault(obj => obj.Name == skinName);
        Debug.Log(skin);

        if (allData.money >= skin.Cost)
        {
            skin.buyed = true;
            skin.Choosed = true;
            ChangeMoney(-skin.Cost);
            Parser.StartSave();

        }
        return skin;
    }
    public Skin BuyLightOffSkin(string skinName)
    {
        Skin skin = allData.LightOffSkins.FirstOrDefault(obj => obj.Name == skinName);
        if (allData.money >= skin.Cost)
        {
            skin.buyed = true;
            skin.Choosed = true;
            ChangeMoney(-skin.Cost);
            Parser.StartSave();
        }
        return skin;

    }
    public Action onMoneyChange;
    public void ChangeMoney(float cost)
    {
        allData.money += cost;
        onMoneyChange?.Invoke();

        Parser.StartSave();
    }
}
[Serializable]
public class SkinsAndNames
{
    public Sprite Skin;
    public string Name;
}