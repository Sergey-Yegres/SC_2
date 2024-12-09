using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnOffGameManager : MonoBehaviour
{
    [SerializeField] private List<TurnOffObj> turnOffObjs;
    private Action onGameStart;
    public float CurrentMultiplier = 1;
    private Coroutine gameRoutine;
    private void Awake()
    {
        SetBets();
        DataProcessor.Instance.onMoneyChange += ChangeMoneyText;
        ChangeMoneyText();
    }
    [SerializeField] private TMP_Text moneyText;

    Coroutine autoplayCoroutine;
    [SerializeField] GameObject autoplayEnabled;
    [SerializeField] GameObject autoplayDisabled;
    bool autoplay = false;

    public void StartAutoPlay()
    {
        autoplayCoroutine = StartCoroutine(Autoplay());
        autoplayEnabled.SetActive(true);
        autoplayDisabled.SetActive(false);
        autoPlayTMP.gameObject.SetActive(true);
        startGame.interactable = false;
        autoplay = true;

    }
    public void StopAutoPlay()
    {
        if (autoplayCoroutine != null)
            StopCoroutine(autoplayCoroutine);

        autoplayEnabled.SetActive(false);
        autoplayDisabled.SetActive(true);
        autoPlayTMP.gameObject.SetActive(false);
        startGame.interactable = true;
        autoplay = false;

    }
    [SerializeField] private TMP_Text autoPlayTMP;
    private IEnumerator Autoplay()
    {
        while (true)
        {
            autoPlayTMP.gameObject.SetActive(true);

            for (int seconds = 5; seconds > 0; seconds--)
            {
                autoPlayTMP.text = "The game will start in " + seconds + " sec";
                yield return new WaitForSeconds(1);
            }
            autoPlayTMP.gameObject.SetActive(false);

            StartGame();
            gameGoing = true;
            while (gameGoing)
            {
                yield return new WaitForSeconds(.1f);
            }
            RefreshButtons();
        }
    }
    bool gameGoing = false;
    private void ChangeMoneyText()
    {
        moneyText.text = ((int)DataProcessor.Instance.allData.money).ToString();
    }
    private void OnDisable()
    {
        StopAutoPlay();
        gameGoing = false;
    }
    private int bet5Count = 0;

    public void StartGame()
    {
        CurrentMultiplier = 1;
        if (bet5Count >= 5)
        {
            var data = DataProcessor.Instance.allData;
            data.place5Bets.Date = DateTime.Now.ToString().Remove(10);
            data.place5Bets.Completed = true;
        }
        foreach (var turnOffObj in turnOffObjs)
        {

            turnOffObj.Init(this);
            if(turnOffObj.objInGame)
                turnOffObj.OnGameStart();
            else
                turnOffObj.gameObject.SetActive(false);
        }
        gameRoutine = StartCoroutine(Game());
        onGameStart?.Invoke();
    }
    private const float maxMultiplier = 10f;
    [SerializeField] private TMP_Text rateText;
    [SerializeField] private Button startGame;
    [SerializeField] private Button allOff;
    [SerializeField] private Button refreshButton;

    public void AllOff()
    {
        foreach (var turnOffObj in turnOffObjs)
        {
            turnOffObj.StopGame();
        }
    }
    private List<BetObject> betObjects = new List<BetObject>();
    [SerializeField] private BetObject betPrefab;
    [SerializeField] private Transform betSpawnPlace;
    [SerializeField] private Color minusColor;
    [SerializeField] private Color plusColor;
    private void SetBets()
    {
        foreach (var betObject in betObjects)
            Destroy(betObject.gameObject);
        betObjects.Clear();

        foreach (var item in DataProcessor.Instance.allData.LightOffBets)
        {
            var obj = Instantiate(betPrefab, betSpawnPlace);
            obj.transform.SetSiblingIndex(1);
            betObjects.Add(obj);

            if (item < 0)
            {
                obj.info.text = (item * (-1)).ToString("0.00");
                obj.info.color = minusColor;
            }
            else
            {
                obj.info.text = item.ToString("0.00");
                obj.info.color = plusColor;
            }
        }
    }
    private IEnumerator Game()
    {
        startGame.gameObject.SetActive(false);
        allOff.gameObject.SetActive(true);
        refreshButton.gameObject.SetActive(false);
        bool gameCanGoing = true;
        float stopMultiplier = UnityEngine.Random.Range(1, maxMultiplier);

        for (; CurrentMultiplier <= maxMultiplier; CurrentMultiplier += .01f)
        {
            rateText.text = CurrentMultiplier.ToString("0.00") + "X";
            yield return new WaitForSeconds(.01f);
            if (autoplay)
            {
                if (stopMultiplier <= CurrentMultiplier)
                {
                    AllOff();
                }
            }
            gameCanGoing = false;
            foreach (var item in  turnOffObjs)
            {
                if (item.gameGoing)
                {
                    gameCanGoing = true; 
                    break;
                }
            }
            if (!gameCanGoing)
            {
                gameGoing = false;
                startGame.gameObject.SetActive(false);
                allOff.gameObject.SetActive(false);
                refreshButton.gameObject.SetActive(true);
                StopCoroutine(gameRoutine);
            }
        }
    }
    public void RefreshButtons()
    {
        startGame.gameObject.SetActive(true);
        allOff.gameObject.SetActive(false);
        refreshButton.gameObject.SetActive(false);
        foreach (var turnOffObj in turnOffObjs)
        {
            turnOffObj.gameObject.SetActive(true);
            turnOffObj.RefreshObject();

            var data = DataProcessor.Instance;

            foreach (var item in data.LightOffSkinsHolder)
            {
                if (data.allData.LightOffSkins[data.allData.CurrentTurnOffSkin].Name == item.Name)
                    turnOffObj.player.sprite = item.Skin;
            }
        }
        CurrentMultiplier = 1;
        rateText.text = "";
        SetBets();
    }
}
