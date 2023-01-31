using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePotion : MonoBehaviour
{
    Character character;

    private void Start()
    {
        character = Character.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            SoundManager.Instance.PlayES("EatSound");
            character.currentHp += 10;

            if (GameManager.Instance.buffNum != 0)
            {
                character.buffTime = 8;
                character.isBuff = true;
            }
            Destroy(gameObject);
        }
    }
}
