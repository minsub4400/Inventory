using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIStorage : MonoBehaviour
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    public string apiKey = "70pNqHWqzZ0DXwsIP0e0bA";
    public string statusCode;
    // 유저 고유 ID
    public string _id;
    public string userName;
    public string sessionId;
    public string message;
    public string balance;

    // 승리자 배당금
    public string amount_won;

    // 배팅에 필요한 ID
    public string bet_id;

    // 배팅을 했다는 배팅 정보가 묶여 있는 ID
    public string betting_id;

    // 우승자 id
    public string winner_id;

    public string currency;
    // 임시 MetaMask _id
    public string MetaMaskSessionID;

    private void Start()
    {
        winner_id = "633b86420e028f7ecb10fd09";
        MetaMaskSessionID = "eiry4c7tix9T06Q2yObbrghBrQTETTTeorDSAv2R";
    }
}
