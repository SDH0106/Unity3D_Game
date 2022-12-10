using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static WeaponInfo;

public class Character : Singleton<Character>
{
    [SerializeField] SpriteRenderer rend;
    Animator anim;
    [SerializeField] ParticleSystem particle;

    [SerializeField] Slider playerHpBar;

    [Header("Stat")]
    [SerializeField] float invincibleTime;
    [HideInInspector] public float totalDamage;

    [HideInInspector] public float dashCoolTime;
    [HideInInspector] public float initDashCoolTime;
    [HideInInspector] public int dashCount;

    [Header("Weapon")]
    [SerializeField] public GameObject[] weapons;
    [SerializeField] public Transform[] weaponPoses;

    bool isRun, isAttacked, isDead = false;

    GameManager gameManager;

    Vector3 dir;
    float x;
    float z;

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

        dashCoolTime = 4;
        dashCount = gameManager.dashCount;
        initDashCoolTime = dashCoolTime;

    }

    void Update()
    {
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        playerHpBar.value = 1 - (gameManager.hp / gameManager.maxHp);

        if (gameManager.currentScene == "Game")
        {
            if (isDead == false)
            {
                Move();
                Dash();
            }

            anim.SetBool("isRun", isRun);
            OnDead();
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

                if (Input.GetKeyDown(KeyCode.Space))
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
        dir = (Vector3.right * x + Vector3.forward * z).normalized;
        transform.position += dir * gameManager.speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rend.flipX = true;
            isRun = true;
        }

        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rend.flipX = false;
            isRun = true;
        }

        else if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            isRun = true;
        }

        else
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

     public void OnDamaged(Collider other)
    {
        if (other.tag == "Monster" && isAttacked == false)
        {
            if (other.gameObject.GetComponent<Monster>().stat.monsterDamage - gameManager.defence > 0)
            {
                gameManager.hp -= (other.gameObject.GetComponent<Monster>().stat.monsterDamage - gameManager.defence);
                StartCoroutine(OnInvincible());
            }
        }

        else if(other.CompareTag("monsterBullet") && isAttacked == false)
        {
            if ((other.gameObject.GetComponent<MonsterBullet>().bulletDamage - gameManager.defence) > 0)
            {
                gameManager.hp -= (other.gameObject.GetComponent<MonsterBullet>().bulletDamage - gameManager.defence);
                StartCoroutine(OnInvincible());
            }
        }
    }

    void OnDead()
    {
        if (gameManager.hp <= 0)
        {
            gameManager.hp = 0;
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
    }

    public IEnumerator OnInvincible()
    {
        anim.SetTrigger("isAttacked");
        isAttacked = true;
        StartCoroutine(PlayerColorBlink());

        yield return new WaitForSeconds(invincibleTime);
        rend.color = Color.white;
        isAttacked = false;
    }
}
