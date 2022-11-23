using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class SwordControl : Weapon
{
    Animator anim;
    float angle;
    Vector3 dir, mouse;

    float delay = 0;
    float swordDelay = 4;

    bool canAttack = true;

    GameManager gameManager;
    Character character;

    private void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameManager.Instance;
        character = Character.Instance;
        count = ItemManager.Instance.weaponCount;
        damageUI = ItemManager.Instance.damageUI[count];
    }

    void Update()
    {
        if (gameManager.currentScene == "Game")
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - transform.position;
            LookMousePosition();
        }

        WeaponSetting();
        Attack();
    }

    void LookMousePosition()
    {
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, -90);

        if (dir.x < 0)
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;

        else
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
    }

    void Attack()
    {
        Vector3 range = (mouse - character.transform.position).normalized * gameManager.range;

        if (canAttack == true)
        {
            if (Input.GetMouseButton(0))
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
            if (delay >= (swordDelay/ gameManager.attackSpeed))
            {
                canAttack = true;
                delay = 0;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster"))
        {
            GameObject pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(gameManager.damageStorage);
            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
            gameManager.hp += gameManager.absorbHp;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, mouse);
    }
}
