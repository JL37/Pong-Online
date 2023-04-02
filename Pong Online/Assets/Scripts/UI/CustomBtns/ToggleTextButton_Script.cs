using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ToggleTextButton_Script : CustomButton_Script
{
    [Header("Toggle Settings")]
    [SerializeField] bool m_Toggled = false;
    [SerializeField] string m_ToggledText = "On";
    [SerializeField] string m_UntoggledText = "Off";

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        UpdateText();
    }

    public void ToggleButton()
    {
        m_Toggled = !m_Toggled;
        UpdateText();
    }

    public void SetToggle(bool toggle)
    {
        m_Toggled = toggle;
        UpdateText();
    }

    protected void UpdateText()
    {
        m_TextObj.text = m_Toggled ? m_ToggledText : m_UntoggledText;
    }

    public bool GetToggle() { return m_Toggled; }
}
