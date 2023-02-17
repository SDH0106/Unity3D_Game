﻿using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float speed;
    public GameObject effectPrefab;

    protected IObjectPool<Bullet> managedPool;
    protected IObjectPool<DamageUI> pool;

    protected float angle;
    public Vector3 dir;

    public float bulletDamage;

    [HideInInspector] public DamageUI damageUI;

    protected GameManager gameManager;

    protected int penetrateNum;

    protected bool isDestroyed = false;

    protected float range;

    public Vector3 initPos;

    protected bool isAbsorb;

    protected virtual void Awake()
    {
        pool = new ObjectPool<DamageUI>(CreateDamageUI, OnGetDamageUI, OnReleaseDamageUI, OnDestroyDamageUI, maxSize: 20);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        isAbsorb = false;
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, initPos) > range)
        {
            DestroyBullet();
        }

        transform.position += dir * speed * Time.deltaTime;

        // 총알 각도
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public virtual void Shoot(Vector3 dir,Vector3 initPos, float range)
    {
        gameManager = GameManager.Instance;

        isDestroyed = false;

        dir.y = 0;
        this.dir = dir;
        this.initPos = initPos;
        this.range = range + gameManager.range;

        if (this.range < range / 2f)
        {
            this.range = range / 2f;
        }
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
     if(collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() !=null)
        {
            Monster monster = collision.collider.GetComponent<Monster>();

            DamageUI damage = pool.Get();

            damage.transform.position = transform.position;
            damage.transform.SetParent(gameManager.damageStorage);

            damage.weaponDamage = bulletDamage;

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate(damage);

            if (damage.weaponDamage > 0)
                damage.isMiss = false;
            else if (damage.weaponDamage <= 0)
                damage.isMiss = true;

            float mDef = monster.defence;
            damage.realDamage = Mathf.Clamp(damage.weaponDamage * (1 - (mDef / (20 + mDef))), 0, damage.weaponDamage);
            damage.UISetting();

            if(gameManager.absorbHp >0 && !damage.isMiss && !isAbsorb)
            {
                Character.Instance.currentHp += Mathf.Clamp(gameManager.absorbHp, 0f, gameManager.maxAbs);
                isAbsorb = true;
            }

            monster.OnDamaged(damage.realDamage);

            if(!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate && !gameManager.penetrate)
            {
                if(!isDestroyed)
                {
                    Instantiate(effectPrefab, transform.position, transform.rotation);
                    DestroyBullet();
                }
            }
        }
    }

    virtual public void Reflect(Collision collision)
    {
        Vector3 normalVector = collision.contacts[0].normal;
        normalVector = new Vector3(normalVector.x, 0, normalVector.y);
        dir = Vector3.Reflect(dir, normalVector).normalized;
        dir.y = 0;
    }

    public virtual void OnePenetrate()
    {
        if (penetrateNum <= 0)
        {
            penetrateNum++;
        }

        else if (penetrateNum > 0)
        {
            CancelInvoke("DestroyBullet");
            DestroyBullet();
            penetrateNum = 0;
        }
    }

    public virtual void LowPenetrate(DamageUI damage)
    {
        if (penetrateNum > 0)
        {
            damage.weaponDamage = damage.weaponDamage * Mathf.Clamp(0.5f - ((penetrateNum - 1) / 10f), 0.1f, 0.5f);
        }

        penetrateNum++;
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    public virtual void DestroyBullet()
    {
        isAbsorb = false;
        isDestroyed = true;
        penetrateNum = 0;
        if (gameObject.activeSelf)
        {
            isDestroyed = false;
            managedPool.Release(this);
        }
    }

    private DamageUI CreateDamageUI()
    {
        DamageUI damageUIPool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
        damageUIPool.SetManagedPool(pool);
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

    protected virtual void OnDestroy()
    {
        if (pool != null)
            pool.Clear();
    }
}
