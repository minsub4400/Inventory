using LitJson;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Fusion;

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

    private GameObject[] basicSpawnerOBJ;
    private NetworkObject[] networkObject;

    void Start()
    {
        networkObject = new NetworkObject[2];
        basicSpawnerOBJ = new GameObject[2];
        storage = GetComponent<APIStorage>();
    }


    private void Update()
    {
        if (storage.ready1 == true && storage.ready2 == true)
        {
            StartCoroutine(getSettingsCaller());
            storage.ready1 = false;
            storage.ready2 = false;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerSelect();
            Debug.Log(storage.ready1);
            Debug.Log(storage.ready2);
        }
    }

    // bet_id 생성
    // 호출 정보 : message, data{balance}
    public IEnumerator getSettingsCaller()
    {

        string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/settings";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrency))
        {
            www.SetRequestHeader("api-key", storage.apiKey);

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
            storage.message[0] = jsonPlayer["message"].ToString();
            storage.bet_id[0] = jsonPlayer["data"]["bets"][0]["_id"].ToString();
            Debug.Log(storage.bet_id[0]);
            Bets_idText.text = $"bet_id : {storage.bet_id[0]}";
            Debug.Log("getSettingsCaller Data Save Complited");
            StartCoroutine(PostPlaveBetCaller());

        }
    }

    // Network Object id가 작으면 플레이어 1 크면 플레이어 2
    // 혹은 player Value가 0이면 플레이어 1 크면 플레이어2
    private void PlayerSelect()
    {
        // 플레이어 게임 오브젝트를 담고
        basicSpawnerOBJ = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < networkObject.Length; i++)
        {
            networkObject[i] = basicSpawnerOBJ[i].GetComponent<NetworkObject>();
        }

        /*Debug.Log(networkObject[0].Id.Raw);
        Debug.Log(networkObject[1].Id.Raw);*/

        if (networkObject[0].Id.Raw < networkObject[1].Id.Raw)
        {
            // 플레이어1
            // 플레이어1의 API데이터를 가져온다.
            GameObject playerAPIOBJ = GameObject.FindGameObjectWithTag("PlayerAPI");
            Player_BettingInfo player_BettingInfo = playerAPIOBJ.GetComponent<Player_BettingInfo>();
            storage.statusCode[0] = player_BettingInfo.playerAPIInfoDB.statusCode;
            storage._id[0] = player_BettingInfo.playerAPIInfoDB._id;
            storage.sessionId[0] = player_BettingInfo.playerAPIInfoDB.sessionId;
        }
        else
        {
            // 플레이어2
            // 플레이어2의 API데이터를 가져온다.
            GameObject playerAPIOBJ = GameObject.FindGameObjectWithTag("PlayerAPI");
            Player_BettingInfo player_BettingInfo = playerAPIOBJ.GetComponent<Player_BettingInfo>();
            storage.statusCode[1] = player_BettingInfo.playerAPIInfoDB.statusCode;
            storage._id[1] = player_BettingInfo.playerAPIInfoDB._id;
            storage.sessionId[1] = player_BettingInfo.playerAPIInfoDB.sessionId;
        }
        Debug.Log("Player 지정 완료");
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
        placeBet.players_session_id[0] = storage.sessionId[0];
        placeBet.players_session_id[1] = storage.MetaMaskSessionID;
        placeBet.bet_id = storage.bet_id[0]; // 일단 하나만 사용 Player1

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
            storage.message[0] = jsonPlayer["message"].ToString();
            storage.betting_id = jsonPlayer["data"]["betting_id"].ToString();
            Betting_idText.text = $"betting_id : {storage.betting_id}";
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
            storage.message[0] = jsonPlayer["message"].ToString();
            storage.amount_won[0] = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");

        }
    }
}
