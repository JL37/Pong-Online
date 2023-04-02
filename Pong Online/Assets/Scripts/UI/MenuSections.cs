using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSections : MonoBehaviour
{
    [SerializeField] List<GameObject> m_Sections;
    [SerializeField] GameObject m_CurrSection;

    // Start is called before the first frame update
    void Start()
    {
        EnterSection(m_CurrSection);
    }

    public void EnterSection(int idx)
    {
        m_CurrSection = m_Sections[idx];

        for (int i = 0; i < m_Sections.Count; ++i)
            m_Sections[i].SetActive(i == idx);
    }

    public void EnterSection(GameObject section)
    {
        if (m_Sections.Contains(section))
        {
            m_CurrSection = section;

            foreach (GameObject indexed_section in m_Sections)
            {
                indexed_section.SetActive(m_CurrSection == indexed_section);
            }
        } 
        else
        {
            print("ERROR! SECTION IS NOT INSIDE OF ARRAY.");
        }
    }
}
