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

    int weaponCount;
    int passiveItemCount;

    GameObject weapon;

    public bool isFool = false;
    public int foolCount;

    public GameObject WeaponPrefab => weaponPrefab;

    private void Start()
    {
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

        weaponCardItem = weaponInfo;

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
    public void Equip(Transform[] pos, int num)
    {
        weapon = Instantiate(weaponPrefab);
        weapon.transform.SetParent(pos[num]);
        weapon.transform.position = pos[num].position;
        num++;
    }
}
