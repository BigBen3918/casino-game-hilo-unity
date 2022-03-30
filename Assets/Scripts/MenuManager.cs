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
    [SerializeField] TMP_InputField _bet;
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

    [Header("GameObeject")]
    [SerializeField] GameObject _beterror;

    private GameManager gameManager;
    private Animator betErrorAnim;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        betErrorAnim = _beterror.GetComponent<Animator>();

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
        _bet.onValueChanged.AddListener(betChange);

        GlobalVariable._bet = 100;
        GlobalVariable._gaming = false;
        GlobalVariable._highX = 0;
        GlobalVariable._highPercent = 0;
        GlobalVariable._lowX = 0;
        GlobalVariable._lowPercent = 0;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameController("Ready");
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // Spin Buttons Setting
        if (GlobalVariable._spining)
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

        if(GlobalVariable._bridge)
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

            GlobalVariable._earn = GlobalVariable._bet;
        }

        // Bet Buttons Setting
        _bet.text = GlobalVariable._bet.ToString();
        if(GlobalVariable._bet == 100000)
        {
            _betIncrease.interactable = false;
            _betDecrease.interactable = true;
        }
        if(GlobalVariable._bet == 10)
        {
            _betIncrease.interactable = true;
            _betDecrease.interactable = false;
        }

        // earn money setting
        if(!GlobalVariable._spining)
        {
            _earn.text = GlobalVariable._earn.ToString();
            _myBalance.text = GlobalVariable._totalBalance.ToString("0.00");
            _highX.text = "×" + GlobalVariable._highX.ToString();
            _highPercent.text = "%" + GlobalVariable._highPercent.ToString();
            _lowX.text = "×" + GlobalVariable._lowX.ToString();
            _lowPercent.text = "%" + GlobalVariable._lowPercent.ToString();
        }
    }

    private void SpinHandler()
    {
        Debug.Log(GlobalVariable._bet.ToString());
        if(GlobalVariable._totalBalance >= (float)GlobalVariable._bet)
        {
            gameManager.SpinCard(0);
            GlobalVariable._earn = GlobalVariable._bet;
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
        if (GlobalVariable._bridge)
        {
            GlobalVariable._ChoosePoint = flag;
            gameManager.SpinCard(2);
        } else
        {
            StartCoroutine(betting());
        }
    }

    IEnumerator betting()
    {
        Debug.Log("abc");
        betErrorAnim.SetBool("beterror", true);
        yield return new WaitForSeconds(2.5f);
        betErrorAnim.SetBool("beterror", false);
    }

    private void BetPlus()
    {
        GlobalVariable._bet = Mathf.Clamp(GlobalVariable._bet + 10, 10, 100000);
        _betDecrease.interactable = true;
    }

    private void BetMinus()
    {
        GlobalVariable._bet = Mathf.Clamp(GlobalVariable._bet - 10, 10, 100000);
        _betIncrease.interactable = true;
    }

    void betChange(string inputData)
    {
        if (string.IsNullOrEmpty(inputData))
        {
            GlobalVariable._bet = 10;
            _bet.text = "10";

            return;
        }
        else
        {
            if (int.Parse(inputData) > 100000)
            {
                GlobalVariable._bet = 100000;
                _bet.text = "100000";

                return;
            }

            if (int.Parse(inputData) < 10)
            {
                GlobalVariable._bet = 10;
                _bet.text = "10";

                return;
            }
        }

        GlobalVariable._bet = int.Parse(inputData);
        _bet.text = GlobalVariable._bet.ToString();
        _betIncrease.interactable = true;
        _betDecrease.interactable = true;
    }

    public void RequestToken(string data)
    {
        JSONNode usersInfo = JSON.Parse(data);
        GlobalVariable._token = usersInfo["token"];
        GlobalVariable._totalBalance = float.Parse(usersInfo["amount"]);
    }

    
}
