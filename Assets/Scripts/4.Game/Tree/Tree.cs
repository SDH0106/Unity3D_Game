using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] GameObject[] potionPrefab;

    int potionsNum;
    SpriteRenderer rend;

    GameManager gameManager;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
        potionsNum = gameManager.buffNum;
    }

    private void Update()
    {
        potionsNum = GameManager.Instance.buffNum;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("myBullet") || other.CompareTag("Sword"))
        { 
            GameObject potion = Instantiate(potionPrefab[potionsNum]);
            potion.transform.position = transform.position;
            gameManager.woodCount++;

            int num = Random.Range(0, 100);

            if (num < 3 + gameManager.luck * 0.4)
            {
                SoundManager.Instance.PlayES("ItemGet");
                GameSceneUI.Instance.chestCount++;
            }

            Destroy(gameObject);
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
