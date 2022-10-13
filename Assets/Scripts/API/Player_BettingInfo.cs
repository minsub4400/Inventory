using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player_BettingInfo : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ------- �ؽ�Ʈ ��� --------
    // _id
    public Text _idText;
    // session_id
    public Text SessionIDText;
    // bet_id
    public Text Bets_idText;
    // userName
    public Text userName;
    // zera
    public Text zeraText;
    // ace
    public Text aceText;

    public int player = -1;

    public PlayerAPIInfoDB playerAPIInfoDB;

    private GameObject storageOBJ;

    private void Start()
    {
        // ����Ʈ ��������� Null
        // Debug.Log(APIStorage.instance.sessionId[0]);
        // Debug.Log(APIStorage.instance.sessionId[1]);
    }

    [SerializeField]
    public Text playerNum;

    // ���� ���� �������� ��ư
    public void GetUserInfoBurtton()
    {
        StartCoroutine(getUserProfileCaller());
        playerNum.text = $"�÷��̾� ��ȣ : {player}";
    }

    // ���� ���� �Ǿ��� ��,
    public void PostBettingSetting()
    {
        // ������ �� �ִ� �������� Ȯ�� �ڵ�
        if (player == 2)
        {
            Debug.Log("�÷��̾� 2");
            // ���� �ϱ⸦ ������ Ready = true;
            storageOBJ = GameObject.FindGameObjectWithTag("LobbyManager");
            APIStorage storage = storageOBJ.GetComponent<APIStorage>();
            storage.ready1 = true;
        }

        if (player == 3)
        {
            Debug.Log("�÷��̾� 3");
            storageOBJ = GameObject.FindGameObjectWithTag("LobbyManager");
            APIStorage storage = storageOBJ.GetComponent<APIStorage>();
            storage.ready2 = true;
        }

        Debug.Log($"Player{player} Betting Complite");
    }

    // ȣ�� ���� : StatusCode, _id, username
    private IEnumerator getUserProfileCaller()
    {
        string getUserProfile = "http://localhost:8546/api/getuserprofile";
        using (UnityWebRequest www = UnityWebRequest.Get(getUserProfile))
        {
            yield return www.SendWebRequest();
            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            playerAPIInfoDB.statusCode = jsonPlayer["StatusCode"].ToString();
            playerAPIInfoDB.userName = jsonPlayer["userProfile"]["username"].ToString();
            playerAPIInfoDB._id = jsonPlayer["userProfile"]["_id"].ToString();
            _idText.text = $"_id : {playerAPIInfoDB._id}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getSessionIDCaller());
        }
    }
    // ȣ�� ���� : StatusCode, sessionId
    private IEnumerator getSessionIDCaller()
    {
        string getSessionID = "http://localhost:8546/api/getsessionid";
        using (UnityWebRequest www = UnityWebRequest.Get(getSessionID))
        {
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            playerAPIInfoDB.statusCode = jsonPlayer["StatusCode"].ToString();
            playerAPIInfoDB.sessionId = jsonPlayer["sessionId"].ToString();
            SessionIDText.text = $"sessionId : {playerAPIInfoDB.sessionId}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
        }
    }
    // ȣ�� ���� : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {

        // Zera
        string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{playerAPIInfoDB.sessionId}";
        //string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/{storage.currency}/balance/{storage.sessionId}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyZera))
        {
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            playerAPIInfoDB.message = jsonPlayer["message"].ToString();
            playerAPIInfoDB.zera = jsonPlayer["data"]["balance"].ToString();
            zeraText.text = playerAPIInfoDB.zera;
            Debug.Log("getbettingCurrency Zera Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }

        // Ace
        string getbettingCurrencyAce = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{playerAPIInfoDB.sessionId}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyAce))
        {
            yield return www.SendWebRequest();

            // HTTP ���� �����
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // ������ ����
            playerAPIInfoDB.message = jsonPlayer["message"].ToString();
            playerAPIInfoDB.ace = jsonPlayer["data"]["balance"].ToString();
            aceText.text = playerAPIInfoDB.ace;
            Debug.Log("getbettingCurrency Ace Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }
    }
}
