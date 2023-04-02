using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputField_Script : MonoBehaviour
{
    protected TMP_InputField m_InputField;

    private void Start()
    {
        m_InputField = GetComponent<TMP_InputField>();
    }

    public void ResetInputField()
    {
        m_InputField.text = "";
    }
}
