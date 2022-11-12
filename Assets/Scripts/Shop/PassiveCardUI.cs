using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassiveCardUI : MonoBehaviour
{
    [SerializeField] public PassiveInfo[] passiveInfo;
    [SerializeField] Image itemSprite;
    [SerializeField] Text itemName;
    [SerializeField] Text hp;
    [SerializeField] Text recoverHp;
    [SerializeField] Text absorbHp;
    [SerializeField] Text defence;
    [SerializeField] Text weaponDamage;
    [SerializeField] Text elementDamage;
    [SerializeField] Text attackSpeed;
    [SerializeField] Text range;
    [SerializeField] Text speed;
    [SerializeField] Text luck;

    int randNum;
    int selectNum;

    [HideInInspector] public PassiveInfo selectedPassive;

    private void Start()
    {
        Setting();
    }

    void Setting()
    {
        itemSprite.sprite = selectedPassive.ItemSprite;
        itemName.text = selectedPassive.ItemName;
        hp.text = selectedPassive.HP.ToString();
        recoverHp.text = selectedPassive.RecoverHp.ToString();
        absorbHp.text = selectedPassive.AbsorbHp.ToString();
        defence.text = selectedPassive.Defence.ToString();
        weaponDamage.text = selectedPassive.WeaponDamage.ToString();
        elementDamage.text = selectedPassive.ElementDamage.ToString();
        attackSpeed.text = selectedPassive.AttackSpeed.ToString();
        range.text = selectedPassive.Range.ToString();
        speed.text = selectedPassive.Speed.ToString();
        luck.text = selectedPassive.Luck.ToString();
    }

    public void Click()
    {
        ItemManager.Instance.GetPassiveInfo(selectedPassive);
        Destroy(gameObject);
    }
}
