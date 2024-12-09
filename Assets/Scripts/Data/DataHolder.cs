using JetBrains.Annotations;
using System;
using System.Collections.Generic;

[Serializable]
public class Root
{
    public float money;
    public List<Skin> SpaceFlightSkins = new List<Skin>();
    public List<Skin> LightOffSkins = new List<Skin>();
    public List<float> SpaceFlightBets = new List<float>();
    public List<float> LightOffBets = new List<float>();
    public int CurrentFlightSkin;
    public int CurrentTurnOffSkin;

    public int everyDay;
    public string lastDate;

    public Reward place5Bets;
    public Reward winGreater2X;
    public Reward win3Row;
    public bool _isAudioDisabled = false;
    //public List<string> everyDays = new List<string>();
}
[Serializable]
public class Reward
{
    public string Date;
    public bool Completed;
    public bool RewardGeted;
}
[Serializable]
public class Skin
{
    public int Cost;
    public bool buyed;
    public string Name;
    public bool Choosed;
}