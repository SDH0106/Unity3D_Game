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
    [SerializeField] Animator anim;
    [SerializeField] LayerMask coinLayer;

    [SerializeField] Slider playerHpBar;

    [Header("Stat")]
    [SerializeField] float invincibleTime;

    [Header("Weapon")]
    [SerializeField] public GameObject[] weapons;
    [SerializeField] public Transform[] weaponPoses;

    bool isRun, isAttacked, isDead = false;

    [HideInInspector] public float totalDamage;

    GameManager gameManager;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        anim = GetComponent<Animator>();

        transform.position = Vector3.zero;

        gameManager = GameManager.Instance;
    }

    void Update()
    {
        playerHpBar.value = 1 - (gameManager.hp / gameManager.maxHp);

        if (gameManager.currentScene == "Game")
        {
            if (isDead == false)
            {
                Move();
            }

            anim.SetBool("isRun", isRun);
            OnDead();
        }
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
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

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
            StartCoroutine(OnInvincible());
        }
    }

    void OnDead()
    {
        if (gameManager.hp <= 0)
        {
            SoundManager.Instance.PlayES("Death");
            gameManager.hp = 0;
            isDead = true;
            isAttacked = true;

            anim.SetBool("isDead", isDead);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDie"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Time.timeScale = 0;
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
