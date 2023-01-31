using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.TextCore.Text;

public class SelectSceneCard : MonoBehaviour
{
    [SerializeField] GameObject lockPanel;
    [SerializeField] public WeaponInfo[] weaponInfos;
    [SerializeField] Image cardBack;
    [SerializeField] Image cardBackLine;
    [SerializeField] Image itemSprite;
    [SerializeField] Text weaponName;
    [SerializeField] Text type;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text magicDamage;
    [SerializeField] Text attackDelay;
    [SerializeField] Text bulletSpeed;
    [SerializeField] Text weaponRange;
    [SerializeField] Text weaponGrade;
    [SerializeField] Text description;

    [HideInInspector] public WeaponInfo selectedWeapon;

    int count;

    ItemManager itemManager;

    private void Start()
    {
        count = 0;
        Setting();
        CardImage();

        if (Character.Instance.characterNum == 0)
        {
            lockPanel.SetActive(false);
        }

        if (Character.Instance.characterNum == 1)
        {
            if (selectedWeapon.Type == WeaponInfo.WEAPON_TYPE.°Ë)
                lockPanel.SetActive(false);

            else
                lockPanel.SetActive(true);
        }
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
        magicDamage.text = selectedWeapon.MagicDamage.ToString();
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
        itemManager = ItemManager.Instance;

        if (selectedWeapon.Type == WeaponInfo.WEAPON_TYPE.°Ë)
        {
            itemManager.GetWeaponInfo(selectedWeapon);
            itemManager.weaponGrade[itemManager.weaponCount] = selectedWeapon.weaponGrade;
            count++;
            SceneManager.LoadScene(sceneName);
            Character.Instance.Equip();
            SoundManager.Instance.PlayES("WeaponSelect");
        }

        else
        {
            if (Character.Instance.characterNum != (int)CHARACTER_NUM.Legendary)
            {
                itemManager.GetWeaponInfo(selectedWeapon);
                itemManager.weaponGrade[itemManager.weaponCount] = selectedWeapon.weaponGrade;
                count++;
                SceneManager.LoadScene(sceneName);
                Character.Instance.Equip();
                SoundManager.Instance.PlayES("WeaponSelect");
            }
        }
    }
}
