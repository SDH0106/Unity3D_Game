using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "new Weapon", menuName = "GameData/Item/Weapon")]
public class WeaponInfo : ScriptableObject
{
    public enum WEAPON_TYPE
    {
        Gun,
        Staff,
        Sword,
    }

    [SerializeField] WEAPON_TYPE type;
    [SerializeField] Sprite itemSprite;
    [SerializeField] string weaponName;
    [SerializeField] int weaponDamage;
    [SerializeField] int elementDamage;
    [SerializeField] float weaponRange;

    public int WeaponDamage => weaponDamage;
    public int ElementDamage => elementDamage;
    public float WeaponRange => weaponRange;
}
