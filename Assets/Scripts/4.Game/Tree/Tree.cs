using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Tree : MonoBehaviour
{
    [SerializeField] GameObject[] potionPrefab;
    [SerializeField] SpriteRenderer rend;

    int potionsNum;

    GameManager gameManager;

    bool isAttaked = false;

    private void Start()
    {
        gameManager = GameManager.Instance;
        potionsNum = gameManager.buffNum;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("myBullet") || other.CompareTag("Sword") || other.CompareTag("Thunder"))
        {
            if (!isAttaked)
            {
                isAttaked = true;
                GameObject potion = Instantiate(potionPrefab[potionsNum]);
                potion.transform.position = transform.position;
                gameManager.woodCount++;

                int num = Random.Range(0, 100);

                if (num < 3 + Mathf.Clamp(gameManager.luck, 0, 100) * 0.4)
                {
                    SoundManager.Instance.PlayES("ItemGet");
                    GameSceneUI.Instance.chestCount++;
                }

                Destroy(gameObject);
            }
        }

        else if (other.CompareTag("Character"))
            rend.color = new Color(1, 1, 1, 0.5f);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
            rend.color = new Color(1, 1, 1, 1);
    }
}
