using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Monster
{
    [SerializeField] GameObject skullBullet;
    [SerializeField] Transform[] bulletPoses;

    float attackTime = 5;

    void Start()
    {
        InitSetting();
    }

    protected override void SetInitMonster()
    {
        base.SetInitMonster();
        attackTime = 5;
    }

    void Update()
    {
        if (isDead == false && !isAttack)
        {
            Move();

            if (!isFreeze)
            {
                attackTime -= Time.deltaTime;

                if (attackTime <= 0)
                {
                    attackTime = 5;
                    Attack();
                }
            }
        }


        if (anim.GetCurrentAnimatorStateInfo(0).IsName("EndAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isAttack = false;
            }
        }

        anim.SetBool("isAttack", isAttack);

        BossDead();
    }

    void BossDead()
    {
        if (hp <= 0 || character.isDead)
        {
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
                        gameManager.level++;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    void Attack()
    {
        isAttack = true;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        int rand = Random.Range(0, 2);
        yield return new WaitForSeconds(0.5f);

        for (int i = 0; i < bulletPoses.Length; i++)
        {
            MonsterBullet bullet = Instantiate(skullBullet, bulletPoses[i].position, skullBullet.transform.rotation).GetComponent<MonsterBullet>();
            bullet.monsPos = transform.position;
            bullet.randNum = rand;
        }
    }
}
