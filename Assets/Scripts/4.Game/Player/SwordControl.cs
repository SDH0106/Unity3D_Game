using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class SwordControl : Weapon
{
    [SerializeField] GameObject swordBullet;
    [SerializeField] Transform firePos;
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    Animator anim;

    Vector3 dir, mouse;

    float delay;
    float swordDelay;
    float attackRange;

    bool canAttack;

    Character character;

    float addRange;

    float angle;

    bool isSwing = false;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

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
        anim.enabled = false;
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

            if (!isSwing)
                LookMousePosition();
            WeaponSetting();
            Attack();
        }

    }

    void LookMousePosition()
    {
        anim.enabled = false;
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, -90);
    }

    void WeaponRotate()
    {
        if (transform.parent.transform.localPosition.x < 0)
        {
            transform.parent.localRotation = Quaternion.Euler(0, 180, 0);

            if (transform.parent.transform.localPosition.y > 0)
                transform.parent.localRotation *= Quaternion.Euler(0, 0, 45);

            else if (transform.parent.transform.localPosition.y < 0)
                transform.parent.localRotation *= Quaternion.Euler(0, 0, -45);
        }

        else if (transform.parent.transform.localPosition.x > 0)
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, 0);

            if (transform.parent.transform.localPosition.y > 0)
                transform.parent.localRotation *= Quaternion.Euler(0, 0, 45);

            else if (transform.parent.transform.localPosition.y < 0)
                transform.parent.localRotation *= Quaternion.Euler(0, 0, -45);
        }
    }

    void Attack()
    {
        if (gameManager.range > 0)
            addRange = (attackRange + gameManager.range * 0.05f);

        else if (gameManager.range <= 0)
            addRange = attackRange;

        Vector3 range = (mouse - character.transform.position).normalized * addRange;
        range.y = 0;

        if (canAttack == true)
        {
            if (Input.GetMouseButton(0) && (!gameManager.isClear || !gameManager.isBossDead))
            {
                isSwing = true;
                anim.enabled = true;
                criRand = UnityEngine.Random.Range(1, 101);

                if (dir.x > 0)
                {
                    transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                }

                else
                {
                    transform.parent.localRotation = Quaternion.Euler(0, -180, 0);
                }

                anim.SetTrigger("RightAttack");

                transform.position = Vector3.MoveTowards(transform.position, character.transform.position + range, 2);

                SoundManager.Instance.PlayES(weaponInfo.WeaponSound);

                if (character.characterNum == (int)CHARACTER_NUM.Legendary)
                {
                    if (character.currentHp / character.maxHp > 0.7)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = new Vector3(firePos.position.x, 0f, firePos.position.z);
                        bullet.Shoot(dir.normalized, firePos.position, 5f);
                        bullet.damageUI = damageUI;
                        bullet.speed = 6f;
                    }
                }

                canAttack = false;
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

    public void EndSwing()
    {
        transform.position = Vector3.MoveTowards(transform.position, character.weaponPoses[count].position, 5);
        //WeaponRotate();
        isSwing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") && other.GetComponent<Monster>() != null)
        {
            criRand = UnityEngine.Random.Range(1, 101);

            if (damageUI.weaponDamage > other.GetComponent<Monster>().defence)
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= other.GetComponent<Monster>().defence)
                damageUI.isMiss = true;

            damageUI.realDamage = damageUI.weaponDamage - other.GetComponent<Monster>().defence;

            other.GetComponent<Monster>().OnDamaged(damageUI.realDamage);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);

            if (gameManager.absorbHp > 0 && isSwing)
                character.currentHp += gameManager.absorbHp;
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(swordBullet, firePos.position, transform.rotation).GetComponent<Bullet>();
        bullet.SetManagedPool(pool);
        bullet.transform.SetParent(gameManager.bulletStorage);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
}
