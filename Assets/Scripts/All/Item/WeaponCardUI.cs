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

    [SerializeField] Image lockBackImage;
    [SerializeField] Image lockImage;
    [SerializeField] Text lockText;

    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponPrice;

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
        weaponDamage.text = selectedWeapon.WeaponDamage.ToString();
        elementDamage.text = selectedWeapon.MagicDamage.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = selectedWeapon.WeaponPrice.ToString();
    }

    public void Click()
    {
        if (itemManager.foolCount < 5 && gameManager.money >= selectedWeapon.WeaponPrice)
        {
            SoundManager.Instance.PlayES("WeaponSelect");
            gameManager.money -= selectedWeapon.WeaponPrice;
            itemManager.foolCount++;
            itemManager.GetWeaponInfo(selectedWeapon);
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
}
