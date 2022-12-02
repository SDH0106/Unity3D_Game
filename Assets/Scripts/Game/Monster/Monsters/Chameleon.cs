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
        isWalk = true;
        hp = stat.monsterMaxHp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        initScale = transform.localScale;
    }

    private void Update()
    {
        if (isDead == false && isWalk && !isFreeze)
        {
            Move();
            anim.SetBool("isWalk", isWalk);
        }

        Attack();
        OnDead();
    }

    void Attack()
    {
        xDistance = Mathf.Abs(Character.Instance.transform.position.x - transform.position.x);
        zDistance = Mathf.Abs(Character.Instance.transform.position.z - transform.position.z);
        anim.SetBool("isAttack", isAttack);

        if (xDistance < 3 && zDistance <= 0.4)
        {
            isWalk = false;
            isAttack = true;
        }

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

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                isWalk = true;
                isAttack = false;
            }
        }
    }

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.CompareTag("Character"))
        {
            Character.Instance.OnDamaged(rightAttackColl);
            Character.Instance.OnDamaged(leftAttackColl);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position,Vector3.up, 3);
    }
}
