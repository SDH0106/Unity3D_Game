using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponControl : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePos; 
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    SpriteRenderer rend;
    Weapon weaponInfo;

    int weaponDamage;
    int elementDamage;
    float weaponRange;

    float angle;
    Vector3 dir,mouse;

    private void Awake()
    {
       pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

    private void Start()
    {
        weaponInfo = GetComponent<Weapon>();
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (GameManager.Instance.currentScene == "Game")
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - Character.Instance.transform.position;

            LookMousePosition();
            FireBullet();
        }
    }

    void LookMousePosition()
    {
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);

        if (dir.x < 0)
        {
            rend.flipY = true;
        }

        else
        {
            rend.flipY = false;
        }
    }

    void FireBullet()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Bullet bullet = pool.Get();
            bullet.transform.position = transform.position;
            bullet.Shoot(dir.normalized);
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePos.position, transform.rotation).GetComponent<Bullet>();
        bullet.SetManagedPool(pool);
        bullet.transform.SetParent(Character.Instance.transform.GetChild(3));
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
