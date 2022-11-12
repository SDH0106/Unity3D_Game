using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Time.timeScale = 0;

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(gameObject);
            }
        }
    }
}
