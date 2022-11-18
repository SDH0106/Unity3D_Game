using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static WeaponInfo;

public class DamageUI : MonoBehaviour
{
    [SerializeField] Weapon weapon;
    [SerializeField] Text damageText;

    private IObjectPool<DamageUI> managedPool;

    float printTime, initPrintTime;

    private void Start()
    {
        damageText.text = Character.Instance.AttackDamage.ToString();
        printTime = 1f;
        initPrintTime = printTime;
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // 텍스트의 색상값
        textXColor.a = alpha;                       // 색상값의 알파값 변경(직접 변경 불가해서 빼옴)
        damageText.color = textXColor;              // 변경한 색상을 대입
    }

    private void Update()
    {
        transform.position += Vector3.forward * 0.5f * Time.deltaTime;

        printTime -= Time.deltaTime;

        if (printTime <= 0)
        {
            DestroyPool();
        }

        ChangeAlpha(printTime / initPrintTime);
    }

    public void TextUpdate(float damage)
    {
        damageText.text = damage.ToString();
    }

    void InitSetting()
    {
        printTime = initPrintTime;
        ChangeAlpha(1);
    }

    public void SetManagedPool(IObjectPool<DamageUI> pool)
    {
        managedPool = pool;
    }

    public void DestroyPool()
    {
        managedPool.Release(this);
        InitSetting();
    }
}
