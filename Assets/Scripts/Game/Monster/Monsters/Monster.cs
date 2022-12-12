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
        InitSetting();
    }

    protected void InitSetting()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        hp = stat.monsterMaxHp * (1 + ((gameManager.round - 1) * 0.25f));
        initScale = transform.localScale;
        speed = stat.monsterSpeed * (1 + gameManager.monsterSpeed);
        initSpeed = speed;
        isWalk = true;
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
        speed = stat.monsterSpeed * (1 + gameManager.monsterSpeed);
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
        if (!isFreeze)
        {
            Vector3 characterPos = character.transform.position;
            dir = characterPos - transform.position;

            speed = stat.monsterSpeed * (1 + gameManager.monsterSpeed);

            transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0, transform.position.z), characterPos, speed * Time.deltaTime);

            isWalk = true;

            if (dir == Vector3.zero || speed == 0)
                isWalk = false;

            if (dir.x < 0)
                rend.flipX = true;

            else if (dir.x >= 0)
                rend.flipX = false;
        }
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
        yield return new WaitForSeconds(3f);

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
        hp -= (damage - (stat.monsterDefence * (1 + gameManager.round * 0.1f)));

        StartCoroutine(MonsterColorBlink());
    }

    public void OnDamaged(float damage, bool freeze)
    {
        isFreeze = freeze;
        hp -= (damage - (stat.monsterDefence * (1 + gameManager.round * 0.1f)));

        if (isFreeze == true)
        {
            isWalk = false;
            StartCoroutine(MonsterFreeze());
        }
    }

    public void OnDead()
    {
        if (hp <= 0 || gameManager.isClear || character.isDead)
        {
            rend.color = Color.white;
            isFreeze = false;
            StopCoroutine(MonsterFreeze());
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
                        int coinValue = Random.Range(stat.monsterCoin - 5, stat.monsterCoin + 1);
                        DropCoin.Instance.Drop(transform.position, coinValue);
                        gameManager.exp += stat.monsterExp * (1 + gameManager.increaseExp);
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
