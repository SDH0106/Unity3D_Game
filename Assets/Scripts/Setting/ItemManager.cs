using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] WeaponInfo[] weaponInfo;


    [HideInInspector] public WeaponInfo selectedWeapon;

    private void Start()
    {
        GetRandomWeapon();
    }

    void GetRandomWeapon()
    {
    }
}
