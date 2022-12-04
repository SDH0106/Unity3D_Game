using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;

public class StaffControl : Weapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePos;
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    Vector3 dir, mouse;

    float delay = 0;
    float bulletDelay = 1;
    float thunderDelay = 3;

    bool canAttack = true;
    bool isTargetFind = false;
    Transform target;

    GameManager gameManager;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        count = ItemManager.Instance.weaponCount;
        damageUI = ItemManager.Instance.damageUI[count];
    }

    void Update()
    {
        grade = (int)(ItemManager.Instance.weaponGrade[count] + 1);

        if (gameManager.currentScene == "Game" && !gameManager.isPause)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - transform.position;

            LookMousePosition();
            FireBullet();
        }

        WeaponSetting();
    }

    void LookMousePosition()
    {
        if (dir.x < 0)
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;

        else
            transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
    }

    void FireBullet()
    {
        if (weaponInfo.WeaponName != "번개 스태프")
        {
            if (canAttack == true)
            {
                if (Input.GetMouseButton(0))
                {
                    Bullet bullet = pool.Get();
                    bullet.transform.position = firePos.position;
                    bullet.Shoot(dir.normalized);
                    bullet.damageUI = damageUI;
                    SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                    canAttack = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;
                if (delay >= (bulletDelay / (1 + gameManager.attackSpeed / 10)))
                {
                    canAttack = true;
                    delay = 0;
                }
            }
        }

        else if (weaponInfo.WeaponName == "번개 스태프")
        {
            if (canAttack == true)
            {
                if (!isTargetFind)
                    FindTarget();

                else if (isTargetFind)
                {
                    Bullet bullet = pool.Get();
                    bullet.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z + 3);
                    bullet.damageUI = damageUI;
                    SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                    canAttack = false;
                    isTargetFind = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;
                if (delay >= (thunderDelay / (1 + gameManager.attackSpeed / 10)))
                {
                    canAttack = true;
                    delay = 0;
                }
            }
        }
    }

    void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(Character.Instance.transform.position, 6f + (gameManager.range/10));

        if (colliders.Length > 0)
        {
            int rand = Random.Range(0, colliders.Length);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].tag == "Monster" && colliders[i].GetComponent<Monster>() != null)
                {
                    if (i == rand)
                    {
                        target = colliders[i].transform;

                        GameObject pool = Instantiate(damageUI, colliders[i].transform.position, Quaternion.Euler(90, 0, 0)).gameObject;
                        pool.transform.SetParent(GameManager.Instance.damageStorage);
                        colliders[i].GetComponent<Monster>().OnDamaged(damageUI.weaponDamage);
                        GameManager.Instance.hp += GameManager.Instance.absorbHp;

                        isTargetFind = true;
                        break;
                    }
                }
            }
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, firePos.position, transform.rotation).GetComponent<Bullet>();
        bullet.SetManagedPool(pool);
        bullet.transform.SetParent(gameManager.bulletStorage);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, mouse);

        Handles.color = Color.blue;
        Handles.DrawWireDisc(Character.Instance.transform.position,Vector3.up, 6f + (gameManager.range/10));
    }
}
