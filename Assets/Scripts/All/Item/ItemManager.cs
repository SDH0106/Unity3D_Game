using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] public Transform coinStorage;

    [Header("Item")]
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] GameObject PassivePerfab;

    [HideInInspector] public WeaponInfo weaponCardItem;
    [HideInInspector] public PassiveInfo passiveCardItem;

    [HideInInspector] public WeaponInfo[] storedWeapon;
    [HideInInspector] public PassiveInfo[] storedPassive;

    [SerializeField] ShopManager shopManager;

    public int weaponCount;
    int passiveItemCount;

    public GameObject WeaponPrefab => weaponPrefab;

    private void Start()
    {
        weaponCount = 0;
        passiveItemCount = 0;
        storedWeapon = new WeaponInfo[6];
        storedPassive = new PassiveInfo[30];
    }

    public void GetWeaponInfo(WeaponInfo weaponInfo)
    {
        if (weaponCount < storedWeapon.Length)
        {
            weaponCardItem = weaponInfo;
            storedWeapon[weaponCount] = weaponInfo;
            weaponCount++;
        }
    }

    public void GetPassiveInfo(PassiveInfo passiveInfo)
    {
        passiveCardItem = passiveInfo;
        storedPassive[passiveItemCount] = passiveInfo;

        if (passiveItemCount == 0)
        {
            shopManager.passiveItem[passiveItemCount]++;
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
                shopManager.passiveItem[i]++;
                passiveItemCount--;
                break;
            }

            else
            {
                if (i == passiveItemCount - 1)
                    shopManager.passiveItem[passiveItemCount]++;
            }
        }
    }
    public void Equip(Transform[] pos, int num)
    {
        GameObject weapon = Instantiate(weaponPrefab);
        weapon.transform.SetParent(pos[num]);
        weapon.transform.position = pos[num].position;
        num++;
    }
}
