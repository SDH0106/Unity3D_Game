using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static WeaponInfo;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponInfo weaponInfo;

    [HideInInspector] public DamageUI damageUI;
    [HideInInspector] public float damage;
    [HideInInspector] public int count;

    [HideInInspector] public int grade;

    public void WeaponSetting()
    { 
        if(weaponInfo.Type == WEAPON_TYPE.½ºÅÂÇÁ)
        {
            damage = weaponInfo.MagicDamage * grade+ GameManager.Instance.elementDamage;
            damageUI.damageText.color = Color.cyan;
        }

        else
        {
            damage = weaponInfo.WeaponDamage *grade + GameManager.Instance.physicDamage;
            damageUI.damageText.color = new Color(1, 0.4871f, 0);
        }

        damageUI.weaponDamage = damage;
    }
}
