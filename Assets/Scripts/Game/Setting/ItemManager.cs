using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] public Transform coinStorage;

    [HideInInspector] public WeaponInfo CardItem;

    public GameObject WeaponPrefab => weaponPrefab;

    public void GetItemInfo(WeaponInfo weaponInfo)
    {
        CardItem = weaponInfo;
    }

    public void Equip(Transform[] pos, int num)
    {
        GameObject weapon = Instantiate(weaponPrefab);
        weapon.transform.SetParent(pos[num]);
        weapon.transform.position = pos[num].position;
        num++;
    }
}
