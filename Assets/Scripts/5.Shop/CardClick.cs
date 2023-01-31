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

    ItemManager itemManager;

    private void Start()
    {
        itemManager = ItemManager.Instance;

        sellCheckPanel.SetActive(false);
        combineCheckPanel.SetActive(false);
        cantSellPanel.SetActive(false);
        cantCombinePanel.SetActive(false);
    }

    public void Setting(int num)
    {
        itemManager = ItemManager.Instance;
        selectedNum = num;
        selectedWeapon = itemManager.storedWeapon[selectedNum];

        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = (selectedWeapon.WeaponDamage * (int)(itemManager.weaponGrade[num] + 1)).ToString();
        magicDamage.text = (selectedWeapon.MagicDamage * (int)(itemManager.weaponGrade[num] + 1)).ToString();
        attackDelay.text = selectedWeapon.AttackDelay.ToString();
        bulletSpeed.text = selectedWeapon.BulletSpeed.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponPrice.text = (selectedWeapon.WeaponPrice * (int)(itemManager.weaponGrade[num] + 1)).ToString();
        weaponGrade.text = itemManager.weaponGrade[num].ToString();
        sellPrice.text = (selectedWeapon.WeaponPrice * (int)(itemManager.weaponGrade[selectedNum] + 1)).ToString();
        description.text = selectedWeapon.Description.ToString();

        if (selectedWeapon.Type == WEAPON_TYPE.검)
            attackTypes.text = "(물리/근거리)";

        else if (selectedWeapon.Type == WEAPON_TYPE.총)
            attackTypes.text = "(물리/원거리)";

        else if (selectedWeapon.Type == WEAPON_TYPE.스태프)
            attackTypes.text = "(마법/원거리)";
    }

    public void CardImage(int num)
    {
        selectedNum = num;
        selectedWeapon = itemManager.storedWeapon[selectedNum];

        if (itemManager.weaponGrade[num] == Grade.일반)
        {
            cardBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            descriptBack.color = new Color(0.142f, 0.142f, 0.142f, 1f);
            cardBackLine.color = Color.black;
            descriptBackLine.color = Color.black;
            weaponName.color = Color.white;
            weaponGrade.color = Color.white;
        }

        else if (itemManager.weaponGrade[num] == Grade.희귀)
        {
            cardBack.color = new Color(0f, 0.6f, 0.8f, 1f);
            descriptBack.color = new Color(0f, 0.6f, 0.8f, 1f);
            cardBackLine.color = Color.blue;
            descriptBackLine.color = Color.blue;
            weaponName.color = new Color(0.5f, 0.8f, 1f, 1f);
            weaponGrade.color = new Color(0.5f, 0.8f, 1f, 1f);
        }

        else if (itemManager.weaponGrade[num] == Grade.전설)
        {
            cardBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            descriptBack.color = new Color(0.5f, 0.2f, 0.4f, 1f);
            cardBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            descriptBackLine.color = new Color(0.5f, 0f, 0.5f, 1f);
            weaponName.color = new Color(0.8f, 0.4f, 1f, 1f);
            weaponGrade.color = new Color(0.8f, 0.4f, 1f, 1f);
        }

        else if (itemManager.weaponGrade[num] == Grade.신화)
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
        itemManager = ItemManager.Instance;

        if (itemManager.fullCount > 0)
        {
            selectedWeapon = itemManager.storedWeapon[selectedNum];
            GameManager.Instance.money += (selectedWeapon.WeaponPrice * (int)(itemManager.weaponGrade[selectedNum] + 1));
            itemManager.storedWeapon[selectedNum] = null;
            itemManager.weaponGrade[selectedNum] = Grade.일반;
            itemManager.fullCount--;
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
        itemManager = ItemManager.Instance;
        bool canCombine = false;

        for (int i = 0; i < itemManager.storedWeapon.Length; i++)
        {
            if (itemManager.storedWeapon[i] != null)
            {
                if (i != selectedNum && itemManager.weaponGrade[selectedNum] != Grade.신화)
                {
                    if ((selectedWeapon.WeaponName == itemManager.storedWeapon[i].WeaponName) && (itemManager.weaponGrade[selectedNum] == itemManager.weaponGrade[i]))
                    {
                        canCombine = true;
                        GameManager.Instance.money -= (int)(itemManager.weaponGrade[selectedNum] + 1) * 20;
                        itemManager.weaponGrade[selectedNum]++;
                        itemManager.storedWeapon[i] = null;
                        itemManager.weaponGrade[i] = Grade.일반;
                        itemManager.fullCount--;
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
