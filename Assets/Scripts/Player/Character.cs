using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Singleton<Character>
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Animator anim;

    [Header("Stat")]
    [SerializeField] float speed;
    [SerializeField] float invincibleTime;
    [SerializeField] public int hp = 10;
    [SerializeField] int attackDamage = 1;

    bool isRun, isAttacked, isDead = false;

    [HideInInspector]
    public int maxHp;

    public int AttackDamage => attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            Move();
        }

        anim.SetBool("isRun", isRun);
    }

    void ReleaseInvincible()
    {
        isAttacked = false;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += new Vector3(x, 0, z) * speed * Time.deltaTime;

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

        else if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
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

    void Ondamaged(Collider other)
    {
        if (other.tag == "Monster" && isAttacked == false)
        {
            hp -= other.gameObject.GetComponent<Monster>().AttackDamage;
            StartCoroutine(OnInvincible());
        }

        if (hp <= 0)
        {
            hp = 0;
            isDead = true;

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

    private void OnTriggerStay(Collider other)
    {
        Ondamaged(other);
    }

    IEnumerator OnInvincible()
    {
        anim.SetTrigger("isAttacked");
        isAttacked = true;
        StartCoroutine(PlayerColorBlink());

        yield return new WaitForSeconds(invincibleTime);
        rend.color = Color.white;
        isAttacked = false;
    }
}
