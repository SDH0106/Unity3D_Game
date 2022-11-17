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

    private void Start()
    {
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
        ShowStatCard.Instance.isSelected = true;

        GameManager.Instance.levelUpCount--;

        for (int i = 0; i < 9; i++)
        {
            if (selectedCard.statName == GameManager.Instance.gameObject.GetComponent<StatCardInfo>().statInfos[i].statName)
                GameManager.Instance.stats[i] += GameManager.Instance.gameObject.GetComponent<StatCardInfo>().statInfos[i].statValue;
        }

        if (GameManager.Instance.levelUpCount <= 0)
            GameManager.Instance.ToShopScene();

        else if (GameManager.Instance.levelUpCount > 0)
            ShowStatCard.Instance.ShowRandomCards();
    }
}
