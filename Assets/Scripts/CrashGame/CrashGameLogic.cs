using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CrashGameLogic : MonoBehaviour
{
    [SerializeField] private TMP_Text rateText;

    [SerializeField] private RectTransform Trajectory;
    private const float  maxMultiplier = 20f;

    [SerializeField] private Button GoButton;
    [SerializeField] private Button StopButton;
    private Coroutine gameCoroutine;
    private List<BetObject> betObjects = new List<BetObject>();
    [SerializeField] private BetObject betPrefab;
    [SerializeField] private Transform betSpawnPlace;

    [SerializeField] private Color minusColor;
    [SerializeField] private Color plusColor;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private Sprite LoseSprite;
    private void SetBets()
    {
        foreach (var betObject in betObjects)
            Destroy(betObject.gameObject);
        betObjects.Clear();

        foreach(var item in DataProcessor.Instance.allData.SpaceFlightBets)
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
    Coroutine autoplayCoroutine;
    [SerializeField] GameObject autoplayEnabled;
    [SerializeField] GameObject autoplayDisabled;
    bool autoplay = false;
    public void StartAutoPlay()
    {
        autoplayCoroutine = StartCoroutine(Autoplay());
        autoplayEnabled.SetActive(true);
        autoplayDisabled.SetActive(false);
        GoButton.interactable = false;
        autoplay = true;

    }
    public void StopAutoPlay()
    {
        if (autoplayCoroutine != null)
            StopCoroutine(autoplayCoroutine);

        autoplayEnabled.SetActive(false);
        autoplayDisabled.SetActive(true);
        GoButton.interactable = true;
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
            StopButton.gameObject.SetActive(true);
            GoButton.gameObject.SetActive(false);
            while (gameGoing)
            {
                yield return new WaitForSeconds(.1f);
            }
            if(gameCoroutine!= null)
            {
                StopGame();
            }
            ResetButtons();
        }
    }
    [SerializeField] private Image player;
    private void SetPlayerSprite()
    {
        var data = DataProcessor.Instance;
        foreach (var item in data.SpaceFlightSkinsHolder)
        {
            if (data.allData.SpaceFlightSkins[data.allData.CurrentFlightSkin].Name == item.Name)
            {
                player.sprite = item.Skin;
            }
        }
    }
    private void OnEnable()
    {
        SetPlayerSprite();
    }
    private void OnDisable()
    {
        StopAutoPlay();
        gameGoing = false;
    }
    private void Awake()
    {
        GoButton.onClick.AddListener(StartGame);
        StopButton.onClick.AddListener(StopGame);
        ResetButtons();
        SetBets();
        DataProcessor.Instance.onMoneyChange += ChangeMoneyText;
        ChangeMoneyText();

    }
    private void ChangeMoneyText()
    {
        moneyText.text = ((int)DataProcessor.Instance.allData.money).ToString();
    }
    private void ResetButtons()
    {
        GoButton.gameObject.SetActive(true); 
        StopButton.gameObject.SetActive(false);
    }
    private void StopGame()
    {
        gameGoing = false;
    }
    bool gameGoing = false;
    float multiplier = 1;
    public void StartGame()
    {
        multiplier = 1;
        bet5Count++;
        gameCoroutine = StartCoroutine(Game());
        if(bet5Count >= 5)
        {
            var data = DataProcessor.Instance.allData;
            data.place5Bets.Date = DateTime.Now.ToString().Remove(10);
            data.place5Bets.Completed = true;
        }
    }
    private int bet5Count = 0;

    public IEnumerator Game()
    {
        audioManager.PlayFlightClip();
        GoButton.gameObject.SetActive(false);
        StopButton.gameObject.SetActive(true);
        SetPlayerSprite();
        gameGoing = true;
        Trajectory.sizeDelta = new Vector2(0, 150);
        float losingMultiplier = UnityEngine.Random.Range(1, maxMultiplier);
        float stopMultiplier = UnityEngine.Random.Range(1, maxMultiplier);
        Sequence sequence = DOTween.Sequence()
            .Append(Trajectory.DOSizeDelta(new Vector2(355, 355), 2 - multiplier, false)) // 2
            .Append(Trajectory.DOSizeDelta(new Vector2(545, 545), 5 - multiplier, false)) // 5
            .Append(Trajectory.DOSizeDelta(new Vector2(735, 735), 10 - multiplier, false)) // 10
            .Append(Trajectory.DOSizeDelta(new Vector2(955, 955), 20 - multiplier, false)); // 20

        for (; multiplier <= losingMultiplier; multiplier += .01f)
        {
            rateText.text = multiplier.ToString("0.00") + "X";
            yield return new WaitForSeconds(.01f);
            if (autoplay)
            {
                if(stopMultiplier <= multiplier)
                {
                    gameGoing = false;
                }
            }
            if (!gameGoing)
            {
                sequence.Kill();
                Win();
                StopCoroutine(gameCoroutine);
            }
        }
        sequence.Kill();
        Lose();
        multiplier = 1;
    }
    [SerializeField] private AudioManager audioManager;
    private int winRow = 0;

    private void Win()
    {
        audioManager.StopFlightAudio();
        audioManager.PlayWinClip();
        DataProcessor.Instance.ChangeMoney(currentBet * multiplier);
        DataProcessor.Instance.allData.SpaceFlightBets.Add(multiplier);
        winRow += 1;
        if (winRow >= 3)
        {
            var data = DataProcessor.Instance.allData;
            data.win3Row.Date = DateTime.Now.ToString().Remove(10);
            data.win3Row.Completed = true;
        }
        if (multiplier>= 2)
        {
            var data = DataProcessor.Instance.allData;
            data.winGreater2X.Date = DateTime.Now.ToString().Remove(10);
            data.winGreater2X.Completed = true;
        }
        ResetButtons();
        SetBets();
    }
    private void Lose()
    {
        audioManager.PlayBoomClip();

        DataProcessor.Instance.ChangeMoney(-currentBet);
        DataProcessor.Instance.allData.SpaceFlightBets.Add(-multiplier);
        winRow = 0;
        player.sprite = LoseSprite;
        player.transform.localScale = new Vector2(0, 0);
        player.transform.DOScale(new Vector2(1, 1), .5f);
        gameGoing = false;
        ResetButtons();
        SetBets();

    }
    private const int maxBet = 200;
    private const int minBet = 10;

    private float currentBet = minBet;
    [SerializeField] private TMP_Text betTMP;

    public void PlusBet(int plus)
    {
        currentBet += plus;
        CheckBet();
        betTMP.text = currentBet.ToString();

    }
    public void MultiplyBet(int multiplier)
    {
        currentBet*=multiplier;
        CheckBet();
        betTMP.text = currentBet.ToString();

    }
    public void DivideBet(int divider)
    {
        currentBet /= divider;
        CheckBet();
        betTMP.text = currentBet.ToString();

    }
    private void CheckBet()
    {
        if (currentBet < minBet)
            currentBet = minBet;

        if(currentBet > maxBet)
            currentBet = maxBet;
        if (currentBet > DataProcessor.Instance.allData.money)
            currentBet = DataProcessor.Instance.allData.money;
    }
}
