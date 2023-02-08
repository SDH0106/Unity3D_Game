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
    protected IObjectPool<DamageUI> damagePool;

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

    int x, z;
    bool xInput;
    bool zInput;

    Vector3 beforeDir;
    Vector3 bulletDir;

    bool isAttack;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
        damagePool = new ObjectPool<DamageUI>(CreateDamageUI, OnGetDamageUI, OnReleaseDamageUI, OnDestroyDamageUI, maxSize: poolCount);
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
        isAttack = false;
    }

    void Update()
    {
        grade = (int)(ItemManager.Instance.weaponGrade[count] + 1);

        if (gameManager.currentScene == "Game" && !gameManager.isPause)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            if (!isSwing)
            {
                LookKeyBoardPos();
                Attack();
            }
        }

    }

    void LookKeyBoardPos()
    {
        xInput = (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left"))) || (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")));
        zInput = (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Up"))) || (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")));

        if (!xInput)
            x = 0;

        if (!zInput)
            z = 0;

        if (zInput)
        {
            if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Up")))
                z = 1;

            else if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")))
                z = -1;
        }

        if (xInput)
        {
            if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left")))
                x = -1;

            else if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")))
                x = 1;
        }

        if (xInput || zInput)
        {
            dir = (Vector3.right * x + Vector3.forward * z).normalized;
            beforeDir = dir;
        }

        else if (!xInput && !zInput)
        {
            dir = beforeDir;
        }

        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, -90);
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

                if ((mouse.x - transform.position.x) > 0)
                {
                    transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
                }

                else if ((mouse.x - transform.position.x) <= 0)
                {
                    transform.parent.localRotation = Quaternion.Euler(0, -180, 0);
                }

                anim.SetTrigger("RightAttack");

                transform.position = Vector3.MoveTowards(transform.position, character.transform.position + range, 2);
                bulletDir = transform.position - character.transform.position + range;
                bulletDir.y = 0;

                SoundManager.Instance.PlayES(weaponInfo.WeaponSound);

                if (character.characterNum == (int)CHARACTER_NUM.Legendary)
                {
                    if (character.currentHp / character.maxHp > 0.7)
                    {
                        criRand = UnityEngine.Random.Range(1, 101);
                        int bulletCri = criRand;
                        Debug.Log(criRand);
                        WeaponSetting();
                        Bullet bullet = pool.Get();
                        bullet.transform.position = new Vector3(firePos.position.x, 0f, firePos.position.z);
                        bullet.bulletDamage = swordBulletDamage;
                        bullet.GetComponent<SwordBullet>().criRand = bulletCri;
                        bullet.damageUI = damageUI;
                        bullet.speed = 6f;
                        bullet.Shoot(bulletDir.normalized, firePos.position, 5f);
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
        transform.parent.localRotation = Quaternion.Euler(0, 0, 0);
        isSwing = false;
        anim.enabled = false;
        isAttack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Monster") && other.GetComponent<Monster>() != null)
        {
            criRand = UnityEngine.Random.Range(1, 101);

            WeaponSetting();

            DamageUI damage = damagePool.Get();
            if (weaponDamage > other.GetComponent<Monster>().defence)
                damage.isMiss = false;
            else if (weaponDamage <= other.GetComponent<Monster>().defence)
                damage.isMiss = true;
            damage.realDamage = Mathf.Clamp(weaponDamage - other.GetComponent<Monster>().defence, 0, weaponDamage - other.GetComponent<Monster>().defence);

            if (criRand <= gameManager.critical || gameManager.critical >= 100)
            {
                damage.damageText.color = new Color(0.9f, 0, 0.7f, 1);
                damage.damageText.fontSize = 65;
            }

            else if (criRand > gameManager.critical)
            {
                damage.damageText.color = new Color(1, 0.4871f, 0);
                damage.damageText.fontSize = 50;
            }

            damage.UISetting();
            damage.transform.position = transform.position;
            damage.gameObject.transform.SetParent(gameManager.damageStorage);

            other.GetComponent<Monster>().OnDamaged(damage.realDamage);

            if (gameManager.absorbHp > 0 && !damage.isMiss && !isAttack && isSwing)
            {
                character.currentHp += gameManager.absorbHp;
                isAttack = true;
            }
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

    private DamageUI CreateDamageUI()
    {
        DamageUI damageUIPool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
        damageUIPool.SetManagedPool(damagePool);
        damageUIPool.transform.SetParent(gameManager.bulletStorage);
        return damageUIPool;
    }

    private void OnGetDamageUI(DamageUI damageUIPool)
    {
        damageUIPool.gameObject.SetActive(true);
    }

    private void OnReleaseDamageUI(DamageUI damageUIPool)
    {
        damageUIPool.gameObject.SetActive(false);
    }

    private void OnDestroyDamageUI(DamageUI damageUIPool)
    {
        Destroy(damageUIPool.gameObject);
    }

    private void OnDestroy()
    {
        pool.Clear();
        damagePool.Clear();
    }
}
