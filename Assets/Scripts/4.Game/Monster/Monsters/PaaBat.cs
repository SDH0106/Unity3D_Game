using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PaaBat : Monster
{
    [SerializeField] GameObject batLazor;
    [SerializeField] Transform rightBulletPos;
    [SerializeField] Transform leftBulletPos;

    float xDistance;
    float zDistance;

    void Start()
    {
        InitSetting();
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

        if (xDistance < 3 && zDistance <= 0.8f)
        {
            isWalk = false;
            isAttack = true;
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

    public void InstantLazor()
    {
        Transform bulletPos = rend.flipX ? leftBulletPos : rightBulletPos;

        PaaLazor lazor = Instantiate(batLazor, bulletPos).GetComponent<PaaLazor>();
        lazor.isFlip = rend.flipX;
    }
}
