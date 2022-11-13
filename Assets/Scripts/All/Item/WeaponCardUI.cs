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

    Color LockImageColor;
    Color LockTextColor;

    [HideInInspector] public WeaponInfo selectedWeapon = null;

    [HideInInspector] public bool isLock = false;

    private void Start()
    {
        LockImageColor = lockBackImage.color;
        LockTextColor = lockText.color;
        Setting();
    }

    private void Update()
    {
        lockImage.gameObject.SetActive(isLock);
    }

    void Setting()
    {
        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = selectedWeapon.WeaponDamage.ToString();
        elementDamage.text = selectedWeapon.ElementDamage.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
    }

    public void Click()
    {
        if (ItemManager.Instance.weaponCount < 6)
        {
            ItemManager.Instance.GetWeaponInfo(selectedWeapon);
            Destroy(gameObject);
            isLock = false;
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
