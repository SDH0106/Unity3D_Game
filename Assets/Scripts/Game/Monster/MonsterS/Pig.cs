using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pig : Monster
{
    bool isRush = false;
    float rushTime = 2f;
    float breakTime = 0f;
    float distance;

    private void Start()
    {
        InitSetting();
    }

    protected override void SetInitMonster()
    {
        base.SetInitMonster();
        isRush = false;
        rushTime = 2f;
        breakTime = 0f;
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

        if (distance <= 3)
            isRush = true;

        if (isRush)
        {
            isAttack = true;
            rushTime -= Time.deltaTime;
            speed = stat.monsterSpeed * (1 + gameManager.monsterSlow * 0.01f) * (1 - gameManager.monsterSlow) * 2f;

            if (rushTime <= 0)
            {
                isAttack = false;
                speed = 0;
                breakTime += Time.deltaTime;

                if (breakTime >= 1)
                {
                    rushTime = 2f;
                    breakTime = 0f;
                    isRush = false;
                    speed = stat.monsterSpeed * (1 + gameManager.monsterSlow * 0.01f) * (1 - gameManager.monsterSlow);
                }
            }
        }
    }
}
