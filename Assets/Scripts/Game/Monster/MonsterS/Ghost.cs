using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Monster
{
    int state = 0;
    GameManager gameManager;

    float disappearTime = 7;

    void Start()
    {
        gameManager = GameManager.Instance;
        hp = stat.monsterMaxHp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        //InvokeRepeating("Disappear", 5f, 7f);
    }

    void Update()
    {
        if (isDead == false && !isFreeze)
        {
            Move();
            anim.SetInteger("state", state);

            disappearTime -= Time.deltaTime;

            if(disappearTime <= 0)
            {
                disappearTime = 7;
                Disappear();
            }

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
