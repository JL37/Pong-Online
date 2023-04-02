using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerScript : MonoBehaviour, IPunObservable
{
    [SerializeField] float m_MoveLerp = 0.025f;
    [SerializeField] float m_YLimit = 8;

    protected Vector2 m_CurrPos;
    protected Rigidbody2D m_RigidBody;
    protected PhotonView m_View;

    protected Vector2 m_NetworkPos;

    // Start is called before the first frame update
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_View = GetComponent<PhotonView>();

        m_NetworkPos = m_RigidBody.position;
    }

    private void FixedUpdate()
    {
        if (m_View.IsMine || !PhotonNetwork.IsConnected)
        {
            UpdateMouseMovement();
        }
        else
        {
            m_RigidBody.position = Vector3.MoveTowards(m_RigidBody.position, m_NetworkPos, Time.fixedDeltaTime);
        }
    }

    public void UpdateMouseMovement()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        Vector3 newPos = transform.position;
        newPos.y = mousePos.y;
        newPos.y = Mathf.Clamp(newPos.y, -m_YLimit, m_YLimit);

        m_CurrPos = Vector2.Lerp(transform.position, newPos, m_MoveLerp);
        m_RigidBody.MovePosition(m_CurrPos);
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
