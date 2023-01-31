using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed;
    public GameObject effectPrefab;

    protected IObjectPool<Bullet> managedPool;

    protected float angle;
    public Vector3 dir;

    [HideInInspector] public DamageUI damageUI;

    protected GameManager gameManager;

    int penetrateNum;

    bool isDestroyed = false;

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

    public virtual void Shoot(Vector3 dir, float range)
    {
        isDestroyed = false;

        this.dir = dir;

        if (GameManager.Instance.range <= 0)
            Invoke("DestroyBullet", range);
        
        else if (GameManager.Instance.range > 0)
            Invoke("DestroyBullet", range + GameManager.Instance.range * 0.2f);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            Instantiate(effectPrefab, transform.position, transform.rotation);

            if (gameManager.absorbHp > 0)
            {
                Debug.Log("1");
                Character.Instance.currentHp += gameManager.absorbHp;
            }

            if (gameManager.isReflect)
                Reflect(collision);

            else if (gameManager.onePenetrate)
                OnePenetrate();

            else if (gameManager.lowPenetrate)
                LowPenetrate();

            else if (!gameManager.isReflect && !gameManager.lowPenetrate && !gameManager.onePenetrate && !gameManager.penetrate)
            {
                CancelInvoke("DestroyBullet");

                if (!isDestroyed)
                    DestroyBullet();
            }

            if (damageUI.weaponDamage > collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= collision.collider.GetComponent<Monster>().defence)
                damageUI.isMiss = true;

            damageUI.realDamage = damageUI.weaponDamage - collision.collider.GetComponent<Monster>().defence;

            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.realDamage);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);
        }
    }

    virtual public void Reflect(Collision collision)
    {
        Vector3 normalVector = collision.contacts[0].normal;
        normalVector = new Vector3(normalVector.x, 0, normalVector.y);
        dir = Vector3.Reflect(dir, normalVector).normalized;
    }

    public void OnePenetrate()
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

    public void LowPenetrate()
    {
        if (penetrateNum <= 0)
        {
            penetrateNum++;
        }

        else if (penetrateNum > 0)
        {
            damageUI.weaponDamage = damageUI.weaponDamage/ 2;
        }
    }

    public void SetManagedPool(IObjectPool<Bullet> pool)
    {
        managedPool = pool;
    }

    public virtual void DestroyBullet()
    {
        isDestroyed = true;
        if (gameObject.activeSelf)
            managedPool.Release(this);
        penetrateNum = 0;
    }
}
