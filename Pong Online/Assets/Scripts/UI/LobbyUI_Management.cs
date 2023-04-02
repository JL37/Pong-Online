using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyUI_Management : MonoBehaviour
{
    [SerializeField] TMP_Text m_UserNameObj;
    [SerializeField] TMP_Text m_UserNameInput;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUserName();
    }

    public void UpdateUserName()
    {
        string currName = PhotonNetwork.NickName != "" ? PhotonNetwork.NickName : "Offline";

        m_UserNameObj.text = currName;
        m_UserNameInput.text = currName;
    }
}
