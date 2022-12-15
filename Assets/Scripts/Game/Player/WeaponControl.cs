using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponControl : Weapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform normalFirePos;
    [SerializeField] Transform doubleFirePos1;
    [SerializeField] Transform doubleFirePos2;
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    float angle;
    Vector3 dir, mouse;

    float delay;
    float bulletDelay;

    bool canAttack;

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
        bulletDelay = weaponInfo.AttackDelay;
        canAttack = true;
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
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);

        if (dir.x < 0)
            transform.rotation *= Quaternion.Euler(180, 0, 0);

        else
            transform.rotation *= Quaternion.Euler(0, 0, 0);
    }

    void FireBullet()
    {
        if (canAttack == true)
        {
            if (Input.GetMouseButton(0))
            {
                if (!gameManager.doubleShot)
                {
                    Bullet bullet = pool.Get();
                    bullet.transform.position = normalFirePos.position;
                    bullet.Shoot(dir.normalized, weaponInfo.WeaponRange);
                    bullet.damageUI = damageUI;
                    bullet.speed = weaponInfo.BulletSpeed;
                }

                if (gameManager.doubleShot)
                {
                    Bullet bullet1 = pool.Get();
                    Bullet bullet2 = pool.Get();
                    bullet1.transform.position = doubleFirePos1.position;
                    bullet2.transform.position = doubleFirePos2.position;
                    bullet1.Shoot(dir.normalized, weaponInfo.WeaponRange);
                    bullet2.Shoot(dir.normalized, weaponInfo.WeaponRange);
                    bullet1.damageUI = damageUI;
                    bullet2.damageUI = damageUI;
                    bullet1.speed = weaponInfo.BulletSpeed;
                    bullet2.speed = weaponInfo.BulletSpeed;
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

            else if (gameManager.attackSpeed < 0)
            {
                if (delay >= (bulletDelay - gameManager.attackSpeed * 0.1))
                {
                    canAttack = true;
                    delay = 0;
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
