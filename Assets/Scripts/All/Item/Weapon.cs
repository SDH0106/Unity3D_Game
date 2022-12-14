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
        if (weaponInfo.Type == WEAPON_TYPE.스태프)
        {
            if (!gameManager.doubleShot)
                damage = weaponInfo.MagicDamage * grade + gameManager.elementDamage + gameManager.longDamage;

            else if (gameManager.doubleShot)
            {
                if (weaponInfo.WeaponName == "번개 스태프")
                    damage = weaponInfo.MagicDamage * grade + gameManager.elementDamage + gameManager.longDamage;

                if (weaponInfo.WeaponName != "번개 스태프")
                    damage = (weaponInfo.MagicDamage * grade + gameManager.elementDamage + gameManager.longDamage) * 0.7f;
            }
            damageUI.damageText.color = Color.cyan;
        }

        else if (weaponInfo.Type == WEAPON_TYPE.총)
        {
            if (!gameManager.doubleShot)
                damage = weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.longDamage;

            else if (gameManager.doubleShot)
                damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.longDamage) * 0.7f;

            damageUI.damageText.color = new Color(1, 0.4871f, 0);
        }

        else if (weaponInfo.Type == WEAPON_TYPE.검)
        {
            if (criRand <= gameManager.critical || gameManager.critical >= 100)
            {
                if (!gameManager.luckCritical)
                    damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage) * 1.5f;

                else if (gameManager.luckCritical)
                {
                    int rand = Random.Range(1, 101);

                    if (rand <= gameManager.luck || gameManager.luck >= 100)
                        damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage) * 2f;

                    else if (rand > gameManager.luck)
                        damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage) * 1.5f;
                }

                damageUI.damageText.color = new Color(0.9f, 0, 0.7f, 1);
                damageUI.damageText.fontSize = 65;
            }

            else if (criRand > gameManager.critical)
            {
                damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage);
                damageUI.damageText.color = new Color(1, 0.4871f, 0);
            }
        }

        if (damage < 1)
            damage = 1;

        if (!gameManager.luckDamage)
        {
            damageUI.weaponDamage = damage;
        }

        else if (gameManager.luckDamage)
        {
            int rand = Random.Range(1, 101);

            if (rand <= gameManager.luck || gameManager.luck >= 100)
                damageUI.weaponDamage = damage * 2;

            else if (rand > gameManager.luck)
                damageUI.weaponDamage = damage;
        }
    }
}
