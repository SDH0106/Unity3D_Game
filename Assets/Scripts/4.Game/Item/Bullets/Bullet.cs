using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed;
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

    public virtual void Shoot(Vector3 dir, float range)
    {
        this.dir = dir;

        if (GameManager.Instance.range < 0)
            Invoke("DestroyBullet", range);
        
        else if (GameManager.Instance.range >= 0)
            Invoke("DestroyBullet", range + GameManager.Instance.range * 0.2f);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Monster") && collision.collider.GetComponent<Monster>() != null)
        {
            collision.collider.GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);

            if (damageUI.weaponDamage > collision.collider.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = false;

            else if (damageUI.weaponDamage <= collision.collider.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f))
                damageUI.isMiss = true;

            damageUI.realDamage = damageUI.weaponDamage - collision.collider.GetComponent<Monster>().stat.monsterDefence * (1 + gameManager.round * 0.1f);

            DamageUI pool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
            pool.gameObject.transform.SetParent(gameManager.damageStorage);

            if (gameManager.absorbHp > 0)
                Character.Instance.currentHp += gameManager.absorbHp;

            Instantiate(effectPrefab, transform.position, transform.rotation);

            if (gameManager.isReflect)
                Reflect(collision);

            else if(!gameManager.isReflect)
            {
                CancelInvoke("DestroyBullet");
                DestroyBullet();
            }
        }
    }

    public void Reflect(Collision collision)
    {
        Vector3 normalVector = collision.contacts[0].normal;
        normalVector = new Vector3(normalVector.x, 0, normalVector.y);
        dir = Vector3.Reflect(dir, normalVector).normalized;
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
