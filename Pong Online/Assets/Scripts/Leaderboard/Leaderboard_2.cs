using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using TMPro;

public class Leaderboard_2 : MonoBehaviour
{
    public static Leaderboard_2 Instance { private set; get; }
    protected string m_PublicKey = "92769ac1f9df5961aec1449c88bbc1c43580ac7fa4087eb3d0b90eaab1c48b25";

    [SerializeField] List<TextMeshProUGUI> m_Names;
    [SerializeField] List<TextMeshProUGUI> m_Scores;

    private void Awake()
    {
        Instance = this;

        GetLeaderBoard();
    }

    public void GetLeaderBoard()
    {
        for (int i = 0; i < m_Names.Count; ++i)
        {
            m_Names[i].gameObject.SetActive(false);
            m_Scores[i].gameObject.SetActive(false);
        }

        LeaderboardCreator.GetLeaderboard(m_PublicKey, ((msg) =>
        {
            int loopLength = (msg.Length < m_Names.Count) ? msg.Length : m_Names.Count;

            for (int i = 0; i < loopLength; ++i)
            {
                m_Names[i].gameObject.SetActive(true);
                m_Scores[i].gameObject.SetActive(true);

                m_Names[i].text = (i + 1).ToString() + ". " + msg[i].Username;
                m_Scores[i].text = msg[i].Score.ToString();
            }
        }));
    }

    public void SetLeaderboardEntry(string username, int score)
    {
        LeaderboardCreator.UploadNewEntry(m_PublicKey, username, score, ((_) => {
            GetLeaderBoard();
        }));
    }
}
