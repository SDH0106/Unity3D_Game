using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SocialPlatforms;
using UnityEngine.SpatialTracking;

public class MonsterBullet : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float bulletDamage;

    protected IObjectPool<MonsterBullet> managedPool;

    protected Vector3 dir;

    [HideInInspector] public int randNum;
    [HideInInspector] public Vector3 monsPos;

    protected GameManager gameManager;

    protected float realDamage;

    private void Start()
    {
        gameManager = GameManager.Instance;
        realDamage = bulletDamage * (1 + Mathf.Floor(gameManager.round / 30)) + Mathf.Floor(gameManager.round / 5) * 2f;  // 트리거에도 있음
    }

    void Update()
    {
        transform.position += dir * speed * Time.deltaTime;

        if (gameManager.isClear && gameManager.isBossDead)
        {
            CancelInvoke("DestroyBullet");
            DestroyBullet();
        }
    }

    public virtual void ShootDir()
    {
        dir = (Character.Instance.transform.position - transform.position).normalized;
        dir = new Vector3(dir.x, 0f, dir.z);
    }

    public virtual void AutoDestroyBullet()
    {
        Invoke("DestroyBullet", 2f);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Character")
        {
            realDamage = bulletDamage * (1 + Mathf.Floor(gameManager.round / 30)) + Mathf.Floor(gameManager.round / 5) * 2f;
            other.GetComponent<Character>().OnDamaged(realDamage);
            DestroyBullet();
            CancelInvoke("DestroyBullet");
        }
    }

    public void SetManagedPool(IObjectPool<MonsterBullet> pool)
    {
        managedPool = pool;
    }

    public virtual void DestroyBullet()
    {
        if (gameObject.activeSelf)
        {
            managedPool.Release(this);
        }
    }
}
