using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Purchasing;
using UnityEngine.UI;
using static WeaponInfo;

public class WeaponCardUI : MonoBehaviour
{
    [SerializeField] WeaponInfo[] weaponInfo;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text weaponRange;

    int randNum;
    int selectNum;

    [HideInInspector] public WeaponInfo selectedWeapon;

    private void Start()
    {
        randNum = Random.Range(0, weaponInfo.Length);
        selectNum = randNum;
        selectedWeapon = weaponInfo[randNum];
    }

    private void Update()
    {
        Setting();
    }

    void Setting()
    {
        itemSprite.sprite = selectedWeapon.ItemSprite;
        weaponName.text = selectedWeapon.WeaponName.ToString();
        type.text = selectedWeapon.Type.ToString();
        weaponDamage.text = selectedWeapon.WeaponDamage.ToString();
        elementDamage.text = selectedWeapon.ElementDamage.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
    }

    public void Click()
    {
        Destroy(gameObject);
    }
}
