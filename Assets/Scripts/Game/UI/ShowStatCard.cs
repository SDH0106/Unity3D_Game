using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowStatCard : Singleton<ShowStatCard>
{
    [SerializeField] GameObject statCard;
    [SerializeField] Text rerollMoneyText;

    public Stat[] statInfo;
    int[] numArray;

    GameObject[] statCards;

    int rerollMoney;

    Color initPriceColor;

    [HideInInspector] public bool isSelected;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        initPriceColor = rerollMoneyText.color;
        rerollMoney = -3;
        Character.Instance.gameObject.SetActive(false);

        statCards = new GameObject[4];
        statInfo = new Stat[gameManager.gameObject.GetComponent<StatCardInfo>().statInfos.Length];

        for (int i = 0; i < statInfo.Length; i++)
        {
            statInfo[i] = gameManager.gameObject.GetComponent<StatCardInfo>().statInfos[i];
        }

        numArray = new int[statInfo.Length];

        ShowRandomCards();
    }

    private void Update()
    {
        if (gameManager.money < -rerollMoney)
            rerollMoneyText.color = Color.red;

        else if (gameManager.money >= -rerollMoney)
            rerollMoneyText.color = initPriceColor;

        rerollMoneyText.text = rerollMoney.ToString();

        Refill();
    }

    void Refill()
    {
        for (int i = 1; i < 5; i++)
        {
            if (statCards[i - 1] != null)
            {
                if (isSelected == true)
                    Destroy(gameObject.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        isSelected = false;
    }

    public void Reroll()
    {
        SoundManager.Instance.PlayES("SelectButton");

        if (gameManager.money >= -rerollMoney)
        {
            for (int i = 1; i < this.transform.childCount - 2; i++)
            {

                Destroy(this.transform.GetChild(i).GetChild(0).gameObject);
            }

            gameManager.money += rerollMoney;
            rerollMoney -= 2;

            ShowRandomCards();
        }
    }

    public void ShowRandomCards()
    {
        for (int j = 0; j < statCards.Length; j++)
        {
            numArray[j] = Random.Range(0, statInfo.Length);
            {
                for (int k = 0; k < j; k++)
                {
                    if (numArray[k] == numArray[j])
                    {
                        j--;
                        break;
                    }
                }
            }
        }

        for (int i = 1; i < this.transform.childCount - 2; i++)
        {
            StatCardUI card = statCard.GetComponent<StatCardUI>();
            card.selectedCard = statInfo[numArray[i - 1]];
            GameObject instant = Instantiate(statCard, this.transform.GetChild(i).transform);
            statCards[i - 1] = instant;
            statCards[i - 1].transform.SetParent(this.transform.GetChild(i));
        }
    }
}
