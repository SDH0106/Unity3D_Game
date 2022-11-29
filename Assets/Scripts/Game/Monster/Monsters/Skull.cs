using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : Monster
{
    [SerializeField] GameObject skullBullet;
    [SerializeField] Transform[] bulletPoses;

    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        hp = stat.monsterMaxHp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        StartCoroutine(Attack());
    }

    void Update()
    {
        if (isDead == false && !isAttack)
        {
            Move();
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
        if (hp <= 0 || gameManager.hp <= 0)
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
                        gameManager.money += 100;
                        gameManager.level++;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    IEnumerator Attack()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(5f);
            isAttack = true;

            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < bulletPoses.Length; i++)
            {
                Instantiate(skullBullet, bulletPoses[i].position, skullBullet.transform.rotation);
            }
        }
    }
}
