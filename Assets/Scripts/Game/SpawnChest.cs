using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChest : MonoBehaviour
{
    [SerializeField] GameObject chest;
    [SerializeField] int spawnRange;

    [HideInInspector] public Vector3 spawnPos;

    private void Start()
    {
        Invoke("GetChest", 10);
    }

    void GetChest()
    {
        int num = Random.Range(0, 100);

        if (num < 10 + GameManager.Instance.luck * 0.4)
        {
            Instantiate(chest, SpawnPosition(), chest.transform.rotation);
        }
    }

    Vector3 SpawnPosition()
    {
        Vector3 playerPos = Character.Instance.transform.position;
        Vector3 randPoint = Random.onUnitSphere * spawnRange;
        randPoint.y = 0;

        spawnPos = randPoint + transform.position;

        float distance = Vector3.Magnitude(playerPos - spawnPos);

        if (distance < 1)
        {
            SpawnPosition();
        }

        return spawnPos;
    }
}
