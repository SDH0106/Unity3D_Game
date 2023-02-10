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
        texts[0] = "�ݰ���. ���� ���Ĵٴ�. ���� Ż���� ���� �� �ְ� ���� �������ְڴٴ�.\n�켱 �̰��� �� ü�°� ����ġ�ٴ�.\nü���� 0�� �Ǹ� Ż�⿡ �����ϰ�, ����ġ�� ������ �� ���� �������� �Ѵٴ�.\n�������� �ϸ� ü���� 1������ �ɷ�ġ ī�带 �شٴ�!";
        texts[1] = "�� ������ ��¥�� ���� �ð��̴ٴ�.\n���� �ð����� ���͵��� ���´ٴ�. ���� �ð��� ������ �Ϸ簡 ������ �������� �Ѿ�ٴ�.\n��¥�� 30���� �Ǹ� ���� Ż���� �� �ִ� ������ �ȴٴ�.";
        texts[2] = "�� ���� ���ĸ� �����ٴ�.\n���ĸ� ������ ��� �踦 ���� Ż���Ѵٴ�. 30���� ������ ������ �� 70���� ������ ��ƾ��Ѵٴ�. ���� ������ ���Ѵٸ� Ż�⿡ �����Ѵٴ�.";
        texts[3] = "���������� �̰��� �Ͻ� ���� ��ư�̴ٴ�.\nŰ���� esc ��ư�� �����ų� �� ��ư�� Ŭ���ϸ� ������ ��� ���߸� �ɼ�â�� �ɷ�ġ â�� ���´ٴ�.\n�׷� ������ ���� Ż�����ڴ�!";
        texts[4] = "������ ���� ����ٴ�! 10, 20, 30���� ���� ����ϱ� ������ �غ� �϶�.\n���� ����� ���� ���Ϳ� �Ϲ� ���Ͱ� �Բ� �����Ѵٴ�.";
        texts[5] = "�Ϲ� ���ʹ� ���� �ð����ȸ� �����Ѵٴ�.\n���� �ð����� ���� �Ϲ� ���ʹ� ��Ʈ���ΰ� ����ġ�� ��� ������, \n���� �ð��� ������ ���� �Ϲ� ���ʹ� ����ġ�� �شٴ�.";
        texts[6] = "�� ���� ���ʹ� 10, 20�Ͽ��� �� ������,\n30�Ͽ��� 10, 20�Ͽ� ���� ���� �θ����� ��� �����Ѵٴ�.\n���� ���͸� ������ 1������ ������ ���� ��Ʈ������ �شٴ�!";
        texts[7] = "���� ���͸� ���� ���� ���Ѵٸ� ���� �ð��� �� �Ǿ �Ϸ簡 ������ �ʴ´ٴ�.\n�׷� �����ؼ� ������!";
    }
}
