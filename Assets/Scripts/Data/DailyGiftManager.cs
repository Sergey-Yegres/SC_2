using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyGiftManager : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Button dailyButton;
    [SerializeField] private GameObject dailyRewardGeted;
    [SerializeField] private List<DailyObj> dailyObjs = new List<DailyObj>();
    public void CheckEveryDay()
    {
        var data = DataProcessor.Instance.allData;
        if(data.everyDay == 7 && data.lastDate == DateTime.Now.ToString().Remove(10))
        {
            data.everyDay = 0;
        }

        if (data.lastDate == DateTime.Now.AddDays(-1).ToString().Remove(10))
        {
            data.everyDay += 1;
            dailyButton.gameObject.SetActive(true);
            dailyRewardGeted.SetActive(false);
        }
        else
        {
            dailyButton.gameObject.SetActive(false);
            dailyRewardGeted.SetActive(true);
            DateTime.TryParse(data.lastDate, out var d);
            if (d < DateTime.Now.AddDays(-1))
            {
                data.everyDay = 1;
                dailyButton.gameObject.SetActive(true);
                dailyRewardGeted.SetActive(false);
            }

        }
        slider.value = (float)((float)data.everyDay / (float)7);
        for(int i = 0; i < dailyObjs.Count; i++)
        {
            dailyObjs[i].Init(data.everyDay > i, data.everyDay == i);
        }
    }
    public void GetDailyReward()
    {
        DataProcessor.Instance.allData.lastDate = DateTime.Now.ToString().Remove(10);
        var data = DataProcessor.Instance.allData;
        if (data.everyDay == 3)
            DataProcessor.Instance.ChangeMoney(100);
        else if (data.everyDay == 7)
            DataProcessor.Instance.ChangeMoney(500);
        else
            DataProcessor.Instance.ChangeMoney(50);

        dailyButton.gameObject.SetActive(false);
        dailyRewardGeted.SetActive(true);
        Parser.StartSave();

    }
}
