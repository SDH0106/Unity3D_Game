using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPassive : Singleton<ShowPassive>
{
    [SerializeField] GameObject passiveCardUI;

    float[] weightPassiveValue;

    GameManager gameManager;
    ItemManager itemManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;

        weightPassiveValue = new float[4];

        ShowRandomPassiveCard();
    }

    public void ShowRandomPassiveCard()
    {
        float totalWeight = 0;

        weightPassiveValue[0] = 150 - (gameManager.round - 1) * 6;
        weightPassiveValue[1] = 10 + (gameManager.round - 1) * 20;
        weightPassiveValue[2] = (gameManager.round - 1);
        weightPassiveValue[3] = (gameManager.round - 1) / 2;

        ChestPassiveCard passiveCard = passiveCardUI.GetComponent<ChestPassiveCard>();

        for (int i = 0; i < passiveCard.passiveInfo.Length; i++)
        {
            passiveCard.passiveInfo[i].weight = weightPassiveValue[(int)passiveCard.passiveInfo[i].ItemGrade];
            totalWeight += passiveCard.passiveInfo[i].weight;
        }

        float rand = Random.Range(0, totalWeight);
        float total = 0;

        for (int i = 0; i < passiveCardUI.GetComponent<ChestPassiveCard>().passiveInfo.Length; i++)
        {
            total += passiveCard.passiveInfo[i].weight;

            if (rand <= total)
            {
                if (itemManager.passiveCounts[i] != 0)
                {
                    passiveCard.selectedPassive = passiveCard.passiveInfo[i];
                    break;
                }

                else
                {
                    rand = Random.Range(0, totalWeight);
                    total = 0;
                    i = 0;
                }
            }
        }

        GameObject instant = Instantiate(passiveCardUI, transform.GetChild(1));
    }
}

