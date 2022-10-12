using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;


// POST����� GET��İ� �ٸ����� apikey�� �Է��ϴ� �κ��� ����.
public class API_TEST_POST : MonoBehaviour
{
    string url = "https://odin-api-sat.browseosiris.com/v1/betting/ace/place-bet";
    string apiKey = "70pNqHWqzZ0DXwsIP0e0bA";
    JObject json1;
    string jsonData;
    string path;
    string[] ourPostData1 = { "GCGvmOvqvVoCnib84c2vQ70LhkG0Tcaspsh4LFEa" };
    string ourPostData2 = "633e959675294e05550c8e69";

    void Start()
    {
        // Json���� ��ȯ
        path = Application.dataPath + "/placebet.json";

        // ������ �о �����´�.
        jsonData = File.ReadAllText(path);
        //Debug.Log(jsonData);

        json1 = JObject.Parse(jsonData);
        //Debug.Log(json1);
        string json2 = JsonUtility.ToJson(jsonData);

        var param1 = JsonConvert.SerializeObject(ourPostData1);
        var param2 = JsonConvert.SerializeObject(ourPostData2);

        //StartCoroutine(WWWPostTest());
        StartCoroutine(UnityWebRequestPost(url, param1, param2));
    }

    // WWW POST ���
    /*IEnumerator WWWPostTest()
    {
        // POST����� �����Ҷ� ������ ��Ƽ� �����ϴ� ��� �߿� �ϳ��� ����
        WWWForm form = new WWWForm();
        Dictionary<string, string> data = form.headers;
        data["players_session_id"] = "KDKomCuHWZr1O2binzPAnwTp5lqyZTDrNrAlvuEx";
        data["bet_id"] = "633e959675294e05550c8e69";
        WWW www = new WWW(url, null, data);

        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.Log("ERROR");
        }
    }*/

    // UnityWebRequest POST ���
    IEnumerator UnityWebRequestPost(string url, string parameter1, string parameter2)
    {
        //List<string> playersSessionId = new List<string>();
        //playersSessionId.Add("tMRBenymFOdswsvBaLzDiO6pJEaPyilwKvw85R5A");
        //var param = JsonConvert.SerializeObject(playersSessionId);

        /*string ourPostData1 = "{\"GCGvmOvqvVoCnib84c2vQ70LhkG0Tcaspsh4LFEa\" }";
        string ourPostData2 = "{\"633e959675294e05550c8e69\" }";*/
        WWWForm form = new WWWForm();

        byte[] jData1 = new System.Text.UTF8Encoding().GetBytes(parameter1);
        byte[] jData2 = new System.Text.UTF8Encoding().GetBytes(parameter2);

        /*byte[] jData1 = System.Text.Encoding.ASCII.GetBytes(ourPostData1.ToCharArray());
        byte[] jData2 = System.Text.Encoding.ASCII.GetBytes(ourPostData2.ToCharArray());*/

        form.AddBinaryData("players_session_id", jData1);
        form.AddBinaryData("bet_id", jData2);

        UnityWebRequest www = UnityWebRequest.Post(url, form);
        www.uploadHandler = new UploadHandlerRaw(jData1);
        www.uploadHandler = new UploadHandlerRaw(jData2);
        //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");
        www.SetRequestHeader("api-key", apiKey);

        yield return www.SendWebRequest();

        //Debug.Log(www.downloadHandler.text);

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(www.error);
        }

        if (www.error == null)
            {
            Debug.Log(www.downloadHandler.text);
        }
            else
        {
            Debug.Log("error");
        }

        /*form.AddField("players_session_id", ourPostData1);
        form.AddField("bet_id", ourPostData2);*/

        /*using (UnityWebRequest www = UnityWebRequest.Post(url, form))
        {
            www.uploadHandler = new UploadHandlerRaw(jData1);
            www.uploadHandler = new UploadHandlerRaw(jData2);
            //www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("api-key", apiKey);

            yield return www.SendWebRequest();

            //Debug.Log(www.downloadHandler.text);

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(www.error);
                yield break;
            }

            *//*if (www.error == null)
            {
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                Debug.Log("error");
            }*//*
        }*/

        /*string apiKey = "70pNqHWqzZ0DXwsIP0e0bA";
        string url = "https://odin-api-sat.browseosiris.com/v1/betting/zera/place-bet";*/
        //string url = "http://localhost:8546/api//getuserprofile";
        /*form.AddField("players_session_id", players_session_id);
        form.AddField("bet_id", bet_id);*/
        //Debug.Log(json1.ToString());

        // ���� ���̶�� �� "InProgress"

        // place-bet.json ������ �ҷ��� ����Ʈ�� ��û
        /*Root hs = new Root();
        string JsonBodyData = JsonUti
        lity.ToJson(hs);*/

        // ���� ������ ���ε�
        //UnityWebRequest www = UnityWebRequest.Put(url, JsonBodyData);



        /*if (www.error == null)
        {
            Debug.Log(www.downloadHandler.text);
        }
        else
        {
            Debug.Log("error");
        }*/
    }
}



