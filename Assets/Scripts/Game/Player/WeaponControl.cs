using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponControl : Weapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePos;
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    float angle;
    Vector3 dir, mouse;

    float delay = 0;
    float bulletDelay = 1;

    bool canAttack = true;

    GameManager gameManager;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        count = ItemManager.Instance.weaponCount;
        damageUI = ItemManager.Instance.damageUI[count];
    }

    void Update()
    {
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
                Bullet bullet = pool.Get();
                bullet.transform.position = firePos.position;
                bullet.Shoot(dir.normalized);
                bullet.damageUI = damageUI;
                SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                canAttack = false;
            }
        }

        else if (canAttack == false)
        {
            delay += Time.deltaTime;
            if (delay >= (bulletDelay / gameManager.attackSpeed))
            {
                canAttack = true;
                delay = 0;
            }
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePos.position, transform.rotation).GetComponent<Bullet>();
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
