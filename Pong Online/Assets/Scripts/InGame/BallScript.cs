using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BallScript : MonoBehaviour, IPunObservable
{
    protected Rigidbody2D m_RigidBody;
    protected PhotonView m_View;

    protected Vector2 m_NetworkPos;

    [SerializeField] float m_Speed = 5f;
    [SerializeField] float m_Timer = 1f;

    private void Awake()
    {
        m_View = GetComponent<PhotonView>();
        m_RigidBody = GetComponent<Rigidbody2D>();

        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (m_View.IsMine)
            m_View.RPC("RPC_LaunchBall", RpcTarget.All);
        else if (!PhotonNetwork.IsConnected)
            StartCoroutine(LaunchBall());
        //else
        //    Destroy(m_RigidBody);
    }

    private void FixedUpdate()
    {
        if (!m_View.IsMine)
            m_RigidBody.position = Vector3.MoveTowards(m_RigidBody.position, m_NetworkPos, Time.fixedDeltaTime);
    }

    [PunRPC]
    protected void RPC_LaunchBall()
    {
        if (m_View.IsMine)
            StartCoroutine(LaunchBall());
    }

    protected IEnumerator LaunchBall()
    {
        yield return new WaitForSecondsRealtime(m_Timer);

        float x = Random.Range(0, 2) == 0 ? -1 : 1;
        float y = Random.Range(0, 2) == 0 ? -1 : 1;

        m_RigidBody.velocity = new Vector2(m_Speed * x, m_Speed * y);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(m_RigidBody.position);
            stream.SendNext(m_RigidBody.velocity);
        }
        else
        {
            m_NetworkPos = (Vector2)stream.ReceiveNext();
            m_RigidBody.velocity = (Vector2)stream.ReceiveNext();

            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            m_NetworkPos += m_RigidBody.velocity * lag;
        }
    }
}
