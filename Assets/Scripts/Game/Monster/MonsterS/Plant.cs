using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Platform;
using UnityEngine;

public class Plant : Monster
{
    [SerializeField] GameObject plantBullet;
    [SerializeField] Transform bulletPos;

    float attackTime = 3;

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
        attackTime = 3;
    }

    private void Update()
    {
        if (isDead == false)
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

        Instantiate(plantBullet, bulletPos.position, plantBullet.transform.rotation);
    }
}
