using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Platform;
using UnityEngine;

public class Plant : Monster
{
    [SerializeField] GameObject plantBullet;
    [SerializeField] Transform bulletPos;

    private void Start()
    {
        hp = stat.monsterMaxHp;
        initScale = transform.localScale;
        speed = stat.monsterSpeed;
        rend = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider>();
        StartCoroutine(Attack());
    }

    private void Update()
    {
        if (isDead == false)
        {
            Move();
        }

        OnDead();
    }

    IEnumerator Attack()
    {
        while (isDead == false)
        {
            yield return new WaitForSeconds(3f);

            anim.SetTrigger("isAttack");

            yield return new WaitForSeconds(0.3f);

            Instantiate(plantBullet, bulletPos.position, plantBullet.transform.rotation);
        }
    }
}
