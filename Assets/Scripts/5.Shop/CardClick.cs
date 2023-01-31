using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static WeaponInfo;

public class CardClick : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text attackTypes;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text magicDamage;
    [SerializeField] Text attackDelay;
    [SerializeField] Text bulletSpeed;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponPrice;
    [SerializeField] Text weaponGrade;
    [SerializeField] Text sellPrice;
    [SerializeField] Image descriptBack;
    [SerializeField] Image descriptBackLine;
    [SerializeField] Text description;

    [Header("Panel")]
    [SerializeField] GameObject sellCheckPanel;
    [SerializeField] GameObject combineCheckPanel;
    [SerializeField] GameObject cantSellPanel;
    [SerializeField] GameObject cantCombinePanel;

    [HideInInspector] public WeaponInfo selectedWeapon;

    int selectedNum;

    private void Start()
    {
        sellCheckPanel.SetActive(false);
        combineCheckPanel.SetActive(false);
        cantSellPanel.SetActive(false);
        cantCombinePanel.SetActive(false);
    }

    public void Setting(int num)
    {
        selectedNum = num;
        selectedWeapon = ItemManager.Instance.storedWeapon[selectedNum];

        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = (selectedWeapon.WeaponDamage * (int)(ItemManager.Instance.weaponGrade[num] + 1)).ToString();
        magicDamage.text = (selectedWeapon.MagicDamage * (int)(ItemManager.Instance.weaponGrade[num] + 1)).ToString();
        attackDelay.text = selectedWeapon.AttackDelay.ToString();
        bulletSpeed.text = selectedWeapon.BulletSpeed.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = (selectedWeapon.WeaponPrice * (int)(ItemManager.Instance.weaponGrade[num] + 1)).ToString();
        weaponGrade.text = ItemManager.Instance.weaponGrade[num].ToString();
        sellPrice.text = (selectedWeapon.WeaponPrice * (int)(ItemManager.Instance.weaponGrade[selectedNum] + 1)).ToString();
        description.text = selectedWeapon.Description.ToString();

        if (selectedWeapon.Type == WEAPON_TYPE.��)
            attackTypes.text = "(����/�ٰŸ�)";

        else if (selectedWeapon.Type == WEAPON_TYPE.��)
            attackTypes.text = "(����/���Ÿ�)";

        else if (selectedWeapon.Type == WEAPON_TYPE.������)
            attackTypes.text = "(����/���Ÿ�)";
    }

    public void CardImage(int num)
    {
        selectedNum = num;
        selectedWeapon = ItemManager.Instance.storedWeapon[selectedNum];

        if (ItemManager.Instance.weaponGrade[num] == Grade.�Ϲ�)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            descriptBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            cardBackLine.color = Color.black;
            descriptBackLine.color = Color.black;
            weaponName.color = Color.white;
            weaponGrade.color = Color.white;
        }

        else if (ItemManager.Instance.weaponGrade[num] == Grade.���)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 1f);
            descriptBack.color = new Color(0f, 0.6f, 0.8f, 1f);
            cardBackLine.color = Color.blue;
            descriptBackLine.color = Color.blue;
            weaponName.color = new Color(0.5f, 0.8f, 1f, 1f);
            weaponGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (ItemManager.Instance.weaponGrade[num] == Grade.����)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            descriptBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            descriptBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            weaponName.color = new Color(0.8f, 0.4f, 1f, 1f);
            weaponGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (ItemManager.Instance.weaponGrade[num] == Grade.��ȭ)
        {
            cardBack.color = new Color(0.7f, 0.1f, 0.1f, 1f);
            descriptBack.color = new Color(0.7f, 0.1f, 0.1f, 1f);
            cardBackLine.color = Color.red;
            descriptBackLine.color = Color.red;
            weaponName.color = new Color(1f, 0.45f, 0.45f, 1f);
            weaponGrade.color = new Color(1f, 0.45f, 0.45f, 1f);
        }
    }

    public void SellWeapon()
    {
        if (ItemManager.Instance.fullCount > 0)
        {
            selectedWeapon = ItemManager.Instance.storedWeapon[selectedNum];
            GameManager.Instance.money += (selectedWeapon.WeaponPrice * (int)(ItemManager.Instance.weaponGrade[selectedNum] + 1));
            ItemManager.Instance.storedWeapon[selectedNum] = null;
            ItemManager.Instance.weaponGrade[selectedNum] = Grade.�Ϲ�;
            ItemManager.Instance.fullCount--;
            Character.Instance.ReleaseEquip(selectedNum);
            ShopManager.Instance.backgroundImage.SetActive(true);
            gameObject.SetActive(false);
        }

        else
        {
            SoundManager.Instance.PlayES("CantBuy");
            cantSellPanel.SetActive(true);
        }
    }

    public void CombineWeapon()
    {
        bool canCombine = false;

        for (int i = 0; i < ItemManager.Instance.storedWeapon.Length; i++)
        {
            if (ItemManager.Instance.storedWeapon[i] != null)
            {
                if (i != selectedNum && ItemManager.Instance.weaponGrade[selectedNum] != Grade.��ȭ)
                {
                    if ((selectedWeapon.WeaponName == ItemManager.Instance.storedWeapon[i].WeaponName) && (ItemManager.Instance.weaponGrade[selectedNum] == ItemManager.Instance.weaponGrade[i]))
                    {
                        canCombine = true;
                        GameManager.Instance.money -= (int)(ItemManager.Instance.weaponGrade[selectedNum] + 1) * 20;
                        ItemManager.Instance.weaponGrade[selectedNum]++;
                        ItemManager.Instance.storedWeapon[i] = null;
                        ItemManager.Instance.weaponGrade[i] = Grade.�Ϲ�;
                        ItemManager.Instance.fullCount--;
                        Character.Instance.ReleaseEquip(i);
                        break;
                    }
                }
            }
        }

        if (canCombine)
        {
            gameObject.SetActive(false);
            ShopManager.Instance.backgroundImage.SetActive(true);
        }

        else if (!canCombine)
        {
            SoundManager.Instance.PlayES("CantBuy");
            cantCombinePanel.SetActive(true);
        }
    }
}
