using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chick : Summons
{
    [SerializeField] GameObject chickenPrefab;

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
        if (!isAttack)
        {
            CheckDistance();

            if (isNear)
                transform.position = Vector3.MoveTowards(transform.position, randomPos, speed * Time.deltaTime);

            else if (!isNear)
                transform.position = Vector3.MoveTowards(transform.position, character.transform.position, character.speed * 2 * Time.deltaTime);
        }

        anim.SetBool("isAttack", isAttack);

        if (gameManager.round - summonRound == 1)
        {
            GameObject summon = Instantiate(chickenPrefab, character.summonPos[summonPosNum].position, chickenPrefab.transform.rotation);
            summon.transform.parent = gameManager.transform;

            Destroy(gameObject);
        }
    }
}
