using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class Player : NetworkBehaviour
{
    // 네트워크로 동기화 되는 변수만들기
    [Networked]
    public byte MyByte { get; set; }

    // 동기화 되어야 하는 변수
    // - 재화

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
        // Object.HasInputAuthority : 자기 자신이 입력을 할 수 있을때
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
            GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += "잘 들어 옴";
        }

    }

    //[SerializeField]
    private GameObject aPIStorageOBJ;
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        // 로컬 플레이어면
        if (info.Source == Runner.Simulation.LocalPlayer)
            message = $"You said: {message}\n";
        else // 리모트 플레이어면
            message = $"Some other player said: {message}\n";
        //GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += message;

        // 자신 것 가져오기
        /*aPIStorageOBJ = aPIStorageOBJ.transform.GetChild(2).gameObject;
        Player_BettingInfo aPIStorage = aPIStorageOBJ.GetComponent<Player_BettingInfo>();
        if (aPIStorage != null)
        {
            GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += aPIStorage.Test;
        }*/

        // 남에 것 가져오기
        // 로비 매니져 찾기
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