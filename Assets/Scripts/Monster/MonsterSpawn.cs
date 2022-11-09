using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class MonsterSpawn : MonoBehaviour
{
    [SerializeField] GameObject monsterPrefab;
    [SerializeField] Transform storageParent;
    [SerializeField] GameObject spawnImage;
    [SerializeField] int poolCount;
    [SerializeField] int spawnRange;
    [SerializeField] float spawnDelay;

    private IObjectPool<Monster> pool;

    Vector3 spawnPos;

    private void Awake()
    {
        pool = new ObjectPool<Monster>(CreateMonster, OnGetMonster, OnReleaseMonster, OnDestroyMonster, maxSize: poolCount);
    }

    private void Start()
    {
        InvokeRepeating("RendSpawnImage", 2f, spawnDelay);
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
        Monster monster = Instantiate(monsterPrefab).GetComponent<Monster>();
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
