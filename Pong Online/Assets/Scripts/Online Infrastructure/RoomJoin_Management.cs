using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class RoomJoin_Management : MonoBehaviourPunCallbacks
{
    protected static readonly char[] m_CharArr = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    [Header("Component reference")] 
    [SerializeField] TMP_InputField m_NameInputField;
    [SerializeField] ComponentShake m_NameInputFailShake;

    [Header("Menu and UI")]
    [SerializeField] MenuSections m_MenuManager;
    [SerializeField] LobbyMenu_Script m_LobbyMenu;
    [SerializeField] ErrorMenu m_ErrorMenu;
    [SerializeField] LobbyUI_Management m_LobbyUIManagement;

    
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        Debug.LogFormat("Current username is {0}.", PhotonNetwork.NickName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateRoom(bool isPrivateRoom)
    {
        //Generating room code
        string code = "";
        for (int i = 0; i < 6; ++i)
        {
            code += m_CharArr[Random.Range(0, m_CharArr.Length)];
        }

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = !isPrivateRoom;

        PhotonNetwork.CreateRoom(code,roomOptions,null);
    }

    public void JoinRoom(string code)
    {
        PhotonNetwork.JoinRoom(code);
    }

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom(false);
    }

    public override void OnJoinedRoom()
    {
        //When room is joined, enter scene...
        Debug.LogFormat("Room name '{0}' has been joined.", PhotonNetwork.CurrentRoom.Name);

        //Change menu section and update whole interface on joined room...
        m_MenuManager.EnterSection(m_LobbyMenu.gameObject);
        m_LobbyMenu.UpdateInterface();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //Updates interface whenever new player joins
        m_LobbyMenu.UpdateInterface();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Updates interface whenever new player joins
        m_LobbyMenu.UpdateInterface();
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //Go back to menu
        m_MenuManager.EnterSection(0);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //Go to error scene...
        Debug.LogFormat("Join room failed, error {0}, with return code {1}", message, returnCode);

        m_MenuManager.EnterSection(m_ErrorMenu.gameObject);
        m_ErrorMenu.SetErrorMessage(message);
    }

    public void UpdateUserName()
    {
        string inputCheck = m_NameInputField.text.Trim();
        ResetNameInputField();


        if (inputCheck.Length >= 4)
        {
            PhotonNetwork.NickName = inputCheck;
        }
        else
        {
            m_NameInputFailShake.RestartShake();
            return;
        }

        m_LobbyUIManagement.UpdateUserName();

        m_MenuManager.EnterSection(0);
    }

    public void GoToLeaderboard()
    {
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void ResetNameInputField()
    {
        m_NameInputField.text = "";
    }
}
