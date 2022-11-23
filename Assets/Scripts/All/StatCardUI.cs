using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StatCardUI : StatCardInfo
{
    [SerializeField] Image itemSprite;
    [SerializeField] Text statName;
    [SerializeField] Text statValue;
    [SerializeField] Text statType;

    [HideInInspector] public Stat selectedCard;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance; 
        Setting();
    }

    void Setting()
    {
        itemSprite.sprite = Resources.Load<Sprite>(selectedCard.statSprite);
        statName.text = selectedCard.statName;
        statValue.text = selectedCard.statValue.ToString();
        statType.text = selectedCard.statType;
    }

    public void SelectCard()
    {
        SoundManager.Instance.PlayES("SelectButton");
        ShowStatCard.Instance.isSelected = true;

        gameManager.levelUpCount--;

        for (int i = 0; i < 9; i++)
        {
            if (selectedCard.statName == gameManager.gameObject.GetComponent<StatCardInfo>().statInfos[i].statName)
                gameManager.stats[i] += gameManager.gameObject.GetComponent<StatCardInfo>().statInfos[i].statValue;
        }

        if (gameManager.levelUpCount <= 0)
            gameManager.ToShopScene();

        else if (gameManager.levelUpCount > 0)
            ShowStatCard.Instance.ShowRandomCards();
    }
}
