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

    protected int criRand;

    protected GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    public void WeaponSetting()
    {
        if (weaponInfo.Type == WEAPON_TYPE.½ºÅÂÇÁ)
        {
            damage = weaponInfo.MagicDamage * grade + GameManager.Instance.elementDamage;
            damageUI.damageText.color = Color.cyan;
        }

        else if (weaponInfo.Type == WEAPON_TYPE.ÃÑ)
        {
            damage = weaponInfo.WeaponDamage * grade + GameManager.Instance.physicDamage;
            damageUI.damageText.color = new Color(1, 0.4871f, 0);
        }

        else if (weaponInfo.Type == WEAPON_TYPE.°Ë)
        {
            if (criRand < gameManager.critical || gameManager.critical >= 100)
            {
                damage = (int)(weaponInfo.WeaponDamage * grade + GameManager.Instance.physicDamage) * 1.5f;
                damageUI.damageText.color = new Color(0.9f, 0, 0.7f, 1);
                damageUI.damageText.fontSize = 65;
            }

            else if (criRand >= gameManager.critical)
            {
                damage = (int)(weaponInfo.WeaponDamage * grade + GameManager.Instance.physicDamage);
                damageUI.damageText.color = new Color(1, 0.4871f, 0);
            }
        }

        damageUI.weaponDamage = damage;
    }
}
