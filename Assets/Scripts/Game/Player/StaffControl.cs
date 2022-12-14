using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class StaffControl : Weapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform normalFirePos;
    [SerializeField] Transform doubleFirePos1;
    [SerializeField] Transform doubleFirePos2;
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    Vector3 dir, mouse;

    float delay;
    float bulletDelay;
    float thunderDelay;

    float detectRange;

    bool canAttack = true;
    bool isTargetFind = false;
    Transform target;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        count = ItemManager.Instance.weaponCount;
        damageUI = ItemManager.Instance.damageUI[count];

        delay = 0;
        bulletDelay = 1;
        thunderDelay = 3;
    }

    void Update()
    {
        grade = (int)(ItemManager.Instance.weaponGrade[count] + 1);

        if (gameManager.currentScene == "Game" && !gameManager.isPause)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - transform.position;

            LookMousePosition();
            FireBullet();
        }

        WeaponSetting();
    }

    void LookMousePosition()
    {
        if (dir.x < 0)
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;

        else
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
    }

    void FireBullet()
    {
        if (weaponInfo.WeaponName != "번개 스태프")
        {
            if (canAttack == true)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!gameManager.doubleShot)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = normalFirePos.position;
                        bullet.Shoot(dir.normalized);
                        bullet.damageUI = damageUI;
                    }

                    if (gameManager.doubleShot)
                    {
                        Bullet bullet1 = pool.Get();
                        Bullet bullet2 = pool.Get();
                        bullet1.transform.position = doubleFirePos1.position;
                        bullet2.transform.position = doubleFirePos2.position;
                        bullet1.Shoot(dir.normalized);
                        bullet2.Shoot(dir.normalized);
                        bullet1.damageUI = damageUI;
                        bullet2.damageUI = damageUI;
                    }

                    SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                    canAttack = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;

                if (gameManager.attackSpeed >= 0)
                {
                    if (delay >= (bulletDelay / (1 + gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }

                else if(gameManager.attackSpeed < 0)
                {
                    if (delay >= (bulletDelay - (gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }
            }
        }

        else if (weaponInfo.WeaponName == "번개 스태프")
        {
            if (canAttack == true)
            {
                if (!isTargetFind)
                    FindTarget();

                else if (isTargetFind)
                {
                    Bullet bullet = pool.Get();
                    bullet.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z + 3);
                    bullet.damageUI = damageUI;
                    SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                    canAttack = false;
                    isTargetFind = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;
                if (gameManager.attackSpeed >= 0)
                {
                    if (delay >= (thunderDelay / (1 + gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }

                else  if (gameManager.attackSpeed < 0)
                {
                    if (delay >= (thunderDelay - gameManager.attackSpeed * 0.1))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }
            }
        }
    }

    void FindTarget()
    {
        detectRange = 4f + (gameManager.range * 0.1f);

        if (detectRange < 1)
            detectRange = 1;

        Collider[] colliders = Physics.OverlapSphere(Character.Instance.transform.position, detectRange);

        if (colliders.Length > 0)
        {
            int rand = Random.Range(0, colliders.Length);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Monster" && colliders[i].GetComponent<Monster>() != null)
                {
                    if (i == rand)
                    {
                        target = colliders[i].transform;

                        GameObject pool = Instantiate(damageUI, colliders[i].transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
                        pool.transform.SetParent(gameManager.damageStorage);
                        colliders[i].GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
                        if (gameManager.absorbHp > 0)
                            gameManager.hp += gameManager.absorbHp;

                        isTargetFind = true;
                        break;
                    }
                }
            }
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, normalFirePos.position, transform.rotation).GetComponent<Bullet>();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, mouse);
    }
}
