using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class SkullBat : Monster
{
    [SerializeField] GameObject skullBullet;
    [SerializeField] Transform[] bulletPoses;
    [SerializeField] Slider monsterHpBar;

    float attackTime = 5;

    private IObjectPool<MonsterBullet> pool;

    private void Awake()
    {
        pool = new ObjectPool<MonsterBullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: 6);
    }

    void Start()
    {
        StartSetting();
    }

    protected override void InitMonsterSetting()
    {
        base.InitMonsterSetting();
        attackTime = 5;
    }

    void Update()
    {
        monsterHpBar.value = 1 - (hp / maxHp);

        if (isDead == false && !isAttack)
        {
            if (!isFreeze)
            {
                if (isWalk)
                {
                    Move();
                }

                attackTime -= Time.deltaTime;

                if (attackTime <= 0)
                {
                    attackTime = 5;
                    Attack();
                }
            }

            if (isFreeze)
            {
                anim.speed = 0f;
            }
        }

        anim.SetBool("isWalk", isWalk);
        anim.SetBool("isAttack", isAttack);

        BossDead();
    }

    void BossDead()
    {
        if (hp <= 0 || character.isDead)
        {
            anim.speed = 1f;
            rend.color = Color.white;
            isFreeze = false;
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
                        gameManager.money += 500;
                        SoundManager.Instance.PlayES("LevelUp");
                        character.level++;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    void Attack()
    {
        isWalk = false;
        isAttack = true;
    }

    public void EndAttack()
    {
        if (rand == 0)
        {
            isAttack = false;
            isWalk = true;
        }

        else if(rand == 1)
        {
            isAttack = false;
            StartCoroutine(AttackDelay());
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(2.5f);
        isWalk = true;
    }

    int rand;

    public void Shoot()
    {
        rand = Random.Range(0, 2);

        for (int i = 0; i < bulletPoses.Length; i++)
        {
            MonsterBullet bullet = pool.Get();
            bullet.transform.position = new Vector3(bulletPoses[i].position.x, 0, bulletPoses[i].position.z);
            bullet.monsPos = transform.position;
            bullet.randNum = rand;
            bullet.GetComponent<SkullBullet>().ShootDir();
            bullet.GetComponent<SkullBullet>().AutoDestroyBullet();
        }
    }

    private MonsterBullet CreateBullet()
    {
        MonsterBullet bullet = Instantiate(skullBullet, bulletPoses[0].position, transform.rotation).GetComponent<MonsterBullet>();
        bullet.SetManagedPool(pool);
        return bullet;
    }

    private void OnGetBullet(MonsterBullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(MonsterBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(MonsterBullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void OnDestroy()
    {
        pool.Clear();
    }
}
