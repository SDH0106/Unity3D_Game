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

    Vector3 characterPos;

    GameManager gameManager;
    Character character;

    private void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
    }

    private void Update()
    {
        if (gameManager.currentScene == "Game")
            MoveCoin();

        if (character.isDead || gameManager.isClear && gameManager.isBossDead)
            DestroyPool();
    }

    public void MoveCoin()
    {
        if (character.gameObject != null)
            characterPos = Character.Instance.transform.position;

        float distance = Vector3.Distance(characterPos, transform.position);

        if (distance <= 2 * (1 + gameManager.coinRange))
        {
            if(character.speed <= 1)
                speed = 2;

            else if (character.speed > 1)
                speed = character.speed + 2;
        }

        else
            speed = 0;

        transform.position = Vector3.MoveTowards(transform.position, characterPos, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Character"))
        {
            SoundManager.Instance.PlayES("Coin");

            if (!gameManager.luckCoin)
            {
                gameManager.money += coinValue;
            }

            else if (gameManager.luckCoin)
            {
                int rand = Random.Range(1, 101);

                if (rand <= gameManager.luck || gameManager.luck >= 100)
                    gameManager.money += coinValue * 2;

                else if(rand > gameManager.luck)
                    gameManager.money += coinValue;
            }

            DestroyPool();
        }
    }

    void InitSetting()
    {
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
