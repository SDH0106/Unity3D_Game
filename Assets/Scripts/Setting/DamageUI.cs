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
    [SerializeField] Text damageText;

    private IObjectPool<DamageUI> managedPool;

    Vector3 initPos;

    Scale damageScale;

    float printTime, initPrintTime;

    string nextText;

    private void Start()
    {
        damageText.text = Character.Instance.AttackDamage.ToString();
        printTime = 5f;
        initPrintTime = printTime;
        initPos = transform.position;
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // �ؽ�Ʈ�� ����
        textXColor.a = alpha;                       // ������ ���İ� ����(���� ���� �Ұ��ؼ� ����)
        damageText.color = textXColor;              // ������ ������ ����
    }

    private void Update()
    {
        nextText = Character.Instance.AttackDamage.ToString();

        printTime -= Time.deltaTime;

        if(printTime <= 0)
        {
            DestroyPool();
        }

        ChangeAlpha(printTime / initPrintTime);

        transform.position += Vector3.forward * 0.35f * Time.deltaTime;
    }

    public void TextUpdate()
    {
        damageText.text = Character.Instance.AttackDamage.ToString();
    }

    void InitSetting()
    {
        printTime = initPrintTime;
        transform.position = initPos;
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
