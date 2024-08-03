using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Auth;

public class Car : MonoBehaviourPunCallbacks, IPunObservable
{
    private Slider HPBar;
    private TMP_Text HPText;
    private GameObject gameOverPanel;
    private TMP_Text gameOverTxt;
    private Button Break;
    private GameObject joyStick;

    public GameObject fire;
    public GameObject smoke;

    public float maxHP;
    public float curHP;
    public float curSpeed;
    float maxSpeed;

    int countdown = 10;
    private bool isGameOver = false;
    private bool countdownStarted = false;

    NetworkPlayer drive;
    Dash dash;
    FirestoreManager firestoreManager;

    private FirebaseAuth auth;
    private Rigidbody rb;

    private Queue<System.Action> actionQueue = new Queue<System.Action>();
    private bool isDataReady = false;

    private Coroutine countdownCoroutine;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        auth = FirebaseAuth.DefaultInstance;

        //drive = GameManager.Instance.drive;
        //dash = GameManager.Instance.dash;
        firestoreManager = GameManager.Instance.firestoreManager;

        HPBar = FindInActiveObjectByName("HPBar")?.GetComponent<Slider>();
        HPText = FindInActiveObjectByName("HPText")?.GetComponent<TMP_Text>();
        gameOverPanel = FindInActiveObjectByName("GameOverPanel");
        Break = FindInActiveObjectByName("BreakBtn")?.GetComponent<Button>();
        joyStick = FindInActiveObjectByName("Joystick");

        if (HPBar == null)
        {
            Debug.LogError("HPBar not found or inactive.");
        }
        else
        {
            Debug.Log("HPBar found and assigned");
        }

        if (gameOverPanel != null)
        {
            gameOverTxt = gameOverPanel.transform.Find("GameOverTxt")?.GetComponent<TMP_Text>();
        }

        actionQueue.Enqueue(() => {
            maxHP = firestoreManager.Hp;
            curHP = maxHP;
            SetMaxHealth(maxHP);
        });

        actionQueue.Enqueue(() => {
            maxSpeed = firestoreManager.MaxSpeed;
        });

