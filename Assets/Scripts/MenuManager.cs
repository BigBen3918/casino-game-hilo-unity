using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using SimpleJSON;

public class MenuManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void GameController(string msg);

    [Header("Text")]
    [SerializeField] TextMeshProUGUI _bet;
    [SerializeField] TextMeshProUGUI _earn;
    [SerializeField] TextMeshProUGUI _myBalance;
    [SerializeField] TextMeshProUGUI _highX;
    [SerializeField] TextMeshProUGUI _highPercent;
    [SerializeField] TextMeshProUGUI _lowX;
    [SerializeField] TextMeshProUGUI _lowPercent;

    [Header("Buttons")]
    [SerializeField] Button _spinButton;
    [SerializeField] Button _cashButton;
    [SerializeField] Button _highButton;
    [SerializeField] Button _lowButton;
    [SerializeField] Button _jokerButton;
    [SerializeField] Button _redButton;
    [SerializeField] Button _blackButton;
    [SerializeField] Button _numberButton;
    [SerializeField] Button _jqkaButton;
    [SerializeField] Button _jqButton;
    [SerializeField] Button _kaButton;
    [SerializeField] Button _betIncrease;
    [SerializeField] Button _betDecrease;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        _spinButton.onClick.AddListener(SpinHandler);
        _cashButton.onClick.AddListener(CashHandler);
        _highButton.onClick.AddListener(() => ChooseHandler(0));
        _lowButton.onClick.AddListener(() => ChooseHandler(1));
        _jokerButton.onClick.AddListener(() => ChooseHandler(2));
        _redButton.onClick.AddListener(() => ChooseHandler(3));
        _blackButton.onClick.AddListener(() => ChooseHandler(4));
        _numberButton.onClick.AddListener(() => ChooseHandler(5));
        _jqkaButton.onClick.AddListener(() => ChooseHandler(6));
        _jqButton.onClick.AddListener(() => ChooseHandler(7));
        _kaButton.onClick.AddListener(() => ChooseHandler(8));
        _betIncrease.onClick.AddListener(BetPlus);
        _betDecrease.onClick.AddListener(BetMinus);

        GlobalVariable.instance._bet = 100;
        GlobalVariable.instance._gaming = false;
        GlobalVariable.instance._highX = 0;
        GlobalVariable.instance._highPercent = 0;
        GlobalVariable.instance._lowX = 0;
        GlobalVariable.instance._lowPercent = 0;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameController("Ready");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // Spin Buttons Setting
        if (GlobalVariable.instance._spining)
        {
             _spinButton.interactable = false;
             _cashButton.interactable = false;
             _highButton.interactable = false;
             _lowButton.interactable = false;
             _jokerButton.interactable = false;
             _redButton.interactable = false;
             _blackButton.interactable = false;
             _numberButton.interactable = false;
             _jqkaButton.interactable = false;
             _jqButton.interactable = false;
             _kaButton.interactable = false;
             _betIncrease.interactable = false;
             _betDecrease.interactable = false;
        }
        else
        {
             _spinButton.interactable = true;
             _cashButton.interactable = true;
             _highButton.interactable = true;
             _lowButton.interactable = true;
             _jokerButton.interactable = true;
             _redButton.interactable = true;
             _blackButton.interactable = true;
             _numberButton.interactable = true;
             _jqkaButton.interactable = true;
             _jqButton.interactable = true;
             _kaButton.interactable = true;
             _betIncrease.interactable = true;
             _betDecrease.interactable = true;
        }

        if(GlobalVariable.instance._bridge)
        {
            _betIncrease.interactable = false;
            _betDecrease.interactable = false;
            _betIncrease.gameObject.SetActive(false);
            _betDecrease.gameObject.SetActive(false);
            _bet.gameObject.SetActive(false);
            _earn.gameObject.SetActive(true);
            _spinButton.gameObject.SetActive(false);
            _cashButton.gameObject.SetActive(true);
        }
        else
        {
            _betIncrease.interactable = true;
            _betDecrease.interactable = true;
            _betIncrease.gameObject.SetActive(true);
            _betDecrease.gameObject.SetActive(true);
            _bet.gameObject.SetActive(true);
            _earn.gameObject.SetActive(false);
            _spinButton.gameObject.SetActive(true);
            _cashButton.gameObject.SetActive(false);

            GlobalVariable.instance._earn = GlobalVariable.instance._bet;
        }

        // Bet Buttons Setting
        _bet.text = GlobalVariable.instance._bet.ToString("0.00");
        if(GlobalVariable.instance._bet == 1000)
        {
            _betIncrease.interactable = false;
            _betDecrease.interactable = true;
        }
        if(GlobalVariable.instance._bet == 100)
        {
            _betIncrease.interactable = true;
            _betDecrease.interactable = false;
        }

        // earn money setting
        if(!GlobalVariable.instance._spining)
        {
            _earn.text = GlobalVariable.instance._earn.ToString("0.00");
            _myBalance.text = GlobalVariable.instance._totalBalance.ToString("0.00");
            _highX.text = "×" + GlobalVariable.instance._highX.ToString();
            _highPercent.text = "%" + GlobalVariable.instance._highPercent.ToString();
            _lowX.text = "×" + GlobalVariable.instance._lowX.ToString();
            _lowPercent.text = "%" + GlobalVariable.instance._lowPercent.ToString();
        }
    }

    private void SpinHandler()
    {
        if(GlobalVariable.instance._totalBalance >= GlobalVariable.instance._bet)
        {
            gameManager.SpinCard(0);
            GlobalVariable.instance._earn = GlobalVariable.instance._bet;
        }
        else
        {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameController("Control");
#endif
        }
    }

    private void CashHandler()
    {
        gameManager.SpinCard(1);
    }

    private void ChooseHandler(int flag)
    {
        if (GlobalVariable.instance._bridge)
        {
            GlobalVariable.instance._ChoosePoint = flag;
            gameManager.SpinCard(2);
        }
    }

    private void BetPlus()
    {
        GlobalVariable.instance._bet = Mathf.Clamp(GlobalVariable.instance._bet + 10, 100, 1000);
        _betDecrease.interactable = true;
    }

    private void BetMinus()
    {
        GlobalVariable.instance._bet = Mathf.Clamp(GlobalVariable.instance._bet - 10, 100, 1000);
        _betIncrease.interactable = true;
    }

    public void RequestToken(string data)
    {
        JSONNode usersInfo = JSON.Parse(data);
        GlobalVariable.instance._token = usersInfo["token"];
        GlobalVariable.instance._totalBalance = float.Parse(usersInfo["amount"]);
    }
}
