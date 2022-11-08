using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Monster : MonoBehaviour
{
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Animator anim;

    [Header("Stat")]
    [SerializeField] int hp = 10;
    [SerializeField] float speed;
    [SerializeField] int attackDamage;

    [Header("DamageText")]
    [SerializeField] GameObject damagePrefab;
    [SerializeField] Transform printPos;

    bool isRun, isDead = false;

    int maxHp;

    Vector3 InitScale;

    private IObjectPool<Monster> managedPool;

    //PrintDamage printDamage;

    public int AttackDamage => attackDamage;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
        InitScale = transform.localScale;
        anim = GetComponent<Animator>();
        //printDamage = GetComponent<PrintDamage>();
    }

    // Update is called once per frame
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
        transform.localScale = InitScale;
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

    void OnDamaged()
    {
        GameObject text = Instantiate(damagePrefab, printPos);

        hp -= Character.Instance.AttackDamage;
        anim.SetTrigger("isAttacked");

        StartCoroutine(MonsterColorBlink());
    }

    void OnDead()
    {
        if (hp <= 0)
        {
            isDead = true;

            anim.SetBool("isDead", isDead);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    DestroyMonster();
                }
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "myBullet" && isDead == false)
        {
            OnDamaged();
            other.gameObject.GetComponent<Bullet>().DestroyBullet();
            other.gameObject.GetComponent<Bullet>().CancleDestroyInvoke();
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
