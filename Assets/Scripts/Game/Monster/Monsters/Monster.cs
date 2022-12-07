using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.PlayerSettings;

public class Monster : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer rend;
    [HideInInspector] public Animator anim;
    [HideInInspector] public Collider coll;

    [HideInInspector] public bool isWalk, isDead, isAttacked, isAttack = false;

    public float hp;

    [HideInInspector] public Vector3 initScale;
    [HideInInspector] public MonsterStat stat;

    private IObjectPool<Monster> managedPool;

    public float speed;

    [HideInInspector] public Vector3 dir;

    protected bool isFreeze = false;

    protected float initSpeed = 0;

    protected GameManager gameManager;
    protected Character character;

    void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        hp = stat.monsterMaxHp * (1 + ((gameManager.round - 1) * 0.25f));
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        initSpeed = speed;
        initSpeed = speed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
    }

    void Update()
    {
        if (isDead == false)
        {
            Move();
            anim.SetBool("isWalk", isWalk);
        }

        OnDead();
    }

    protected virtual void SetInitMonster()
    {
        hp = stat.monsterMaxHp * (1 + ((gameManager.round - 1) * 0.25f));
        speed = stat.monsterSpeed;
        isWalk = true;
        isDead = false;
        isAttacked = false;
        isAttack = false;
        coll.enabled = true;
        transform.localScale = initScale;
        initScale = transform.localScale;
        rend.color = Color.white;
    }

    public void Move()
    {
        Vector3 characterPos = character.transform.position;
        dir = characterPos - transform.position;

        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x,0,transform.position.z), characterPos, speed * Time.deltaTime);


        isWalk = true;

        if (dir == Vector3.zero || speed == 0)
            isWalk = false;

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

    private IEnumerator MonsterFreeze()
    {
        rend.color = Color.cyan;
        yield return new WaitForSeconds(2f);

        isFreeze = false;
        speed = initSpeed;
        rend.color = Color.white;
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            character.OnDamaged(coll);
        }
    }

    public void OnDamaged(float damage)
    {
        Debug.Log("Damaged");
        hp -= (damage - (stat.monsterDefence * (1 + gameManager.round * 0.1f)));

        StartCoroutine(MonsterColorBlink());
    }

    public void OnDamaged(float damage, bool freeze)
    {
        Debug.Log("Damaged");
        isFreeze = freeze;
        hp -= (damage - (stat.monsterDefence * (1 + gameManager.round * 0.1f)));

        if (isFreeze == true)
        {
            speed = 0;
            StartCoroutine(MonsterFreeze());
        }
    }

    public void OnDead()
    {
        if (hp <= 0 || gameManager.isClear || gameManager.hp <= 0)
        {
            coll.enabled = false;
            isDead = true;
            isAttacked = true;

            anim.SetBool("isAttacked", isAttacked);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    if (hp <= 0)
                    {
                        DropCoin.Instance.Drop(transform.position);
                        gameManager.exp += stat.monsterExp;
                    }
                    DestroyMonster();
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
        managedPool.Release(this);
        SetInitMonster();
    }
}
