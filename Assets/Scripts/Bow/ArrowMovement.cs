using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] float range;
    [SerializeField] float speed;

    Vector3 dir;
    Vector3 initPos;
    float angle;

    private void Update()
    {
        if (Vector3.Distance(transform.position, initPos) > range)
            GetComponent<ProjectileObjectPool>().DestroyProjectile();

        transform.position += dir * speed * Time.deltaTime;

        // ÃÑ¾Ë °¢µµ
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(90, -angle, 0);
    }

    public virtual void Shoot(Vector3 dir, Vector3 initPos)
    {
        dir.y = 0;
        this.dir = dir;
        this.initPos = initPos;
    }
}
