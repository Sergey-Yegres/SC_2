using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private bool isInited = false;

    public void Init()
    {
        isInited = true;
        DataProcessor.Instance.onMoneyChange += ChangeMoneyText;
        ChangeMoneyText();

        CheckAll();
    }
    private void OnEnable()
    {
        if (isInited)
        {
            CheckAll();
        }
    }
    [SerializeField] private GameObject place5BetsButton;
    [SerializeField] private GameObject place5BetsCompleted;

    [SerializeField] private GameObject winGreater2XButton;
    [SerializeField] private GameObject winGreater2XCompleted;

    [SerializeField] private GameObject win3RowButton;
    [SerializeField] private GameObject win3RowCompleted;

    [SerializeField] private TMP_Text moneyText;

    private void ChangeMoneyText()
    {
        moneyText.text = ((int)DataProcessor.Instance.allData.money).ToString();
    }
    private void CheckNormalStateInReward(Reward reward)
    {
        if (reward.Date != DateTime.Now.ToString().Remove(10))
        {
            reward.Date = DateTime.Now.ToString().Remove(10);
            reward.RewardGeted = false;
            reward.Completed = false;
        }
    }
    private void CheckRewardCompleted(Reward reward, GameObject button, GameObject completed)
    {
        if (!reward.RewardGeted)
        {
            if (reward.Completed)
            {
                button.SetActive(true);
            }
        }
        else
            completed.SetActive(true);
    }
    public void CheckAll()
    {
        var data = DataProcessor.Instance.allData;
        CheckNormalStateInReward(data.place5Bets);
        CheckNormalStateInReward(data.winGreater2X);
        CheckNormalStateInReward(data.win3Row);

        CheckRewardCompleted(data.place5Bets, place5BetsButton, place5BetsCompleted);
        CheckRewardCompleted(data.winGreater2X, winGreater2XButton, winGreater2XCompleted);
        CheckRewardCompleted(data.win3Row, win3RowButton, win3RowCompleted);

        Parser.StartSave();

    }

    public void Complete5Bets()
    {
        var data = DataProcessor.Instance;

        SetCompletedState(data.allData.place5Bets, place5BetsButton, place5BetsCompleted);
        data.ChangeMoney(50);
    }
    public void CompleteWinGreater2X()
    {
        var data = DataProcessor.Instance;

        SetCompletedState(data.allData.winGreater2X, winGreater2XButton, winGreater2XCompleted);
        data.ChangeMoney(100);
    }
    public void CompleteWin3Row()
    {
        var data = DataProcessor.Instance;

        SetCompletedState(data.allData.win3Row, win3RowButton, win3RowCompleted);
        data.ChangeMoney(200);
    }
    private void SetCompletedState(Reward reward, GameObject button, GameObject completed)
    {
        reward.RewardGeted = true;
        button.SetActive(false);
        completed.SetActive(true);
    }
}
