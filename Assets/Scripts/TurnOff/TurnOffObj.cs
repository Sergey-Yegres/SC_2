using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffObj : MonoBehaviour
{
    [SerializeField] private GameObject onStart;
    [SerializeField] private GameObject inGame;

    public bool objInGame = true;

    [SerializeField] private GameObject inGameMark;
    [SerializeField] private GameObject notInGameMark;

    public void ChangeObjectInGameState()
    {
        objInGame = !objInGame;
        inGameMark.SetActive(objInGame);
        notInGameMark.SetActive(!objInGame);
    }
    public void RefreshObject()
    {
        onStart.SetActive(true);
        inGame.SetActive(false);
    }
    public void SetGameState()
    {
        onStart.SetActive(false);
        inGame.SetActive(true);
        winObject.SetActive(false);
        loseObject.SetActive(false);
    }
    private TurnOffGameManager gameManager;
    private void OnEnable()
    {
        var data = DataProcessor.Instance;
        foreach (var item in data.LightOffSkinsHolder)
        {
            if (data.allData.LightOffSkins[data.allData.CurrentTurnOffSkin].Name == item.Name)
            {
                player.sprite = item.Skin;
            }
        }
    }
    public void Init(TurnOffGameManager gameManager)
    {
        this.gameManager = gameManager;
        var data = DataProcessor.Instance;
        foreach (var item in data.LightOffSkinsHolder)
        {
            if (data.allData.LightOffSkins[data.allData.CurrentTurnOffSkin].Name == item.Name)
            {
                player.sprite = item.Skin;
            }
        }
    }
    public bool gameGoing = false;
    public void StopGame()
    {
        gameGoing = false;
    }
    public void OnGameStart()
    {
        SetGameState();
        gameCoroutine = StartCoroutine(CheckObj());
    }
    private const float maxMultiplier = 10f;
    private Coroutine gameCoroutine;
    [SerializeField] private TMP_Text loseMultiplier;
    [SerializeField] private TMP_Text winMultiplier;


    public IEnumerator CheckObj()
    {
        gameGoing = true;
        float losingMultiplier = UnityEngine.Random.Range(1, maxMultiplier);
        for (; gameManager.CurrentMultiplier <= losingMultiplier;)
        {
            yield return new WaitForSeconds(.01f);
            if (!gameGoing)
            {
                Win();
            }
        }
        Lose();
    }
    public Image player;
    [SerializeField] private Sprite LoseSprite;
    private float currentBet = minBet;
    [SerializeField] private TMP_Text betTMP;
    [SerializeField] private GameObject winObject;
    [SerializeField] private GameObject loseObject;
    public float multiplier;

    [SerializeField] private AudioManager audioManager;
    private int winRow = 0;

    [SerializeField] private TMP_Text winMultiplierTMP;
    [SerializeField] private TMP_Text loseMultiplierTMP;
    [SerializeField] private TMP_Text winnedTMP;
    private void Win()
    {
        gameGoing = false;
        StopCoroutine(gameCoroutine);
        audioManager.PlayWinClip();
        winnedTMP.text = "+" + (currentBet * gameManager.CurrentMultiplier).ToString();
        winMultiplierTMP.text = gameManager.CurrentMultiplier.ToString("0.00") + "X";
        DataProcessor.Instance.ChangeMoney(currentBet * gameManager.CurrentMultiplier);
        DataProcessor.Instance.allData.LightOffBets.Add(gameManager.CurrentMultiplier);
        loseMultiplier.text = gameManager.CurrentMultiplier.ToString("0.00") + "X";
        winObject.SetActive(true);
        loseObject.SetActive(false);
        winRow += 1;
        if(winRow >= 3)
        {
            var data = DataProcessor.Instance.allData;
            data.win3Row.Date = DateTime.Now.ToString().Remove(10);
            data.win3Row.Completed = true;
        }
        if (multiplier >= 2)
        {
            var data = DataProcessor.Instance.allData;
            data.winGreater2X.Date = DateTime.Now.ToString().Remove(10);
            data.winGreater2X.Completed = true;
        }
    }
    private void Lose()
    {
        gameGoing = false;
        audioManager.PlayBoomClip();
        loseMultiplierTMP.text = gameManager.CurrentMultiplier.ToString("0.00") + "X";

        DataProcessor.Instance.ChangeMoney(-currentBet);
        DataProcessor.Instance.allData.LightOffBets.Add(-gameManager.CurrentMultiplier);
        winRow = 0;
        player.sprite = LoseSprite;
        player.transform.localScale = new Vector2(0, 0);
        player.transform.DOScale(new Vector2(1, 1), .5f);
        winMultiplier.text = gameManager.CurrentMultiplier.ToString("0.00") + "X";
        winObject.SetActive(false);
        loseObject.SetActive(true);
    }
    public void PlusBet(int plus)
    {
        currentBet += plus;
        CheckBet();
        betTMP.text = currentBet.ToString();
    }
    private const int maxBet = 100;
    private const int minBet = 10;
    private void CheckBet()
    {
        if (currentBet < minBet)
            currentBet = minBet;

        if (currentBet > maxBet)
            currentBet = maxBet;
        if (currentBet > DataProcessor.Instance.allData.money/3)
            currentBet = DataProcessor.Instance.allData.money/3;
    }
}
