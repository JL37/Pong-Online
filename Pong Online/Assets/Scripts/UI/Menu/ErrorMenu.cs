using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ErrorMenu : MonoBehaviour
{
    [SerializeField] TMP_Text m_DescText;

    public void SetErrorMessage(string msg)
    {
        m_DescText.text = msg;
    }
}
