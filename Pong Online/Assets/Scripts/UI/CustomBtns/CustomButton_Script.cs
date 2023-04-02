using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton_Script : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Color m_OriginalColor;
    [SerializeField] Color m_HoverColor;
    [SerializeField] Color m_ClickColor;

    [SerializeField] float m_EventDelay = 0f;
    [SerializeField] UnityEvent m_Events;

    protected float m_CurrEventDelay;
    protected bool m_EventActive = false;
    protected bool m_Hover = false;
    protected bool m_Click = false;

    protected GameObject[] m_ButtonArr;
    protected TMP_Text m_TextObj;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_TextObj = GetComponent<TMP_Text>();
        m_TextObj.color = m_OriginalColor;

        m_ButtonArr = GameObject.FindGameObjectsWithTag(tag);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_EventActive)
            return;


        //Clicking inputs and stuff
        if (Input.GetMouseButtonDown(0) && m_Hover)
        {
            m_TextObj.color = m_ClickColor;
            m_Click = true;
        }
        else if (Input.GetMouseButtonUp(0) && m_Click)
        {
            m_Click = false;

            //Perform button functions here
            if (m_Hover)
                OnButtonClicked();
            else
                ManualPointerCheck();
        }
    }

    protected void OnButtonClicked()
    {
        Debug.LogFormat("Button {0} has been clicked.", name);

        if (m_EventDelay <= 0)
        {
            m_Events.Invoke();
            ManualPointerCheck();
        }
        else
        {
            StartCoroutine(DelayCountdown());
        }
    }

    protected IEnumerator DelayCountdown()
    {
        foreach (GameObject btn in m_ButtonArr)
            btn.GetComponent<CustomButton_Script>().m_EventActive = true;

        print("Delay...");
        yield return new WaitForSecondsRealtime(m_EventDelay);

        Debug.LogFormat("Event from button {0} has been invoked.",name);
        m_Events.Invoke();
        ManualPointerCheck();

        foreach (GameObject btn in m_ButtonArr)
            btn.GetComponent<CustomButton_Script>().m_EventActive = false;
    }

    protected void ManualPointerCheck()
    {
        m_TextObj.color = m_Hover ? m_HoverColor : m_OriginalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.LogFormat("Button {0} pointer enter.",name);

        m_Hover = true;
        if (!m_Click && !m_EventActive)
            m_TextObj.color = m_HoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.LogFormat("Button {0} pointer exit.",name);

        m_Hover = false;
        if (!m_Click && !m_EventActive)
            m_TextObj.color = m_OriginalColor;
    }

    private void OnDisable()
    {
        m_Hover = false;
        m_Click = false;
    }
}
