using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ChestOpen();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Character"))
        {
            anim.SetTrigger("isOpen");
            GameManager.Instance.levelUpCount++;
            SoundManager.Instance.PlayES("Chest");
        }
    }

    void ChestOpen()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
