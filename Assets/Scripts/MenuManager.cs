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

        GlobalVariable._bet = 100;
        GlobalVariable._gaming = false;
        GlobalVariable._highX = 0;
        GlobalVariable._highPercent = 0;
        GlobalVariable._lowX = 0;
        GlobalVariable._lowPercent = 0;

#if UNITY_WEBGL == true && UNITY_EDITOR == false
    GameController("Ready");
#endif

//#if UNITY_EDITOR == true
//        GlobalVariable._token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJfaWQiOiI2MWU0MzllZDM3ZTA5OThjMWI5ZjVjMTYiLCJhY2NvdW50IjoiMHg4NDEyNDlmYTZCOTg4Njg0NDAzQ0RhNjYzZTEyNzI4MmViMTM2Q0JFIiwiYmFsYW5jZSI6MTAwMDQ1OTU1NS4wMywibmFtZSI6IkFuZHJleSBMb3ppbiIsImVtYWlsIjoiTG96aW5AZ21haWwuY29tIiwiYXZhdGFyIjoiLy93d3cuZ3JhdmF0YXIuY29tL2F2YXRhci9kNDE1ZjBlMzBjNDcxZGZkZDliYzRmODI3MzI5ZWY0OD9zPTIwMCZyPXBnJmQ9bW0iLCJkYXRlIjoiMjAyMi0wMS0xNlQxNToyOTo0OS40NDNaIiwiYWxsb3dhbmNlcyI6W3siZ2FtZVBvb2xBZGRyZXNzIjoiMHhENDRCMDJDZWYxOTUwMDc5OGNCZTdiNjc3M2FFOUE5NzgxMTlhYjRlIiwiYW1vdW50IjoxODAwLCJfaWQiOiI2MWU1ODNkODlmMzY4ZGE4ODY3ZThkM2YifSx7ImdhbWVQb29sQWRkcmVzcyI6IjB4RGE0RTNjOWY2ODcwMjI3RmNjRTg0ZDZjMjA2NzUwM0NCYjA1NDM3YyIsImFtb3VudCI6MTAwMCwiX2lkIjoiNjFlYTUxYWEzZTc0YTliMWI0YjcxMWM5In0seyJnYW1lUG9vbEFkZHJlc3MiOiIweDlEQjVGMTVkOUMyZDc3MEI3QzhlRTU5NTQ1OTJjNzkwQjQwZDM4MzQiLCJhbW91bnQiOjEwMDAwMDAsIl9pZCI6IjYxZWE1MWI4M2U3NGE5YjFiNGI3MTFkOCJ9LHsiZ2FtZVBvb2xBZGRyZXNzIjoiMHgzRmI3N0Y1QUI3MDMzZjU1MDBiQUMzMzZGQUE2MURmQkM4M2I4MTAwIiwiYW1vdW50Ijo0ODgyMDAsIl9pZCI6IjYxZWE1Y2VmM2U3NGE5YjFiNGI3MTVmNSJ9LHsiZ2FtZVBvb2xBZGRyZXNzIjoiMHhDYjFjYjc0MDM2OUIyNzA0MTkyRGVlNGE5RDk4NTg4NDU0MDUyNjU5IiwiYW1vdW50IjoxMDY2OTkwLCJfaWQiOiI2MWVhNjA5ODNlNzRhOWIxYjRiNzFhNjcifSx7ImdhbWVQb29sQWRkcmVzcyI6IjB4ZjBmOTE3MDgyOGYyN2EzNGNDMzgzNTk3MTRFNmI4QmU1MDBFOTRhNiIsImFtb3VudCI6MTAwMCwiX2lkIjoiNjFlYTcwYjRlMGRiOTNkOTg3ODhmZTY2In0seyJnYW1lUG9vbEFkZHJlc3MiOiIweDAxZDlGOEZlNWE0MTAyNTkyRjU0Nzc0MDRBZDFEYWZiYjM2MmQ3RGEiLCJhbW91bnQiOjk4MCwiX2lkIjoiNjFlYWIwZWU3ZTRhYzJjOTA3MzQ5MDRjIn0seyJnYW1lUG9vbEFkZHJlc3MiOiIweEQ0ZEVjMTE3QUYwYzkyRThlQTY0YTI3NUEzNjdmYWNlZDczYWMzMGMiLCJhbW91bnQiOjM1OTAwLCJfaWQiOiI2MWViMDk2MGJmODU4ZWQ4YjEzZmFlZjUifSx7ImdhbWVQb29sQWRkcmVzcyI6IjB4MDIzNDhDMzdBMTFGNThFMTcyMWY2MGNkMTdiNEVFMDZCMzQ2ZDc4YiIsImFtb3VudCI6MTAwMCwiX2lkIjoiNjFlYmQyYzYyM2JiNWI3YWQ3YjY4MDYzIn0seyJnYW1lUG9vbEFkZHJlc3MiOiIweEYwREE1NzI2MEI0MDVhRUM5ZmMxYTU3QmY4NzhkYTRlRTU0RjRjNTgiLCJhbW91bnQiOjQ2MDAsIl9pZCI6IjYxZWM1N2ZkOTRiYzg0ZmUzMmIyY2QyMyJ9LHsiZ2FtZVBvb2xBZGRyZXNzIjoiMHgzMDY0NTU4NzZCZWYxZkVFNWJlQjc4OWU5NzA4MzFmODRDRWYzYjI2IiwiYW1vdW50IjoyMDAwMCwiX2lkIjoiNjFlYzc5NjNjYzAxZDBmZmNiNWNiMDUyIn1dLCJfX3YiOjExLCJpYXQiOjE2NDI5MTU2NzIsImV4cCI6MTY0MjkyMjg3Mn0.-bDM1wqnvhgmX2ypJPDOcCiRHoP3xxPK4ioMovInR7U";
//        GlobalVariable._totalBalance = (float)1000;
//#endif
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
        _bet.text = GlobalVariable._bet.ToString("0.00");
        if(GlobalVariable._bet == 1000)
        {
            _betIncrease.interactable = false;
            _betDecrease.interactable = true;
        }
        if(GlobalVariable._bet == 100)
        {
            _betIncrease.interactable = true;
            _betDecrease.interactable = false;
        }

        // earn money setting
        if(!GlobalVariable._spining)
        {
            _earn.text = GlobalVariable._earn.ToString("0.00");
            _myBalance.text = GlobalVariable._totalBalance.ToString("0.00");
            _highX.text = "×" + GlobalVariable._highX.ToString();
            _highPercent.text = "%" + GlobalVariable._highPercent.ToString();
            _lowX.text = "×" + GlobalVariable._lowX.ToString();
            _lowPercent.text = "%" + GlobalVariable._lowPercent.ToString();
        }
    }

    private void SpinHandler()
    {
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
        }
    }

    private void BetPlus()
    {
        GlobalVariable._bet = Mathf.Clamp(GlobalVariable._bet + 10, 100, 1000);
        _betDecrease.interactable = true;
    }

    private void BetMinus()
    {
        GlobalVariable._bet = Mathf.Clamp(GlobalVariable._bet - 10, 100, 1000);
        _betIncrease.interactable = true;
    }

    public void RequestToken(string data)
    {
        JSONNode usersInfo = JSON.Parse(data);
        GlobalVariable._token = "";
        GlobalVariable._token = usersInfo["token"];
        GlobalVariable._totalBalance = (float)usersInfo["amount"];
    }
}
