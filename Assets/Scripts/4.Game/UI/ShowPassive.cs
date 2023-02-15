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

        weightPassiveValue[0] = 200 - (gameManager.round - 1) * 6;
        weightPassiveValue[1] = 10 * gameManager.round;
        weightPassiveValue[2] = (gameManager.round - 1) * (gameManager.round) * 0.2f * (1 + Mathf.Clamp(gameManager.luck, 0, 100) * 0.01f);
        weightPassiveValue[3] = (gameManager.round - 1) * (gameManager.round) * 0.04f * (1 + Mathf.Clamp(gameManager.luck, 0, 100) * 0.01f);

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

