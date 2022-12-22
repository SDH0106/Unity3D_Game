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

    float[] stats = new float[13];
    string[] statTypes = new string[13];

    int[] passiveIntVariables = new int[10];
    float[] passiveFloatVariables = new float[10];
    bool[] passiveBoolVariables = new bool[5];

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public bool isLock;

    Color initPriceColor;

    GameManager gameManager;
    ItemManager itemManager;

    int arrayCount;
    int price;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        initPriceColor = itemPrice.color;

        LockImageColor = lockBackImage.color;
        LockTextColor = lockText.color;

        Setting();
        CardImage();
        StatArray();
        DescriptionInfo();
        StartLockColor();   

        for(int i=0;i<passiveInfo.Length;i++)
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

        if (gameManager.money < selectedPassive.ItemPrice)
            itemPrice.color = Color.red;

        else if (gameManager.money >= selectedPassive.ItemPrice)
            itemPrice.color = initPriceColor;
    }

    void Setting()
    {
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
            cardBack.color = new Color(0, 0.77f, 1, 0.8235f);
            cardBackLine.color = Color.blue;
            itemName.color = Color.blue;
            itemGrade.color = Color.blue;
        }

        else if (selectedPassive.ItemGrade == Grade.전설)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
            cardBackLine.color = new Color(0.5f, 0, 0.5f, 1);
            itemName.color = new Color(0.5f, 0, 0.5f, 1);
            itemGrade.color = new Color(0.5f, 0, 0.5f, 1);
        }

        else if (selectedPassive.ItemGrade == Grade.신화)
        {
            cardBack.color = new Color(1, 0.31f, 0.31f, 0.8235f);
            cardBackLine.color = Color.red;
            itemName.color = Color.red;
            itemGrade.color = Color.red;
        }
    }

    void StatArray()
    {
        stats[0] = selectedPassive.Hp;
        stats[1] = selectedPassive.RecoverHp;
        stats[2] = selectedPassive.AbsorbHp;
        stats[3] = selectedPassive.Defence;
        stats[4] = selectedPassive.PhysicDamage;
        stats[5] = selectedPassive.ElementDamage;
        stats[6] = selectedPassive.ShortDamage;
        stats[7] = selectedPassive.LongDamage;
        stats[8] = selectedPassive.AttackSpeed;
        stats[9] = selectedPassive.Speed;
        stats[10] = selectedPassive.Luck;
        stats[11] = selectedPassive.Range;
        stats[12] = selectedPassive.Critical;

        statTypes[0] = "최대 체력";
        statTypes[1] = "체력 회복";
        statTypes[2] = "체력 흡수";
        statTypes[3] = "방어력";
        statTypes[4] = "물리 공격력";
        statTypes[5] = "원소 공격력";
        statTypes[6] = "근거리 공격력";
        statTypes[7] = "원거리 공격력";
        statTypes[8] = "공격 속도";
        statTypes[9] = "이동 속도";
        statTypes[10] = "행운";
        statTypes[11] = "사거리";
        statTypes[12] = "크리티컬";

        passiveIntVariables[0] = selectedPassive.DashCount;

        passiveFloatVariables[0] = selectedPassive.CoinRange;
        passiveFloatVariables[1] = selectedPassive.IncreaseExp;
        passiveFloatVariables[2] = selectedPassive.MonsterSpeed;
        passiveFloatVariables[3] = selectedPassive.SalePercent;

        passiveBoolVariables[0] = selectedPassive.LuckCoin;
        passiveBoolVariables[1] = selectedPassive.LuckDamage;
        passiveBoolVariables[2] = selectedPassive.LuckCritical;
        passiveBoolVariables[3] = selectedPassive.DoubleShot;
        passiveBoolVariables[4] = selectedPassive.Revive;
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

    public void Click()
    {
        SoundManager.Instance.PlayES("SelectButton");

        if (gameManager.money >= selectedPassive.ItemPrice && itemManager.passiveCounts[arrayCount] > 0)
        {
            itemManager.GetPassiveInfo(selectedPassive);
            itemManager.passiveCounts[arrayCount]--;
            Destroy(gameObject);
            isLock = false;
            gameManager.money -= price;

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

            for(int i=0;i<passiveBoolVariables.Length;i++)
            {
                if (passiveBoolVariables[i] == true)
                    gameManager.passiveBoolVariables[i] = passiveBoolVariables[i];
            }
        }
    }

    public void Lock()
    {
        if (!isLock)
        {
            lockBackImage.color = Color.white;
            lockText.color = Color.black;
            isLock = true;
        }

        else if (isLock)
        {
            lockBackImage.color = LockImageColor;
            lockText.color = LockTextColor;
            isLock = false;
        }
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
