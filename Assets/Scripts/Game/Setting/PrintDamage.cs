using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class PrintDamage : MonoBehaviour
{
    [SerializeField] GameObject damagePrefab;
    [SerializeField] int poolCount;

    private IObjectPool<DamageUI> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<DamageUI>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, maxSize: poolCount);
    }

    public void PrintDamageText(Vector3 pos)
    {
        DamageUI damage = objectPool.Get();
        damage.transform.position = pos;
        damage.TextUpdate();
    }

    private DamageUI CreatePool()
    {
        DamageUI pool = Instantiate(damagePrefab).GetComponent<DamageUI>();
        pool.SetManagedPool(objectPool);
        pool.transform.SetParent(GameSceneUI.Instance.damageStorage);

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
