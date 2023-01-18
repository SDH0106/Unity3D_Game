using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wakgood : Summons
{
    [SerializeField] DamageUI damageUIPreFab;

    float angle;
    float damage;

    Collider monster;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitSetting();
        damage = gameManager.shortDamage * 2;
    }

    private void Update()
    {
        if (!isAttack)
        {
            CheckDistance();

            if (isNear)
                transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

            else if (!isNear)
                transform.position = Vector3.MoveTowards(transform.position, character.transform.position, character.speed * 2 * Time.deltaTime);
        }

        anim.SetBool("isAttack", isAttack);
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (canAttack)
        {
            if (other.CompareTag("Monster"))
            {
                monster = other;
                isAttack = true;
                Vector3 dir = other.gameObject.transform.position - transform.position;

                if (dir.x < 0)
                    rend.flipX = true;

                else if (dir.x >= 0)
                    rend.flipX = false;

                CancelInvoke("GetRandomPos");

                speed = 0;
                canAttack = false;
            }
        }
    }

    public override void EndAttack()
    {
        base.EndAttack();
        DamageUI damageUI = Instantiate(damageUIPreFab, monster.transform.position, damageUIPreFab.transform.rotation);
        damageUI.realDamage = damage;
        monster.GetComponent<Monster>().PureOnDamaged(damage);
    }
}
