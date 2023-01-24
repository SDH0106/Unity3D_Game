using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum StatText
{
    MaxHp,
    ReHp,
    AbHp,
    Def,
    Avoid,
    PDmg,
    WAtk,
    EAtk,
    SAtk,
    LAtk,
    ASpd,
    Spd,
    Ran,
    Luk,
    Cri,
}

public class DescriptionUI : MonoBehaviour
{
    [SerializeField] Text descriptionText;


    public void SetTextInfo(int num)
    {
        transform.position = Input.mousePosition;

        switch(num)
        {
            case 0:
                descriptionText.text = "ĳ������ �ִ� ü�°� ���õ� �ɷ�ġ.";
                break;

            case 1:
                descriptionText.text = "ĳ���Ͱ� 2�ʴ� �ڵ����� ȸ���ϴ� ü�� ��ġ�� ���õ� �ɷ�ġ.";
                break;

            case 2:
                descriptionText.text = "ĳ���Ͱ� ���� ���ݿ� �������� ��� ȸ���ϴ� ü�� ��ġ�� ���õ� �ɷ�ġ.";
                break;

            case 3:
                descriptionText.text = "ĳ���Ͱ� ���Ϳ��� ���ݹ��� ��� �Դ� ���ظ� ���ҽ��� �ִ� �ɷ�ġ.";
                break;

            case 4:
                descriptionText.text = "ĳ���Ͱ� ���Ϳ��� ���ݹ��� ��� ���ظ� �����ϴ� Ȯ���� ���õ� �ɷ�ġ.";
                break;

            case 5:
                descriptionText.text = "��� ���ݷ��� �ջ��� ������ �������� ���ݷ� �������� ���õ� �ɷ�ġ. �� ĳ���͸��� �ٸ� ������ �����ϴ�.";
                break;

            case 6:
                descriptionText.text = "��, �ѷ� ������ ���ݷ¿� ���õ� �ɷ�ġ. �� ��ġ�� ������ ���� ���� ������ ������� ���� �ʽ��ϴ�.";
                break;

            case 7:
                descriptionText.text = "�������� ������ ���ݷ¿� ���õ� �ɷ�ġ. �� ��ġ�� ������ ���� ���� ������ ������� ���� �ʽ��ϴ�.";
                break;

            case 8:
                descriptionText.text = "��, �������� ������ ���ݷ¿� ���õ� �ɷ�ġ. �� ��ġ�� ������ ���� ���� ������ ������� ���� �ʽ��ϴ�.";
                break;

            case 9:
                descriptionText.text = "�˷� ������ ���ݷ¿� ���õ� �ɷ�ġ. �� ��ġ�� ������ ���� ���� ������ ������� ���� �ʽ��ϴ�.";
                break;

            case 10:
                descriptionText.text = "���ݽ��� �˷� ������ �ֵθ��� �ӵ�, �Ѱ� �������� ������ ����ü �߻� �ӵ��� ���õ� �ɷ�ġ.";
                break;

            case 11:
                descriptionText.text = "ĳ���Ͱ� �̵��ϴ� �ӵ��� ���õ� �ɷ�ġ.";
                break;

            case 12:
                descriptionText.text = "���ݽ��� �˷� ������ �ֵθ��Ⱑ �����Ǵ� �Ÿ�, �Ѱ� �������� ������ ����ü�� �߻�ǰ� ������� ��Ÿ��� ���õ� �ɷ�ġ.";
                break;

            case 13:
                descriptionText.text = "���� ������ ���� ������ ���� Ȯ��, ���������� Ư�� �ɷ� �ߵ�, Ư�� ī�� �������� �ߵ� ���ǰ� ���õ� �ɷ�ġ.";
                break;

            case 14:
                descriptionText.text = "�˷� ������ ������� 1.5��� �� ���ϰ� ����Ǵ� Ȯ���� ���õ� �ɷ�ġ.";
                break;
        }
    }
}
