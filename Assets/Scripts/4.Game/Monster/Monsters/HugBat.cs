using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HugBat : Monster
{
    bool isRush = false;
    float rushTime;
    float breakTime;
    float attackCoolTime;
    float distance;

    private void Start()
    {
        rushTime = 2f;
        breakTime = 1f;
        attackCoolTime = 1f;
        InitSetting();
    }

    protected override void SetInitMonster()
    {
        base.SetInitMonster();
        isRush = false;
        rushTime = 2f;
        breakTime = 0f;
        attackCoolTime = 1f;
    }

    private void Update()
    {
        if (isDead == false)
        {
            Move();
            anim.SetBool("isWalk", isWalk);

            if (!isFreeze)
                Attack();
        }

        OnDead();
    }

    void Attack()
    {
        distance = Vector3.Magnitude(character.transform.position - transform.position);
        anim.SetBool("isAttack", isAttack);

        if (distance <= 3f && !isRush)
            isRush = true;

        if (isRush)
        {
            if (attackCoolTime > 0f)
            {
                isAttack = true;
                rushTime -= Time.deltaTime;
                speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f) * 2f;

                if (rushTime <= 0f)
                {
                    isAttack = false;
                    speed = 0f;
                    breakTime -= Time.deltaTime;
                }

                if (breakTime <= 0f)
                {
                    speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f);

                    attackCoolTime -= Time.deltaTime;
                }
            }

            if (attackCoolTime <= 0f)
            {
                rushTime = 2f;
                breakTime = 1f;
                attackCoolTime = 1f;
                isRush = false;
            }
        }
    }
}