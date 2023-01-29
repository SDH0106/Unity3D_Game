using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.TextCore.Text;
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
    [SerializeField] Text magicDamage;
    [SerializeField] Text attackDelay;
    [SerializeField] Text bulletSpeed;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponPrice;
    [SerializeField] Text weaponGrade;
    [SerializeField] Text description;

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public WeaponInfo selectedWeapon;

    [HideInInspector] public bool isLock;

    Color initPriceColor;

    GameManager gameManager;
    ItemManager itemManager;

    int price;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;

        initPriceColor = weaponPrice.color;
        LockImageColor = new Color(0.17f, 0.17f, 0.17f);
        LockTextColor = Color.white;

        Setting();
        CardColor();
        StartLockColor();
    }

    private void Update()
    {
        lockImage.gameObject.SetActive(isLock);

        if (gameManager.money < price * (int)(selectedWeapon.weaponGrade + 1))
            weaponPrice.color = Color.red;

        else if (gameManager.money >= price * (int)(selectedWeapon.weaponGrade + 1))
            weaponPrice.color = initPriceColor;
    }

    void Setting()
    {
        price = Mathf.CeilToInt(selectedWeapon.WeaponPrice * (1 - gameManager.salePercent));
        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = (selectedWeapon.WeaponDamage * (int)(selectedWeapon.weaponGrade + 1)).ToString();
        magicDamage.text = (selectedWeapon.MagicDamage * (int)(selectedWeapon.weaponGrade + 1)).ToString();
        attackDelay.text = selectedWeapon.AttackDelay.ToString();
        bulletSpeed.text = selectedWeapon.BulletSpeed.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = (price * (int)(selectedWeapon.weaponGrade + 1)).ToString();
        weaponGrade.text = selectedWeapon.weaponGrade.ToString();
        description.text = selectedWeapon.Description.ToString();
    }

    void CardColor()
    {
        if (selectedWeapon.weaponGrade == Grade.일반)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 0.8235f);
            cardBackLine.color = Color.black;
            weaponName.color = Color.white;
            weaponGrade.color = Color.white;
        }

        else if(selectedWeapon.weaponGrade == Grade.희귀)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 0.8235f);
            cardBackLine.color = Color.blue;
            weaponName.color = new Color(0.5f, 0.8f, 1f, 1f);
            weaponGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (selectedWeapon.weaponGrade == Grade.전설)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            weaponName.color = new Color(0.8f, 0.4f, 1f, 1f);
            weaponGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (selectedWeapon.weaponGrade == Grade.신화)
        {
            cardBack.color = new Color(0.7f, 0.1f, 0.1f, 0.8235f);
            cardBackLine.color = Color.red;
            weaponName.color = new Color(1f, 0.45f, 0.45f, 1f);
            weaponGrade.color = new Color(1f, 0.45f, 0.45f, 1f);
        }
    }

    public void Click()
    {
        bool canSwordBuy = false;

        if (selectedWeapon.Type == WEAPON_TYPE.검)
        {
            if (itemManager.foolCount < 5 && gameManager.money >= (price * (int)(selectedWeapon.weaponGrade + 1)))
            {
                canSwordBuy = true;
                SoundManager.Instance.PlayES("WeaponSelect");
                gameManager.money -= (price * (int)(selectedWeapon.weaponGrade + 1));
                itemManager.foolCount++;
                itemManager.GetWeaponInfo(selectedWeapon);
                itemManager.weaponGrade[itemManager.weaponCount] = selectedWeapon.weaponGrade;
                Destroy(gameObject);
                isLock = false;
                Character.Instance.Equip();
            }

            else if (itemManager.foolCount >= 5 && gameManager.money >= (price * (int)(selectedWeapon.weaponGrade + 1)))
            {
                for (int i = 0; i < itemManager.storedWeapon.Length; i++)
                {
                    if (itemManager.storedWeapon[i] != null && selectedWeapon.weaponGrade != Grade.신화)
                    {
                        if ((selectedWeapon.WeaponName == itemManager.storedWeapon[i].WeaponName) && (selectedWeapon.weaponGrade == itemManager.weaponGrade[i]))
                        {
                            canSwordBuy = true;
                            SoundManager.Instance.PlayES("WeaponSelect");
                            gameManager.money -= (int)(selectedWeapon.weaponGrade + 1) * 20;
                            gameManager.money -= (price * (int)(selectedWeapon.weaponGrade + 1));
                            itemManager.weaponGrade[i]++;
                            Destroy(gameObject);
                            isLock = false;
                            break;
                        }
                    }
                }
            }

            if(!canSwordBuy)
                SoundManager.Instance.PlayES("CantBuy");
        }

        // 검신캐릭터는 검외엔 끼지 못하게
        else if (selectedWeapon.Type != WEAPON_TYPE.검)
        {
            bool canBuy = false;
            if (Character.Instance.characterNum != (int)CHARACTER_NUM.Legendary)
            {
                if (itemManager.foolCount < 5 && gameManager.money >= (price * (int)(selectedWeapon.weaponGrade + 1)))
                {
                    canBuy = true;
                    SoundManager.Instance.PlayES("WeaponSelect");
                    gameManager.money -= (price * (int)(selectedWeapon.weaponGrade + 1));
                    itemManager.foolCount++;
                    itemManager.GetWeaponInfo(selectedWeapon);
                    itemManager.weaponGrade[itemManager.weaponCount] = selectedWeapon.weaponGrade;
                    Destroy(gameObject);
                    isLock = false;
                    Character.Instance.Equip();
                }

                else if (itemManager.foolCount >= 5 && gameManager.money >= (price * (int)(selectedWeapon.weaponGrade + 1)))
                {
                    for (int i = 0; i < itemManager.storedWeapon.Length; i++)
                    {
                        if (itemManager.storedWeapon[i] != null && selectedWeapon.weaponGrade != Grade.신화)
                        {
                            if ((selectedWeapon.WeaponName == itemManager.storedWeapon[i].WeaponName) && (selectedWeapon.weaponGrade == itemManager.weaponGrade[i]))
                            {
                                canBuy = true;
                                SoundManager.Instance.PlayES("WeaponSelect");
                                gameManager.money -= (int)(selectedWeapon.weaponGrade + 1) * 20;
                                gameManager.money -= (price * (int)(selectedWeapon.weaponGrade + 1));
                                itemManager.weaponGrade[i]++;
                                Destroy(gameObject);
                                isLock = false;
                                break;
                            }
                        }
                    }
                }

                if (!canBuy)
                    SoundManager.Instance.PlayES("CantBuy");
            }

            else if (Character.Instance.characterNum == (int)CHARACTER_NUM.Legendary)
                SoundManager.Instance.PlayES("CantBuy");
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
