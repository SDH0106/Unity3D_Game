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
    [SerializeField] int weaponDamage;
    [SerializeField] int elementDamage;
    [SerializeField] float weaponRange;
    [SerializeField] int weaponPrice;

    public Sprite ItemSprite => itemSprite;
    public string WeaponName => weaponName;
    public WEAPON_TYPE Type => type;
    public int WeaponDamage => weaponDamage;
    public int ElementDamage => elementDamage;
    public float WeaponRange => weaponRange;
    public int WeaponPrice => weaponPrice;
}
