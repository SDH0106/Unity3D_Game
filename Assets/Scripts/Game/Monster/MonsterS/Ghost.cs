using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Monster
{
    int state = 0;

    void Start()
    {
        hp = stat.monsterMaxHp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        InvokeRepeating("Disappear", 3f, 3f);
    }

    void Update()
    {
        if (isDead == false)
        {
            Move();
            anim.SetInteger("state", state);
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Disappear"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                gameObject.SetActive(false);
                Appear();
            }
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Appear"))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                state = 0;
                coll.enabled = true;
            }
        }

        OnDead();

        if (isDead == true)
            GameManager.Instance.isClear = true;
    }

    void Disappear()
    {
        state = 1;
    }

    void Appear()
    {
        state = 2;
        transform.position = Character.Instance.transform.position;
        gameObject.SetActive(true);
        coll.enabled = false;
    }
}
