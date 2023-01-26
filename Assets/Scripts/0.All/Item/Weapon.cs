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
    [HideInInspector] public float swordBulletDamage;
    [HideInInspector] public int count;

    [HideInInspector] public int grade;

    protected int criRand;

    protected GameManager gameManager;

    public void WeaponSetting()
    {
        if (weaponInfo.Type == WEAPON_TYPE.스태프)
        {
            if (!gameManager.doubleShot)
                damage = weaponInfo.MagicDamage * grade + gameManager.magicDamage + gameManager.longDamage;

            else if (gameManager.doubleShot)
            {
                if (weaponInfo.WeaponName == "번개 스태프")
                    damage = weaponInfo.MagicDamage * grade + gameManager.magicDamage + gameManager.longDamage;

                if (weaponInfo.WeaponName != "번개 스태프")
                    damage = (weaponInfo.MagicDamage * grade + gameManager.magicDamage + gameManager.longDamage) * 0.7f;
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
                {
                    damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage) * 1.5f;
                    swordBulletDamage = (weaponInfo.WeaponDamage * grade + gameManager.magicDamage + gameManager.longDamage) * 1.5f;
                }

                else if (gameManager.luckCritical)
                {
                    int rand = Random.Range(1, 101);

                    if (rand <= gameManager.luck || gameManager.luck >= 100)
                    {
                        damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage) * 2f;
                        swordBulletDamage = (weaponInfo.WeaponDamage * grade + gameManager.magicDamage + gameManager.longDamage) * 2f;
                    }

                    else if (rand > gameManager.luck)
                    {
                        damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage) * 1.5f;
                        swordBulletDamage = (weaponInfo.WeaponDamage * grade + gameManager.magicDamage + gameManager.longDamage) * 1.5f;
                    }
                }

                damageUI.damageText.color = new Color(0.9f, 0, 0.7f, 1);
                damageUI.damageText.fontSize = 65;
            }

            else if (criRand > gameManager.critical)
            {
                damage = (weaponInfo.WeaponDamage * grade + gameManager.physicDamage + gameManager.shortDamage);
                swordBulletDamage = (weaponInfo.WeaponDamage * grade + gameManager.magicDamage + gameManager.longDamage);
                damageUI.damageText.color = new Color(1, 0.4871f, 0);
            }
        }

        damage = Mathf.Round(damage * gameManager.percentDamage * 10) * 0.1f;
        swordBulletDamage = Mathf.Round(swordBulletDamage * gameManager.percentDamage * 10) * 0.1f;

        if (damage < 0)
        {
            damage = 0;
            swordBulletDamage = 0;
        }

        if (!gameManager.luckDamage)
        {
            damageUI.weaponDamage = damage;
            damageUI.swordBulletDamage = swordBulletDamage;
        }

        else if (gameManager.luckDamage)
        {
            int rand = Random.Range(1, 101);

            if (rand <= gameManager.luck || gameManager.luck >= 100)
            {
                damageUI.weaponDamage = damage * 2f;
                damageUI.swordBulletDamage = swordBulletDamage * 2f;
            }

            else if (rand > gameManager.luck)
            {
                damageUI.weaponDamage = damage;
                damageUI.swordBulletDamage = swordBulletDamage;
            }
        }
    }
}
