using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class DropCoin : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] int poolCount;

    private IObjectPool<Coin> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<Coin>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, maxSize: poolCount);
    }

    public void Drop(Vector3 dropPos)
    {
        Coin coin = objectPool.Get();
        coin.transform.position = dropPos;
    }

    private Coin CreatePool()
    {
        Coin pool = Instantiate(coinPrefab).GetComponent<Coin>();
        pool.SetManagedPool(objectPool);
        //pool.transform.SetParent(Character.Instance.DamageStorage);

        return pool;
    }

    private void OnGetPool(Coin pool)
    {
        pool.gameObject.SetActive(true);
    }

    private void OnReleasePool(Coin pool)
    {
        pool.gameObject.SetActive(false);
    }

    private void OnDestroyPool(Coin pool)
    {
        Destroy(pool.gameObject);
    }
}
