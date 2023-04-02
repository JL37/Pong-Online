using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyMenu_Script : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] TMP_Text m_RoomCodeText;
    [SerializeField] TMP_Text m_VisibilityText;

    [SerializeField] TMP_Text m_WaitingText;
    [SerializeField] TMP_Text m_ReadyText;

    [SerializeField] TMP_Text m_CountdownText;
    [SerializeField] CustomButton_Script m_BackButton;

    [Header("Values")]
    [SerializeField] float m_CountdownTimer = 5f;

    protected bool m_CountdownActivate = false;
    protected float m_CurrentTimer = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (m_CountdownActivate)
        {
            //If 0, go to actual match area
            if (m_CurrentTimer <= 0)
            {
                m_CountdownActivate = false;
                if (PhotonNetwork.IsMasterClient)
                    PhotonNetwork.LoadLevel("MainGameScene");
            }

            //Update text
            int timeInt = Mathf.CeilToInt(m_CurrentTimer);
            m_CountdownText.text = string.Format("Starting match in {0}...", timeInt);

            //Count down to 0
            m_CurrentTimer -= Time.deltaTime;
        }
    }

    public void UpdateInterface()
    {
        bool roomFull = PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers;

        UpdateCenterText(roomFull);
        UpdateRoomCode();
        UpdateVisibilityText();
        UpdateButtonVisibility(roomFull);
        UpdateCountdown(roomFull);
    }

    public void UpdateVisibilityText()
    {
        if (PhotonNetwork.CurrentRoom.IsVisible)
            m_VisibilityText.text = "(PUBLIC)";
        else
            m_VisibilityText.text = "(PRIVATE)";
    }

    public void UpdateCountdown(bool roomFull)
    {
        m_CountdownActivate = roomFull;
        m_CurrentTimer = m_CountdownTimer;

        m_CountdownText.gameObject.SetActive(roomFull);
    }

    public void UpdateCenterText(bool roomFull)
    {
        m_ReadyText.gameObject.SetActive(roomFull);
        m_WaitingText.gameObject.SetActive(!roomFull);
    }

    public void UpdateRoomCode()
    {
        string roomCode = "Room Code:<br>" + PhotonNetwork.CurrentRoom.Name;
        m_RoomCodeText.text = roomCode;
    }

    public void UpdateButtonVisibility(bool roomFull)
    {
        m_BackButton.gameObject.SetActive(!roomFull);
    }
}
