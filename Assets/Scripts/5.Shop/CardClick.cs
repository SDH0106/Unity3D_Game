using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardClick : MonoBehaviour
{
    [Header("Info")]
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

    [HideInInspector] public WeaponInfo selectedWeapon;

    int selectedNum;

    public void Setting(int num)
    {
        selectedNum = num;
        selectedWeapon = ItemManager.Instance.storedWeapon[num];

        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = (selectedWeapon.WeaponDamage * (int)(ItemManager.Instance.weaponGrade[num] + 1)).ToString();
        magicDamage.text = (selectedWeapon.MagicDamage * (int)(ItemManager.Instance.weaponGrade[num] + 1)).ToString();
        attackDelay.text = selectedWeapon.AttackDelay.ToString();
        bulletSpeed.text = selectedWeapon.BulletSpeed.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = (selectedWeapon.WeaponPrice * (int)(ItemManager.Instance.weaponGrade[num] + 1)).ToString();
        weaponGrade.text = selectedWeapon.weaponGrade.ToString();
    }

    public void CardImage(int num)
    {
        selectedNum = num;
        selectedWeapon = ItemManager.Instance.storedWeapon[num];

        if (ItemManager.Instance.weaponGrade[num] == Grade.일반)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            cardBackLine.color = Color.black;
            weaponName.color = Color.white;
            weaponGrade.color = Color.white;
        }

        else if (ItemManager.Instance.weaponGrade[num] == Grade.희귀)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 1f);
            cardBackLine.color = Color.blue;
            weaponName.color = new Color(0.5f, 0.8f, 1f, 1f);
            weaponGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (ItemManager.Instance.weaponGrade[num] == Grade.전설)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            weaponName.color = new Color(0.8f, 0.4f, 1f, 1f);
            weaponGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (ItemManager.Instance.weaponGrade[num] == Grade.신화)
        {
            cardBack.color = new Color(0.7f, 0.1f, 0.1f, 1f);
            cardBackLine.color = Color.red;
            weaponName.color = new Color(1f, 0.45f, 0.45f, 1f);
            weaponGrade.color = new Color(1f, 0.45f, 0.45f, 1f);
        }
    }

    public void SellWeapon()
    {
        if (ItemManager.Instance.foolCount > 0)
        {
            selectedWeapon = ItemManager.Instance.storedWeapon[selectedNum];
            GameManager.Instance.money += (selectedWeapon.WeaponPrice * (int)(ItemManager.Instance.weaponGrade[selectedNum] + 1));
            ItemManager.Instance.storedWeapon[selectedNum] = null;
            ItemManager.Instance.weaponGrade[selectedNum] = Grade.일반;
            ItemManager.Instance.foolCount--;
            Character.Instance.ReleaseEquip(selectedNum);
        }
    }

    public void CombineWeapon()
    {
        for (int i = 0; i < ItemManager.Instance.storedWeapon.Length; i++)
        {
            if (ItemManager.Instance.storedWeapon[i] != null)
            {
                if (i != selectedNum && ItemManager.Instance.weaponGrade[selectedNum] != Grade.신화)
                {
                    if ((selectedWeapon.WeaponName == ItemManager.Instance.storedWeapon[i].WeaponName) && (ItemManager.Instance.weaponGrade[selectedNum] == ItemManager.Instance.weaponGrade[i]))
                    {
                        GameManager.Instance.money -= (int)(ItemManager.Instance.weaponGrade[selectedNum] + 1) * 20;
                        ItemManager.Instance.weaponGrade[selectedNum]++;
                        ItemManager.Instance.storedWeapon[i] = null;
                        ItemManager.Instance.weaponGrade[i] = Grade.일반;
                        ItemManager.Instance.foolCount--;
                        Character.Instance.ReleaseEquip(i);
                        break;
                    }
                }
            }
        }
    }
}
