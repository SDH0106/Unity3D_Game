using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using System.Linq;

public class StaffControl : Weapon
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform normalFirePos;
    [SerializeField] Transform doubleFirePos1;
    [SerializeField] Transform doubleFirePos2;
    [SerializeField] int poolCount;

    private IObjectPool<Bullet> pool;

    Vector3 dir, mouse;

    float delay;
    float bulletDelay;

    float detectRange;
    float attackRange;

    bool canAttack;
    bool isTargetFind;

    Transform[] targets;

    int monsterCount;

    Character character;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        count = ItemManager.Instance.weaponCount;
        damageUI = ItemManager.Instance.damageUI[count];

        targets = new Transform[3];

        delay = 0;
        bulletDelay = weaponInfo.AttackDelay;
        attackRange = weaponInfo.WeaponRange;

        canAttack = true;
        isTargetFind = false;
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
        if (weaponInfo.WeaponName == "얼음 스태프")
        {
            if (canAttack == true)
            {
                if (Input.GetMouseButton(0) && (!gameManager.isClear || !gameManager.isBossDead))
                {
                    if (!gameManager.doubleShot)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = normalFirePos.position;
                        bullet.Shoot(dir.normalized,normalFirePos.position, weaponInfo.WeaponRange);
                        bullet.damageUI = damageUI;
                        bullet.speed = weaponInfo.BulletSpeed;
                    }

                    if (gameManager.doubleShot)
                    {
                        Bullet bullet1 = pool.Get();
                        Bullet bullet2 = pool.Get();
                        bullet1.transform.position = doubleFirePos1.position;
                        bullet2.transform.position = doubleFirePos2.position;
                        bullet1.Shoot(dir.normalized,doubleFirePos1.position, weaponInfo.WeaponRange);
                        bullet2.Shoot(dir.normalized,doubleFirePos2.position, weaponInfo.WeaponRange);
                        bullet1.damageUI = damageUI;
                        bullet2.damageUI = damageUI;
                        bullet1.speed = weaponInfo.BulletSpeed;
                        bullet2.speed = weaponInfo.BulletSpeed;
                    }

                    SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                    canAttack = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;

                if (gameManager.attackSpeed >= 0)
                {
                    if (delay >= (bulletDelay / (1 + gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }

                else if (gameManager.attackSpeed < 0)
                {
                    // 공속이 음수인 경우이므로 음수를 빼 (+로 만들어) 딜레이를 늘린다.
                    if (delay >= (bulletDelay - (gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }
            }
        }
        if (weaponInfo.WeaponName == "화염 스태프")
        {
            if (canAttack == true)
            {
                if (Input.GetMouseButton(0) && (!gameManager.isClear || !gameManager.isBossDead))
                {
                    if (!gameManager.doubleShot)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = normalFirePos.position;
                        bullet.Shoot(dir.normalized,normalFirePos.position, weaponInfo.WeaponRange);
                        bullet.damageUI = damageUI;
                        bullet.speed = weaponInfo.BulletSpeed;
                        bullet.GetComponent<Fire>().grade = grade;
                    }

                    if (gameManager.doubleShot)
                    {
                        Bullet bullet1 = pool.Get();
                        Bullet bullet2 = pool.Get();
                        bullet1.transform.position = doubleFirePos1.position;
                        bullet2.transform.position = doubleFirePos2.position;
                        bullet1.Shoot(dir.normalized,doubleFirePos1.position, weaponInfo.WeaponRange);
                        bullet2.Shoot(dir.normalized,doubleFirePos2.position, weaponInfo.WeaponRange);
                        bullet1.damageUI = damageUI;
                        bullet2.damageUI = damageUI;
                        bullet1.speed = weaponInfo.BulletSpeed;
                        bullet2.speed = weaponInfo.BulletSpeed;
                        bullet1.GetComponent<Fire>().grade = grade;
                        bullet2.GetComponent<Fire>().grade = grade;
                    }

                    SoundManager.Instance.PlayES(weaponInfo.WeaponSound);
                    canAttack = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;

                if (gameManager.attackSpeed >= 0)
                {
                    if (delay >= (bulletDelay / (1 + gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }

                else if (gameManager.attackSpeed < 0)
                {
                    if (delay >= (bulletDelay - (gameManager.attackSpeed * 0.1)))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }
            }
        }

        if (weaponInfo.WeaponName == "번개 스태프")
        {
            if (canAttack == true)
            {
                if (!isTargetFind)
                {
                    FindTarget();
                }

                else if (isTargetFind)
                {
                    if (monsterCount > 0)
                        SoundManager.Instance.PlayES(weaponInfo.WeaponSound);

                    for (int i = 0; i < monsterCount; i++)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = new Vector3(targets[i].transform.position.x, 0, targets[i].transform.position.z + 3);
                        bullet.damageUI = damageUI;
                    }

                    canAttack = false;
                    isTargetFind = false;
                }
            }

            else if (canAttack == false)
            {
                delay += Time.deltaTime;
                if (gameManager.attackSpeed >= 0)
                {
                    if (delay >= (bulletDelay / (1 + (gameManager.attackSpeed * 0.05f))))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }

                else if (gameManager.attackSpeed < 0)
                {
                    if (delay >= (bulletDelay - gameManager.attackSpeed * 0.2f))
                    {
                        canAttack = true;
                        delay = 0;
                    }
                }
            }
        }
    }

    void FindTarget()
    {
        monsterCount = 0;

        detectRange = Mathf.Clamp(attackRange + gameManager.range, 1f, 12f);

        Collider[] colliders = Physics.OverlapSphere(character.transform.position, detectRange);

        if (colliders.Length > 0)
        {
            var find = from target in colliders
                       orderby Vector3.Distance(character.transform.position, target.transform.position)
                       where target.gameObject.CompareTag("Monster") && target.GetComponent<Monster>() != null
                       select target.gameObject;

            int rand = Random.Range(0, (find.Count() / 3) + (monsterCount + 1));

            int num = 0;

            foreach(var target in find)
            {
                if (num == rand)
                {
                    targets[monsterCount] = target.transform;

                    if (damageUI.weaponDamage > target.GetComponent<Monster>().defence)
                        damageUI.isMiss = false;

                    else if (damageUI.weaponDamage <= target.GetComponent<Monster>().defence)
                        damageUI.isMiss = true;

                    damageUI.realDamage = damageUI.weaponDamage - target.GetComponent<Monster>().defence;

                    target.GetComponent<Monster>().OnDamaged(damageUI.realDamage);

                    DamageUI pool = Instantiate(damageUI, targets[monsterCount].transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
                    pool.gameObject.transform.SetParent(gameManager.damageStorage);

                    if (gameManager.absorbHp > 0)
                        character.currentHp += gameManager.absorbHp;

                    monsterCount++;

                    if (monsterCount == 1)
                        rand = Random.Range(rand, (find.Count() / 3) * 2 + (monsterCount + 1));

                    else
                        rand = Random.Range(rand, find.Count());

                    if (monsterCount >= 3)
                    {
                        isTargetFind = true;
                        break;
                    }
                }

                num++;
            }

            if(find.Count() < 3 && find.Count() > 0)
            {
                isTargetFind = true;
            }
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, normalFirePos.position, transform.rotation).GetComponent<Bullet>();
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
        Gizmos.DrawWireSphere(Character.Instance.transform.position, 4f);
    }
}
