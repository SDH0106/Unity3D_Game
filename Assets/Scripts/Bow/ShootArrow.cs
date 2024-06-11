using UnityEngine;

public class ShootArrow : FireProjectile
{
    [SerializeField] GameObject enhancedProjectilePrefab;

    void Update()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouse.y = 0;

        dir = mouse - transform.position;

        FireAppliedCoolTime();
    }

    protected override void SettingProjectile()
    {
        base.SettingProjectile();
        projectile.GetComponent<ArrowMovement>().Shoot(dir.normalized, normalFirePos.position); projectile.GetComponent<ArrowMovement>().Shoot(dir.normalized, normalFirePos.position);
    }

    public bool checkCanFire()
    {
        return canFire;
    }
}
