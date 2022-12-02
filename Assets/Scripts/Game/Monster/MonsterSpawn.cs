using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.XR;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] normalMonsterPrefab;
    [SerializeField] GameObject[] bossMonsterPrefab;
    [SerializeField] Transform storageParent;
    [SerializeField] Transform bosssParent;
    [SerializeField] GameObject spawnImage;
    [SerializeField] GameObject bossSpawnImage;
    [SerializeField] int poolCount;
    [SerializeField] int spawnRange;
    [SerializeField] float spawnDelay;

    private IObjectPool<Monster> pool;

    [HideInInspector] public Vector3 spawnPos;

    int[] weightValue;
    int totalWeight = 0;

    GameManager gameManager;

    private void Awake()
    {
        pool = new ObjectPool<Monster>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        weightValue = new int[] { 0, 0, 0, 1 };

        for (int i = 0; i < weightValue.Length; i++)
        {
            totalWeight += weightValue[i];
        }

        InvokeRepeating("RendSpawnImage", 1f, spawnDelay);

        if (gameManager.round % 10 == 0)
        {
            gameManager.isBossDead = false;
            RendBossSpawnImage(gameManager.round);
        }
    }

    private void Update()
    {
        if (gameManager.isClear || gameManager.hp <= 0)
        {
            CancelInvoke("RendSpawnImage");
        }

        if (bosssParent.transform.childCount == 0)
            gameManager.isBossDead = true;
    }

    void SpawnMonster()
    {
        Monster monster = pool.Get();
        monster.transform.position = spawnPos;
    }

    Vector3 SpawnPosition()
    {
        Vector3 playerPos = Character.Instance.transform.position;
        Vector3 randPoint = Random.onUnitSphere * spawnRange;
        randPoint.y = 0;

        spawnPos = randPoint + transform.position;

        float distance = Vector3.Magnitude(playerPos - spawnPos);

        if(distance < 2)
        {
            SpawnPosition();
        }

        return spawnPos;
    }

    void RendSpawnImage()
    {
        GameObject spawnMark = Instantiate(spawnImage, SpawnPosition(), spawnImage.transform.rotation);
        spawnMark.transform.SetParent(storageParent);
        Destroy(spawnMark, 1f);
        Invoke("SpawnMonster", 1f);
    }

    void RendBossSpawnImage(int round)
    {
        if (round == 10)
        {
            GameObject spawnMark = Instantiate(bossSpawnImage, new Vector3(3, 0, 3), bossSpawnImage.transform.rotation);
            Destroy(spawnMark, 2.1f);
            spawnMark.transform.SetParent(bosssParent);
            StartCoroutine(CreateBossMonster(round));
        }

        else if (round == 20)
        {
            GameObject spawnMark = Instantiate(bossSpawnImage, new Vector3(3, 0, 3), bossSpawnImage.transform.rotation);
            Destroy(spawnMark, 2.1f);
            spawnMark.transform.SetParent(bosssParent);
            GameObject spawnMark2 = Instantiate(bossSpawnImage, new Vector3(-3, 0, 3), bossSpawnImage.transform.rotation);
            Destroy(spawnMark2, 2.1f);
            spawnMark2.transform.SetParent(bosssParent);
            StartCoroutine(CreateBossMonster(round));
        }
    }

    private Monster CreateMonster()
    {
        int num = RandomMonster();
        Monster monster = Instantiate(normalMonsterPrefab[num]).GetComponent<Monster>();
        monster.stat = MonsterInfo.Instance.monsterInfos[num];
        monster.SetManagedPool(pool);
        monster.transform.SetParent(storageParent);

        return monster;
    }

    private IEnumerator CreateBossMonster(int round)
    {
        yield return new WaitForSeconds(2);

        if (round == 10)
        {
            int rand = Random.Range(0, 2);
            GameObject inst = Instantiate(bossMonsterPrefab[rand]);
            Monster monster = inst.GetComponent<Monster>();
            monster.stat = MonsterInfo.Instance.monsterInfos[normalMonsterPrefab.Length + rand];
            monster.transform.position = new Vector3(3, 0, 3);
            monster.transform.SetParent(bosssParent);
        }

        else if (round == 20)
        {
            GameObject inst1 = Instantiate(bossMonsterPrefab[0]);
            GameObject inst2 = Instantiate(bossMonsterPrefab[1]);
            Monster monster1 = inst1.GetComponent<Monster>();
            Monster monster2 = inst2.GetComponent<Monster>();
            monster1.stat = MonsterInfo.Instance.monsterInfos[normalMonsterPrefab.Length];
            monster1.transform.position = new Vector3(3, 0, 3);
            monster2.stat = MonsterInfo.Instance.monsterInfos[normalMonsterPrefab.Length + 1];
            monster2.transform.position = new Vector3(-3, 0, 3);
            monster2.transform.SetParent(bosssParent);
            monster1.transform.SetParent(bosssParent);
        }
    }

    int RandomMonster()
    {
        int rand = Random.Range(0, totalWeight);
        int spawnNum = 0;
        int total = 0;

        for (int i = 0; i < normalMonsterPrefab.Length; i++)
        { 
            total += weightValue[i];
            if (rand < total)
            {
                spawnNum = i;
                break;
            }
        }

        return spawnNum;
    }

    private void OnGetMonster(Monster monster)
    {
        monster.gameObject.SetActive(true);
    }

    private void OnReleaseMonster(Monster monster)
    {
        monster.gameObject.SetActive(false);
    }

    private void OnDestroyMonster(Monster monster)
    {
        Destroy(monster.gameObject);
    }
}
