using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Chameleon : Monster
{
    [SerializeField] Collider rightAttackColl;
    [SerializeField] Collider leftAttackColl;

    float xDistance;
    float zDistance;

    void Start()
    {
        InitSetting();
        rightAttackColl.gameObject.SetActive(false);
        leftAttackColl.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isDead == false)
        {
            if (!isAttack)
                Move();
            anim.SetBool("isWalk", isWalk);

            if (!isFreeze)
                Attack();
        }

        OnDead();
    }

    void Attack()
    {
        xDistance = Mathf.Abs(character.transform.position.x - transform.position.x);
        zDistance = Mathf.Abs(character.transform.position.z - transform.position.z);
        anim.SetBool("isAttack", isAttack);

        if (xDistance < 3 && zDistance <= 0.4)
        {
            isWalk = false;
            isAttack = true;

            if (dir.x < 0)
            {
                rightAttackColl.gameObject.SetActive(false);
                leftAttackColl.gameObject.SetActive(true);
            }

            else if (dir.x >= 0)
            {
                rightAttackColl.gameObject.SetActive(true);
                leftAttackColl.gameObject.SetActive(false);
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isWalk = true;
                isAttack = false;
            }
        }
    }

    protected override void SetInitMonster()
    {
        base.SetInitMonster();
        rightAttackColl.gameObject.SetActive(false);
        leftAttackColl.gameObject.SetActive(false);
    }
}
