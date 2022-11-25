using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : Singleton<Coin>
{
    private IObjectPool<Coin> managedPool;

    float speed;

    public int coinValue;

    bool isGet = false;

    Vector3 characterPos;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        RandomValue();
    }

    private void Update()
    {
        MoveCoin();

        if (gameManager.isClear)
            DestroyPool();
    }

    public void MoveCoin()
    {
        characterPos = Character.Instance.transform.position;

        float distance = Vector3.Distance(characterPos, transform.position);

        if (isGet == false)
        {
            if (distance <= 2)
            {
                speed = gameManager.speed + 1;
                isGet = true;
            }
            else
                speed = 0;
        }

        transform.position = Vector3.MoveTowards(transform.position, characterPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Character"))
        {
            SoundManager.Instance.PlayES("Coin");
            gameManager.money += coinValue;
            DestroyPool();
        }
    }

    void RandomValue()
    {
        coinValue = Random.Range(1,11);
    }

    void InitSetting()
    {
        isGet = false;
        speed = 0;
        RandomValue();
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
