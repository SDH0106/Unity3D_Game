﻿using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 3f;
    public GameObject effectPrefab;

    private IObjectPool<Bullet> managedPool;

    protected float angle;
    protected Vector3 dir;

    [HideInInspector] public DamageUI damageUI;

    protected GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // 총알 각도
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public virtual void Shoot(Vector3 dir)
    {
        this.dir = dir;

        if (GameManager.Instance.range < 0)
            Invoke("DestroyBullet", 1f);
        
        else if (GameManager.Instance.range >= 0)
            Invoke("DestroyBullet", 1f + GameManager.Instance.range * 0.2f);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster" && other.GetComponent<Monster>() != null)
        {
            GameObject pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
            pool.transform.SetParent(gameManager.damageStorage);
            other.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
            if (gameManager.absorbHp > 0)
                gameManager.hp += gameManager.absorbHp;
            DestroyBullet();
            CancelInvoke("DestroyBullet");

            Instantiate(effectPrefab, transform.position, transform.rotation);
        }
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    public virtual void DestroyBullet()
    {
        managedPool.Release(this);
    }
}
