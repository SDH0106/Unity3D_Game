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
            count = 5;

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
                if (count < 5)
                {
                    tutoUis[count].SetActive(true);
                    descripText.text = texts[count];
                }

                else if (count >= 5)
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
                if (count < 9)
                {
                    descripText.text = texts[count];
                }

                else if(count >= 9)
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
        texts[3] = "�������� �뽬 �нú� ī�带 �����ϸ� �� ���� �뽬 ��ųâ�� �����ȴٴ�.\n��ų ��ư�� ������ ĳ���Ͱ� �̵� �������� ������ �����ϰ� �ϴ� ��ų�̴ٴ�.\n������ ��� �������°� �ȴٴ�.";
        texts[4] = "���������� �̰��� �Ͻ� ���� ��ư�̴ٴ�.\nŰ���� esc ��ư�� �����ų� �� ��ư�� Ŭ���ϸ� ������ ��� ���߸� �ɼ�â�� �ɷ�ġ â�� ���´ٴ�.\n�׷� ������ ���� Ż�����ڴ�!";
        texts[5] = "������ ���� ����ٴ�! 10, 20, 30���� ���� ����ϱ� ������ �غ� �϶�.\n���� ����� ���� ���Ϳ� �Ϲ� ���Ͱ� �Բ� �����Ѵٴ�.";
        texts[6] = "�Ϲ� ���ʹ� ���� �ð����ȸ� �����Ѵٴ�.\n���� �ð����� ���� �Ϲ� ���ʹ� ��Ʈ���ΰ� ����ġ�� ��� ������, \n���� �ð��� ������ ���� �Ϲ� ���ʹ� ����ġ�� �شٴ�.";
        texts[7] = "���� ���ʹ� 10, 20�Ͽ��� �� ������,\n30�Ͽ��� 10, 20�Ͽ� ���� ���� �θ����� ��� �����Ѵٴ�.\n���� ���͸� ������ 1������ ������ ���� ��Ʈ������ �شٴ�!";
        texts[8] = "������ ���� ���� ���ʹ� ���ѽð��� ������ ����ȭ ���°� �ȴٴ�.\n����ȭ�� �Ǹ� �������Ͱ� ���� �������ٴ�!.\n�׷� �����ؼ� ��ƺ��ڴ�!";
    }
}
