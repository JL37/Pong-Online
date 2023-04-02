using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class OnlineInGameManager : MonoBehaviourPunCallbacks
{
    public static OnlineInGameManager Instance { get; private set; }

    [Header("Spawning")]
    [SerializeField] Transform[] m_SpawnPoints;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject ballPrefab;

    [Header("UI")]
    [SerializeField] Canvas m_Canvas;
    [SerializeField] TMP_Text[] m_NameTextArr;
    [SerializeField] TMP_Text[] m_ScoreTextArr;

    [Header("Game settings")]
    [SerializeField] float m_UITimer = 0.5f;

    protected PhotonView m_View;

    private void Start()
    {
        Instance = this;
        InitialSpawn();
        InitialiseNameTexts();

        //Starting round...
        m_View = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            CallRPC_InitialiseRound();
        else if (!PhotonNetwork.IsConnected)
            StartCoroutine(StartRound());
    }

    public void CallRPC_InitialiseRound()
    {
        m_View.RPC("RPC_InitialiseRound", RpcTarget.All);
    }

    [PunRPC]
    protected void RPC_InitialiseRound()
    {
        StartCoroutine(StartRound());
    }

    protected IEnumerator StartRound()
    {
        m_Canvas.gameObject.SetActive(true);
        UpdateScoreUI();

        yield return new WaitForSecondsRealtime(m_UITimer);

        //Hide text
        m_Canvas.gameObject.SetActive(false);

        //Spawn ball into world
        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate(ballPrefab.name, Vector3.zero, Quaternion.identity);
        else if (!PhotonNetwork.IsConnected)
            Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
    }

    protected void UpdateScoreUI()
    {
        int currNum = PhotonNetwork.LocalPlayer.ActorNumber;
        currNum = Mathf.Max(1, currNum);

        for (int i = 0; i < 2; ++i)
            m_ScoreTextArr[i].text = currNum == i + 1 ? ScoreManager.Instance.GetPlayerScore().ToString() : ScoreManager.Instance.GetOpponentScore().ToString();
    }

    public void InitialiseNameTexts()
    {
        if (!PhotonNetwork.IsConnected)
            return;

        for (int i = 1; i <= 2; ++i)
        {
            Photon.Realtime.Player currPlayer = PhotonNetwork.CurrentRoom.GetPlayer(i);
            string newName = currPlayer.NickName + "<br>";

            if (currPlayer.IsLocal)
                newName += "(YOU)";
            else
                newName += "(OPP)";

            m_NameTextArr[i - 1].text = newName;
        }
    }

    protected void InitialSpawn()
    {
        int num = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        num = Mathf.Max(0, num);

        //Spawn player into scene
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.Instantiate(playerPrefab.name, m_SpawnPoints[num].position, Quaternion.identity);
        else
            Instantiate(playerPrefab, m_SpawnPoints[num].position, Quaternion.identity);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //Leave room
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        //Go to game summary
        SceneManager.LoadScene("SummaryScene");
    }
}
