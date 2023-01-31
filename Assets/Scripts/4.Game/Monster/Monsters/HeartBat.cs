using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Platform;
using UnityEngine;

public class HeartBat : Monster
{
    [SerializeField] GameObject plantBullet;
    [SerializeField] Transform bulletPos;

    float attackTime = 3;

    private void Start()
    {
       StartSetting();
    }

    protected override void InitMonsterSetting()
    {
        base.InitMonsterSetting();
        attackTime = 3;
    }

    private void Update()
    {
        if (!isDead)
        {
            Move();

            if (!isFreeze)
            {
                attackTime -= Time.deltaTime;

                if (attackTime <= 0)
                {
                    attackTime = 3;
                    Attack();
                }
            }

            if (isFreeze)
            {
                anim.speed = 0f;
            }
        }

        OnDead();
    }

    void Attack()
    {
        anim.SetTrigger("isAttack");
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {

        yield return new WaitForSeconds(0.3f);

        MonsterBullet bullet = Instantiate(plantBullet, bulletPos.position, plantBullet.transform.rotation).GetComponent<MonsterBullet>();
        bullet.randNum = 0;
    }
}
