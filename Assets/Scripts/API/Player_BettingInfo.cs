using Fusion;
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

    // ------- 텍스트 출력 --------
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

    public int player;

    public PlayerAPIInfoDB playerAPIInfoDB;

    [SerializeField]
    private APIStorage aPIStorage;

    private void Start()
    {
        player = -1;
        // 리스트 비어있으면 Null
        // Debug.Log(APIStorage.instance.sessionId[0]);
        // Debug.Log(APIStorage.instance.sessionId[1]);
    }

    // 유저 정보 가져오기 버튼
    public void GetUserInfoBurtton()
    {
        Debug.Log(playerAPIInfoDB.sessionId);

        // 플레이어 값을 받았는지 확인
        if (player != -1)
        {
            StartCoroutine(getUserProfileCaller());
            return;
        }

        // 처음에는 다 널일테니까 다 0번만 받겠는데..
        if (playerAPIInfoDB.sessionId == null)
        {
            // Player1
            player = 0;
            StartCoroutine(getUserProfileCaller());
        }
        else
        {
            // player2
            player = 1;
            StartCoroutine(getUserProfileCaller());
        }
    }

    // 서버 접속 되었을 때,
    public void PostBettingSetting()
    {
        // 배팅할 수 있는 상태인지 확인 코드
        if (player == 0)
        {
            // 배팅 하기를 누르면 Ready = true;
            aPIStorage.ready1 = true;
        }
        else if (player == 1)
        {
            aPIStorage.ready2 = true;
        }

        Debug.Log($"Player{player} Betting Complite");
    }

    // 호출 정보 : StatusCode, _id, username
    private IEnumerator getUserProfileCaller()
    {
        string getUserProfile = "http://localhost:8546/api/getuserprofile";
        using (UnityWebRequest www = UnityWebRequest.Get(getUserProfile))
        {
            yield return www.SendWebRequest();
            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            playerAPIInfoDB.statusCode = jsonPlayer["StatusCode"].ToString();
            playerAPIInfoDB.userName = jsonPlayer["userProfile"]["username"].ToString();
            playerAPIInfoDB._id = jsonPlayer["userProfile"]["_id"].ToString();
            _idText.text = $"_id : {playerAPIInfoDB._id}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getSessionIDCaller());
        }
    }
    // 호출 정보 : StatusCode, sessionId
    private IEnumerator getSessionIDCaller()
    {
        string getSessionID = "http://localhost:8546/api/getsessionid";
        using (UnityWebRequest www = UnityWebRequest.Get(getSessionID))
        {
            yield return www.SendWebRequest();

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            playerAPIInfoDB.statusCode = jsonPlayer["StatusCode"].ToString();
            playerAPIInfoDB.sessionId = jsonPlayer["sessionId"].ToString();
            SessionIDText.text = $"sessionId : {playerAPIInfoDB.sessionId}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
        }
    }
    // 호출 정보 : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {

        // Zera
        string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{playerAPIInfoDB.sessionId}";
        //string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/{storage.currency}/balance/{storage.sessionId}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrencyZera))
        {
            yield return www.SendWebRequest();

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
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

            // HTTP 에러 디버그
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);

            // 데이터 저장
            playerAPIInfoDB.message = jsonPlayer["message"].ToString();
            playerAPIInfoDB.ace = jsonPlayer["data"]["balance"].ToString();
            aceText.text = playerAPIInfoDB.ace;
            Debug.Log("getbettingCurrency Ace Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }
    }
}
