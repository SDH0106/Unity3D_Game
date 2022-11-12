using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SelectSceneCard : MonoBehaviour
{
    [SerializeField] public WeaponInfo[] weaponInfo;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text weaponRange;

    int randNum;
    int selectNum;

    [HideInInspector] public WeaponInfo selectedWeapon;

    int count;

    private void Start()
    {
        count = 0;
        Setting();
    }

    private void Update()
    {
        if (count >= 1)
            Destroy(gameObject);
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

    public void MoveScene(string sceneName)
    {
        ItemManager.Instance.GetItemInfo(selectedWeapon);
        count++;
        SceneManager.LoadScene(sceneName);
    }

}
