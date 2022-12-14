using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class AutoTarget : Bullet
{
    Transform target;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        transform.position += new Vector3(dir.x, 0, dir.z) * speed * Time.deltaTime;
        FindTarget();

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 2f);
        float[] distances = new float[colliders.Length];

        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Monster")
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

            if (target != null)
                dir = (target.transform.position - transform.position).normalized;
        }
    }

    public override void DestroyBullet()
    {
        base.DestroyBullet();
        target = null;
    }
}
