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
        gameManager = GameManager.Instance;
        character = Character.Instance;
        hp = stat.monsterMaxHp * (1 + (float)((gameManager.round - 1) * 0.25));
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        initScale = transform.localScale;
        initSpeed = speed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        if (isDead == false && isWalk)
        {
            rightAttackColl.gameObject.SetActive(false);
            leftAttackColl.gameObject.SetActive(false);
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

    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);

        if (other.CompareTag("Character"))
        {
            character.OnDamaged(rightAttackColl);
            character.OnDamaged(leftAttackColl);
        }
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.DrawWireDisc(transform.position,Vector3.up, 3);
    }
}
