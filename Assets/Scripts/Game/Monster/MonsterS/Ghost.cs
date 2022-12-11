using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Ghost : Monster
{
    int state = 0;

    float disappearTime = 7;

    void Start()
    {
       InitSetting();
    }

    protected override void SetInitMonster()
    {
        base.SetInitMonster();
        state = 0;
        disappearTime = 7;
    }

    void Update()
    {
        if (isDead == false && !isFreeze)
        {
            Move();
            anim.SetInteger("state", state);

            if (!isFreeze)
            {
                disappearTime -= Time.deltaTime;

                if (disappearTime <= 0)
                {
                    disappearTime = 7;
                    Disappear();
                }
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
        transform.position = character.transform.position;
    }
}
