using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePotion : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            Character.Instance.currentHp += 10;

            if (GameManager.Instance.buffNum != 0)
            {
                Character.Instance.buffTime = 8;
                Character.Instance.isBuff = true;
            }
            Destroy(gameObject);
        }
    }
}
