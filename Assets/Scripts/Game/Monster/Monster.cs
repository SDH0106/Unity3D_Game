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
    [SerializeField] float hp = 10;
    [SerializeField] float speed;
    [SerializeField] int attackDamage;

    bool isRun, isDead, isAttacked = false;

    float maxHp;
    int exp = 2;

    Vector3 initScale;

    Collider myCollider;

    private IObjectPool<Monster> managedPool;

    DropCoin coin;

    public int AttackDamage => attackDamage;

    void Start()
    {
        maxHp = hp;
        initScale = transform.localScale;
        anim = GetComponent<Animator>();
        coin = GetComponent<DropCoin>();
        myCollider = GetComponent<Collider>();
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            Character.Instance.OnDamaged(myCollider);
        }
    }

    public void OnDamaged(float damage)
    {
        hp -= damage;

        StartCoroutine(MonsterColorBlink());
    }

    void OnDead()
    {
        if (hp <= 0 || GameManager.Instance.currentGameTime <= 0)
        {
            Vector3 deadPos = transform.position;

            anim.SetBool("isAttacked", isAttacked);

            isAttacked = true;
            isDead = true;

            coll.enabled = false;

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    if (hp <= 0)
                    {
                        coin.Drop(deadPos);
                        GameManager.Instance.exp += exp;
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
