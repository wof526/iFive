using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    public int hp = 100;
    public float healRadius = 5f;

    void Update()
    {
        if (photonView.IsMine)
        {
            CheckAndHealNearbyPlayers();
        }
    }

    void CheckAndHealNearbyPlayers()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, healRadius);
        foreach (var hitCollider in hitColliders)
        {
            Player otherPlayer = hitCollider.GetComponent<Player>();
            if (otherPlayer != null && otherPlayer != this)
            {
                photonView.RPC("HealPlayer", RpcTarget.All, otherPlayer.photonView.ViewID);
            }
        }
    }

    [PunRPC]
    void HealPlayer(int playerId)
    {
        PhotonView target = PhotonView.Find(playerId);
        if (target != null && target.IsMine)
        {
            target.GetComponent<Player>().hp += 10;
            // Firebase 업데이트
            FirebaseManager.Instance.UpdatePlayerHP(target.Owner.UserId, target.GetComponent<Player>().hp);
        }
    }
}