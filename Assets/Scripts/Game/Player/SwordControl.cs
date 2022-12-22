using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class SwordControl : Weapon
{
    Animator anim;

    Vector3 dir, mouse;

    float delay;
    float swordDelay;
    float attackRange;

    bool canAttack;

    Character character;

    float addRange;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        character = Character.Instance;
        count = ItemManager.Instance.weaponCount;
        damageUI = ItemManager.Instance.damageUI[count];
        delay = 0;
        swordDelay = weaponInfo.AttackDelay;
        attackRange = weaponInfo.WeaponRange;
        canAttack = true;
        //WeaponRotate();
    }

    void Update()
    {
        grade = (int)(ItemManager.Instance.weaponGrade[count] + 1);

        if (gameManager.currentScene == "Game" && !gameManager.isPause)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - transform.position;
            Attack();
        }
        WeaponRotate();
        WeaponSetting();
    }

    void WeaponRotate()
    {
        if (transform.parent.transform.localPosition.x < 0)
        {
            Debug.Log(transform.localRotation.eulerAngles);
            transform.localRotation *= Quaternion.AngleAxis(180, Vector3.up);
            Debug.Log(transform.localRotation.eulerAngles);
        }

        else if (transform.parent.transform.localPosition.x > 0)
        {
            //transform.rotation = Quaternion.Euler(90, 70, 80);
            Debug.Log(transform.localRotation.eulerAngles);
            transform.localRotation = Quaternion.Euler(10, 39, 20);
            Debug.Log(transform.localRotation.eulerAngles);
        }
    }

    void Attack()
    {
        if (gameManager.range > 0)
            addRange = (attackRange + gameManager.range * 0.05f);

        else if (gameManager.range <= 0)
            addRange = attackRange;

        Vector3 range = (mouse - character.transform.position).normalized * addRange;

        if (canAttack == true)
        {
            if (Input.GetMouseButton(0) && (!gameManager.isClear || !gameManager.isBossDead))
            {
                transform.position = Vector3.MoveTowards(transform.position, character.transform.position + range, 2);

                if (dir.x > 0)
                {
                    anim.SetTrigger("RightAttack");
                }

                else
                {
                    anim.SetTrigger("LeftAttack");
                }

                SoundManager.Instance.PlayES(weaponInfo.WeaponSound);

                canAttack = false;            
            }

        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") || anim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, character.weaponPoses[count].position, 5);
            }
        }

        if(canAttack == false)
        {
            delay += Time.deltaTime;

            if (gameManager.attackSpeed >= 0)
            {
                if (delay >= (swordDelay / (1 + gameManager.attackSpeed * 0.1)))
                {
                    canAttack = true;
                    delay = 0;
                }
            }

            else if (gameManager.attackSpeed < 0)
            {
                if (delay >= (swordDelay - gameManager.attackSpeed * 0.1))
                {
                    canAttack = true;
                    delay = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") && other.GetComponent<Monster>() != null)
        {
            criRand = UnityEngine.Random.Range(1, 101);
            GameObject pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(gameManager.damageStorage);
            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
            if (gameManager.absorbHp > 0)
                gameManager.hp += gameManager.absorbHp;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, mouse);
    }
}
