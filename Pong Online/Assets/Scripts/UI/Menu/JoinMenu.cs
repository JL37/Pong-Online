using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JoinMenu : MonoBehaviour
{
    [Header("Outside stuff")]
    [SerializeField] RoomJoin_Management m_RoomManager;
    [SerializeField] MenuSections m_SectionManager;


    [Header("Menu stuff")]
    [SerializeField] TMP_InputField m_Input;
    [SerializeField] TMP_Text m_ErrorText;

    public void JoinRoom()
    {
        if (CheckValidity()) {
            //Join room if valid
            m_RoomManager.JoinRoom(m_Input.text.ToUpper());

            //Go to connecting area
            m_SectionManager.EnterSection(2);
            m_ErrorText.gameObject.SetActive(false);
        } 
        else
        {
            //Display error
            m_ErrorText.gameObject.SetActive(true);
            m_ErrorText.GetComponent<ComponentShake>().RestartShake();

            //Reset input field value
            m_Input.GetComponent<InputField_Script>().ResetInputField();
        }
    }

    public bool CheckValidity()
    {
        bool valid = m_Input.text.Length == m_Input.characterLimit;

        return valid;
    }
}
