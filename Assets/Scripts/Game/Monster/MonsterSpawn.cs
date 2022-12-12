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

    float[] weightValue;
    float totalWeight = 0;

    GameManager gameManager;
    Character character;

    private void Awake()
    {
        pool = new ObjectPool<Monster>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        weightValue = new float[] { 100, 0, 0, 0 };

        for (int i = 0; i < weightValue.Length; i++)
        {
            totalWeight += weightValue[i];
        }

        InvokeRepeating("RendSpawnImage", 1f, spawnDelay / ((gameManager.round + 4) / 5));

        if (gameManager.round % 10 == 0)
        {
            gameManager.isBossDead = false;
            RendBossSpawnImage(gameManager.round);
        }
    }

    private void Update()
    {
        if (gameManager.isClear || character.isDead)
        {
            CancelInvoke("RendSpawnImage");
        }

        if (bosssParent.transform.childCount == 0)
            gameManager.isBossDead = true;
    }

    IEnumerator SpawnMonster(Vector3 pos)
    {
        yield return new WaitForSeconds(1);

        Monster monster = pool.Get();
        monster.transform.position = pos;
    }

    Vector3 SpawnPosition()
    {
        Vector3 playerPos = character.transform.position;
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
        Vector3 pos = SpawnPosition();
        GameObject spawnMark = Instantiate(spawnImage, pos, spawnImage.transform.rotation);
        spawnMark.transform.SetParent(storageParent);
        Destroy(spawnMark, 1f);
        StartCoroutine(SpawnMonster(pos));
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
        if (gameManager.round >= 5)
        {
            weightValue[0] = 100 - (gameManager.round * 3f);
            weightValue[1] = (gameManager.round * 3f) * 0.2f;
            weightValue[2] = (gameManager.round * 3f) * 0.3f;
            weightValue[3] = (gameManager.round * 3f) * 0.5f;
        }

        float rand = Random.Range(0, totalWeight);
        int spawnNum = 0;
        float total = 0;

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
