using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 3f;

    private IObjectPool<Bullet> managedPool;

    float angle;
    Vector3 dir;

    void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // 총알 각도
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public void Shoot(Vector3 dir)
    {
        this.dir = dir;

        Invoke("DestroyBullet", 2f);
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    public void DestroyBullet()
    {
        managedPool.Release(this);
    }

    public void CancleDestroyInvoke()
    {
        CancelInvoke("DestroyBullet");
    }
}
