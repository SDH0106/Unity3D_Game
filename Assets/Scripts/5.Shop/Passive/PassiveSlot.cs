using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSlot : MonoBehaviour
{
    [SerializeField] ShowPassiveSlotCard infoUIPrefab;
    [HideInInspector] public int slotNum;

    RectTransform rectTransform;

    void Start()
    {
        slotNum = transform.GetSiblingIndex();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnInfoUI()
    {
        if (ItemManager.Instance.storedPassive[slotNum] != null)
        {
            infoUIPrefab.selectedNum = slotNum;
            infoUIPrefab.rect.position = rectTransform.position;
            infoUIPrefab.infoChange = true;
            infoUIPrefab.gameObject.SetActive(true);
        }
    }

    public void OffInfoUI()
    {
        if (ItemManager.Instance.storedPassive[slotNum] != null)
        {
            infoUIPrefab.gameObject.SetActive(false);
        }
    }
}
