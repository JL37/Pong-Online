using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GoalScript : MonoBehaviour
{
    [SerializeField] int m_PlayerNumber = 1;
    protected PhotonView m_View;

    private void Start()
    {
        m_View = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ball") && PhotonNetwork.IsMasterClient)
        {
            //Destroy ball
            PhotonNetwork.Destroy(collision.gameObject);

            //Add points to score manager accordingly
            m_View.RPC("RPC_UpdateScore", RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPC_UpdateScore()
    {
        //Updating of score
        int currNum = PhotonNetwork.LocalPlayer.ActorNumber;
        currNum = Mathf.Max(1, currNum);

        if (currNum != m_PlayerNumber)
            ScoreManager.Instance.AddPlayerScore();
        else
            ScoreManager.Instance.AddOpponentScore();

        if (!PhotonNetwork.IsMasterClient)
            return;

        //Initialise ball and stuff again if game has not ended. Else, go to game overview screen
        if (!ScoreManager.Instance.GameEnded())
            OnlineInGameManager.Instance.CallRPC_InitialiseRound();
        else
            PhotonNetwork.LeaveRoom(); //Leave room as game has ended
    }
}
