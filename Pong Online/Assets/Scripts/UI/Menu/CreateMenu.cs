using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMenu : MonoBehaviour
{
    [SerializeField] RoomJoin_Management m_RoomManager;
    [SerializeField] ToggleTextButton_Script m_PublicToggle;

    public void CreateRoom()
    {
        //Create room based on settings
        m_RoomManager.CreateRoom(!m_PublicToggle.GetToggle());
    }
}
