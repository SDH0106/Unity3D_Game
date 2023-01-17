using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] GameObject[] potionPrefab;

    int potionsNum;
    SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        potionsNum = GameManager.Instance.buffNum;
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
