using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SelectSceneCard : MonoBehaviour
{
    [SerializeField] public WeaponInfo[] weaponInfos;
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text attackDelay;
    [SerializeField] Text bulletSpeed;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponGrade;
    [SerializeField] Text description;

    [HideInInspector] public WeaponInfo selectedWeapon;

    int count;

    private void Start()
    {
        count = 0;
        Setting();
        CardImage();
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
        elementDamage.text = selectedWeapon.MagicDamage.ToString();
        attackDelay.text = selectedWeapon.AttackDelay.ToString();
        bulletSpeed.text = selectedWeapon.BulletSpeed.ToString();
        weaponRange.text = selectedWeapon.WeaponRange.ToString();
        weaponGrade.text = selectedWeapon.weaponGrade.ToString();
        description.text = selectedWeapon.Description.ToString();
    }

    void CardImage()
    {
        cardBack.color = new Color(0.1415f, 0.1415f, 0.1415f, 0.8235f);
        cardBackLine.color = Color.black;
        weaponName.color = Color.white;
        weaponGrade.color = Color.white;
    }

    public void MoveScene(string sceneName)
    {
        ItemManager.Instance.GetWeaponInfo(selectedWeapon);
        ItemManager.Instance.weaponGrade[ItemManager.Instance.weaponCount] = selectedWeapon.weaponGrade;
        count++;
        SceneManager.LoadScene(sceneName);
        Character.Instance.Equip();
        SoundManager.Instance.PlayES("WeaponSelect");
    }
}
