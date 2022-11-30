using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndUI : Singleton<EndUI>
{
    [Header("UI")]
    [SerializeField] GameObject overUI;
    [SerializeField] GameObject clearUI;
    [SerializeField] Text round;

    [Header("WeaponSlot")]
    [SerializeField] Transform weaponSlotParent;
    [SerializeField] GameObject weaponSlotPrefab;

    [Header("PassSlot")]
    [SerializeField] Transform passSlotParent;
    [SerializeField] GameObject passSlotPrefab;

    [Header("Stat")]
    [SerializeField] Text lv;
    [SerializeField] Text maxHp;
    [SerializeField] Text reHp;
    [SerializeField] Text apHp;
    [SerializeField] Text def;
    [SerializeField] Text wAtk;
    [SerializeField] Text eAtk;
    [SerializeField] Text aSpd;
    [SerializeField] Text spd;
    [SerializeField] Text ran;
    [SerializeField] Text luk;

    GameManager gameManager;
    ItemManager itemManager;

    [HideInInspector] public int[] weaponCount;

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        weaponCount = new int[6];
        UISetting();
        SettingStatText();
        WeaponSlotSetting();
        PassiveSlotSetting();
    }

    void UISetting()
    {
        if (gameManager.hp == 0)
        {
            overUI.SetActive(true);
            clearUI.SetActive(false);
            round.text = gameManager.round.ToString();
        }

        else if (gameManager.hp != 0)
        {
            overUI.SetActive(false);
            clearUI.SetActive(true);
        }
    }

    void WeaponSlotSetting()
    {
        int count = 0;
        for (int i = 0; i < itemManager.storedWeapon.Length; i++)
        {
            if (itemManager.storedWeapon[i] != null)
            {
                GameObject slot = Instantiate(weaponSlotPrefab);
                slot.transform.SetParent(weaponSlotParent);
                weaponCount[count] = i;
                count++;
            }
        }
    }

    void PassiveSlotSetting()
    {
        for (int i = 0; i < itemManager.storedPassive.Length; i++)
        {
            if (itemManager.storedPassive[i] != null)
            {
                GameObject slot = Instantiate(passSlotPrefab);
                slot.transform.SetParent(passSlotParent);
            }
        }
    }

    void SettingStatText()
    {
        lv.text = gameManager.level.ToString();
        maxHp.text = gameManager.maxHp.ToString();
        reHp.text = gameManager.recoverHp.ToString();
        apHp.text = gameManager.absorbHp.ToString();
        def.text = gameManager.defence.ToString();
        wAtk.text = gameManager.physicDamage.ToString();
        eAtk.text = gameManager.elementDamage.ToString();
        aSpd.text = gameManager.attackSpeed.ToString();
        spd.text = gameManager.speed.ToString();
        ran.text = gameManager.range.ToString();
        luk.text = gameManager.luck.ToString();
    }

    public void ToTitle()
    {
        Destroy(GameManager.Instance.gameObject);
        Destroy(Character.Instance.gameObject);
        Destroy(ItemManager.Instance.gameObject);
        Destroy(SoundManager.Instance.gameObject);
        SceneManager.LoadScene("StartTitle");
    }
}
