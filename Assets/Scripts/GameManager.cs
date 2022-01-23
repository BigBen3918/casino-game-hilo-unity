using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{

    [Header("Material")]
    [SerializeField] Material _jokerMaterial;
    [SerializeField] Material[] _redMaterials;
    [SerializeField] Material[] _blackMaterials;

    [Header("GameObject")]
    [SerializeField] GameObject _Card;
    [SerializeField] GameObject _WinTitle;
    [SerializeField] GameObject _LoseTitle;
    [SerializeField] GameObject _WinLoseBack;
    [SerializeField] GameObject _ErrorMessage;

    private Animator rotationAnim;
    private Animator winAnim;
    private Animator loseAnim;
    private Animator errorAnim;

    private bool loseFlag;
    // Start is called before the first frame update
    void Start()
    {
        rotationAnim = _Card.GetComponent<Animator>();
        winAnim = _WinTitle.GetComponent<Animator>();
        loseAnim = _LoseTitle.GetComponent<Animator>();
        errorAnim = _ErrorMessage.GetComponent<Animator>();

        GlobalVariable._spining = false;
        loseFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpinCard(int flag)
    {
        if (GlobalVariable._spining) return;

        GlobalVariable._spining = true;
        StartCoroutine(SendSignal(flag));
    }

    IEnumerator SendSignal(int flag)
    {
        WWWForm form = new WWWForm();
        string subPath = "";
        if(flag == 0)
        {
            form.AddField("token", GlobalVariable._token);
            form.AddField("betValue", GlobalVariable._bet);

            subPath = "api/start-signal";
        }
        if(flag == 1)
        {
            form.AddField("token", GlobalVariable._token);

            subPath = "api/start-cashout";
        }
        if(flag == 2)
        {
            form.AddField("token", GlobalVariable._token);
            form.AddField("betPosition", GlobalVariable._ChoosePoint);

            subPath = "api/start-game";
        }

        UnityWebRequest www = UnityWebRequest.Post(GlobalVariable.BaseUrl + subPath, form);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            GlobalVariable._spining = false;
            GlobalVariable._gaming = false;
            errorAnim.SetBool("error", true);
            yield return new WaitForSeconds(2.1f);
            errorAnim.SetBool("error", false);
        }
        else
        {
            string strdata = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JSONNode reqData = JSON.Parse(strdata);

            if (reqData["status"])
            {
                if (flag == 0) // Spin Handler
                {
                    GlobalVariable._totalBalance -= (float)GlobalVariable._bet;
                    GlobalVariable._cardNum = reqData["cardNum"];
                    GlobalVariable._colorNum = reqData["colorNum"];

                    StartCoroutine(Spin());

                    GlobalVariable._gaming = true;
                    GlobalVariable._bridge = true;
                }
                if (flag == 1) // Cash Handler
                {
                    _WinLoseBack.SetActive(true);
                    winAnim.SetBool("Win", true);
                    yield return new WaitForSeconds(1.2f);
                    _WinLoseBack.SetActive(false);
                    winAnim.SetBool("Win", false);

                    GlobalVariable._spining = false;
                    GlobalVariable._gaming = false;
                    GlobalVariable._bridge = GlobalVariable._gaming;
                    GlobalVariable._totalBalance += (float)GlobalVariable._earn;
                }
                if (flag == 2) // Choose Handler
                {
                    if (reqData["gameStatus"])
                    {
                        GlobalVariable._cardNum = reqData["cardNum"];
                        GlobalVariable._colorNum = reqData["colorNum"];

                        StartCoroutine(Spin());

                        GlobalVariable._earn = reqData["moneyResult"];
                    }
                    else
                    {
                        GlobalVariable._cardNum = reqData["cardNum"];
                        GlobalVariable._colorNum = reqData["colorNum"];

                        loseFlag = true;

                        StartCoroutine(Spin());

                        GlobalVariable._gaming = false;
                    }
                }
            }
            else
            {
                GlobalVariable._spining = false;
                GlobalVariable._gaming = false;    
                errorAnim.SetBool("error", true);
                yield return new WaitForSeconds(2.1f);
                errorAnim.SetBool("error", false);
            }
        }
    }

    IEnumerator Spin()
    {
        rotationAnim.SetBool("rotation", true);
        yield return new WaitForSeconds(2.5f);

        if(GlobalVariable._cardNum == 12)
        {
            _Card.GetComponent<MeshRenderer>().material = _jokerMaterial;
        }
        else
        {
            if(GlobalVariable._colorNum == 0)
            {
                _Card.GetComponent<MeshRenderer>().material = _redMaterials[GlobalVariable._cardNum];
            }
            else
            {
                _Card.GetComponent<MeshRenderer>().material = _blackMaterials[GlobalVariable._cardNum];
            }
        }

        rotationAnim.SetBool("rotation", false);
        ShowPercent();

        if (loseFlag)
        {
            _WinLoseBack.SetActive(true);
            loseAnim.SetBool("Lose", true);
            yield return new WaitForSeconds(1.2f);
            _WinLoseBack.SetActive(false);
            loseAnim.SetBool("Lose", false);
            loseFlag = false;
        }

        yield return new WaitForSeconds(0.15f);

        GlobalVariable._spining = false;
        GlobalVariable._bridge = GlobalVariable._gaming;
    }

    private void ShowPercent()
    {
        switch(GlobalVariable._cardNum)
        {
            case 0:
                GlobalVariable._highX = 1.09f;
                GlobalVariable._highPercent = 88;
                GlobalVariable._lowX = 0f;
                GlobalVariable._lowPercent = 0;
                break;
            case 1:
                GlobalVariable._highX = 1.2f;
                GlobalVariable._highPercent = 80;
                GlobalVariable._lowX = 12f;
                GlobalVariable._lowPercent = 8;
                break;
            case 2:
                GlobalVariable._highX = 1.33f;
                GlobalVariable._highPercent = 72;
                GlobalVariable._lowX = 6f;
                GlobalVariable._lowPercent = 16;
                break;
            case 3:
                GlobalVariable._highX = 1.5f;
                GlobalVariable._highPercent = 64;
                GlobalVariable._lowX = 4f;
                GlobalVariable._lowPercent = 24;
                break;
            case 4:
                GlobalVariable._highX = 1.71f;
                GlobalVariable._highPercent = 52;
                GlobalVariable._lowX = 3f;
                GlobalVariable._lowPercent = 26;
                break;
            case 5:
                GlobalVariable._highX = 2f;
                GlobalVariable._highPercent = 48;
                GlobalVariable._lowX = 2.4f;
                GlobalVariable._lowPercent = 40;
                break;
            case 6:
                GlobalVariable._highX = 2.4f;
                GlobalVariable._highPercent = 40;
                GlobalVariable._lowX = 2f;
                GlobalVariable._lowPercent = 48;
                break;
            case 7:
                GlobalVariable._highX = 3f;
                GlobalVariable._highPercent = 26;
                GlobalVariable._lowX = 1.71f;
                GlobalVariable._lowPercent = 52;
                break;
            case 8:
                GlobalVariable._highX = 4f;
                GlobalVariable._highPercent = 24;
                GlobalVariable._lowX = 1.5f;
                GlobalVariable._lowPercent = 64;
                break;
            case 9:
                GlobalVariable._highX = 6f;
                GlobalVariable._highPercent = 16;
                GlobalVariable._lowX = 1.33f;
                GlobalVariable._lowPercent = 72;
                break;
            case 10:
                GlobalVariable._highX = 12f;
                GlobalVariable._highPercent = 8;
                GlobalVariable._lowX = 1.2f;
                GlobalVariable._lowPercent = 80;
                break;
            case 11:
                GlobalVariable._highX = 0f;
                GlobalVariable._highPercent = 0;
                GlobalVariable._lowX = 1.09f;
                GlobalVariable._lowPercent = 88;
                break;
            case 12:
                GlobalVariable._highX = 0f;
                GlobalVariable._highPercent = 0;
                GlobalVariable._lowX = 0f;
                GlobalVariable._lowPercent = 0;
                break;
        }
    }
}
