using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] GameObject sellUI;
    [SerializeField] Image back;

    [HideInInspector] public int slotNum;

    private void Start()
    {
        slotNum = transform.GetSiblingIndex();
    }

    private void Update()
    {
        SlotColor();
    }

    void SlotColor()
    {
        if (ItemManager.Instance.weaponGrade[slotNum] == Grade.�Ϲ�)
        {
            back.color = new Color(0.53f, 0.53f, 0.53f, 0.8235f);
        }

        else if (ItemManager.Instance.weaponGrade[slotNum] == Grade.���)
        {
            back.color = new Color(0, 0.77f, 1, 0.8235f);
        }

        else if (ItemManager.Instance.weaponGrade[slotNum] == Grade.����)
        {
            back.color = new Color(0.5f, 0.2f, 0.4f, 0.8235f);
        }

        else if (ItemManager.Instance.weaponGrade[slotNum] == Grade.��ȭ)
        {
            back.color = new Color(1, 0.31f, 0.31f, 0.8235f);
        }
    }

    public void ShowClickUI()
    {
        if (ItemManager.Instance.storedWeapon[slotNum] != null)
        {
            sellUI.gameObject.SetActive(true);
            sellUI.transform.position = Input.mousePosition;
            sellUI.GetComponent<CardClick>().Setting(slotNum);
            sellUI.GetComponent<CardClick>().CardImage(slotNum);
        }
    }
}
