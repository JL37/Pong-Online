using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    [SerializeField] int m_MaxScore = 10;

    protected int m_PlayerScore = 0;
    protected int m_EnemyScore = 0;

    protected int m_PlayerNumber;
    protected string m_EnemyName;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            print("ScoreManager instance has been deleted as another instance already exists.");
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        foreach (KeyValuePair<int,Photon.Realtime.Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.NickName != PhotonNetwork.LocalPlayer.NickName)
                m_EnemyName = player.Value.NickName;
        }

        m_PlayerNumber = PhotonNetwork.LocalPlayer.ActorNumber;
    }

    public void AddPlayerScore()
    {
        m_PlayerScore += 1;
    }

    public int GetPlayerScore() { return m_PlayerScore; }

    public void AddOpponentScore()
    {
        m_EnemyScore += 1;
    }

    public int GetOpponentScore() { return m_EnemyScore; }

    public bool GameEnded()
    {
        return m_PlayerScore >= m_MaxScore || m_EnemyScore >= m_MaxScore;
    }

    public int GetPlayerNumber() { return m_PlayerNumber; }

    public string GetEnemyName() { return m_EnemyName; }

    public bool PlayerWin() { return m_PlayerScore >= m_MaxScore; }

    public int GetPerformanceRating()
    {
        float perc = ((float)m_PlayerScore / (float)(m_PlayerScore + m_EnemyScore)) * 100f;
        return Mathf.RoundToInt(perc);
    }
}
