using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class Coin : Singleton<Coin>
{
    [SerializeField] Collider coll;

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
        if (character.isDead)
            DestroyPool();

        if (gameManager.currentScene == "Game")
        {
            if (gameManager.isClear && gameManager.isBossDead)
            {
                EndGameCoinMove();
            }

            else
                MoveCoin();
        }

    }

    public void EndGameCoinMove()
    {
        if (GameSceneUI.Instance.gameObject != null)
        {
            coll.enabled = false;
            Vector3 endPos = GameSceneUI.Instance.coinTextPos.position;
            endPos.y = 0;

            transform.position = Vector3.MoveTowards(transform.position, endPos, 40f * Time.deltaTime);

            if (transform.position == endPos)
            {
                gameManager.money += 1;
                DestroyPool();
            }
        }
    }

    public void MoveCoin()
    {
        if (character.gameObject != null)
            characterPos = Character.Instance.transform.position;

        float distance = Vector3.Distance(characterPos, transform.position);

        if (distance <= 2 * (1 + gameManager.coinRange))
        {
            if(gameManager.speed < 1)
                speed = 2;

            else if (gameManager.speed >= 1)
                speed = gameManager.speed + 2;
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
        coll.enabled = true;
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
