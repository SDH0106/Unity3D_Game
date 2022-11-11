using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static WeaponInfo;

public class Weapon : MonoBehaviour
{
    WeaponCardUI weaponCardUI;

    string weaponName;
    WEAPON_TYPE type;
    int weaponDamage;
    int elementDamage;
    float weaponRange;

    int weaponCount;

    SpriteRenderer spriteRenderer;

    private void Start()
    {
        Debug.Log("weapon");
        weaponCount = 0;

        spriteRenderer = GetComponent<SpriteRenderer>();
        weaponCardUI = GetComponent<WeaponCardUI>();
    }

    private void Update()
    {
        Setting();
    }

    void Setting()
    {
        /*spriteRenderer.sprite = weaponInfo.ItemSprite;
        weaponName = weaponCardUI.selectedWeapon.WeaponName;
        type = weaponCardUI.selectedWeapon.Type;
        weaponDamage = weaponCardUI.selectedWeapon.WeaponDamage;
        elementDamage= weaponCardUI.selectedWeapon.ElementDamage;
        weaponRange = weaponCardUI.selectedWeapon.WeaponRange;*/
    }

    public void Equip()
    {
        // 0~5±îÁö
        if (weaponCount < 6)
        {
            Character.Instance.EquipWeapon();
            weaponCount++;
        }
    }
}