        firestoreManager.OnDataReady += HandleDataReady;
    }

    private void HandleDataReady()
    {
        isDataReady = true;

        while (actionQueue.Count > 0)
        {
            actionQueue.Dequeue().Invoke();
        }
    }

    private GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>();
        foreach (Transform obj in objs)
        {
            if (obj.hideFlags == HideFlags.None && obj.name == name)
            {
                return obj.gameObject;
            }
        }
        return null;
    }

    public void SetMaxHealth(float health)
    {
        HPBar.maxValue = health;
        HPBar.value = health;
    }

    public void GetDamaged(float damage)
    {
        curHP -= (float)damage;
        PhotonView myPhotonView = GetComponent<PhotonView>();
        int myPhotonViewID = myPhotonView != null ? myPhotonView.ViewID : -1;

        if (HPBar != null)
        {
            HPBar.value = curHP;
        }
        else
        {
            HPBar = FindInActiveObjectByName("HPBar")?.GetComponent<Slider>();
            if (HPBar == null)
            {
                Debug.LogError("GetDamaged: Failed to reassign HPBar.");
            }
            else
            {
                HPBar.value = curHP;
            }
        }
        UpdateHPText();
    }

    private void UpdateHPText()
    {
        if (HPText != null)
        {
            HPText.text = string.Format("{0}/{1}", (int)curHP, (int)maxHP);
        }
        else
        {
            Debug.LogError("UpdateHPText: HPText is null");

            HPText = FindInActiveObjectByName("HPText")?.GetComponent<TMP_Text>();
            if (HPText == null)
            {
                Debug.LogError("UpdateHPText: Failed to reassign HPText");
            }
            else
            {
                Debug.Log("UpdateHPText: Successfully reassigned HPText");
                HPText.text = string.Format("{0}/{1}", (int)curHP, (int)maxHP);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Car"))
        {
            PhotonView myPhotonView = GetComponent<PhotonView>();
            PhotonView otherPhotonView = collision.transform.GetComponent<PhotonView>();

            int myPhotonViewID = myPhotonView != null ? myPhotonView.ViewID : -1;
            int otherPhotonViewID = otherPhotonView != null ? otherPhotonView.ViewID : -1;

            if (otherPhotonView != null && otherPhotonViewID != myPhotonViewID)
            {
                Debug.Log($"Collided with car. Other PhotonView ID: {otherPhotonViewID}, Damage: {curSpeed}, My PhotonView ID: {myPhotonViewID}");
                otherPhotonView.RPC("ReduceHP", RpcTarget.All, (double)(curSpeed * 3));
            }
        }
        
        else
        {
            PhotonView myPhotonView = GetComponent<PhotonView>();
            int myPhotonViewID = myPhotonView != null ? myPhotonView.ViewID : -1;
            Debug.Log($"Collided with a non-Car object. Damage: {curSpeed}, My PhotonView ID: {myPhotonViewID}");
            myPhotonView.RPC("ReduceHP", RpcTarget.All, (double)(curSpeed * 0.7));
        }
    }

    [PunRPC]
    public void ReduceHP(double damage)
    {
        GetDamaged((float)damage);
    }

    private void Update()
    {
        if (!isDataReady) return;

        HPBar.value = curHP;
        UpdateHPText();
        curSpeed = rb.velocity.magnitude;

        if (curSpeed >= maxSpeed)
        {
            curSpeed = maxSpeed;
        }

        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
        else if (curHP < 0)
        {
            curHP = 0;
        }

        if (curHP <= 0 && !isGameOver)
        {
            isGameOver = true;

            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }

            GameOver();
        }

        if (Mathf.Abs(transform.eulerAngles.z) > 80 && curSpeed < 0.03)
        {
            Vector3 newRotation = transform.eulerAngles;
            newRotation.z = 0;
            transform.eulerAngles = newRotation;
        }
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        curSpeed = 0;
        smoke.SetActive(true);
        gameOverPanel.SetActive(true);

        if (!countdownStarted)
        {
            countdownStarted = true;
            countdownCoroutine = StartCoroutine(RestartCountdown());
        }
    }

    private IEnumerator RestartCountdown()
    {
        Break.gameObject.SetActive(false);
        dash.dashButton.gameObject.SetActive(false);
        joyStick.gameObject.SetActive(false);

        while (countdown > 0)
        {
            gameOverTxt.text = $"Restart in {countdown} seconds";
            yield return new WaitForSeconds(1f);
            countdown--;
        }

        string userId = auth.CurrentUser?.UserId;

        if (userId != null)
        {
            /*databaseManager.GetUserTeam(userId, team =>
            {
                if (!string.IsNullOrEmpty(team))
                {
                    databaseManager.GetTeamRespawnArea(team, respawnArea =>
                    {
                        GameManager.Instance.RespawnUser(userId, respawnArea);

                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        transform.position = respawnArea;
                        transform.rotation = Quaternion.identity;
                        isGameOver = false;
                        countdownStarted = false;
                    });
                }
                else
                {
                    Debug.LogError("User team not found.");
                }
            });*/
        }
        else
        {
            Debug.LogError("User is not logged in.");
        }

        gameOverPanel.SetActive(false);
        Break.gameObject.SetActive(true);
        dash.dashButton.gameObject.SetActive(true);
        joyStick.gameObject.SetActive(true);
        countdown = 10;
        curHP = maxHP;

        countdownCoroutine = null;
        yield return null;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.IsWriting)
        {
            // 데이터 전송
            stream.SendNext(curHP);
            stream.SendNext(curSpeed);
        }
        else
        {
            // 데이터 수신
            curHP = (float)stream.ReceiveNext();
            curSpeed = (float)stream.ReceiveNext();
        }
    }
}
