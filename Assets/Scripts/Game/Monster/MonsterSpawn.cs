using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] GameObject[] normalMonsterPrefab;
    [SerializeField] GameObject[] bossMonsterPrefab;
    [SerializeField] Transform storageParent;
    [SerializeField] GameObject spawnImage;
    [SerializeField] int poolCount;
    [SerializeField] int spawnRange;
    [SerializeField] float spawnDelay;

    private IObjectPool<Monster> pool;

    [HideInInspector] public Vector3 spawnPos;

    int[] weightValue;
    int totalWeight = 0;

    private void Awake()
    {
        pool = new ObjectPool<Monster>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: poolCount);
    }

    private void Start()
    {
        weightValue = new int[] { 0, 5 };

        for (int i = 0; i < weightValue.Length; i++)
        {
            totalWeight += weightValue[i];
        }

        InvokeRepeating("RendSpawnImage", 1f, spawnDelay);
    }

    private void Update()
    {
        if (GameManager.Instance.currentGameTime <= 0)
        {
            CancelInvoke("RendSpawnImage");
        }
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

    private Monster CreateMonster()
    {
        int num = RandomMonster();
        Monster monster = Instantiate(normalMonsterPrefab[num]).GetComponent<Monster>();
        monster.stat = MonsterInfo.Instance.monsterInfos[num];
        monster.SetManagedPool(pool);
        monster.transform.SetParent(storageParent);

        return monster;
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
