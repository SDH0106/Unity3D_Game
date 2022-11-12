using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] Image[] weapon;
    [SerializeField] WeaponCardUI[] cards;

    int[] numArray;

    private void Start()
    {
        numArray = new int[cards[0].weaponInfo.Length];
        GetRandomNum(cards.Length, cards[0].weaponInfo.Length);
    }

    private void Update()
    {
        WeaponSlot();
    }

    void AlphaChange(int i, int a)
    {
        Color color = weapon[i].color;
        color.a = a;
        weapon[i].color = color;
    }

    void WeaponSlot()
    {
        for(int i = 0; i < weapon.Length; i++)
        {
            if (ItemManager.Instance.storedWeapon[i] != null)
            {
                AlphaChange(i, 1);
                weapon[i].sprite = ItemManager.Instance.storedWeapon[i].ItemSprite;
            }

            else if (ItemManager.Instance.storedWeapon[i] == null)
            {
                AlphaChange(i, 0);
            }
        }
    }

    void PassiveSlot()
    {

    }

    void GetRandomNum(int count, int length)
    {
        for (int i = 0; i < count; i++)
        {
            numArray[i] = UnityEngine.Random.Range(0, length);

            for (int j = 0; j < i; j++)
            {
                if (numArray[j] == numArray[i])
                {
                    i--;
                    break;
                }
            }
        }

        for (int i = 0; i < cards.Length; i++)
            cards[i].selectedWeapon = cards[i].weaponInfo[numArray[i]];
    }
}
