using System;
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
    [SerializeField] Text round;
    [SerializeField] Text money;
    [SerializeField] Text rerollMoneyText;
    [SerializeField] Transform cardsParent;
    [SerializeField] GameObject weaponCardUI;
    [SerializeField] GameObject passiveCardUI;

    [Header("")]
    [SerializeField] public GameObject backgroundImage;
    [SerializeField] GameObject sellUI;

    [Header("")]
    [SerializeField] public GameObject[] weaponSlots;
    [SerializeField] GameObject[] passiveSlots;

    [HideInInspector] public int[] passiveItem;

    PassiveInfo[] storedPassive;

    GameObject[] cards;
    int[] num;

    bool[] bools;

    int rerollMoney;

    Color initPriceColor;

    [HideInInspector] public int WeaponSlotCount = 0;

    private void Start()
    {
        initPriceColor = rerollMoneyText.color;
        sellUI.gameObject.SetActive(false);
        rerollMoney = -GameManager.Instance.round;
        bools = new bool[4];
        num = new int[4];
        cards = new GameObject[4];
        passiveItem = new int[passiveSlots.Length];
        storedPassive = ItemManager.Instance.storedPassive;

        CardSlot();
    }

    private void Update()
    {
        round.text = GameManager.Instance.round.ToString();
        money.text = GameManager.Instance.money.ToString();
        rerollMoneyText.text = rerollMoney.ToString();

        if (GameManager.Instance.money < -rerollMoney)
            rerollMoneyText.color = Color.red;

        else if (GameManager.Instance.money >= -rerollMoney)
            rerollMoneyText.color = initPriceColor;

        if (GameManager.Instance.currentScene == "Shop")
        {
            gameObject.SetActive(true);

            CheckLock();
            WeaponSlot();
            PassiveSlot();
            Refill();
        }

        else if (GameManager.Instance.currentScene == "Game")
        {
            gameObject.SetActive(false);
            rerollMoney = 0;
        }
    }

    public void ToGameScene()
    {
        GameManager.Instance.currentScene = "Game";
        SceneManager.LoadScene("Game");
        //Character.Instance.transform.position = Vector3.zero;
        GameManager.Instance.round++;
        rerollMoney = -GameManager.Instance.round;
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
        sellUI.gameObject.SetActive(false);
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
        if (GameManager.Instance.money >= -rerollMoney)
        {
            for (int i = 0; i < cardsParent.childCount; i++)
            {
                if (cards[i] != null)
                {
                    if (bools[i] == false)
                        Destroy(cardsParent.GetChild(i).GetChild(0).gameObject);
                }
            }

            rerollMoney -= (GameManager.Instance.round) / 2;
            GameManager.Instance.money += rerollMoney;

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
                    GameObject instance = Instantiate(weaponCardUI, cardsParent.GetChild(i).transform);
                    cards[i] = instance;
                    num[i] = 0;
                    instance.transform.SetParent(cardsParent.GetChild(i));
                }

                else if (rand < 80)
                {
                    GetRandomPassiveCard();
                    GameObject instance = Instantiate(passiveCardUI, cardsParent.GetChild(i).transform);
                    cards[i] = instance;
                    num[i] = 1;
                    instance.transform.SetParent(cardsParent.GetChild(i));
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

            if (ItemManager.Instance.storedWeapon[i] != null)
            {
                ImageAlphaChange(i, 1, image);
                image.sprite = ItemManager.Instance.storedWeapon[i].ItemSprite;
            }


            else if (ItemManager.Instance.storedWeapon[i] == null)
            {
                ImageAlphaChange(i, 0, image);
            }
        }
    }

    void PassiveSlot()
    {
        for (int i = 0; i < passiveSlots.Length; i++)
        {
            Image image = passiveSlots[i].transform.GetChild(2).GetComponent<Image>();
            Text text = passiveSlots[i].transform.GetChild(3).GetComponent<Text>();
            Text x = passiveSlots[i].transform.GetChild(4).GetComponent<Text>();

            if (ItemManager.Instance.storedPassive[i] != null)
            {
                ImageAlphaChange(i, 1, image);
                image.sprite = ItemManager.Instance.storedPassive[i].ItemSprite;
                text.text = passiveItem[i].ToString();

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

            else if (ItemManager.Instance.storedPassive[i] == null)
            {
                ImageAlphaChange(i, 0, image);
                TextAlphaChange(i, 0, text);
                TextAlphaChange(i, 0, x);
            }
        }
    }

    void GetRandomWeaponCard()
    {
        WeaponCardUI weaponCard = weaponCardUI.GetComponent<WeaponCardUI>();
        int rand = UnityEngine.Random.Range(0, weaponCardUI.GetComponent<WeaponCardUI>().weaponInfo.Length);
        weaponCard.selectedWeapon= weaponCard.weaponInfo[rand];
    }

    void GetRandomPassiveCard()
    {
        PassiveCardUI passiveCard = passiveCardUI.GetComponent<PassiveCardUI>();
        int rand = UnityEngine.Random.Range(0, passiveCardUI.GetComponent<PassiveCardUI>().passiveInfo.Length);
        passiveCard.selectedPassive= passiveCard.passiveInfo[rand];
    }
}
