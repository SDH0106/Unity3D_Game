using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestPassiveCard : MonoBehaviour
{
    [SerializeField] public PassiveInfo[] passiveInfo;

    [Header("decript")]
    [SerializeField] GameObject[] descriptPrefabs;
    [SerializeField] Transform decriptParent;

    [Header("Card")]
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text itemGrade;
    [SerializeField] Text itemCount;
    [SerializeField] Text sellPriceText;

    [HideInInspector] public PassiveInfo selectedPassive;

    float[] stats;
    string[] statTypes;

    int[] passiveIntVariables;
    float[] passiveFloatVariables;
    bool[] passiveBoolVariables;

    GameManager gameManager;
    ItemManager itemManager;

    int arrayCount;
    int sellPrice;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;

        sellPrice = Mathf.CeilToInt(selectedPassive.ItemPrice * 0.7f);

        Setting();
        CardImage();
        StatArray();
        DescriptionInfo();

        for (int i = 0; i < passiveInfo.Length; i++)
        {
            if (passiveInfo[i].ItemName == selectedPassive.ItemName)
            {
                arrayCount = i;
                break;
            }
        }
    }

    void Setting()
    {
        sellPrice = Mathf.CeilToInt(selectedPassive.ItemPrice * (1 - gameManager.salePercent) * 0.7f);
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
        itemGrade.text = selectedPassive.ItemGrade.ToString();
        itemCount.text = selectedPassive.MaxCount.ToString();
        sellPriceText.text = sellPrice.ToString();
    }

    void CardImage()
    {
        if (selectedPassive.ItemGrade == Grade.일반)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 1);
            cardBackLine.color = Color.black;
            itemName.color = Color.white;
            itemGrade.color = Color.white;
        }

        else if (selectedPassive.ItemGrade == Grade.희귀)
        {
            cardBack.color = new Color(0, 0.77f, 1, 1);
            cardBackLine.color = Color.blue;
            itemName.color = Color.blue;
            itemGrade.color = Color.blue;
        }

        else if (selectedPassive.ItemGrade == Grade.전설)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 1);
            cardBackLine.color = new Color(0.5f, 0, 0.5f, 1);
            itemName.color = new Color(0.5f, 0, 0.5f, 1);
            itemGrade.color = new Color(0.5f, 0, 0.5f, 1);
        }

        else if (selectedPassive.ItemGrade == Grade.신화)
        {
            cardBack.color = new Color(1, 0.31f, 0.31f, 1);
            cardBackLine.color = Color.red;
            itemName.color = Color.red;
            itemGrade.color = Color.red;
        }
    }

    void StatArray()
    {
        stats = new float[15];
        stats[0] = selectedPassive.Hp;
        stats[1] = selectedPassive.RecoverHp;
        stats[2] = selectedPassive.AbsorbHp;
        stats[3] = selectedPassive.Defence;
        stats[4] = selectedPassive.PhysicDamage;
        stats[5] = selectedPassive.MagicDamage;
        stats[6] = selectedPassive.ShortDamage;
        stats[7] = selectedPassive.LongDamage;
        stats[8] = selectedPassive.AttackSpeed;
        stats[9] = selectedPassive.Speed;
        stats[10] = selectedPassive.Luck;
        stats[11] = selectedPassive.Range;
        stats[12] = selectedPassive.Critical;
        stats[13] = selectedPassive.PercentDamage;
        stats[14] = selectedPassive.Avoid;

        statTypes = new string[15];
        statTypes[0] = "최대 체력";
        statTypes[1] = "체력 회복";
        statTypes[2] = "체력 흡수";
        statTypes[3] = "방어력";
        statTypes[4] = "물리 공격력";
        statTypes[5] = "마법 공격력";
        statTypes[6] = "근거리 공격력";
        statTypes[7] = "원거리 공격력";
        statTypes[8] = "공격 속도";
        statTypes[9] = "이동 속도";
        statTypes[10] = "행운";
        statTypes[11] = "사거리";
        statTypes[12] = "크리티컬";
        statTypes[13] = "공격력 배율";
        statTypes[14] = "회피율";

        passiveIntVariables = new int[3];
        passiveIntVariables[0] = selectedPassive.DashCount;
        passiveIntVariables[1] = selectedPassive.BuffNum;
        passiveIntVariables[2] = selectedPassive.ExDmg;

        passiveFloatVariables = new float[10];
        passiveFloatVariables[0] = selectedPassive.CoinRange;
        passiveFloatVariables[1] = selectedPassive.IncreaseExp;
        passiveFloatVariables[2] = selectedPassive.MonsterSpeed;
        passiveFloatVariables[3] = selectedPassive.SalePercent;
        passiveFloatVariables[4] = selectedPassive.SummonASpd;
        passiveFloatVariables[5] = selectedPassive.SummonPDmg;
        passiveFloatVariables[6] = selectedPassive.MonsterDef;

        passiveBoolVariables = new bool[17];
        passiveBoolVariables[0] = selectedPassive.LuckCoin;
        passiveBoolVariables[1] = selectedPassive.LuckDamage;
        passiveBoolVariables[2] = selectedPassive.LuckCritical;
        passiveBoolVariables[3] = selectedPassive.DoubleShot;
        passiveBoolVariables[4] = selectedPassive.Revive;
        passiveBoolVariables[5] = selectedPassive.GgoGgo;
        passiveBoolVariables[6] = selectedPassive.Ilsoon;
        passiveBoolVariables[7] = selectedPassive.Wakgood;
        passiveBoolVariables[8] = selectedPassive.Ddilpa;
        passiveBoolVariables[9] = selectedPassive.Butterfly;
        passiveBoolVariables[10] = selectedPassive.SubscriptionFee;
        passiveBoolVariables[11] = selectedPassive.SpawnTree;
        passiveBoolVariables[12] = selectedPassive.Dotgu;
        passiveBoolVariables[13] = selectedPassive.IsReflect;
        passiveBoolVariables[14] = selectedPassive.OnePenetrate;
        passiveBoolVariables[15] = selectedPassive.LowPenetrate;
        passiveBoolVariables[16] = selectedPassive.Penetrate;
    }

    void DescriptionInfo()
    {
        int max = descriptPrefabs.Length;
        int count = 0;

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] != 0)
            {
                descriptPrefabs[count].transform.GetChild(0).GetComponent<Text>().text = statTypes[i];
                descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().text = stats[i].ToString();
                if (stats[i] < 0)
                    descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().color = Color.red;
                descriptPrefabs[count].transform.GetChild(3).gameObject.SetActive(false);
                count++;
            }
        }

        for (int i = max - 1; i >= count; i--)
        {
            if (i == count)
            {
                descriptPrefabs[count].transform.GetChild(0).gameObject.SetActive(false);
                descriptPrefabs[count].transform.GetChild(1).gameObject.SetActive(false);
                descriptPrefabs[count].transform.GetChild(2).gameObject.SetActive(false);
                descriptPrefabs[count].transform.GetChild(3).gameObject.SetActive(true);
                descriptPrefabs[count].transform.GetChild(3).GetComponent<Text>().text = selectedPassive.Description;
            }

            else
                descriptPrefabs[i].gameObject.SetActive(false);
        }
    }

    public void Select()
    {
        SoundManager.Instance.PlayES("SelectButton");

        itemManager.GetPassiveInfo(selectedPassive);
        itemManager.passiveCounts[arrayCount]--;
        GameSceneUI.Instance.chestCount--;
        Destroy(gameObject);

        for (int i = 0; i < stats.Length; i++)
        {
            gameManager.stats[i] = Mathf.Round((gameManager.stats[i] + stats[i]) * 10) * 0.1f;
        }

        for (int i = 0; i < passiveIntVariables.Length; i++)
        {
            gameManager.passiveIntVariables[i] += passiveIntVariables[i];
        }

        for (int i = 0; i < passiveFloatVariables.Length; i++)
        {
            gameManager.passiveFloatVariables[i] = Mathf.Round((gameManager.passiveFloatVariables[i] + passiveFloatVariables[i]) * 10) * 0.1f;
        }

        for (int i = 0; i < passiveBoolVariables.Length; i++)
        {
            if (passiveBoolVariables[i] == true)
                gameManager.passiveBoolVariables[i] = passiveBoolVariables[i];
        }

        if (GameSceneUI.Instance.chestCount > 0)
            ShowPassive.Instance.ShowRandomPassiveCard();
    }

    public void Sell()
    {
        SoundManager.Instance.PlayES("SelectButton");

        itemManager.passiveCounts[arrayCount]--;
        GameSceneUI.Instance.chestCount--;
        Destroy(gameObject);
        gameManager.money += sellPrice;

        if (GameSceneUI.Instance.chestCount > 0)
            ShowPassive.Instance.ShowRandomPassiveCard();
    }
}
