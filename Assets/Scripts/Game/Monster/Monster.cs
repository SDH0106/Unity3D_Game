using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

public class Monster : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Animator anim;
    [SerializeField] Collider coll;
    [SerializeField] Transform printPos;

    [Header("Stat")]
    [SerializeField] int hp = 10;
    [SerializeField] float speed;
    [SerializeField] int attackDamage;

    bool isRun, isDead, isAttacked = false;

    int maxHp;
    float exp = 2;

    Vector3 initScale;

    private IObjectPool<Monster> managedPool;

    PrintDamage printDamage;
    DropCoin coin;

    public int AttackDamage => attackDamage;
    public int Hp => hp;
    public bool IsDead => isDead;

    void Start()
    {
        maxHp = hp;
        initScale = transform.localScale;
        anim = GetComponent<Animator>();
        printDamage = GetComponent<PrintDamage>();
        coin = GetComponent<DropCoin>();
    }

    void Update()
    {
        if (isDead == false)
        {
            Move();
            anim.SetBool("isRun", isRun);
        }

        OnDead();
    }

    void SetInitMonster()
    {
        hp = maxHp;
        isRun = false;
        isDead = false;
        isAttacked = false;
        coll.enabled = true;
        transform.localScale = initScale;
        rend.color = Color.white;
        hp = maxHp;
    }

    void Move()
    {
        Vector3 characterPos = Character.Instance.transform.position;
        Vector3 dir = characterPos - transform.position;

        transform.position = Vector3.MoveTowards(transform.position, characterPos, speed * Time.deltaTime);

        isRun = true;

        if (dir == Vector3.zero)
            isRun = false;

        if (dir.x < 0)
            rend.flipX = true;

        else if (dir.x >= 0)
            rend.flipX = false;
    }

    private IEnumerator MonsterColorBlink()
    {
        Color semiWhite = new Color(1, 1, 1, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            rend.color = Color.white;
            yield return new WaitForSeconds(0.1f);

            rend.color = semiWhite;
            yield return new WaitForSeconds(0.1f);
        }

        rend.color = Color.white;
    }

    public void OnDamaged()
    {
        printDamage.PrintDamageText(printPos.position);

        hp -= Character.Instance.AttackDamage;

        //anim.SetTrigger("isAttacked");

        StartCoroutine(MonsterColorBlink());
    }

    void OnDead()
    {
        if (hp <= 0)
        {
            Vector3 deadPos = transform.position;

            //anim.SetTrigger("isAttacked");
            anim.SetBool("isAttacked", isAttacked);

            isAttacked = true;
            isDead = true;

            coll.enabled = false;

            //anim.SetBool("isDead", isDead);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    DestroyMonster();
                    coin.Drop(deadPos);
                }
            }
        }
    }

    public void SetManagedPool(IObjectPool<Monster> pool)
    {
        managedPool = pool;
    }

    public void DestroyMonster()
    {
        Character.Instance.exp += exp;
        managedPool.Release(this);
        SetInitMonster();
    }
}
