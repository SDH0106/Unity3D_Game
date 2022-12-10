using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
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

    [HideInInspector] public PassiveInfo selectedPassive;

    float[] stats = new float[11];
    string[] statTypes = new string[11];

    int[] passiveIntVariables = new int[10];
    float[] passiveFloatVariables = new float[10];

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public bool isLock = false;

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
        price = Mathf.CeilToInt(selectedPassive.ItemPrice * (1 - gameManager.passiveIntVariables[0]));
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
        itemPrice.text = price.ToString();
        itemGrade.text = selectedPassive.ItemGrade.ToString();
    }

    void CardImage()
    {
        if (selectedPassive.ItemGrade == Grade.�Ϲ�)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 0.8235f);
            cardBackLine.color = Color.black;
            itemName.color = Color.white;
            itemGrade.color = Color.white;
        }

        else if (selectedPassive.ItemGrade == Grade.���)
        {
            cardBack.color = new Color(0, 0.77f, 1, 0.8235f);
            cardBackLine.color = Color.blue;
            itemName.color = Color.blue;
            itemGrade.color = Color.blue;
        }

        else if (selectedPassive.ItemGrade == Grade.����)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
            cardBackLine.color = new Color(0.5f, 0, 0.5f, 1);
            itemName.color = new Color(0.5f, 0, 0.5f, 1);
            itemGrade.color = new Color(0.5f, 0, 0.5f, 1);
        }

        else if (selectedPassive.ItemGrade == Grade.��ȭ)
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
        stats[6] = selectedPassive.AttackSpeed;
        stats[7] = selectedPassive.Speed;
        stats[8] = selectedPassive.Luck;
        stats[9] = selectedPassive.Range;
        stats[10] = selectedPassive.Critical;

        statTypes[0] = "�ִ� ü��";
        statTypes[1] = "ü�� ȸ��";
        statTypes[2] = "ü�� ���";
        statTypes[3] = "����";
        statTypes[4] = "���� ���ݷ�";
        statTypes[5] = "���� ���ݷ�";
        statTypes[6] = "���� �ӵ�";
        statTypes[7] = "�̵� �ӵ�";
        statTypes[8] = "���";
        statTypes[9] = "��Ÿ�";
        statTypes[10] = "ũ��Ƽ��";

        passiveIntVariables[0] = selectedPassive.SalePercent;
        passiveIntVariables[1] = selectedPassive.DashCount;

        passiveFloatVariables[0] = selectedPassive.CoinRange;
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
                gameManager.stats[i] += stats[i];
            }

            for (int i = 0; i < passiveIntVariables.Length; i++)
            {
                gameManager.passiveIntVariables[i] += passiveIntVariables[i];
            }

            for(int i=0;i<passiveFloatVariables.Length;i++)
            {
                gameManager.passiveFloatVariables[i] += passiveFloatVariables[i];
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
