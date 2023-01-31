using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkullBat : Monster
{
    [SerializeField] GameObject skullBullet;
    [SerializeField] Transform[] bulletPoses;
    [SerializeField] Slider monsterHpBar;

    float attackTime = 5;

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
                Move();

                if (isWalk)
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
                        gameManager.money += 800;
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
        yield return new WaitForSeconds(2f);

        isWalk = true;
    }

    int rand;

    public void Shoot()
    {
        rand = Random.Range(0, 2);

        for (int i = 0; i < bulletPoses.Length; i++)
        {
            MonsterBullet bullet = Instantiate(skullBullet, bulletPoses[i].position, skullBullet.transform.rotation).GetComponent<MonsterBullet>();
            bullet.monsPos = transform.position;
            bullet.randNum = rand;
        }
    }
}
