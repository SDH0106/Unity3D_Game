using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static WeaponInfo;

public class Weapon : MonoBehaviour
{
    [SerializeField] public WeaponInfo weaponInfo;
    [SerializeField] DamageUI damageUI;

    [HideInInspector] public float damage;

    private void Update()
    {
        weaponSetting();
        Debug.Log(damage);
    }

    public void weaponSetting()
    { 
        if(weaponInfo.Type == WEAPON_TYPE.½ºÅÂÇÁ)
        {
            damage = weaponInfo.MagicDamage + GameManager.Instance.elementDamage;
            damageUI.damageText.color = Color.cyan;
        }

        else
        {
            damage = weaponInfo.WeaponDamage + GameManager.Instance.physicDamage;
            damageUI.damageText.color = new Color(1, 0.4871f, 0);
        }

        damageUI.weaponDamage = damage;
    }
}
