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
        gameManager = GameManager.Instance;
        character = Character.Instance;
        hp = stat.monsterMaxHp * (1 + (float)((gameManager.round - 1) * 0.25)); ;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        initSpeed = speed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
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
            rushTime -= Time.deltaTime;
            speed = stat.monsterSpeed * 2f;
            isAttack = true;

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
                    speed = stat.monsterSpeed;
                }
            }
        }

    }
}
