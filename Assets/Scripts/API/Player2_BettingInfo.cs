using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player2_BettingInfo : MonoBehaviour
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

    // 유저 정보 가져오기 버튼
    public void GetUserInfoBurtton()
    {
        StartCoroutine(getUserProfileCaller());
    }

    public void PostBettingSetting()
    {
        // 배팅할 수 있는 상태인지 확인 코드

        // 배팅 하기를 누르면 Ready = true;
        APIStorage.instance.ready2 = true;
        Debug.Log("Player2 Betting Complite");
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
            APIStorage.instance.statusCode[1] = jsonPlayer["StatusCode"].ToString();
            APIStorage.instance.userName[1] = jsonPlayer["userProfile"]["username"].ToString();
            APIStorage.instance._id[1] = jsonPlayer["userProfile"]["_id"].ToString();
            _idText.text = $"_id : {APIStorage.instance._id[1]}";
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
            /*APIStorage.instance.statusCode[1] = jsonPlayer["StatusCode"].ToString();
            APIStorage.instance.sessionId[1] = jsonPlayer["sessionId"].ToString();*/
            APIStorage.instance.sessionId[1] = "aEwKulUkUB6c9lw3u8QxJE2lJ21SsD1Ne4DG45JA";
            SessionIDText.text = $"sessionId : {APIStorage.instance.sessionId[1]}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
        }
    }
    // 호출 정보 : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {
        
        // Zera
        //string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{APIStorage.instance.sessionId[1]}";
        string getbettingCurrencyZera = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/aEwKulUkUB6c9lw3u8QxJE2lJ21SsD1Ne4DG45JA";
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
            APIStorage.instance.message[1] = jsonPlayer["message"].ToString();
            APIStorage.instance.zera[1] = jsonPlayer["data"]["balance"].ToString();
            zeraText.text = APIStorage.instance.zera[1];
            Debug.Log("getbettingCurrency Zera Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }

        // Ace
        //string getbettingCurrencyAce = $"https://odin-api-sat.browseosiris.com/v1/betting/ace/balance/{APIStorage.instance.sessionId[1]}";
        string getbettingCurrencyAce = $"https://odin-api-sat.browseosiris.com/v1/betting/ace/balance/aEwKulUkUB6c9lw3u8QxJE2lJ21SsD1Ne4DG45JA";
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
            APIStorage.instance.message[1] = jsonPlayer["message"].ToString();
            APIStorage.instance.ace[1] = jsonPlayer["data"]["balance"].ToString();
            aceText.text = APIStorage.instance.ace[1];
            Debug.Log("getbettingCurrency Ace Data Save Complited");
            //StartCoroutine(getSettingsCaller());
        }
    }
}
