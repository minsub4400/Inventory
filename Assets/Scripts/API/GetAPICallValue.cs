//using Newtonsoft.Json;
using LitJson;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class GetAPICallValue : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // API Value Storage에서 변수가져다 사용
    [SerializeField]
    private APIStorage storage;

    // 텍스트 출력
    //[SerializeField]
    public Text Bets_idText;
    //[SerializeField]
    public Text SessionIDText;
    //[SerializeField]
    public Text _idText;

    [SerializeField]
    private TextMeshProUGUI currencyInputFieldText;

    public void InputCurrencyBurtton()
    {
        storage.currency = currencyInputFieldText.text;
        StartCoroutine(getUserProfileCaller());
    }


    

    // API 호출 경로 변수
    // ----- Get method -----
    // 호출 정보 : StatusCode, _id, username
    string getUserProfile = "http://localhost:8546/api/getuserprofile";
    private IEnumerator getUserProfileCaller()
    {
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
            storage.statusCode = jsonPlayer["StatusCode"].ToString();
            storage.userName = jsonPlayer["userProfile"]["username"].ToString();
            storage._id = jsonPlayer["userProfile"]["_id"].ToString();
            _idText.text = $"_id : {storage._id}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getSessionIDCaller());
            //www.Dispose();
        }
    }

    // 호출 정보 : StatusCode, sessionId
    string getSessionID = "http://localhost:8546/api/getsessionid";
    private IEnumerator getSessionIDCaller()
    {
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
            storage.statusCode = jsonPlayer["StatusCode"].ToString();
            storage.sessionId = jsonPlayer["sessionId"].ToString();
            SessionIDText.text = $"sessionId : {storage.sessionId}";
            Debug.Log("getUserProfile Data Save Complited");
            StartCoroutine(getbettingCurrencyCaller());
            //www.Dispose();
        }
    }

    // 호출 정보 : message, data{balance}
    public IEnumerator getbettingCurrencyCaller()
    {
        string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/zera/balance/{storage.sessionId}";
        //string getbettingCurrency = $"https://odin-api-sat.browseosiris.com/v1/betting/{storage.currency}/balance/{storage.sessionId}";
        using (UnityWebRequest www = UnityWebRequest.Get(getbettingCurrency))
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
            storage.message = jsonPlayer["message"].ToString();
            storage.balance = jsonPlayer["data"]["balance"].ToString();
            //Debug.Log(storage.balance);
            Debug.Log("getbettingCurrency Data Save Complited");
            StartCoroutine(getSettingsCaller());
            //www.Dispose();
        }
    }

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
            storage.message = jsonPlayer["message"].ToString();
            storage.bet_id = jsonPlayer["data"]["bets"][0]["_id"].ToString();
            Bets_idText.text = $"bet_id : {storage.bet_id}";
            Debug.Log("getSettingsCaller Data Save Complited");
            StartCoroutine(PostAPICallValue.instance.PostPlaveBetCaller());
            //www.Dispose();
        }
    }


    void Start()
    {
        storage = GetComponent<APIStorage>();
        

        //StartCoroutine(getUserProfileCaller());
        //StartCoroutine(UnityWebRequestGet());
    }

    

    IEnumerator UnityWebRequestGet()
    {
        //string userInfoGet = "/getuserprofile";    + "?apikey=" + apiKey" 
        //string url = $"https://play-sat.dappstore.me{bettingInfo}?api-key={apiKey}";
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/settings";
        //string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/balance";
        //string url = "http://localhost:8546/api/getuserprofile";

        UnityWebRequest www = UnityWebRequest.Get(url);
        www.SetRequestHeader("api-key", storage.apiKey);
        //www.GetResponseHeader("api-key");

        yield return www.SendWebRequest();

        // 에러가 없으면
        if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.data);
        }
        else
        {
            Debug.Log("ERROR");
        }

        // 파일을 유니티 프로젝트 경로에 저장하기(윈도우)
        string path = Application.dataPath + "/APIData.json";
        //string path = Path.Combine(Application.dataPath, "/playerData.json");
        // 파일로 저장 (경로, 저장할 문자열(데이터, 파일))
        // JsonUtility.ToJson(playerData, true); true는 Json파일을 보기 좋게 저장해준다.

        //string jsonData = JsonUtility.ToJson(www.downloadHandler.text);
        //string jsonData = JsonUtility.ToJson("hhhhhhhhhhhhhhhhhhhhhh", true);

        JObject json = JObject.Parse(www.downloadHandler.text);
        //Debug.Log(json);

        /*FileStream fs = new FileStream(path, FileMode.Open);
        byte[] buffer = Encoding.UTF8.GetBytes(jsonData);
        fs.Write(buffer, 0, buffer.Length);
        fs.Close();*/

        // 파일로 변환
        //File.WriteAllText(path, json.ToString());

        // 데이터 분리.....
        if (www.isDone)
        {
            string jsonResult = System.Text.Encoding.UTF8.GetString(www.downloadHandler.data);

            JsonData jsonPlayer = JsonMapper.ToObject(jsonResult);
            //Debug.Log(jsonPlayer);
            Debug.Log(jsonPlayer["data"]["settings"]["_id"]);
        }
    }
}

