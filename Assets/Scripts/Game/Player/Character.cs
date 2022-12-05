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

    [Header("Weapon")]
    [SerializeField] public GameObject[] weapons;
    [SerializeField] public Transform[] weaponPoses;

    bool isRun, isAttacked, isDead = false;

    [HideInInspector] public float totalDamage;

    GameManager gameManager;

    public float dashCoolTime = 2;
    public float initDashCoolTime;

    public bool isDash = true;

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

        transform.position = Vector3.zero;
        initDashCoolTime = dashCoolTime;

        gameManager = GameManager.Instance;
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
        Vector3 beforePos;
        Vector3 afterPos;

        if (isDash)
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
                isDash = false;
                Invoke("particleOff", 0.4f);
            }
        }

        else if(!isDash)
        {
            dashCoolTime -= Time.deltaTime;

            if(dashCoolTime <=0)
            {
                isDash = true;
                dashCoolTime = initDashCoolTime;
            }
        }    
    }

    void particleOff()
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
        transform.position += new Vector3(x, 0, z) * gameManager.speed * Time.deltaTime;

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
            gameManager.hp -= (int)(other.gameObject.GetComponent<Monster>().stat.monsterDamage / gameManager.defence);
            if ((other.gameObject.GetComponent<Monster>().stat.monsterDamage / gameManager.defence) != 0)
                StartCoroutine(OnInvincible());
        }

        else if(other.CompareTag("monsterBullet") && isAttacked == false)
        {
            gameManager.hp -= (int)(other.gameObject.GetComponent<MonsterBullet>().bulletDamage / gameManager.defence);
            if ((other.gameObject.GetComponent<Monster>().stat.monsterDamage / gameManager.defence) != 0)
                StartCoroutine(OnInvincible());
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
