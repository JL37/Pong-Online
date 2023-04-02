using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Typewriter_Effect : MonoBehaviour
{
    [SerializeField] int m_StartIdx = -1;
    [SerializeField] int m_EndIdx = -1;

    [SerializeField] bool m_Loop = false;
    [SerializeField] float m_Speed = 0.1f;

    protected int m_CurrIdx;
    protected float m_CurrSpd;

    protected TMP_Text m_TextObj;
    protected string m_TextStr;

    // Start is called before the first frame update
    void Start()
    {
        //Getting components and stuff
        m_TextObj = GetComponent<TMP_Text>();
        m_TextStr = m_TextObj.text;

        //Set to initial index of text
        if (m_StartIdx < 0)
            m_StartIdx = 0;

        //Set to final index of text
        if (m_EndIdx < 0 || m_EndIdx < m_StartIdx)
            m_EndIdx = m_TextStr.Length - 1;


        //Setting initial values
        RestartAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrSpd -= Time.deltaTime;

        if (m_CurrSpd <= 0)
        {
            m_CurrIdx += 1;
            m_CurrSpd = m_Speed;

            if (m_CurrIdx > m_EndIdx)
                On_EndOfAnimation();
            else
                UpdateTextComponent();
        }
    }

    protected void On_EndOfAnimation()
    {
        if (m_Loop)
            RestartAnimation();
        else
            enabled = false;
    }

    protected void RestartAnimation()
    {
        m_CurrSpd = m_Speed;
        m_CurrIdx = m_StartIdx;

        UpdateTextComponent();
        enabled = true;
    }

    protected void UpdateTextComponent()
    {
        m_TextObj.text = GetCurrentText();
    }

    protected string GetCurrentText()
    {
        string currText = m_TextStr;
        currText = currText.Substring(0, m_CurrIdx + 1);


        return currText;
    }
}
