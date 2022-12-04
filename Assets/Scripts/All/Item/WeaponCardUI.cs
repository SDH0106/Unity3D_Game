using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using static WeaponInfo;

public class WeaponCardUI : MonoBehaviour
{
    [SerializeField] public WeaponInfo[] weaponInfo;

    [Header("Lock")]
    [SerializeField] Image lockBackImage;
    [SerializeField] Image lockImage;
    [SerializeField] Text lockText;

    [Header("Card")]
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponPrice;
    [SerializeField] Text weaponGrade;

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public WeaponInfo selectedWeapon = null;

    [HideInInspector] public bool isLock = false;

    Color initPriceColor;

    GameManager gameManager;
    ItemManager itemManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;

        initPriceColor = weaponPrice.color;
        LockImageColor = lockBackImage.color;
        LockTextColor = lockText.color;
        Setting();
        CardImage();
        StartLockColor();
    }

    private void Update()
    {
        lockImage.gameObject.SetActive(isLock);

        if (gameManager.money < selectedWeapon.WeaponPrice)
            weaponPrice.color = Color.red;

        else if (gameManager.money >= selectedWeapon.WeaponPrice)
            weaponPrice.color = initPriceColor;
    }

    void Setting()
    {
        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = (selectedWeapon.WeaponDamage * (int)(selectedWeapon.weaponGrade + 1)).ToString();
        elementDamage.text = (selectedWeapon.MagicDamage * (int)(selectedWeapon.weaponGrade + 1)).ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = (selectedWeapon.WeaponPrice * (int)(selectedWeapon.weaponGrade + 1)).ToString();
        weaponGrade.text = selectedWeapon.weaponGrade.ToString();
    }

    void CardImage()
    {
        if (selectedWeapon.weaponGrade == Grade.�Ϲ�)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 0.8235f);
            cardBackLine.color = Color.black;
            weaponName.color = Color.white;
            weaponGrade.color = Color.white;
        }

        else if(selectedWeapon.weaponGrade == Grade.���)
        {
            cardBack.color = new Color(0, 0.77f, 1, 0.8235f);
            cardBackLine.color = Color.blue;
            weaponName.color = Color.blue;
            weaponGrade.color = Color.blue;
        }

        else if (selectedWeapon.weaponGrade == Grade.����)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
            cardBackLine.color = new Color(0.5f, 0, 0.5f, 1);
            weaponName.color = new Color(0.5f, 0, 0.5f, 1);
            weaponGrade.color = new Color(0.5f, 0, 0.5f, 1);
        }

        else if (selectedWeapon.weaponGrade == Grade.��ȭ)
        {
            cardBack.color = new Color(1, 0.31f, 0.31f, 0.8235f);
            cardBackLine.color = Color.red;
            weaponName.color = Color.red;
            weaponGrade.color = Color.red;
        }
    }

    public void Click()
    {
        if (itemManager.foolCount < 5 && gameManager.money >= selectedWeapon.WeaponPrice)
        {
            SoundManager.Instance.PlayES("WeaponSelect");
            gameManager.money -= (selectedWeapon.WeaponPrice * (int)(selectedWeapon.weaponGrade + 1));
            itemManager.foolCount++;
            itemManager.GetWeaponInfo(selectedWeapon);
            itemManager.weaponGrade[itemManager.weaponCount] = selectedWeapon.weaponGrade;
            Destroy(gameObject);
            isLock = false;
            Character.Instance.Equip();
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
