using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : MonoBehaviour
{
    private IObjectPool<Coin> managedPool;

    Vector3 moveDir;

    Vector3 characterPos;

    float speed;

    bool isGet = false;

    private void Update()
    {
        characterPos = Character.Instance.transform.position;

        float distance = Vector3.Distance(characterPos, transform.position);
        moveDir = characterPos - transform.position;

        if (isGet == false)
        {
            if (distance <= 2)
            {
                //speed = Character.Instance.speed + 1;
                speed = 2;
                isGet = true;
            }
            else
                speed = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, characterPos, speed * Time.deltaTime);

        if (distance == 0)
            DestroyPool();
    }

    void InitSetting()
    {
        isGet = false;
        speed = 0;
    }

    public void SetManagedPool(IObjectPool<Coin> pool)
    {
        managedPool = pool;
    }

    public void DestroyPool()
    {
        managedPool.Release(this);
        InitSetting();
    }
}
