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

    float angle;
    Vector3 dir, mouse;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

    void Update()
    {
        if (GameManager.Instance.currentScene == "Game")
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - transform.position;

            LookMousePosition();
            FireBullet();
        }
    }

    void LookMousePosition()
    {
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);

        if (dir.x < 0)
            transform.rotation *= Quaternion.Euler(180, 0, 0);
            //transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;

        else
            transform.rotation *= Quaternion.Euler(0, 0, 0);
        //transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;

        /*if (transform.localRotation.z > 0.9 || transform.localRotation.z < -0.9)
        {
            transform.rotation *= Quaternion.Euler(180, 0, 0);
        }*/
    }

    void FireBullet()
    {
        if (Input.GetMouseButtonDown(0) && firePos != null)
        {
            Bullet bullet = pool.Get();
            bullet.transform.position = firePos.position;
            bullet.Shoot(dir.normalized);
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePos.position, transform.rotation).GetComponent<Bullet>();
        bullet.SetManagedPool(pool);
        bullet.transform.SetParent(GameManager.Instance.bulletStorage);
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
