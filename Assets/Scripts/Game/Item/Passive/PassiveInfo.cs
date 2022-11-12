using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static WeaponInfo;

[CreateAssetMenu(fileName = "new Item", menuName = "GameData/Item/Passive")]
public class PassiveInfo : ScriptableObject
{
    [SerializeField] Sprite itemSprite;
    [SerializeField] string itemName;
    [SerializeField] float hp;
    [SerializeField] float recoverHp;
    [SerializeField] float absorbHp;
    [SerializeField] int defence;
    [SerializeField] int weaponDamage;
    [SerializeField] int elementDamage;
    [SerializeField] float attackSpeed;
    [SerializeField] float range;
    [SerializeField] float speed;
    [SerializeField] int luck;

    public Sprite ItemSprite => itemSprite;
    public string ItemName => itemName;
    public int WeaponDamage => weaponDamage;
    public int ElementDamage => elementDamage;
    public float Range => range;
    public int Defence => defence;
    public float Speed => speed;
    public int Luck => luck;
    public float HP => hp;
    public float AbsorbHp => absorbHp;
    public float RecoverHp => recoverHp;
    public float AttackSpeed => attackSpeed;

}
