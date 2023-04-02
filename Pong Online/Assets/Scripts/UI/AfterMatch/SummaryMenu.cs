using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.SceneManagement;

public class SummaryMenu : MonoBehaviour
{
    [SerializeField] TMP_Text[] m_PlayerNameArr;
    [SerializeField] TMP_Text[] m_ScoreTextArr;
    [SerializeField] TMP_Text m_WinLoseText;
    [SerializeField] TMP_Text m_PerformanceText;
    [SerializeField] GameObject m_Button;

    // Start is called before the first frame update
    void Start()
    {
        InitialiseNameTexts();
        InitialiseScoreTexts();
        InitialiseWinLoseText();
        InitialisePerformanceText();

        UploadScore();
    }


    protected void InitialisePerformanceText()
    {
        m_PerformanceText.text = string.Format("Performance rating: {0}", ScoreManager.Instance.GameEnded() ? ScoreManager.Instance.GetPerformanceRating() : "NIL");
    }

    protected void InitialiseNameTexts()
    {
        for (int i = 1; i <= 2; ++i)
        {
            string newName = "";
            if (i == ScoreManager.Instance.GetPlayerNumber())
                newName = PhotonNetwork.LocalPlayer.NickName + "<br>(YOU)";
            else
                newName = ScoreManager.Instance.GetEnemyName() + "<br>(OPP)";

            m_PlayerNameArr[i - 1].text = newName;
        }
    }

    protected void InitialiseScoreTexts()
    {
        int currNum = ScoreManager.Instance.GetPlayerNumber();
        currNum = Mathf.Max(1, currNum);

        for (int i = 0; i < 2; ++i)
            m_ScoreTextArr[i].text = currNum == i + 1 ? ScoreManager.Instance.GetPlayerScore().ToString() : ScoreManager.Instance.GetOpponentScore().ToString();
    }

    protected void InitialiseWinLoseText()
    {
        if (ScoreManager.Instance.PlayerWin())
        {
            m_WinLoseText.text = "You win!";
            m_WinLoseText.color = Color.green;
        } 
        else if (!ScoreManager.Instance.GameEnded())
        {
            m_WinLoseText.text = "You win! Opponent quit.";
            m_WinLoseText.color = Color.green;
        }
        else
        {
            m_WinLoseText.text = "You lose!";
            m_WinLoseText.color = Color.red;
        }
    }

    public void GoToLeaderboard()
    {
        //Delete score manager
        Destroy(ScoreManager.Instance.gameObject);

        //Load scene after a set amount of time
        m_Button.SetActive(false);
        StartCoroutine(LeaderBoardTransition());
    }

    protected IEnumerator LeaderBoardTransition()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        SceneManager.LoadScene("LeaderboardScene");
    }

    protected void UploadScore()
    {
        if (!ScoreManager.Instance.PlayerWin() || !ScoreManager.Instance.GameEnded())
            return;

        //HighScores.UploadScore(PhotonNetwork.NickName, ScoreManager.Instance.GetPerformanceRating());
        Leaderboard_2.Instance.SetLeaderboardEntry(PhotonNetwork.NickName, ScoreManager.Instance.GetPerformanceRating());
    }
}
