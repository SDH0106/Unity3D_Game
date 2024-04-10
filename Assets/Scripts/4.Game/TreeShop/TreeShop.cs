using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class TreeShop : Singleton<TreeShop>
{
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] Transform weaponParent;
    [SerializeField] Text upgradeCostText;

    ItemManager itemManager;

    [HideInInspector] public int upgradeCost;

    void Start()
    {
        itemManager = ItemManager.Instance;
        upgradeCost = 10;

        upgradeCostText.text = (-upgradeCost).ToString();

        for (int i = 0; i < itemManager.storedWeapon.Length; i++)
        {
            if (itemManager.storedWeapon[i] != null && itemManager.weaponGrade[i] != Grade.½ÅÈ­)
            {
                TreeShopWeaponInfo weapon;
                weapon = Instantiate(weaponPrefab, weaponParent).GetComponent<TreeShopWeaponInfo>();
                weapon.siblingNum = i;
            }
        }
    }

}
