using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkill : MonoBehaviourPun, IPunObservable
{
    public float speed = 5f;
    private Vector3 networkPosition;

    void Update()
    {
        if (photonView.IsMine)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            Vector3 direction = new Vector3(h, 0, v);
            transform.Translate(direction * speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, networkPosition, speed * Time.deltaTime);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            networkPosition = (Vector3)stream.ReceiveNext();
        }
    }
}