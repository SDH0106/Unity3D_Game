using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChest : MonoBehaviour
{
    [SerializeField] GameObject chest;
    [SerializeField] int spawnRange;

    [HideInInspector] public Vector3 spawnPos;

    Vector3 playerPos;
    Vector3 randPoint;

    GameManager gameManager;
    Character character;
    float distance;

    private void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        Invoke("GetChest", 22);
    }

    void GetChest()
    {
        int num = Random.Range(0, 100);

        if (num < 5 + gameManager.luck * 0.4)
        {
            Instantiate(chest, SpawnPosition(), chest.transform.rotation);
        }
    }

    Vector3 SpawnPosition()
    {
        playerPos = character.transform.position;
        randPoint = Random.onUnitSphere * spawnRange;
        randPoint.y = 0;

        spawnPos = randPoint + transform.position;

        distance = Vector3.Magnitude(playerPos - spawnPos);

        if (distance < 1)
        {
            SpawnPosition();
        }

        return spawnPos;
    }
}
