using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class InitialStartup : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = GenerateUserName();

        print("Lobby has been joined");
        SceneManager.LoadScene("LobbyScene");
    }

    protected string GenerateUserName()
    {
        char[] charArr = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        string name = "User ";

        for (int i = 0; i < 3; ++i)
            name += charArr[Random.Range(0, charArr.Length)];

        return name;
    }
}
