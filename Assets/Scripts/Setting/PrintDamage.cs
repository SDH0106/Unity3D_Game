using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PrintDamage : MonoBehaviour
{
    [SerializeField] GameObject damagePrefab;
    [SerializeField] Transform printPos;
    [SerializeField] int poolCount;

    private IObjectPool<DamageUI> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<DamageUI>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, maxSize: poolCount);
    }

    public void PrintDamageText()
    {
        DamageUI damage = objectPool.Get();
        damage.TextUpdate();
    }

    private DamageUI CreatePool()
    {
        DamageUI pool = Instantiate(damagePrefab, printPos.position, transform.rotation).GetComponent<DamageUI>();
        pool.SetManagedPool(objectPool);
        pool.transform.SetParent(Character.Instance.DamageStorage);

        return pool;
    }

    private void OnGetPool(DamageUI pool)
    {
        pool.gameObject.SetActive(true);
    }

    private void OnReleasePool(DamageUI pool)
    {
        pool.gameObject.SetActive(false);
    }

    private void OnDestroyPool(DamageUI pool)
    {
        Destroy(pool.gameObject);
    }
}
