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
    // ���� ���� ID
    public string _id;
    public string userName;
    public string sessionId;
    public string message;
    public string balance;

    // �¸��� ����
    public string amount_won;

    // ���ÿ� �ʿ��� ID
    public string bet_id;

    // ������ �ߴٴ� ���� ������ ���� �ִ� ID
    public string betting_id;

    // ����� id
    public string winner_id;

    public string currency;
    // �ӽ� MetaMask _id
    public string MetaMaskSessionID;

    private void Start()
    {
        winner_id = "633b86420e028f7ecb10fd09";
        MetaMaskSessionID = "eiry4c7tix9T06Q2yObbrghBrQTETTTeorDSAv2R";
    }
}
