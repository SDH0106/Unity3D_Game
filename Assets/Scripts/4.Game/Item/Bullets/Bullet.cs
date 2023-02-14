using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject effectPrefab;

    protected IObjectPool<Bullet> managedPool;
    protected IObjectPool<DamageUI> pool;

    protected float angle;
    public Vector3 dir;

    public float bulletDamage;

    [HideInInspector] public DamageUI damageUI;

    protected GameManager gameManager;

    int penetrateNum;

    protected bool isDestroyed = false;

    protected float range;

    public Vector3 initPos;

    protected bool isAbsorb;

    protected virtual void Awake()
    {
        pool = new ObjectPool<DamageUI>(CreateDamageUI, OnGetDamageUI, OnReleaseDamageUI, OnDestroyDamageUI, maxSize: 10);
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

        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        // 총알 각도
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public virtual void Shoot(Vector3 dir,Vector3 initPos, float range)
    {
        gameManager = GameManager.Instance;

        isDestroyed = false;

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
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            Monster monster = collision.collider.GetComponent<Monster>();
            
            DamageUI damage = pool.Get();

            damage.weaponDamage = bulletDamage;

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate(damage);

            /* if (damage.weaponDamage > monster.defence)
             {
                 damage.isMiss = false;
             }
             else if (damage.weaponDamage <= monster.defence)
             {
                 damage.isMiss = true;
             }*/

            if (damage.weaponDamage > 0)
                damage.isMiss = false;

            else if(damage.weaponDamage <= 0)
                damage.isMiss = true;

            float mDef = monster.defence;
            damage.realDamage = Mathf.Clamp(damage.weaponDamage * (1 - (mDef / (20 + mDef))), 0, damage.weaponDamage * (1 - (mDef / (20 + mDef))));
            damage.UISetting();
            damage.transform.position = transform.position;
            damage.gameObject.transform.SetParent(gameManager.damageStorage);
            
            if (gameManager.absorbHp > 0 && !damage.isMiss && !isAbsorb)
            {
                Character.Instance.currentHp += Mathf.Clamp(gameManager.absorbHp, 0f, 1f);
                isAbsorb = true;
            }

            monster.OnDamaged(damage.realDamage);

            if (!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate && !gameManager.penetrate)
            {
                if (!isDestroyed)
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
        if (penetrateNum <= 0)
        {
            penetrateNum++;
        }

        else if (penetrateNum > 0)
        {
            damage.weaponDamage = damage.weaponDamage/ 2;
        }
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
