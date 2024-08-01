using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Auth;

public class Car : MonoBehaviour
{
    private Slider HPBar;
    private TMP_Text HPText;
    private GameObject gameOverPanel;
    private TMP_Text gameOverTxt;
    private Button Skill;
    private Button Break;
    private GameObject joyStick;

    public GameObject fire;   // �ҹ汸 ��
    public GameObject smoke;  // ���� ���� ��

    // Hp, Speed
    float maxHP;
    public float curHP;
    public float curSpeed;
    float maxSpeed;

    // GameOver
    int countdown = 10;  // ���� ���� �� �����
    private bool isGameOver = false;
    private bool countdownStarted = false;

    // Script
    DatabaseManager databaseManager;
    Drive drive;
    Dash dash;

    private FirebaseAuth auth;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        auth = FirebaseAuth.DefaultInstance;

        // Script
        databaseManager = GameManager.Instance.databaseManager;
        drive = GameManager.Instance.drive;
        dash = GameManager.Instance.dash;

        // Find and assign UI components
        HPBar = FindInActiveObjectByName("HPBar")?.GetComponent<Slider>();
        if (HPBar != null) Debug.Log("HPBar found and assigned.");
        else Debug.LogError("HPBar not found!");

        HPText = FindInActiveObjectByName("HPText")?.GetComponent<TMP_Text>();
        if (HPText != null) Debug.Log("HPText found and assigned.");
        else Debug.LogError("HPText not found!");

        gameOverPanel = FindInActiveObjectByName("GameOverPanel");
        if (gameOverPanel != null)
        {
            Debug.Log("gameOverPanel found and assigned.");
            gameOverTxt = gameOverPanel.transform.Find("GameOverTxt")?.GetComponent<TMP_Text>();
            if (gameOverTxt != null) Debug.Log("gameOverTxt found and assigned.");
            else Debug.LogError("gameOverTxt not found!");
        }
        else Debug.LogError("gameOverPanel not found!");

        Skill = FindInActiveObjectByName("SkillBtn")?.GetComponent<Button>();
        if (Skill != null) Debug.Log("Skill found and assigned.");
        else Debug.LogError("Skill not found!");

        Break = FindInActiveObjectByName("BreakBtn")?.GetComponent<Button>();
        if (Break != null) Debug.Log("Break found and assigned.");
        else Debug.LogError("Break not found!");

        joyStick = FindInActiveObjectByName("Joystick");
        if (joyStick != null) Debug.Log("Joystick found and assigned.");
        else Debug.LogError("Joystick not found!");

        // HP Management
        maxHP = GameManager.Instance.CarInfo.maxHp;
        curHP = maxHP;
        SetMaxHealth(maxHP);

        maxSpeed = GameManager.Instance.CarInfo.maxSpeed;
    }

    // Method to find inactive GameObject by name
    private GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None && objs[i].name == name)
            {
                return objs[i].gameObject;
            }
        }
        return null;
    }

    // HP Initial Setting
    public void SetMaxHealth(float health)
    {
        HPBar.maxValue = health;
        HPBar.value = health;
    }

    // Damage control
    public void GetDamaged(float damage)
    {
        float getDamagedHP = curHP - damage;
        curHP = getDamagedHP;
        HPBar.value = curHP;
    }

    // HP Text Management
    private void UpdateHPText()
    {
        HPText.text = string.Format("{0}/{1}", (int)curHP, (int)maxHP);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Car otherCar = collision.transform.GetComponent<Car>();  // ��� ��

        // Handle collision with another car
        bool thisCarIsDashing = GameManager.Instance.dash.isDash; // �� ���� ��� ����

        if (thisCarIsDashing)  // �� ���� �뽬 ���¶��
        {
            Debug.Log("dash");
            return;   // ������ ����
        }
        else
        {
            // ���� �浹�� ������
            if (otherCar != null && collision.transform.CompareTag("Car"))
            {
                Debug.Log("car accident");
                GetDamaged(collision.relativeVelocity.magnitude * 3);   // ��� ���� �ӷ¸�ŭ ������
            }
            else
            {
                Debug.Log("non car accident");
                // ���� �ƴ� ��ü�� ���ؼ� ������ �ޱ�
                GetDamaged(curSpeed * 3);
            }
        }
    }

    private void Update()
    {
        //HPBar.value = curHP;
        UpdateHPText();
        curSpeed = rb.velocity.magnitude;

        // Max Speed
        if (curSpeed >= maxSpeed)
        {
            curSpeed = maxSpeed;
        }

        // Max Hp
        if (curHP >= maxHP)
        {
            curHP = maxHP;
        }
        else if (curHP < 0)
        {
            curHP = 0;
        }

        // Game Over
        if (curHP <= 0 && !isGameOver)
        {
            Debug.Log("Game Over");
            isGameOver = true;
            GameOver();
        }

        // Reset Rotation
        if (Mathf.Abs(transform.eulerAngles.z) > 80 && curSpeed < 0.03)
        {
            // Reset the z rotation to 0
            Vector3 newRotation = transform.eulerAngles;
            newRotation.z = 0;
            transform.eulerAngles = newRotation;
        }
    }

    private void GameOver()
    {
        curSpeed = 0;
        smoke.SetActive(true);
        gameOverPanel.SetActive(true);
        if (!countdownStarted)
        {
            countdownStarted = true;
            StartCoroutine(RestartCountdown());
        }
    }

    private IEnumerator RestartCountdown()
    {
        Skill.gameObject.SetActive(false);
        Break.gameObject.SetActive(false);
        dash.dashButton.gameObject.SetActive(false);
        joyStick.gameObject.SetActive(false);

        while (countdown > 0)
        {
            gameOverTxt.text = $"Restart in {countdown} seconds";
            yield return new WaitForSeconds(1f);  // Wait for 1 second
            countdown--;
        }

        // Game Over Panel off
        gameOverPanel.SetActive(false);

        string userId = auth.CurrentUser?.UserId;

        if (userId != null)
        {
            // ������ ���� �ҷ��ͼ� ������ ó��
            databaseManager.GetUserTeam(userId, team =>
            {
                if (!string.IsNullOrEmpty(team))
                {
                    databaseManager.GetTeamRespawnArea(team, respawnArea =>
                    {
                        GameManager.Instance.RespawnUser(userId, respawnArea);

                        // �ӵ� �� ȸ�� �ӵ� �ʱ�ȭ
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;

                        // ��ġ �� ȸ�� �ʱ�ȭ
                        transform.position = respawnArea;
                        transform.rotation = Quaternion.identity;

                        curHP = maxHP;
                        isGameOver = false;
                        countdownStarted = false;
                    });
                }
                else
                {
                    Debug.LogError("User team not found.");
                }
            });
        }
        else
        {
            Debug.LogError("User is not logged in.");
        }

        Skill.gameObject.SetActive(true);
        Break.gameObject.SetActive(true);
        dash.dashButton.gameObject.SetActive(true);
        joyStick.gameObject.SetActive(true);

        yield return null;
    }
}
