using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIStorage : MonoBehaviour
{

    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    // ������ ������ 2�δ����� �Ѵ�.
    // [0] : Player1(Host), [1] : Player2(Client)

    public string apiKey
    {
        get { return apiKey; }
        private set { apiKey = "70pNqHWqzZ0DXwsIP0e0bA"; }
    }

    public static APIStorage instance;

    private void Awake()
    {
        instance = this;
        statusCode = new string[2];
        _id = new string[2];
        userName = new string[2];
        sessionId = new string[2];
        message = new string[2];
        amount_won = new string[2];
        bet_id = new string[2];
        betting_id = new string[2];
        currency = new string[2];
        zera = new string[2];
        ace = new string[2];

    }

    // ������ ��ȭ�� ������ ����
    public string[] zera;
    public string[] ace;

    // API ȣ�� ���� �ڵ� 200�� �ƴϸ� ������ ��ȯ�Ѵ�.
    public string[] statusCode;

    // ���� ���� ID
    public string[] _id;

    // ���� ���� �̸�
    public string[] userName;

    // �Ź� �ٲ�� ���� ID
    public string[] sessionId;

    // Post �޼��� ȣ�� ��, "success"
    public string[] message;

    // �¸��� ����
    public string[] amount_won;

    // ���ÿ� �ʿ��� ID
    public string[] bet_id;

    // ������ �ߴٴ� ���� ������ ���� �ִ� ID
    public string[] betting_id;

    // (�ӽ�)��ȭ�� �̸��� ���� ����(�Է��� �޴´�)
    public string[] currency;

    // ����� id
    public string winner_id;


    // (�ӽ�) MetaMask _id(�׽�Ʈ �뵵)
    public string MetaMaskSessionID;

    private void Start()
    {
        // �׽�Ʈ �뵵 ID
        winner_id = "633b86420e028f7ecb10fd09";
        MetaMaskSessionID = "eiry4c7tix9T06Q2yObbrghBrQTETTTeorDSAv2R";
    }
}
