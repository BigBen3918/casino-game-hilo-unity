using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;

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

    private Animator rotationAnim;
    private Animator winAnim;
    private Animator loseAnim;

    private bool loseFlag;
    // Start is called before the first frame update
    void Start()
    {
        rotationAnim = _Card.GetComponent<Animator>();
        winAnim = _WinTitle.GetComponent<Animator>();
        loseAnim = _LoseTitle.GetComponent<Animator>();

        GlobalVariable.instance._spining = false;
        loseFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpinCard(int flag)
    {
        if (GlobalVariable.instance._spining) return;

        StartCoroutine(SendSignal(flag));
    }

    IEnumerator SendSignal(int flag)
    {
        WWWForm form = new WWWForm();
        string subPath = "";
        if(flag == 0)
        {
            form.AddField("token", GlobalVariable.instance._token);
            form.AddField("betValue", GlobalVariable.instance._bet);

            subPath = "api/start-signal";
        }
        if(flag == 1)
        {
            form.AddField("token", GlobalVariable.instance._token);

            subPath = "api/start-cashout";
        }
        if(flag == 2)
        {
            form.AddField("token", GlobalVariable.instance._token);
            form.AddField("betPosition", GlobalVariable.instance._ChoosePoint);

            subPath = "api/start-game";
        }

        UnityWebRequest www = UnityWebRequest.Post(GlobalVariable.instance.BaseUrl + subPath, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string strdata = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JSONNode reqData = JSON.Parse(strdata);

            if(reqData["status"])
            {
                if(flag == 0)
                {
                    GlobalVariable.instance._totalBalance -= GlobalVariable.instance._bet;
                    GlobalVariable.instance._cardNum = reqData["cardNum"];
                    GlobalVariable.instance._colorNum = reqData["colorNum"];

                    StartCoroutine(Spin());

                    GlobalVariable.instance._gaming = true;
                    GlobalVariable.instance._bridge = true;
                }
                if(flag == 1)
                {
                    _WinLoseBack.SetActive(true);
                    winAnim.SetBool("Win", true);
                    yield return new WaitForSeconds(1.2f);
                    _WinLoseBack.SetActive(false);
                    winAnim.SetBool("Win", false);

                    GlobalVariable.instance._spining = false;
                    GlobalVariable.instance._gaming = false;
                    GlobalVariable.instance._bridge = GlobalVariable.instance._gaming;
                    GlobalVariable.instance._totalBalance += GlobalVariable.instance._earn;
                }
                if(flag == 2)
                {
                    if (reqData["gameStatus"])
                    {
                        GlobalVariable.instance._cardNum = reqData["cardNum"];
                        GlobalVariable.instance._colorNum = reqData["colorNum"];

                        StartCoroutine(Spin());

                        GlobalVariable.instance._earn = reqData["moneyResult"];
                    }
                    else
                    {
                        GlobalVariable.instance._cardNum = reqData["cardNum"];
                        GlobalVariable.instance._colorNum = reqData["colorNum"];

                        loseFlag = true;

                        StartCoroutine(Spin());

                        GlobalVariable.instance._gaming = false;
                    }
                }
            }
            else
            {
                Debug.Log("Community Error");
            }
        }
    }

    IEnumerator Spin()
    {
        GlobalVariable.instance._spining = true;
        

        rotationAnim.SetBool("rotation", true);
        yield return new WaitForSeconds(2.5f);

        if(GlobalVariable.instance._cardNum == 12)
        {
            _Card.GetComponent<MeshRenderer>().material = _jokerMaterial;
        }
        else
        {
            if(GlobalVariable.instance._colorNum == 0)
            {
                _Card.GetComponent<MeshRenderer>().material = _redMaterials[GlobalVariable.instance._cardNum];
            }
            else
            {
                _Card.GetComponent<MeshRenderer>().material = _blackMaterials[GlobalVariable.instance._cardNum];
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

        GlobalVariable.instance._spining = false;
        GlobalVariable.instance._bridge = GlobalVariable.instance._gaming;
    }

    private void ShowPercent()
    {
        switch(GlobalVariable.instance._cardNum)
        {
            case 0:
                GlobalVariable.instance._highX = 1.09f;
                GlobalVariable.instance._highPercent = 88;
                GlobalVariable.instance._lowX = 0f;
                GlobalVariable.instance._lowPercent = 0;
                break;
            case 1:
                GlobalVariable.instance._highX = 1.2f;
                GlobalVariable.instance._highPercent = 80;
                GlobalVariable.instance._lowX = 12f;
                GlobalVariable.instance._lowPercent = 8;
                break;
            case 2:
                GlobalVariable.instance._highX = 1.33f;
                GlobalVariable.instance._highPercent = 72;
                GlobalVariable.instance._lowX = 6f;
                GlobalVariable.instance._lowPercent = 16;
                break;
            case 3:
                GlobalVariable.instance._highX = 1.5f;
                GlobalVariable.instance._highPercent = 64;
                GlobalVariable.instance._lowX = 4f;
                GlobalVariable.instance._lowPercent = 24;
                break;
            case 4:
                GlobalVariable.instance._highX = 1.71f;
                GlobalVariable.instance._highPercent = 52;
                GlobalVariable.instance._lowX = 3f;
                GlobalVariable.instance._lowPercent = 26;
                break;
            case 5:
                GlobalVariable.instance._highX = 2f;
                GlobalVariable.instance._highPercent = 48;
                GlobalVariable.instance._lowX = 2.4f;
                GlobalVariable.instance._lowPercent = 40;
                break;
            case 6:
                GlobalVariable.instance._highX = 2.4f;
                GlobalVariable.instance._highPercent = 40;
                GlobalVariable.instance._lowX = 2f;
                GlobalVariable.instance._lowPercent = 48;
                break;
            case 7:
                GlobalVariable.instance._highX = 3f;
                GlobalVariable.instance._highPercent = 26;
                GlobalVariable.instance._lowX = 1.71f;
                GlobalVariable.instance._lowPercent = 52;
                break;
            case 8:
                GlobalVariable.instance._highX = 4f;
                GlobalVariable.instance._highPercent = 24;
                GlobalVariable.instance._lowX = 1.5f;
                GlobalVariable.instance._lowPercent = 64;
                break;
            case 9:
                GlobalVariable.instance._highX = 6f;
                GlobalVariable.instance._highPercent = 16;
                GlobalVariable.instance._lowX = 1.33f;
                GlobalVariable.instance._lowPercent = 72;
                break;
            case 10:
                GlobalVariable.instance._highX = 12f;
                GlobalVariable.instance._highPercent = 8;
                GlobalVariable.instance._lowX = 1.2f;
                GlobalVariable.instance._lowPercent = 80;
                break;
            case 11:
                GlobalVariable.instance._highX = 0f;
                GlobalVariable.instance._highPercent = 0;
                GlobalVariable.instance._lowX = 1.09f;
                GlobalVariable.instance._lowPercent = 88;
                break;
            case 12:
                GlobalVariable.instance._highX = 0f;
                GlobalVariable.instance._highPercent = 0;
                GlobalVariable.instance._lowX = 0f;
                GlobalVariable.instance._lowPercent = 0;
                break;
        }
    }
}
