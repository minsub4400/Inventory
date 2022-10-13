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

    // �׽�Ʈ ����
    public string Test;

    private void Start()
    {
        Test = "���������������K";
        // ����Ʈ ��������� Null
        // Debug.Log(APIStorage.instance.sessionId[0]);
        // Debug.Log(APIStorage.instance.sessionId[1]);
    }

    public int player;

    // ���� ���� �������� ��ư
    public void GetUserInfoBurtton()
    {
        Debug.Log(APIStorage.instance.sessionId[0]);

        if (APIStorage.instance.sessionId[0] == null)
        {
            player = 0;
        }
        else
        {
            player = 1;
        }

        StartCoroutine(getUserProfileCaller());
    }

    public void PostBettingSetting()
    {
        // ������ �� �ִ� �������� Ȯ�� �ڵ�

        // ���� �ϱ⸦ ������ Ready = true;
        APIStorage.instance.ready[0] = true;
        Debug.Log("Player1 Betting Complite");
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
            APIStorage.instance.statusCode[player] = jsonPlayer["StatusCode"].ToString();
            APIStorage.instance.userName[player] = jsonPlayer["userProfile"]["username"].ToString();
            APIStorage.instance._id[player] = jsonPlayer["userProfile"]["_id"].ToString();
            _idText.text = $"_id : {APIStorage.instance._id[player]}";
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
            APIStorage.instance.statusCode[player] = jsonPlayer["StatusCode"].ToString();
            APIStorage.instance.sessionId[player] = jsonPlayer["sessionId"].ToString();
            SessionIDText.text = $"sessionId : {APIStorage.instance.sessionId[player]}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
        }
    }
    // ȣ�� ���� : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {

        // Zera
        string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{APIStorage.instance.sessionId[player]}";
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
            APIStorage.instance.message[player] = jsonPlayer["message"].ToString();
            APIStorage.instance.zera[player] = jsonPlayer["data"]["balance"].ToString();
            zeraText.text = APIStorage.instance.zera[player];
            Debug.Log("getbettingCurrency Zera Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }

        // Ace
        string getbettingCurrencyAce = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{APIStorage.instance.sessionId[player]}";
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
            APIStorage.instance.message[player] = jsonPlayer["message"].ToString();
            APIStorage.instance.ace[player] = jsonPlayer["data"]["balance"].ToString();
            aceText.text = APIStorage.instance.ace[player];
            Debug.Log("getbettingCurrency Ace Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }
    }
}
