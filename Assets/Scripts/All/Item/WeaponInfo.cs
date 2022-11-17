using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Weapon", menuName = "GameData/Item/Weapon")]
public class WeaponInfo : ScriptableObject
{
    public enum WEAPON_TYPE
    {
        ÃÑ,
        ½ºÅÂÇÁ,
        °Ë,
    }

    [SerializeField] Sprite itemSprite;
    [SerializeField] string weaponName;
    [SerializeField] WEAPON_TYPE type;
    [SerializeField] float weaponDamage;
    [SerializeField] float magicDamage;
    [SerializeField] float weaponRange;
    [SerializeField] int weaponPrice;

    public Sprite ItemSprite => itemSprite;
    public string WeaponName => weaponName;
    public WEAPON_TYPE Type => type;
    public float WeaponDamage => weaponDamage;
    public float MagicDamage => magicDamage;
    public float WeaponRange => weaponRange;
    public int WeaponPrice => weaponPrice;
}
