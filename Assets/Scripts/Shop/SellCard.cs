using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SellCard : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponPrice;

    [HideInInspector] public WeaponInfo selectedWeapon;

    int selectedNum;

    public void Setting(int num)
    {
        selectedNum = num;
        selectedWeapon = ItemManager.Instance.storedWeapon[num];

        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = selectedWeapon.WeaponDamage.ToString();
        elementDamage.text = selectedWeapon.MagicDamage.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = selectedWeapon.WeaponPrice.ToString();
    }

    public void SellWeapon()
    {
        if (ItemManager.Instance.foolCount > 0)
        {
            selectedWeapon = ItemManager.Instance.storedWeapon[selectedNum];
            GameManager.Instance.money += selectedWeapon.WeaponPrice;
            ItemManager.Instance.storedWeapon[selectedNum] = null;
            ItemManager.Instance.foolCount--;
            Character.Instance.ReleaseEquip(selectedNum);
        }
    }
}
