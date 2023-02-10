using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTutorial : MonoBehaviour
{
    [SerializeField] GameObject[] tutoUis;
    [SerializeField] Text descripText;

    GameManager gameManager;

    int count;

    string[] texts;

    private void Start()
    {
        Time.timeScale = 0;

        gameManager = GameManager.Instance;
        gameManager.isPause = true;

        if (gameManager.round == 1)
            count = 0;

        else if (gameManager.round == 10)
            count = 4;

        for (int i = 0; i < tutoUis.Length; i++)
        {
            tutoUis[i].gameObject.SetActive(false);
        }

        TextChange();

        descripText.text = texts[count];

        tutoUis[count].SetActive(true);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (gameManager.round == 1)
            {
                tutoUis[count].SetActive(false);
            }

            count++;

            if (gameManager.round == 1)
            {
                if (count < 4)
                {
                    tutoUis[count].SetActive(true);
                    descripText.text = texts[count];
                }

                else if (count >= 4)
                {
                    gameManager.isPause = false;
                    Time.timeScale = 1;
                    gameObject.SetActive(false);
                    PlayerPrefs.SetInt("GameTuto", 0);
                    GameSceneUI.Instance.monsterSpawn.SetActive(true);
                }
            }

            if(gameManager.round == 10)
            {
                if (count < 8)
                {
                    descripText.text = texts[count];
                }

                else if(count >= 8)
                {
                    gameManager.isPause = false;
                    Time.timeScale = 1;
                    gameObject.SetActive(false);
                    PlayerPrefs.SetInt("BossTuto", 0);
                    GameSceneUI.Instance.monsterSpawn.SetActive(true);
                }
            }
        }
    }

    void TextChange()
    {
        texts = new string[10];
        texts[0] = "반갑뇨. 나는 뇨파다뇨. 나의 탈출을 도울 수 있게 내가 설명해주겠다뇨.\n우선 이것은 내 체력과 경험치다뇨.\n체력이 0이 되면 탈출에 실패하고, 경험치가 끝까지 다 차면 레벨업을 한다뇨.\n레벨업을 하면 체력이 1오르고 능력치 카드를 준다뇨!";
        texts[1] = "그 다음은 날짜와 제한 시간이다뇨.\n제한 시간동안 몬스터들이 나온다뇨. 제한 시간이 지나면 하루가 끝나고 상점으로 넘어간다뇨.\n날짜가 30일이 되면 내가 탈출할 수 있는 조건이 된다뇨.";
        texts[2] = "이 것은 이파리 나무다뇨.\n이파리 나무를 모아 배를 만들어서 탈출한다뇨. 30일이 끝나기 전까지 총 70개의 나무를 모아야한다뇨. 만약 모으지 못한다면 탈출에 실패한다뇨.";
        texts[3] = "마지막으로 이것은 일시 정지 버튼이다뇨.\n키보드 esc 버튼을 누르거나 이 버튼을 클릭하면 게임이 잠시 멈추며 옵션창과 능력치 창이 나온다뇨.\n그럼 힘내서 나와 탈출하자뇨!";
        texts[4] = "오늘은 보스 라운드다뇨! 10, 20, 30일은 보스 라운드니까 마음의 준비를 하라뇨.\n보스 라운드는 보스 몬스터와 일반 몬스터가 함께 등장한다뇨.";
        texts[5] = "일반 몬스터는 제한 시간동안만 등장한다뇨.\n제한 시간내에 잡은 일반 몬스터는 배트코인과 경험치를 모두 주지만, \n제한 시간이 지나고 잡은 일반 몬스터는 경험치만 준다뇨.";
        texts[6] = "또 보스 몬스터는 10, 20일에는 한 마리씩,\n30일에는 10, 20일에 나온 보스 두마리가 모두 등장한다뇨.\n보스 몬스터를 잡으면 1레벨이 오르고 많은 배트코인을 준다뇨!";
        texts[7] = "보스 몬스터를 전부 잡지 못한다면 제한 시간이 다 되어도 하루가 끝나지 않는다뇨.\n그럼 조심해서 잡으라뇨!";
    }
}
