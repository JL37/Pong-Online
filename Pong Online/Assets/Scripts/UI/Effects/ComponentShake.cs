using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentShake : MonoBehaviour
{
    [SerializeField] float m_ShakeIntensity = 1f;
    [SerializeField] float m_ShakeDuration = 0.25f;

    protected RectTransform m_RectTransform;
    protected float m_CurrDuration = 0f;
    protected Vector3 m_OriginalPos;

    // Start is called before the first frame update
    void Start()
    {
        RestartShake();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurrDuration > 0)
        {
            //Timer
            m_CurrDuration -= Time.deltaTime;

            //Randomise shake intensity
            float xShake = GenerateShakeIntensityValue();
            float yShake = GenerateShakeIntensityValue();

            Vector3 updatedPos = new Vector3(xShake, yShake, 0);

            //Set new pos
            m_RectTransform.anchoredPosition = updatedPos;
        } 
        else
        {
            //Disable shake and reset to original position
            m_RectTransform.anchoredPosition = m_OriginalPos;
            enabled = false;
        }
    }

    protected float GenerateShakeIntensityValue()
    {
        return Random.Range(-m_ShakeIntensity, m_ShakeIntensity);
    }

    public void RestartShake()
    {
        if (!m_RectTransform)
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_OriginalPos = m_RectTransform.anchoredPosition;
        }

        m_RectTransform.anchoredPosition = m_OriginalPos;

        m_CurrDuration = m_ShakeDuration;
        enabled = true;
    }
}
