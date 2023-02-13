using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.UI;

public class AutoTarget : Bullet
{
    Transform target;
    bool isFind;

    private void Start()
    {
        gameManager = GameManager.Instance;
        isFind = false;
        isAbsorb = false;
    }

    void Update()
    {
        FindTarget();
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, initPos) > range)
        {
            DestroyBullet();
        }

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    void FindTarget()
    {
        if (target != null && target.GetComponent<Monster>().hp <= 0)
        {
            target = null;
            isFind = false;
        }

        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        float[] distances = new float[colliders.Length];

        if (colliders.Length > 0 && !isFind)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Monster")
                {
                    if (colliders[i].GetComponent<Monster>().hp > 0)
                    {
                        distances[i] = Vector3.Magnitude(colliders[i].transform.position - transform.position);
                        if (i == 0)
                            target = colliders[i].transform;

                        else if (i > 0)
                        {
                            if (distances[i] > distances[i - 1])
                                target = colliders[i].transform;

                            else if (distances[i] <= distances[i - 1])
                                target = colliders[i].transform;
                        }
                    }
                }
            }

            if (target != null && target.GetComponent<Monster>().hp > 0)
            {
                if (!isFind)
                {
                    dir = (target.transform.position - transform.position).normalized;
                    isFind = true;
                }
            }
        }
    }

    public override void Reflect(Collision collision)
    {
        base.Reflect(collision);
        target = null;
        isFind = false;
    }

    public override void DestroyBullet()
    {
        base.DestroyBullet();
        target = null;
        isFind = false;
    }
}
