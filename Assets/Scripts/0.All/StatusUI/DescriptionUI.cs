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
                descriptionText.text = "캐릭터의 최대 체력과 관련된 능력치. 1이하로 떨어지지 않습니다.";
                break;

            case 1:
                descriptionText.text = "캐릭터가 2초마다 자동으로 회복하는 체력 수치와 관련된 능력치. 0이하인 경우 적용되지 않습니다.";
                break;

            case 2:
                descriptionText.text = "캐릭터가 몬스터 공격에 성공했을 경우 회복하는 체력 수치와 관련된 능력치. 0이하인 경우 적용되지 않습니다. 폭발 공격에는 적용되지 않습니다.";
                break;

            case 3:
                descriptionText.text = "캐릭터가 몬스터에게 공격받을 경우 입는 피해를 감소시켜 주는 능력치. 0이하인 경우 적용되지 않습니다.";
                break;

            case 4:
                descriptionText.text = "캐릭터가 몬스터에게 공격받을 경우 피해를 무시하는 확률과 관련된 능력치. 0이하인 경우 적용되지 않습니다.";
                break;

            case 5:
                descriptionText.text = "모든 공격력의 합산이 끝난후 곱해지는 공격력 배율값과 관련된 능력치. 각 캐릭터마다 다른 배율을 가집니다.";
                break;

            case 6:
                descriptionText.text = "검, 총류 무기의 공격력와 관련된 능력치. 이 수치가 몬스터의 방어력보다 낮으면 공격이 불가능합니다.";
                break;

            case 7:
                descriptionText.text = "검기와 스태프류 무기의 공격력와 관련된 능력치. 이 수치가 몬스터의 방어력보다 낮으면 공격이 불가능합니다.";
                break;

            case 8:
                descriptionText.text = "검기와 총, 스태프류 무기의 공격력와 관련된 능력치. 이 수치가 몬스터의 방어력보다 낮으면 공격이 불가능합니다.";
                break;

            case 9:
                descriptionText.text = "검류 무기의 공격력과 관련된 능력치. 이 수치가 몬스터의 방어력보다 낮으면 공격이 불가능합니다.";
                break;

            case 10:
                descriptionText.text = "공격시의 검류 무기의 휘두르는 속도, 총과 스태프류 무기의 투사체 발사 속도와 관련된 능력치. 0이하인 경우 무기의 기본 공격 속도로 적용됩니다.";
                break;

            case 11:
                descriptionText.text = "캐릭터가 이동하는 속도와 관련된 능력치. 0이하인 경우 캐릭터의 기본 이동 속도로 이동합니다.";
                break;

            case 12:
                descriptionText.text = "공격시 검류 무기의 휘두르기가 시전되는 거리, 투사체가 발사되고 사라지는 사거리와 관련된 능력치.";
                break;

            case 13:
                descriptionText.text = "이파리나무에서 특별 아이템의 등장 확률, 스태프류의 특수 능력 발동, 특수 패시브 아이템의 발동 조건등과 관련된 능력치. 0이하인 경우 적용되지 않습니다.";
                break;

            case 14:
                descriptionText.text = "검류 무기의 공격력이 1.5배로 더 강하게 적용되는 확률과 관련된 능력치. 0이하인 경우 적용되지 않습니다.";
                break;
        }
    }
}
