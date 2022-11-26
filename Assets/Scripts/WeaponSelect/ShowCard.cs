using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowCard : MonoBehaviour
{
    [SerializeField] SelectSceneCard[] cards;

    int[] numArray;

    private void Start()
    {
        numArray = new int[cards[0].weaponInfos.Length];
        GetRandomNum(cards.Length ,cards[0].weaponInfos.Length);
    }

    void GetRandomNum(int count, int length)
    {
        for (int i = 0; i < count; i++)
        {
            numArray[i] = UnityEngine.Random.Range(0, length);

            for (int j = 0; j < i; j++)
            {
                if (numArray[j] == numArray[i])
                {
                    i--;
                    break;
                }
            }
        }

        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].selectedWeapon = cards[i].weaponInfos[numArray[i]];
            cards[i].selectedWeapon.weaponGrade = Grade.ÀÏ¹Ý;
        }
    }
}
