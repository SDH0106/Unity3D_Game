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

    Vector3 printPos;

    float printTime;
    float InitPrintTime;

    string text;

    private void Start()
    {
        damageText.text = Character.Instance.AttackDamage.ToString();
        printTime = 1f;
        InitPrintTime = printTime;
        Invoke("DestroyObject", printTime);
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // �ؽ�Ʈ�� ����
        textXColor.a = alpha;                       // ������ ���İ� ����(���� ���� �Ұ��ؼ� ����)
        damageText.color = textXColor;              // ������ ������ ����
    }

    private void Update()
    {
        printTime -= Time.deltaTime;

        if(printTime <= 0)
        {
            printTime = InitPrintTime;
        }

        ChangeAlpha(printTime / InitPrintTime);

        transform.position += Vector3.forward * 0.35f * Time.deltaTime;
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    /*private void Start()
    {
        damageText.text = Character.Instance.AttackDamage.ToString();
        printTime = 2f;
        InitPrintTime = printTime;
    }

    void InitSetting()
    {
        damageText.text = Character.Instance.AttackDamage.ToString();
        printTime = InitPrintTime;
        ChangeAlpha(1);
    }

    private void ChangeAlpha(float alpha)
    {
        Color textXColor = damageText.color;        // �ؽ�Ʈ�� ����
        textXColor.a = alpha;                       // ������ ���İ� ����(���� ���� �Ұ��ؼ� ����)
        damageText.color = textXColor;              // ������ ������ ����
    }

    void TextDisappear()
    {
        ChangeAlpha(printTime / InitPrintTime);

        if (printTime <= 0f)
        {
            printTime = InitPrintTime;
        }
    }

    private void Update()
    {
        Debug.Log(printTime);
        transform.position += Vector3.up * 0.2f * Time.deltaTime;
        printTime -= Time.deltaTime;
        TextDisappear();
    }

    public void TextMove(Vector3 position)
    {
        this.printPos = position;

        Invoke("DestroyPool", printTime);
    }

    public void SetManagedPool(IObjectPool<DamageUI> pool)
    {
        managedPool = pool;
    }

    public void DestroyPool()
    {
        managedPool.Release(this);
        InitSetting();
    }*/
}
