using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static WeaponInfo;

public class Weapon : MonoBehaviour
{
    string weaponName;
    WEAPON_TYPE type;
    int weaponDamage;
    int elementDamage;
    float weaponRange;

    SpriteRenderer spriteRenderer;

    WeaponInfo weaponInfo;

    private void Start()
    {
        weaponInfo = ItemManager.Instance.CardItem;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Setting();
    }

    void Setting()
    {
        spriteRenderer.sprite = weaponInfo.ItemSprite;
        weaponName = weaponInfo.WeaponName;
        type = weaponInfo.Type;
        weaponDamage = weaponInfo.WeaponDamage;
        elementDamage= weaponInfo.ElementDamage;
        weaponRange = weaponInfo.WeaponRange;
    }
}
