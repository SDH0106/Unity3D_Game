using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class Chick : Summons
{
    [SerializeField] GameObject chickenPrefab;

    int summonRound;
    int summonPosNum;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        InitSetting();
        summonRound = gameManager.round;
        summonPosNum = character.summonNum;
    }

    private void Update()
    {
        if (gameManager.round - summonRound == 15)
        {
            GameObject summon = Instantiate(chickenPrefab);
            summon.transform.position = character.summonPos[summonPosNum].position;
            summon.transform.SetParent(gameManager.transform);
            summonRound = gameManager.round;

            Destroy(gameObject);
        }

        if (!isAttack)
        {
            CheckDistance();

            if (isNear)
                transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

            else if (!isNear)
                transform.position = Vector3.MoveTowards(transform.position, character.transform.position, gameManager.speed * 2 * Time.deltaTime);
        }

        anim.SetBool("isAttack", isAttack);
    }
}
