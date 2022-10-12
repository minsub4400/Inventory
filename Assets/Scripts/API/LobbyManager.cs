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

    // bet_id ����
    // ȣ�� ���� : message, data{balance}
    public IEnumerator getSettingsCaller()
    {

        string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/settings";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrency))
        {
            www.SetRequestHeader("api-key", APIStorage.instance.apiKey);

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
            APIStorage.instance.message[0] = jsonPlayer["message"].ToString();
            APIStorage.instance.bet_id[0] = jsonPlayer["data"]["bets"][0]["_id"].ToString();
            Debug.Log(APIStorage.instance.bet_id[0]);
            Bets_idText.text = $"bet_id : {APIStorage.instance.bet_id[0]}";
            Debug.Log("getSettingsCaller Data Save Complited");
            StartCoroutine(PostPlaveBetCaller());

        }
    }


    // betting_id ����
    // ȣ�� ���� : message, betting_id
    public IEnumerator PostPlaveBetCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/place-bet";

        // ���⼱ �θ��� ���� ���̵� �����;���.
        WWWForm form = new WWWForm();

        placeBet placeBet = new placeBet();
        placeBet.players_session_id = new string[2];
        placeBet.players_session_id[0] = APIStorage.instance.sessionId[0];
        placeBet.players_session_id[1] = APIStorage.instance.MetaMaskSessionID;
        placeBet.bet_id = APIStorage.instance.bet_id[0]; // �ϴ� �ϳ��� ��� Player1

        // ����ȭ
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

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // ������ ����
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

    // �̱� ����� ������ �̱� ����� id�� ������ ȣ��
    public IEnumerator WinnerCaller()
    {
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/declare-winner";

        // ���� ID�� �������� �̱����� id
        WWWForm form = new WWWForm();

        winner winnerBet = new winner();
        winnerBet.betting_id = APIStorage.instance.betting_id;
        winnerBet.winner_player_id = APIStorage.instance.winner_id;
        winnerBet.match_details = new MatchDetails();
        Debug.Log($"winnerBet.winner_player_id : {winnerBet.winner_player_id}");
        Debug.Log($"storage.winner_id : {APIStorage.instance.winner_id}");

        // ����ȭ
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

            // ������ �Ľ�
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);
            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            // ������ ����
            APIStorage.instance.message[0] = jsonPlayer["message"].ToString();
            APIStorage.instance.amount_won[0] = jsonPlayer["data"]["amount_won"].ToString();
            Debug.Log("WinnerCaller Data Save Complited");

        }
    }
}
