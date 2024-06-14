using Unity.VisualScripting;
using UnityEngine;

public class ShootArrow : FireProjectile
{
    [SerializeField] GameObject catchBar;

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
        projectile.GetComponent<ArrowTypeChange>().ChangeArrowType(GetComponent<BowCatchBar>().IsCatch());
        projectile.GetComponent<ArrowMovement>().Shoot(dir.normalized, normalFirePos.position, 10f);
    }

    public bool checkCanFire()
    {
        return canFire;
    }
}
