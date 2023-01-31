using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    [HideInInspector] protected SpriteRenderer rend;
    [HideInInspector] protected Animator anim;
    [HideInInspector] protected Collider coll;

    [HideInInspector] public bool isWalk, isDead, isAttacked, isAttack = false;

    public float hp;
    protected float maxHp;

    [HideInInspector] protected Vector3 initScale;
    [HideInInspector] public MonsterStat stat;

    private IObjectPool<Monster> managedPool;

    protected float speed;

    [HideInInspector] public Vector3 dir;

    protected bool isFreeze = false;

    protected float initSpeed = 0;

    protected GameManager gameManager;
    protected Character character;

    IEnumerator runningCoroutine;

    public float defence;

    void Start()
    {
        StartSetting();
    }

    protected void StartSetting()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();

        hp = stat.monsterMaxHp * (1 + (Mathf.Floor(gameManager.round / 5) * 0.25f));
        maxHp = hp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f);
        initSpeed = speed;
        defence = stat.monsterDefence * (1 + Mathf.Floor(gameManager.round / 5) * 0.1f) * gameManager.monsterDef;
        isWalk = true;
        isDead = false;
        isAttacked = false;
        isAttack = false;
        coll.enabled = true;
    }

    void Update()
    {
        if (isDead == false)
        {
            Move();
            anim.SetBool("isWalk", isWalk);
        }

        if (isFreeze)
        {
            anim.speed = 0f;
        }

        OnDead();
    }

    protected virtual void InitMonsterSetting()
    {
        hp = stat.monsterMaxHp * (1 + (Mathf.Floor(gameManager.round / 5) * 0.25f));
        maxHp = hp;
        speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f);
        initSpeed = speed;
        isWalk = true;
        isDead = false;
        isAttacked = false;
        isAttack = false;
        coll.enabled = true;
        anim.speed = 1f;
        transform.localScale = initScale;
        initScale = transform.localScale;
        rend.color = Color.white;
    }

    public void Move()
    {
        if (!isFreeze)
        {
            anim.speed = 1f;

            Vector3 characterPos = character.transform.position;
            dir = characterPos - transform.position;

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

    protected IEnumerator MonsterColorBlink()
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

    protected IEnumerator MonsterFreeze()
    {
        rend.color = Color.cyan;
        yield return new WaitForSeconds(1.5f);

        isFreeze = false;
        speed = initSpeed;
        rend.color = Color.white;
    }
        

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            character.OnDamaged(coll, stat.monsterDamage);
        }
    }

    public void SubscriptionFee()
    {
        if (gameManager.subscriptionFee)
        {
            int feeRand = Random.Range(0, 100);

            if (feeRand < 3)
                gameManager.money += 38;
        }
    }

    // 방어력 없이 바로 깎이는 대미지
    public void PureOnDamaged(float damage)
    {
        hp -= damage;

        if (runningCoroutine != null)
            StopCoroutine(runningCoroutine);

        if (runningCoroutine != MonsterFreeze())
        {
            runningCoroutine = MonsterColorBlink();
            StartCoroutine(runningCoroutine);
        }
    }

    public void OnDamaged(float damage)
    {
        hp -= damage;

        if (!isFreeze)
        {
            if (runningCoroutine != null)
                StopCoroutine(runningCoroutine);

            runningCoroutine = MonsterColorBlink();
            StartCoroutine(runningCoroutine);
        }
    }

    public void OnDamaged(float damage, bool freeze)
    {
        if (!isFreeze)
            isFreeze = freeze;

        hp -= damage;

        if (isFreeze == true)
        {
            //isWalk = false;

            if (runningCoroutine != null)
                StopCoroutine(runningCoroutine);

            runningCoroutine = MonsterFreeze();
            StartCoroutine(runningCoroutine);
        }

        if (isFreeze == false)
        {
            if (runningCoroutine != MonsterFreeze())
            {
                runningCoroutine = MonsterColorBlink();
                StartCoroutine(runningCoroutine);
            }
        }

    }

    public void OnDead()
    {
        if (hp <= 0 || (gameManager.isClear && gameManager.isBossDead) || character.isDead)
        {
            anim.speed = 1f;
            isDead = true;
            rend.color = Color.white;
            isFreeze = false;
            StopCoroutine(MonsterFreeze());
            coll.enabled = false;

            if (hp <= 0 && !isAttacked)
            {
                SubscriptionFee();
                int coinValue = Random.Range(stat.monsterCoin - 2, stat.monsterCoin);
                DropCoin.Instance.Drop(transform.position, coinValue);
                character.exp += stat.monsterExp * (1 + gameManager.increaseExp);
            }

            isAttacked = true;

            anim.SetBool("isAttacked", isAttacked);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
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
        InitMonsterSetting();
    }
}
