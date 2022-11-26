using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using static WeaponInfo;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] public DamageUI[] damageUI;
    [SerializeField] public Transform coinStorage;

    [HideInInspector] public PassiveInfo passiveCardItem;

    [HideInInspector] public WeaponInfo[] storedWeapon;
    [HideInInspector] public PassiveInfo[] storedPassive;

    [HideInInspector] public int weaponCount;
    int passiveItemCount;

    [HideInInspector] public bool isFool = false;
    [HideInInspector] public int foolCount;

    public Grade[] weaponGrade;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        weaponGrade = new Grade[6];

        foolCount = 0;
        weaponCount = 0;
        passiveItemCount = 0;
        storedWeapon = new WeaponInfo[6];
        storedPassive = new PassiveInfo[30];
    }

    public void GetWeaponInfo(WeaponInfo weaponInfo)
    {
        if (foolCount > 5)
        {
            isFool = true;
        }

        else if (foolCount <= 5)
            isFool = false;

        if (isFool == false)
        {
            for (int i = 0; i < storedWeapon.Length; i++)
            {
                if (storedWeapon[i] == null)
                {
                    weaponCount = i;
                    break;
                }
            }
            storedWeapon[weaponCount] = weaponInfo;
        }
    }

    public void GetPassiveInfo(PassiveInfo passiveInfo)
    {
        passiveCardItem = passiveInfo;
        storedPassive[passiveItemCount] = passiveInfo;

        if (passiveItemCount == 0)
        {
            ShopManager.Instance.passiveItem[passiveItemCount]++;
        }

        else if (passiveItemCount > 0)
        {
            CheckItemEquel();
        }

        passiveItemCount++;
    }

    void CheckItemEquel()
    {
        for (int i = 0; i < passiveItemCount; i++)
        {
            if (storedPassive[passiveItemCount] == storedPassive[i])
            {
                storedPassive[passiveItemCount] = null;
                ShopManager.Instance.passiveItem[i]++;
                passiveItemCount--;
                break;
            }

            else
            {
                if (i == passiveItemCount - 1)
                    ShopManager.Instance.passiveItem[passiveItemCount]++;
            }
        }
    }
}
