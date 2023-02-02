using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PassiveCardUI : MonoBehaviour
{
    [SerializeField] public PassiveInfo[] passiveInfo;

    [Header("Lock")]
    [SerializeField] Image lockBackImage;
    [SerializeField] Image lockImage;
    [SerializeField] Text lockText;

    [Header("decript")]
    [SerializeField] GameObject[] descriptPrefabs;
    [SerializeField] Transform decriptParent;

    [Header("Card")]
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text itemPrice;
    [SerializeField] Text itemGrade;
    [SerializeField] Text maxCount;

    [HideInInspector] public PassiveInfo selectedPassive;

    float[] stats;
    string[] statTypes;

    int[] passiveIntVariables;
    float[] passiveFloatVariables;
    bool[] passiveBoolVariables;

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public bool isLock;

    Color initPriceColor;

    GameManager gameManager;
    ItemManager itemManager;

    int arrayCount;
    int price;

    int num;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        initPriceColor = itemPrice.color;

        LockImageColor = new Color(0.17f, 0.17f, 0.17f);
        LockTextColor = Color.white;

        num = transform.parent.GetSiblingIndex();

        Setting();
        CardImage();
        StatArray();
        DescriptionInfo();
        StartLockColor();

        for (int i = 0; i < passiveInfo.Length; i++)
        {
            if (passiveInfo[i].ItemName == selectedPassive.ItemName)
            {
                arrayCount = i;
                break;
            }
        }
    }

    private void Update()
    {
        lockImage.gameObject.SetActive(isLock);

        if (gameManager.money < price)
            itemPrice.color = Color.red;

        else if (gameManager.money >= price)
            itemPrice.color = initPriceColor;
    }

    void Setting()
    {
        if (isLock)
            selectedPassive = itemManager.lockedPassCards[num];

        price = Mathf.CeilToInt(selectedPassive.ItemPrice * (1 - gameManager.salePercent));
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
        itemPrice.text = price.ToString();
        itemGrade.text = selectedPassive.ItemGrade.ToString();
        maxCount.text = selectedPassive.MaxCount.ToString();
    }

    void CardImage()
    {
        if (selectedPassive.ItemGrade == Grade.일반)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 0.8235f);
            cardBackLine.color = Color.black;
            itemName.color = Color.white;
            itemGrade.color = Color.white;
        }

        else if (selectedPassive.ItemGrade == Grade.희귀)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 0.8235f);
            cardBackLine.color = Color.blue;
            itemName.color = new Color(0.5f, 0.8f, 1f, 1f);
            itemGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (selectedPassive.ItemGrade == Grade.전설)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            itemName.color = new Color(0.8f, 0.4f, 1f, 1f);
            itemGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (selectedPassive.ItemGrade == Grade.신화)
        {
            cardBack.color = new Color(0.7f, 0.1f, 0.1f, 0.8235f);
            cardBackLine.color = Color.red;
            itemName.color = new Color(1f, 0.45f, 0.45f, 1f);
            itemGrade.color = new Color(1f, 0.45f, 0.45f, 1f);
        }
    }

    protected void StatArray()
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

        passiveIntVariables = new int[4];
        passiveIntVariables[0] = selectedPassive.DashCount;
        passiveIntVariables[1] = selectedPassive.BuffNum;
        passiveIntVariables[2] = selectedPassive.ExDmg;
        passiveIntVariables[3] = selectedPassive.IsedolCount;

        passiveFloatVariables = new float[7];
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

    protected void DescriptionInfo()
    {
        int max = descriptPrefabs.Length;
        int count = 0;

        for (int i = 0; i < stats.Length; i++)
        {
            if (stats[i] != 0)
            {
                descriptPrefabs[count].transform.GetChild(0).GetComponent<Text>().text = statTypes[i];
                if (stats[i] < 0)
                {
                    descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().color = Color.red;
                    descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().text = stats[i].ToString();
                }

                else if(stats[i] >= 0)
                {
                    descriptPrefabs[count].transform.GetChild(2).GetComponent<Text>().text = ($"+{stats[i]}");
                }
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

    public void Click()
    {
        if (gameManager.money >= price && itemManager.passiveCounts[arrayCount] > 0)
        {
            SoundManager.Instance.PlayES("SelectButton");

            if (selectedPassive.GgoGgo || selectedPassive.Ilsoon || selectedPassive.Wakgood)
            {
                if (Character.Instance.summonNum < 3)
                {
                    itemManager.GetPassiveInfo(selectedPassive);
                    itemManager.passiveCounts[arrayCount]--;
                    Destroy(gameObject);

                    isLock = false;
                    itemManager.cardLocks[num] = isLock;
                    itemManager.lockedPassCards[num] = null;
                    gameManager.money -= price;

                    for (int i = 0; i < passiveBoolVariables.Length; i++)
                    {
                        if (passiveBoolVariables[i] == true)
                            gameManager.passiveBoolVariables[i] = passiveBoolVariables[i];
                    }
                }
            }

            else
            {
                itemManager.GetPassiveInfo(selectedPassive);
                itemManager.passiveCounts[arrayCount]--;
                Destroy(gameObject);

                isLock = false;
                itemManager.cardLocks[num] = isLock;
                itemManager.lockedPassCards[num] = null;
                gameManager.money -= price;

                for (int i = 0; i < stats.Length; i++)
                {
                    if (i == 13)
                        gameManager.stats[i] = Mathf.Round((gameManager.stats[i] + stats[i]) * 100) * 0.01f;

                    else
                        gameManager.stats[i] = Mathf.Round((gameManager.stats[i] + stats[i]) * 10) * 0.1f;
                }

                for (int i = 0; i < passiveIntVariables.Length; i++)
                {
                    if (i == 1 && passiveIntVariables[i] != 0) // 버프 포션
                    {
                        gameManager.passiveIntVariables[i] = passiveIntVariables[i];
                    }

                    else
                    {
                        gameManager.passiveIntVariables[i] += passiveIntVariables[i];
                    }

                }

                for (int i = 0; i < passiveFloatVariables.Length; i++)
                {
                    gameManager.passiveFloatVariables[i] = Mathf.Round((gameManager.passiveFloatVariables[i] + passiveFloatVariables[i]) * 10) * 0.1f;
                }

                for (int i = 0; i < passiveBoolVariables.Length; i++)
                {
                    if (passiveBoolVariables[i] == true)
                    {
                        if (i == 14 || i == 15 || i == 16)
                        {
                            if (gameManager.isReflect)
                            {
                                passiveBoolVariables[i] = false;
                            }
                        }

                        gameManager.passiveBoolVariables[i] = passiveBoolVariables[i];

                        if (i == 8)
                        {
                            Ddilpa();
                            passiveBoolVariables[i] = false;
                        }

                        else if (i == 9)
                        {
                            Butterfly();
                            passiveBoolVariables[i] = false;
                        }

                        else if(i == 13)
                        {
                            gameManager.passiveBoolVariables[14] = false;   // 하나 관통
                            gameManager.passiveBoolVariables[15] = false;   // 뎀감 관통
                            gameManager.passiveBoolVariables[16] = false;   //  풀관통
                        }

                        else if(i == 14)
                        {
                            gameManager.passiveBoolVariables[15] = false;
                            gameManager.passiveBoolVariables[16] = false;
                        }

                        else if (i == 15)
                        {
                            gameManager.passiveBoolVariables[14] = false;
                            gameManager.passiveBoolVariables[16] = false;
                        }

                        else if (i == 16)
                        {
                            gameManager.passiveBoolVariables[14] = false;
                            gameManager.passiveBoolVariables[15] = false;
                        }
                    }
                }
            }
        }

        else
            SoundManager.Instance.PlayES("CantBuy");
    }

    void Ddilpa()
    {
        // 마뎀(5) > 물뎀(4)
        gameManager.stats[4] += Mathf.Round((gameManager.stats[5] / 2) * 10) * 0.1f;
        gameManager.stats[5] = 0;
    }

    void Butterfly()
    {
        // 물뎀(4) > 마뎀(5)
        gameManager.stats[5] += Mathf.Round((gameManager.stats[4] / 2) * 10) * 0.1f;
        gameManager.stats[4] = 0;
    }

    public void Lock()
    {
        if (!isLock)
        {
            lockBackImage.color = Color.white;
            lockText.color = Color.black;
            itemManager.lockedPassCards[num] = selectedPassive;
            isLock = true;
        }

        else if (isLock)
        {
            lockBackImage.color = LockImageColor;
            lockText.color = LockTextColor;
            itemManager.lockedPassCards[num] = null;
            isLock = false;
        }

        itemManager.cardLocks[num] = isLock;
    }

    void StartLockColor()
    {
        if (isLock)
        {
            lockBackImage.color = Color.white;
            lockText.color = Color.black;
        }

        else if (!isLock)
        {
            lockBackImage.color = LockImageColor;
            lockText.color = LockTextColor;
        }
    }
}
