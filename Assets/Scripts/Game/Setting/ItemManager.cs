using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    [HideInInspector] public WeaponInfo CardItem;

    public void GetItemInfo(WeaponInfo weaponInfo)
    {
        CardItem = weaponInfo;
    }

}
