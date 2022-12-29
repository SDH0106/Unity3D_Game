using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class Ghost : Monster
{
    [SerializeField] Slider monsterHpBar;

    int state = 0;

    float attackTime = 5;
    float collTime = 0.5f;
    bool isRush;

    Vector3 rushDir;

    void Start()
    {
        InitSetting();
    }

    protected override void SetInitMonster()
    {
        base.SetInitMonster();
        state = 0;
        isRush = false;
        attackTime = 5;
        collTime = 0.5f;
    }

    void Update()
    {
        monsterHpBar.value = 1 - (hp / maxHp);

        if (!isDead && !isFreeze)
        {
            attackTime -= Time.deltaTime;

            if (!isRush)
                Move();
            
            else if (isRush)
            {
                transform.position += rushDir * 8 * Time.deltaTime;
                transform.position = gameManager.ground.ClosestPoint(transform.position);
            }

            if (attackTime <= 0)
            {
                int rand = Random.Range(0, 2);

                if (rand == 0)
                    StartCoroutine(Rush());

                else if (rand == 1)
                    Disappear();

                attackTime = 5;
            }

            Appear();
            anim.SetInteger("state", state);
        }

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

    void Disappear()
    {
        state = 1;
        coll.enabled = false;
    }

    void Appear()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Disappear"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                state = 2;
                transform.position = character.transform.position;
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Appear"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                state = 0;
            }
        }

        if (state == 0)
        {
            if (coll.enabled == false)
            {
                collTime -= Time.deltaTime;
            }

            if (collTime <= 0)
            {
                collTime = 0.5f;
                coll.enabled = true;
            }
        }
    }

    IEnumerator Rush()
    {
        speed = 0;
        rushDir = (character.transform.position - transform.position).normalized;
        rend.color = Color.red;
        yield return new WaitForSeconds(0.5f);

        isRush = true;
        yield return new WaitForSeconds(1f);

        isRush = false;
        rend.color = Color.white;
        speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f);
    }
}
