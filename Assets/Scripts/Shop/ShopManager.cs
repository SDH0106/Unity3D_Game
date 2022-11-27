using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : Singleton<ShopManager>
{
    [Header("UI")]
    [SerializeField] Text round;
    [SerializeField] Text money;
    [SerializeField] Text rerollMoneyText;
    [SerializeField] public GameObject backgroundImage;

    [Header("Stat")]
    [SerializeField] Text maxHp;
    [SerializeField] Text reHp;
    [SerializeField] Text apHp;
    [SerializeField] Text def;
    [SerializeField] Text wAtk;
    [SerializeField] Text eAtk;
    [SerializeField] Text aSpd;
    [SerializeField] Text spd;
    [SerializeField] Text ran;
    [SerializeField] Text luk;

    [Header("Prefabs")]
    [SerializeField] Transform cardsParent;
    [SerializeField] GameObject weaponCardUI;
    [SerializeField] GameObject passiveCardUI;
    [SerializeField] GameObject clickUI;


    [Header("Slots")]
    [SerializeField] public GameObject[] weaponSlots;
    [SerializeField] GameObject[] passiveSlots;

    [HideInInspector] public int[] passiveItem;

    GameObject[] cards;
    int[] num;

    bool[] bools;

    int rerollMoney;

    Color initPriceColor;

    [HideInInspector] public int WeaponSlotCount = 0;

    GameManager gameManager;
    ItemManager itemManager;

    float[] weightWeaponValue;
    float[] weightPassiveValue;

    private void Start()
    {
        weightWeaponValue = new float[4];
        weightPassiveValue = new float[4];
        SoundManager.Instance.PlayBGM(2);
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        initPriceColor = rerollMoneyText.color;
        clickUI.gameObject.SetActive(false);
        rerollMoney = -gameManager.round;
        bools = new bool[4];
        num = new int[4];
        cards = new GameObject[4];
        passiveItem = new int[passiveSlots.Length];

        CardSlot();
    }

    private void Update()
    {
        round.text = gameManager.round.ToString();
        money.text = gameManager.money.ToString();
        rerollMoneyText.text = rerollMoney.ToString();

        if (gameManager.money < -rerollMoney)
            rerollMoneyText.color = Color.red;

        else if (gameManager.money >= -rerollMoney)
            rerollMoneyText.color = initPriceColor;

        if (gameManager.currentScene == "Shop")
        {
            gameObject.SetActive(true);
            SettingStatText();

            CheckLock();
            WeaponSlot();
            PassiveSlot();
            Refill();
        }

        else if (gameManager.currentScene == "Game")
        {
            gameObject.SetActive(false);
            rerollMoney = 0;
        }
    }

    void SettingStatText()
    {
        maxHp.text = gameManager.maxHp.ToString();
        reHp.text = gameManager.recoverHp.ToString();
        apHp.text = gameManager.absorbHp.ToString();
        def.text = gameManager.defence.ToString();
        wAtk.text = gameManager.physicDamage.ToString();
        eAtk.text = gameManager.elementDamage.ToString();
        aSpd.text = gameManager.attackSpeed.ToString();
        spd.text = gameManager.speed.ToString();
        ran.text = gameManager.range.ToString();
        luk.text = gameManager.luck.ToString();
    }

    public void ToGameScene()
    {
        gameManager.currentScene = "Game";
        SceneManager.LoadScene("Game");
        Character.Instance.transform.position = Vector3.zero;
        gameManager.round++;
        rerollMoney = -gameManager.round;
        gameManager.hp = gameManager.maxHp;
    }

    void ImageAlphaChange(int i, int a, Image image)
    {
        Color imageColor = image.color;
        imageColor.a = a;
        image.color = imageColor;
    }

    void TextAlphaChange(int i, int a, Text text)
    {
        Color textColor = text.color;
        textColor.a = a;
        text.color = textColor;
    }

    public void CloseUI()
    {
        backgroundImage.SetActive(false);
        clickUI.gameObject.SetActive(false);
    }

    void Refill()
    {
        int refillNum = 0;

        for (int i = 0; i < 4; i++)
        {
            if (cardsParent.GetChild(i).childCount == 0)
            {
                refillNum++;
            }
        }

        if (refillNum == 4)
            CardSlot();
    }

    public void Reroll()
    {
        SoundManager.Instance.PlayES("SelectButton");

        if (gameManager.money >= -rerollMoney)
        {
            for (int i = 0; i < cardsParent.childCount; i++)
            {
                if (cards[i] != null)
                {
                    if (bools[i] == false)
                        Destroy(cardsParent.GetChild(i).GetChild(0).gameObject);
                }
            }

            gameManager.money += rerollMoney;

            rerollMoney -= Mathf.CeilToInt((float)(gameManager.round) / 2);

            CardSlot();
        }
    }

    void CheckLock()
    {
        for (int i = 0; i < cardsParent.childCount; i++)
        {
            if (cards[i] != null)
            {
                if (num[i] == 0)
                {
                    bools[i] = cards[i].GetComponent<WeaponCardUI>().isLock;
                }

                else if (num[i] == 1)
                {
                    bools[i] = cards[i].GetComponent<PassiveCardUI>().isLock;
                }
            }
        }
    }

    void CardSlot()
    {
        for (int i = 0; i < 4; i++)
        {
            if (bools[i] == false)
            {
                int rand = UnityEngine.Random.Range(0, 100);

                if (rand >= 80)
                {
                    GetRandomWeaponCard();
                    GameObject instant = Instantiate(weaponCardUI, cardsParent.GetChild(i).transform);
                    cards[i] = instant;
                    num[i] = 0;
                    instant.transform.SetParent(cardsParent.GetChild(i));
                }

                else if (rand < 80)
                {
                    GetRandomPassiveCard();
                    GameObject instant = Instantiate(passiveCardUI, cardsParent.GetChild(i).transform);
                    cards[i] = instant;
                    num[i] = 1;
                    instant.transform.SetParent(cardsParent.GetChild(i));
                }
            }

        }
    }

    void WeaponSlot()
    {
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            Image image = weaponSlots[i].transform.GetChild(2).GetComponent<Image>();
            Text text = weaponSlots[i].transform.GetChild(3).GetComponent<Text>();
            Text x = weaponSlots[i].transform.GetChild(4).GetComponent<Text>();

            TextAlphaChange(i, 0, text);
            TextAlphaChange(i, 0, x);

            if (itemManager.storedWeapon[i] != null)
            {
                ImageAlphaChange(i, 1, image);
                image.sprite = itemManager.storedWeapon[i].ItemSprite;
            }


            else if (itemManager.storedWeapon[i] == null)
            {
                ImageAlphaChange(i, 0, image);
            }
        }
    }

    void PassiveSlot()
    {
        for (int i = 0; i < passiveSlots.Length; i++)
        {
            Image back = passiveSlots[i].transform.GetChild(0).GetComponent<Image>();
            Image image = passiveSlots[i].transform.GetChild(2).GetComponent<Image>();
            Text text = passiveSlots[i].transform.GetChild(3).GetComponent<Text>();
            Text x = passiveSlots[i].transform.GetChild(4).GetComponent<Text>();

            if (itemManager.storedPassive[i] != null)
            {
                ImageAlphaChange(i, 1, image);
                image.sprite = itemManager.storedPassive[i].ItemSprite;
                text.text = passiveItem[i].ToString();

                if (itemManager.storedPassive[i].ItemGrade == Grade.�Ϲ�)
                {
                    back.color = new Color(0.53f, 0.53f, 0.53f, 0.8235f);
                }

                else if (itemManager.storedPassive[i].ItemGrade == Grade.���)
                {
                    back.color = new Color(0, 0.77f, 1, 0.8235f);
                }

                else if (itemManager.storedPassive[i].ItemGrade == Grade.����)
                {
                    back.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
                }

                else if (itemManager.storedPassive[i].ItemGrade == Grade.��ȭ)
                {
                    back.color = new Color(1, 0.31f, 0.31f, 0.8235f);
                }

                if (passiveItem[i] > 1)
                {
                    TextAlphaChange(i, 1, text);
                    TextAlphaChange(i, 1, x);
                }

                else if (passiveItem[i] <= 1)
                {
                    TextAlphaChange(i, 0, text);
                    TextAlphaChange(i, 0, x);
                }
            }

            else if (itemManager.storedPassive[i] == null)
            {
                back.color = new Color(0.53f, 0.53f, 0.53f, 0.8235f);
                ImageAlphaChange(i, 0, image);
                TextAlphaChange(i, 0, text);
                TextAlphaChange(i, 0, x);
            }
        }
    }

    Grade RandomWeaponGrade()
    {
        float totalWeight = 0;

        weightWeaponValue[0] = 150 - (gameManager.round - 1) * 5;
        weightWeaponValue[1] = 10 + (gameManager.round - 1) * 20;
        weightWeaponValue[2] = (gameManager.round - 1);
        weightWeaponValue[3] = (gameManager.round - 1) / 2;

        for (int i = 0; i < weightWeaponValue.Length; i++)
        {
            totalWeight += weightWeaponValue[i];
        }

        float rand = Random.Range(0, totalWeight);
        float gradeNum = 0;
        float total = 0;

        for (int i = 0; i < weightWeaponValue.Length; i++)
        {
            total += weightWeaponValue[i];
            if (rand <= total)
            {
                gradeNum = i;
                break;
            }
        }

        Grade grade;

        if(gradeNum == 1)
            grade = Grade.���;
        else if (gradeNum == 2)
            grade = Grade.����;
        else if (gradeNum == 3)
            grade = Grade.��ȭ;
        else
            grade = Grade.�Ϲ�;

        return grade;
    }

    void GetRandomWeaponCard()
    {
        WeaponCardUI weaponCard = weaponCardUI.GetComponent<WeaponCardUI>();
        int rand = UnityEngine.Random.Range(0, weaponCardUI.GetComponent<WeaponCardUI>().weaponInfo.Length);
        weaponCard.selectedWeapon.weaponGrade = RandomWeaponGrade();
        weaponCard.selectedWeapon = weaponCard.weaponInfo[rand];
    }

    void GetRandomPassiveCard()
    {
        float totalWeight = 0;

        weightPassiveValue[0] = 150 - (gameManager.round - 1) * 6;
        weightPassiveValue[1] = 10 + (gameManager.round - 1) * 20;
        weightPassiveValue[2] = (gameManager.round - 1);
        weightPassiveValue[3] = (gameManager.round - 1) / 2;

        PassiveCardUI passiveCard = passiveCardUI.GetComponent<PassiveCardUI>();

        for (int i = 0; i < passiveCardUI.GetComponent<PassiveCardUI>().passiveInfo.Length; i++)
        {
            passiveCard.passiveInfo[i].weight = weightPassiveValue[(int)passiveCard.passiveInfo[i].ItemGrade];
            totalWeight += passiveCard.passiveInfo[i].weight;
        }

        float rand = Random.Range(0, totalWeight);
        float total = 0;

        for (int i = 0; i < passiveCardUI.GetComponent<PassiveCardUI>().passiveInfo.Length; i++)
        {
            Debug.Log(passiveCard.passiveInfo[i].weight);
            total += passiveCard.passiveInfo[i].weight;

            if (rand <= total)
            {
                passiveCard.selectedPassive = passiveCard.passiveInfo[i];
                break;
            }
        }
    }
}
