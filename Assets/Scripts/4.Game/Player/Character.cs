using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum CHARACTER_NUM
{
    Bagic,
    Legendary,
    Count,
}

public class Character : Singleton<Character>
{
    [SerializeField] public SpriteRenderer rend;
    [SerializeField] ParticleSystem particle;
    [SerializeField] GameObject gardianAngel;
    [SerializeField] GameObject gardianEffect;

    [SerializeField] Slider playerHpBar;

    [Header("Stat")]
    [SerializeField] public float characterNum;
    public int level;
    [SerializeField] float characterHp;
    [SerializeField] public float maxExp;
    [SerializeField] public float damageRatio;
    [SerializeField] float characterSpeed;
    [SerializeField] float invincibleTime;

    [HideInInspector] public float dashCoolTime;
    [HideInInspector] public float initDashCoolTime;
    [HideInInspector] public int dashCount;

    [Header("Weapon")]
    [SerializeField] public GameObject[] weapons;
    [SerializeField] public Transform[] weaponPoses;

    [HideInInspector] public float exp;
    [HideInInspector] public float maxHp;
    [HideInInspector] public float currentHp;
    [HideInInspector] public float speed;

    Animator anim;
    Collider ground;

    bool isRun, isAttacked = false;
    [HideInInspector] public bool isDead = false;

    GameManager gameManager;

    Vector3 dir;
    public float x;
    public float z;

    float recoverTime;

    [HideInInspector] public int levelUpCount;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        particle.GetComponentInChildren<Renderer>().enabled = false;

        gameManager = GameManager.Instance;
        transform.position = Vector3.zero;

        gardianAngel.SetActive(false);
        gardianEffect.SetActive(false);

        characterHp = 10;
        maxHp = characterHp + gameManager.maxHp;
        currentHp = maxHp;
        speed = gameManager.speed + characterSpeed;
        maxExp = 10;
        level = 1;
        levelUpCount = 0;
        recoverTime = 2;
        dashCoolTime = 4;
        dashCount = gameManager.dashCount;
        initDashCoolTime = dashCoolTime;
    }

    void Update()
    {
        bool xInput = (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left"))) || (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")));
        bool zInput = (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Up"))) || (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")));

        if (!xInput)
            x = 0;

        if (!zInput)
            z = 0;

        if (zInput)
        {
            if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Up")))
                z = 1;

            else if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")))
                z = -1;
        }

        if (xInput)
        {
            if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left")))
                x = -1;

            else if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")))
                x = 1;
        }

        /*x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");*/

        maxHp = characterHp + gameManager.maxHp;
        speed = gameManager.speed + characterSpeed;

        if (exp >= maxExp)
        {
            SoundManager.Instance.PlayES("LevelUp");
            level++;
            levelUpCount++;
            exp = exp - maxExp;
            maxExp = 10 * level;
        }

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        playerHpBar.value = 1 - (currentHp / maxHp);

        if (gameManager.revive)
            gardianAngel.SetActive(true);

        if (gameManager.currentScene == "Game")
        {
            isRun = false;

            if (currentHp > 0 && (!gameManager.isClear || !gameManager.isBossDead))
            {
                Move();
                Dash();
                AutoRecoverHp();
            }

            anim.SetBool("isRun", isRun);
        }
    }

    void AutoRecoverHp()
    {
        if (gameManager.recoverHp > 0 && currentHp < maxHp)
        {
            recoverTime -= Time.deltaTime;
            if (recoverTime <= 0)
            {
                recoverTime = 2;
                currentHp += gameManager.recoverHp;
            }
        }
    }

    void Dash()
    {
        if (gameManager.dashCount > 0)
        {
            Vector3 beforePos;
            Vector3 afterPos;

            if (dashCount > 0)
            {
                if (rend.flipX == true)
                {
                    particle.transform.localScale = new Vector3(-1, 1, 1);
                }

                else if (rend.flipX == false)
                {
                    particle.transform.localScale = new Vector3(1, 1, 1);
                }

                if (Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt("Key_Dash")))
                {
                    particle.GetComponentInChildren<Renderer>().enabled = true;

                    beforePos = transform.position;

                    if (x == 0 && z == 0)
                        afterPos = new Vector3(transform.position.x + 2, 0, 0);
                    else
                        afterPos = transform.position + new Vector3(x, 0, z) * 4;

                    transform.position = Vector3.Lerp(beforePos, afterPos, 1);
                    dashCount--;
                    Invoke("ParticleOff", 0.4f);
                }
            }

            if (dashCount != gameManager.dashCount)
            {
                dashCoolTime -= Time.deltaTime;

                if (dashCoolTime <= 0)
                {
                    dashCount++;
                    dashCoolTime = initDashCoolTime;
                }
            }
        }
    }

    void ParticleOff()
    {
        particle.GetComponentInChildren<Renderer>().enabled = false;
    }

    public void Equip()
    {
        int count = ItemManager.Instance.weaponCount;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].GetComponent<Weapon>().weaponInfo.WeaponName == ItemManager.Instance.storedWeapon[count].WeaponName)
            {
               Instantiate(weapons[i], weaponPoses[count]);
            }
        }
    }

    public void ReleaseEquip(int num)
    {
        Destroy(weaponPoses[num].GetChild(0).gameObject);
    }

    void Move()
    {
        if (ground == null)
            ground = GameSceneUI.Instance.ground;

        dir = (Vector3.right * x + Vector3.forward * z).normalized;

        if (speed <= 1)
            transform.position += dir * Time.deltaTime;

        else if (speed > 1)
            transform.position += dir * speed * Time.deltaTime;

        transform.position = ground.bounds.ClosestPoint(transform.position);

        if (dir != Vector3.zero)
        {
            isRun = true;
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left")))
            {
                rend.flipX = true;
            }

            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")))
            {
                rend.flipX = false;
            }
        }

        else if(dir == Vector3.zero)
            isRun = false;
    }

    private IEnumerator PlayerColorBlink()
    {
        Color red = new Color(1, 0, 0, 0.5f);
        Color white = new Color(1, 1, 1, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            rend.color = red;
            yield return new WaitForSeconds(0.1f);

            rend.color = white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void OnDamaged(Collider other, float damage)
    {
        if (!isAttacked)
        {
            currentHp -= Mathf.Round((damage - gameManager.defence) * 10) / 10;

            if (currentHp > 0)
                StartCoroutine(OnInvincible());

            else if (currentHp <= 0)
                OnDead();
        }
    }

    void OnDead()
    {
        if (!gameManager.revive)
        {
            currentHp = 0;
            isDead = true;
            isAttacked = true;

            anim.SetBool("isDead", isDead);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDie"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        else if (gameManager.revive)
        {
            isAttacked = true;
            isRun = false;
            StartCoroutine(OnRevive());
        }
    }

    public IEnumerator OnInvincible()
    {
        anim.SetTrigger("isAttacked");
        isAttacked = true;
        if (currentHp > 0)
            StartCoroutine(PlayerColorBlink());

        yield return new WaitForSeconds(invincibleTime);
        rend.color = Color.white;
        isAttacked = false;
    }

    IEnumerator OnRevive()
    {
        rend.color = new Color(1, 1, 1, 0.5f);
        gardianEffect.SetActive(true);

        yield return new WaitForSeconds(2.5f);
        gameManager.passiveBoolVariables[4] = false;    // gameManager.revive = false
        currentHp = Mathf.Ceil(maxHp * 0.5f);

        yield return new WaitForSeconds(1.5f);
        rend.color = Color.white;
        isAttacked = false;
        gardianAngel.SetActive(false);
    }
}
