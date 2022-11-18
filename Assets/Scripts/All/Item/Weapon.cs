using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static WeaponInfo;

public class Weapon : MonoBehaviour
{
    public string weaponName;
    public WEAPON_TYPE type;
    public float weaponDamage;
    public float magicDamage;
    float weaponRange;
    int weaponPrice;

    SpriteRenderer spriteRenderer;

    [HideInInspector] public WeaponInfo weaponInfo;

    public float damage;

    private void Start()
    {
        weaponInfo = ItemManager.Instance.weaponCardItem;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Setting();
    }

    private void Update()
    {
        Damage();
    }

    public void Damage()
    { 
        if(type == WEAPON_TYPE.½ºÅÂÇÁ)
        {
            damage = magicDamage + GameManager.Instance.elementDamage;
        }

        else
        {
            damage = weaponDamage + GameManager.Instance.physicDamage;
        }
    }

    void Setting()
    {
        spriteRenderer.sprite = weaponInfo.ItemSprite;
        weaponName = weaponInfo.WeaponName;
        type = weaponInfo.Type;
        weaponDamage = weaponInfo.WeaponDamage;
        magicDamage= weaponInfo.MagicDamage;
        weaponRange = weaponInfo.WeaponRange;
        weaponPrice = weaponInfo.WeaponPrice;
    }
}
