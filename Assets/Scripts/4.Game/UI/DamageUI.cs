using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Pool;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class DamageUI : MonoBehaviour
{
    [SerializeField] public Text damageText;

    float printTime, initPrintTime;

    public float weaponDamage;
    public float realDamage;
    public float swordBulletDamage;

    [HideInInspector] public bool isMiss;

    protected IObjectPool<DamageUI> managedPool;

    private void Start()
    {
        UISetting();
    }

    public void UISetting()
    {
        printTime = 1f;
        initPrintTime = printTime;

        if (!isMiss)
            damageText.text = realDamage.ToString("0.##");

        else if (isMiss)
        {
            damageText.color = Color.white;
            damageText.text = "Miss";
        }
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // �ؽ�Ʈ�� ����
        textXColor.a = alpha;                       // ������ ���İ� ����(���� ���� �Ұ��ؼ� ����)
        damageText.color = textXColor;              // ������ ������ ����
    }

    private void Update()
    {
        transform.position += Vector3.forward * 0.5f * Time.deltaTime;

        printTime -= Time.deltaTime;

        ChangeAlpha(printTime / initPrintTime);

        if (printTime <= 0)
        {
            DestroyUI();
        }
    }

    public void SetManagedPool(IObjectPool<DamageUI> pool)
    {
        managedPool = pool;
    }

    public virtual void DestroyUI()
    {
        damageText.fontSize = 50;

        if (gameObject.activeSelf)
        {
            managedPool.Release(this);
        }
    }
}
