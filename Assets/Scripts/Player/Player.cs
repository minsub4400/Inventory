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
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_SendMessage(string message, RpcInfo info = default)
    {
        // 로컬 플레이어면
        if (info.Source == Runner.Simulation.LocalPlayer)
            message = $"You said: {message}\n";
        else // 리모트 플레이어면
            message = $"Some other player said: {message}\n";
        GameObject.FindGameObjectWithTag("Chet").GetComponent<Text>().text += message;
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