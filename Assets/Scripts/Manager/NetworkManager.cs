using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // ###############################################
    //             NAME : Simstealer                      
    //             MAIL : minsub4400@gmail.com         
    // ###############################################

    void Start()
    {
        // 서버에 참여
        PhotonNetwork.ConnectUsingSettings();
    }

    // 마스터 서버에 참여하면
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2}, null);
    }

    public override void OnJoinedRoom()
    {
        // 방에 참가 했을때
        Vector3 vector3 = new Vector3(0f, 1f, 0f);
        PhotonNetwork.Instantiate("Player", vector3, Quaternion.identity);
    }


}
