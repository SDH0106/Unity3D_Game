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
    int passiveCount;

    public GameObject WeaponPrefab => weaponPrefab;

    private void Start()
    {
        weaponCount = 0;
        passiveCount = 0;
        storedWeapon = new WeaponInfo[6];
        storedPassive = new PassiveInfo[18];
    }

    public void GetWeaponInfo(WeaponInfo weaponInfo)
    {
        if (weaponCount < 6)
        {
            weaponCardItem = weaponInfo;
            storedWeapon[weaponCount] = weaponInfo;
            weaponCount++;
        }
    }

    public void GetPassiveInfo(PassiveInfo passiveInfo)
    {
        if (weaponCount < 18)
        {
            passiveCardItem = passiveInfo;
            storedPassive[passiveCount] = passiveInfo;
            passiveCount++;
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
