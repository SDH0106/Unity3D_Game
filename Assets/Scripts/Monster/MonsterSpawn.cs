using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] Transform storageParent;
    [SerializeField] GameObject spawnImage;
    [SerializeField] int poolCount;
    [SerializeField] int spawnRange;

    float spawnDelay;

    private IObjectPool<Monster> pool;

    Vector3 spawnIamgePos;
    Vector3 spawnMonPos;

    private void Awake()
    {
        pool = new ObjectPool<Monster>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: poolCount);
    }

    private void Start()
    {
        spawnDelay = 2f;
        InvokeRepeating("RendSpawnImage", 2f, 5f);
    }

    void SpawnMonster()
    {
        Monster monster = pool.Get();
    }

    Vector3 SpawnPosition()
    {
        Vector3 playerPos = CharacterMove.Instance.transform.position;
        Vector3 randPoint = Random.onUnitSphere * spawnRange;
        randPoint.y = 0;

        spawnIamgePos = randPoint + transform.position;

        float distance = Vector3.Magnitude(playerPos - spawnIamgePos);

        if(distance < 2)
        {
            SpawnPosition();
        }
        
        return spawnIamgePos;
    }

    void RendSpawnImage()
    {
        GameObject spawnMark = Instantiate(spawnImage, SpawnPosition(), spawnImage.transform.rotation);
        spawnMark.transform.SetParent(storageParent);
        Destroy(spawnMark, spawnDelay);
        Invoke("SpawnMonster", spawnDelay);

        spawnMonPos = spawnIamgePos;
    }

    private Monster CreateMonster()
    {
        Monster monster = Instantiate(monsterPrefab, spawnMonPos, monsterPrefab.transform.rotation).GetComponent<Monster>();
        monster.SetManagedPool(pool);
        monster.transform.SetParent(storageParent);
        return monster;
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
