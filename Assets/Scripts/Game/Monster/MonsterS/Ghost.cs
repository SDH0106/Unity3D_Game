using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Monster
{
    int state = 0;

    void Start()
    {
        hp = stat.monsterMaxHp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        InvokeRepeating("Disappear", 3f, 3f);
    }

    void Update()
    {
        if (isDead == false)
        {
            Move();
            anim.SetInteger("state", state);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Disappear"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                coll.enabled = false;
                Appear();
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Appear"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                state = 0;
                coll.enabled = true;
            }
        }

        BossDead();
    }

    public void BossDead()
    {
        if (hp <= 0 || GameManager.Instance.hp <= 0)
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
                        GameManager.Instance.isClear = true;
                        GameManager.Instance.money += 100;
                        GameManager.Instance.level++;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }


    void Disappear()
    {
        state = 1;
    }

    void Appear()
    {
        state = 2;
        transform.position = Character.Instance.transform.position;
    }
}
