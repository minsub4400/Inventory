using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using LitJson;
using UnityEngine.UI;
using TMPro;
using System.Text;


// Place-bet
public struct placeBet
{
    public string[] players_session_id;
    public string bet_id;
}

public class MatchDetails
{
}


// Winner
public struct winner
{
    public string betting_id;
    public string winner_player_id;
    public MatchDetails match_details; // 빈 값
}

// Disconnect
public struct disconnect
{
    public string betting_id;
}


public class PostAPICallValue : MonoBehaviour
{
    public static PostAPICallValue instance;

    private void Awake()
    {
        instance = this;
    }
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // API Value Storage에서 변수가져다 사용
    //[SerializeField]
    public APIStorage storage;

    // 텍스트 출력
    //[SerializeField]
    public Text Betting_idText;

    void Start()
    {
        storage = GetComponent<APIStorage>();
        //StartCoroutine(UnityWebRequestPost());
    }

    // Get API에서 호출
    // 호출 정보 : message, betting_id
    public IEnumerator PostPlaveBetCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/place-bet";

        // 여기선 두명의 세션 아이디를 가져와야함.
        WWWForm form = new WWWForm();

        placeBet placeBet = new placeBet();
        placeBet.players_session_id = new string[2];
        placeBet.players_session_id[0] = storage.sessionId;
        placeBet.players_session_id[1] = storage.MetaMaskSessionID;
        placeBet.bet_id = storage.bet_id;

        // 직렬화
        var serializeObject = JsonConvert.SerializeObject(placeBet);

        using (UnityWebRequest www = UnityWebRequest.Post(url, serializeObject))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);

            www.SetRequestHeader("api-key", storage.apiKey);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // 데이터 저장
            storage.message = jsonPlayer["message"].ToString();
            storage.betting_id = jsonPlayer["data"]["betting_id"].ToString();
            Betting_idText.text = $"betting_id : {storage.betting_id}";
            Debug.Log("PostPlaveBetCaller Data Save Complited");


            StartCoroutine(WinnerCaller());
            
        }
    }

    // 이긴 사람이 나오면 이긴 사람의 id를 가지고 호출
    public IEnumerator WinnerCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/declare-winner";

        // 배팅 ID을 가져오고 이긴사람의 id
        WWWForm form = new WWWForm();

        winner winnerBet = new winner();
        winnerBet.betting_id = storage.betting_id;
        winnerBet.winner_player_id = storage.winner_id;
        winnerBet.match_details = new MatchDetails();
        Debug.Log($"winnerBet.winner_player_id : {winnerBet.winner_player_id}");
        Debug.Log($"storage.winner_id : {storage.winner_id}");
        // 직렬화
        var serializeObject = JsonConvert.SerializeObject(winnerBet);

        using (UnityWebRequest www = UnityWebRequest.Post(url, serializeObject))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);


            www.SetRequestHeader("api-key", storage.apiKey);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            // 데이터 파싱
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // 데이터 저장
            storage.message = jsonPlayer["message"].ToString();
            storage.amount_won = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");
            
        }
    }




    /*public struct postData
    {
        public string[] players_session_id;
        public string bet_id;
    }

    // UnityWebRequest POST 방식
    IEnumerator UnityWebRequestPost()
    {
        WWWForm form = new WWWForm();

        postData postData = new postData();
        postData.players_session_id = new string[1];
        postData.players_session_id[0] = player_session_id;
        postData.bet_id = bet_id;

        var param1 = JsonConvert.SerializeObject(postData);

        using (UnityWebRequest www = UnityWebRequest.Post(url, param1))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(param1);
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);

            www.SetRequestHeader("api-key", apiKey);
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }
            
            Debug.Log(www.downloadHandler.text);
            www.Dispose();
        }
    }*/
}



