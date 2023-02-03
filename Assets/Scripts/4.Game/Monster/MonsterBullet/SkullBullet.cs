using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullBullet : MonsterBullet
{
    void Start()
    {
        gameManager = GameManager.Instance;
        coll = GetComponent<Collider>();
        Invoke("DestroyBullet", 3f);
        ShootDir();
        speed = (randNum == 0) ? 6f : 3f;
        realDamage = bulletDamage * (1 + Mathf.Floor(gameManager.round / 30)) + Mathf.Floor(gameManager.round / 5) * 2f;  // 트리거에도 있음
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (randNum == 0)
            transform.position += dir * speed * Time.deltaTime;

        else if (randNum == 1)
        {
            transform.Translate(new Vector3(dir.x, dir.z, 0f) * speed * Time.deltaTime, Space.Self);
            transform.position = new Vector3(transform.position.x , 0, transform.position.z);
            transform.RotateAround(monsPos, Vector3.up, 120f * Time.deltaTime);
        }
    }

    void ShootDir()
    {
        if (randNum == 0)
        {
            dir = (Character.Instance.transform.position - transform.position).normalized;
            dir = new Vector3(dir.x, 0f, dir.z);
        }

        else if (randNum == 1)
        {
            dir = (transform.position - monsPos).normalized;
            dir = new Vector3(dir.x, 0f, dir.z);
        }
    }
}
