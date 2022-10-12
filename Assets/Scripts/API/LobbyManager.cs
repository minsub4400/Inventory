using LitJson;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    [SerializeField]
    private APIStorage storage;

    public Text Betting_idText;
    public Text Bets_idText;

    void Start()
    {
        storage = GetComponent<APIStorage>();
    }


    private void Update()
    {
        if (storage.ready[0] == true && storage.ready[1] == true)
        {
            StartCoroutine(getSettingsCaller());
            storage.ready[0] = false;
            storage.ready[1] = false;
        }
    }

    // bet_id 생성
    // 호출 정보 : message, data{balance}
    public IEnumerator getSettingsCaller()
    {

        string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/settings";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrency))
        {
            www.SetRequestHeader("api-key", APIStorage.instance.apiKey);

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
            APIStorage.instance.message[0] = jsonPlayer["message"].ToString();
            APIStorage.instance.bet_id[0] = jsonPlayer["data"]["bets"][0]["_id"].ToString();
            Debug.Log(APIStorage.instance.bet_id[0]);
            Bets_idText.text = $"bet_id : {APIStorage.instance.bet_id[0]}";
            Debug.Log("getSettingsCaller Data Save Complited");
            StartCoroutine(PostPlaveBetCaller());

        }
    }


    // betting_id 생성
    // 호출 정보 : message, betting_id
    public IEnumerator PostPlaveBetCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/place-bet";

        // 여기선 두명의 세션 아이디를 가져와야함.
        WWWForm form = new WWWForm();

        placeBet placeBet = new placeBet();
        placeBet.players_session_id = new string[2];
        placeBet.players_session_id[0] = APIStorage.instance.sessionId[0];
        placeBet.players_session_id[1] = APIStorage.instance.MetaMaskSessionID;
        placeBet.bet_id = APIStorage.instance.bet_id[0]; // 일단 하나만 사용 Player1

        // 직렬화
        var serializeObject = JsonConvert.SerializeObject(placeBet);

        using (UnityWebRequest www = UnityWebRequest.Post(url, serializeObject))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);

            www.SetRequestHeader("api-key", APIStorage.instance.apiKey);
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
            APIStorage.instance.message[0] = jsonPlayer["message"].ToString();
            APIStorage.instance.betting_id = jsonPlayer["data"]["betting_id"].ToString();
            Betting_idText.text = $"betting_id : {APIStorage.instance.betting_id}";
            Debug.Log("PostPlaveBetCaller Data Save Complited");
        }
    }

    public void WinnerButton()
    {
        StartCoroutine(WinnerCaller());
    }

    // 이긴 사람이 나오면 이긴 사람의 id를 가지고 호출
    public IEnumerator WinnerCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/declare-winner";

        // 배팅 ID을 가져오고 이긴사람의 id
        WWWForm form = new WWWForm();

        winner winnerBet = new winner();
        winnerBet.betting_id = APIStorage.instance.betting_id;
        winnerBet.winner_player_id = APIStorage.instance.winner_id;
        winnerBet.match_details = new MatchDetails();
        Debug.Log($"winnerBet.winner_player_id : {winnerBet.winner_player_id}");
        Debug.Log($"storage.winner_id : {APIStorage.instance.winner_id}");

        // 직렬화
        var serializeObject = JsonConvert.SerializeObject(winnerBet);

        using (UnityWebRequest www = UnityWebRequest.Post(url, serializeObject))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(serializeObject);
            www.uploadHandler.Dispose();
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);


            www.SetRequestHeader("api-key", APIStorage.instance.apiKey);
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
            APIStorage.instance.message[0] = jsonPlayer["message"].ToString();
            APIStorage.instance.amount_won[0] = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");

        }
    }
}
