using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    // ��Ʈ��ũ�� ����ȭ �Ǵ� ���������
    [Networked]
    public byte MyByte { get; set; }

    // ����ȭ �Ǿ�� �ϴ� ����
    // - ��ȭ

    private NetworkCharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            data.direction.Normalize();
            _cc.Move(5 * data.direction * Runner.DeltaTime);
        }
    }

    public override void Render()
    {
        // Object.HasInputAuthority : �ڱ� �ڽ��� �Է��� �� �� ������
        if (Input.GetKeyDown(KeyCode.E) && Object.HasInputAuthority)
        {
            MyByte = (byte)UnityEngine.Random.Range(0, 255);
        }
    }

    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hey Mate!");
        }

        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.T))
        {
            GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += "�� ��� ��";
        }

    }

    //[SerializeField]
    private GameObject aPIStorageOBJ;
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        // ���� �÷��̾��
        if (info.Source == Runner.Simulation.LocalPlayer)
            message = $"You said: {message}\n";
        else // ����Ʈ �÷��̾��
            message = $"Some other player said: {message}\n";
        //GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += message;

        // �ڽ� �� ��������
        /*aPIStorageOBJ = aPIStorageOBJ.transform.GetChild(2).gameObject;
        Player_BettingInfo aPIStorage = aPIStorageOBJ.GetComponent<Player_BettingInfo>();
        if (aPIStorage != null)
        {
            GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += aPIStorage.Test;
        }*/

        // ���� �� ��������
        // �κ� �Ŵ��� ã��
        /*aPIStorageOBJ = GameObject.FindGameObjectWithTag("LobbyManager").gameObject;
        APIStorage apiSotrage = aPIStorageOBJ.transform.GetComponent<APIStorage>();
        if (apiSotrage != null)
        {
            GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += apiSotrage.winner_id;
        }*/


    }


    /*private Text _messages;
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        if (_messages == null)
            _messages = FindObjectOfType<Text>();
        if (info.Source == Runner.Simulation.LocalPlayer)
            message = $"You said: {message}\n";
        else
            message = $"Some other player said: {message}\n";
        _messages.text += message;
    }*/
}