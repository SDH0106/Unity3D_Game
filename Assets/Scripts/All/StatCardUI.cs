using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatCardUI : StatCardInfo
{
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text statName;
    [SerializeField] Text statGrade;
    [SerializeField] Text statValue;
    [SerializeField] Text statType;

    [HideInInspector] public Stat selectedCard;

    GameManager gameManager;

    [HideInInspector] public Grade cardGrade;

    private void Start()
    {
        gameManager = GameManager.Instance; 
        Setting();
        CardColor();
    }

    void Setting()
    {
        itemSprite.sprite = Resources.Load<Sprite>(selectedCard.statSprite);
        statName.text = selectedCard.statName;
        statValue.text = (selectedCard.statValue * (int)(cardGrade + 1)).ToString();
        statType.text = selectedCard.statType;
        statGrade.text = cardGrade.ToString();
    }

    void CardColor()
    {
        if (cardGrade == Grade.¿œπ›)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            cardBackLine.color = Color.black;
            statName.color = Color.white;
            statGrade.color = Color.white;
        }

        else if (cardGrade == Grade.»Ò±Õ)
        {
            cardBack.color = new Color(0, 0.77f, 1, 1f);
            cardBackLine.color = Color.blue;
            statName.color = Color.blue;
            statGrade.color = Color.blue;
        }

        else if (cardGrade == Grade.¿¸º≥)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            cardBackLine.color = new Color(0.5f, 0, 0.5f, 1);
            statName.color = new Color(0.5f, 0, 0.5f, 1);
            statGrade.color = new Color(0.5f, 0, 0.5f, 1);
        }

        else if (cardGrade == Grade.Ω≈»≠)
        {
            cardBack.color = new Color(1, 0.31f, 0.31f, 1f);
            cardBackLine.color = Color.red;
            statName.color = Color.red;
            statGrade.color = Color.red;
        }
    }

    public void SelectCard()
    {
        SoundManager.Instance.PlayES("SelectButton");
        ShowStatCard.Instance.isSelected = true;

        gameManager.levelUpCount--;

        for (int i = 0; i < 11; i++)
        {
            if (selectedCard.statName == gameManager.gameObject.GetComponent<StatCardInfo>().statInfos[i].statName)
            {
                gameManager.stats[i] += (gameManager.gameObject.GetComponent<StatCardInfo>().statInfos[i].statValue * (int)(cardGrade + 1));
                gameManager.stats[i] = Mathf.Round(gameManager.stats[i] * 10) * 0.1f;
            }
        }

        if (gameManager.levelUpCount > 0)
            ShowStatCard.Instance.ShowRandomCards();
    }
}
