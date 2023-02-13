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
    [SerializeField] LayerMask monsterLayer;

    private IObjectPool<Bullet> pool;
    protected IObjectPool<DamageUI> damagePool;

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

    public int thunderCount;
    float luckThunderDmg;

    bool isLuck = false;

    Vector3 initBulletScale;

    Color initBulletColor;

    private void Awake()
    {
        pool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, maxSize: poolCount);
        damagePool = new ObjectPool<DamageUI>(CreateDamageUI, OnGetDamageUI, OnReleaseDamageUI, OnDestroyDamageUI, maxSize: poolCount);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        itemManager = ItemManager.Instance;
        character = Character.Instance;
        count = itemManager.weaponCount;
        damageUI = itemManager.damageUI[count];

        targets = new Transform[3];

        delay = 0;
        bulletDelay = weaponInfo.AttackDelay;
        attackRange = weaponInfo.WeaponRange;

        initBulletScale = bulletPrefab.transform.localScale;

        initBulletColor = bulletPrefab.GetComponent<SpriteRenderer>().color;

        canAttack = true;
        isTargetFind = false;
        thunderCount = character.thunderCount - 1;
        if (thunderCount > 0)
            itemManager.isThunderCountChange[thunderCount] = false;
    }

    void Update()
    {
        if (thunderCount > itemManager.thunderCount && itemManager.isThunderCountChange[thunderCount])
        {
            itemManager.isThunderCountChange[thunderCount] = false;
            thunderCount--;
        }

        grade = (int)(itemManager.weaponGrade[count] + 1);

        if (gameManager.currentScene == "Game" && !gameManager.isPause)
        {
            mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouse.y = transform.position.y;

            dir = mouse - transform.position;

            LookMousePosition();
            FireBullet();
        }
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
                    WeaponSetting();

                    if (!gameManager.doubleShot)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = normalFirePos.position;
                        bullet.bulletDamage = weaponDamage;
                        bullet.damageUI = damageUI;
                        bullet.speed = weaponInfo.BulletSpeed;
                        bullet.Shoot(dir.normalized,normalFirePos.position, weaponInfo.WeaponRange);
                    }

                    else if (gameManager.doubleShot)
                    {
                        Bullet bullet1 = pool.Get();
                        Bullet bullet2 = pool.Get();
                        bullet1.transform.position = doubleFirePos1.position;
                        bullet2.transform.position = doubleFirePos2.position;
                        bullet1.bulletDamage = weaponDamage;
                        bullet2.bulletDamage = weaponDamage;
                        bullet1.damageUI = damageUI;
                        bullet2.damageUI = damageUI;
                        bullet1.speed = weaponInfo.BulletSpeed;
                        bullet2.speed = weaponInfo.BulletSpeed;
                        bullet1.Shoot(dir.normalized,doubleFirePos1.position, weaponInfo.WeaponRange);
                        bullet2.Shoot(dir.normalized,doubleFirePos2.position, weaponInfo.WeaponRange);
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
                    WeaponSetting();

                    if (!gameManager.doubleShot)
                    {
                        Bullet bullet = pool.Get();
                        bullet.transform.position = normalFirePos.position;
                        bullet.bulletDamage = weaponDamage;
                        bullet.damageUI = damageUI;
                        bullet.speed = weaponInfo.BulletSpeed;
                        bullet.Shoot(dir.normalized,normalFirePos.position, weaponInfo.WeaponRange);
                        bullet.GetComponent<Fire>().grade = grade;
                    }

                    if (gameManager.doubleShot)
                    {
                        Bullet bullet1 = pool.Get();
                        Bullet bullet2 = pool.Get();
                        bullet1.transform.position = doubleFirePos1.position;
                        bullet2.transform.position = doubleFirePos2.position;
                        bullet1.damageUI = damageUI;
                        bullet2.damageUI = damageUI;
                        bullet1.speed = weaponInfo.BulletSpeed;
                        bullet2.speed = weaponInfo.BulletSpeed;
                        bullet1.GetComponent<Fire>().grade = grade;
                        bullet2.GetComponent<Fire>().grade = grade;
                        bullet1.Shoot(dir.normalized,doubleFirePos1.position, weaponInfo.WeaponRange);
                        bullet2.Shoot(dir.normalized,doubleFirePos2.position, weaponInfo.WeaponRange);
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
                        if (isLuck)
                        {
                            bullet.transform.localScale = new Vector3(initBulletScale.x * 2f, initBulletScale.y, initBulletScale.z);
                            bullet.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0);
                        }

                        else
                        {
                            bullet.transform.localScale = initBulletScale;
                            bullet.GetComponent<SpriteRenderer>().color = initBulletColor;
                        }
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

        detectRange = Mathf.Clamp(attackRange + gameManager.range * 0.5f, 1f, 12f);

        Collider[] colliders = Physics.OverlapSphere(character.transform.position, detectRange, monsterLayer);

        if (colliders.Length > 0)
        {
            var find = from target in colliders
                       where target.gameObject.CompareTag("Monster") && target.GetComponent<Monster>() != null
                       orderby Vector3.Distance(character.transform.position, target.transform.position)
                       select target.gameObject;

            int num = 0;

            if (find.Count() > 0)
            {
                WeaponSetting();
                isLuck = false;

                int rand = Random.Range(0, 100);

                foreach (var target in find)
                {
                    Monster monster = target.GetComponent<Monster>();

                    if (rand <= 5 + Mathf.Clamp(gameManager.luck, 0, 100) * 0.1)
                    {
                        luckThunderDmg = monster.maxHp * 0.05f;
                        isLuck = true;
                    }

                    else
                        luckThunderDmg = 0;

                    if (find.Count() <= 3)
                    {
                        targets[monsterCount] = target.transform;

                        DamageUI damage = damagePool.Get();
                        if (weaponDamage > monster.defence)
                            damage.isMiss = false;
                        else if (weaponDamage <= monster.defence)
                            damage.isMiss = true;
                        damage.realDamage = Mathf.Clamp(weaponDamage - monster.defence + luckThunderDmg, 0, weaponDamage - monster.defence + luckThunderDmg);
                        damage.UISetting();
                        damage.transform.position = target.transform.position;
                        damage.gameObject.transform.SetParent(gameManager.damageStorage);

                        monster.OnDamaged(damage.realDamage);

                        if (gameManager.absorbHp > 0 && !damage.isMiss && monsterCount == 0)
                            character.currentHp += gameManager.absorbHp;

                        monsterCount++;

                        if (monsterCount >= 3)
                        {
                            isTargetFind = true;
                            break;
                        }
                    }

                    else if (find.Count() > 3)
                    {
                        if (find.Count() >= character.thunderCount * 3)
                        {
                            if (num >= thunderCount * 3 && num < thunderCount * 3 + 3)
                            {
                                targets[monsterCount] = target.transform;

                                DamageUI damage = damagePool.Get();
                                if (weaponDamage > monster.defence)
                                    damage.isMiss = false;
                                else if (weaponDamage <= monster.defence)
                                    damage.isMiss = true;
                                damage.realDamage = Mathf.Clamp(weaponDamage - monster.defence + luckThunderDmg, 0, weaponDamage - monster.defence + luckThunderDmg);
                                damage.UISetting();
                                damage.transform.position = target.transform.position;
                                damage.gameObject.transform.SetParent(gameManager.damageStorage);

                                monster.OnDamaged(damage.realDamage);

                                if (gameManager.absorbHp > 0 && !damage.isMiss && monsterCount == 0)
                                    character.currentHp += gameManager.absorbHp;

                                monsterCount++;

                                if (monsterCount >= 3)
                                {
                                    isTargetFind = true;
                                    break;
                                }
                            }
                        }

                        else
                        {
                            int count = Mathf.Clamp(find.Count() - (thunderCount + 1) * 3, 0, find.Count() - 3);

                            if (num >= count && num < count + 3)
                            {
                                targets[monsterCount] = target.transform;

                                DamageUI damage = damagePool.Get();
                                if (weaponDamage > monster.defence)
                                    damage.isMiss = false;
                                else if (weaponDamage <= monster.defence)
                                    damage.isMiss = true;
                                damage.realDamage = Mathf.Clamp(weaponDamage - monster.defence + luckThunderDmg, 0, weaponDamage - monster.defence + luckThunderDmg);
                                damage.UISetting();
                                damage.transform.position = target.transform.position;
                                damage.gameObject.transform.SetParent(gameManager.damageStorage);

                                monster.OnDamaged(damage.realDamage);

                                if (gameManager.absorbHp > 0 && !damage.isMiss && monsterCount == 0)
                                    character.currentHp += gameManager.absorbHp;

                                monsterCount++;

                                if (monsterCount >= 3)
                                {
                                    isTargetFind = true;
                                    break;
                                }
                            }
                        }
                    }

                    num++;
                }

                if (find.Count() < 3)
                {
                    isTargetFind = true;
                }
            }
        }
    }

    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab, normalFirePos.position, bulletPrefab.transform.rotation).GetComponent<Bullet>();
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

    private DamageUI CreateDamageUI()
    {
        DamageUI damageUIPool = Instantiate(damageUI, transform.position, Quaternion.Euler(90, 0, 0)).GetComponent<DamageUI>();
        damageUIPool.SetManagedPool(damagePool);
        damageUIPool.transform.SetParent(gameManager.bulletStorage);
        return damageUIPool;
    }

    private void OnGetDamageUI(DamageUI damageUIPool)
    {
        damageUIPool.gameObject.SetActive(true);
    }

    private void OnReleaseDamageUI(DamageUI damageUIPool)
    {
        damageUIPool.gameObject.SetActive(false);
    }

    private void OnDestroyDamageUI(DamageUI damageUIPool)
    {
        Destroy(damageUIPool.gameObject);
    }

    private void OnDestroy()
    {
        if (pool != null)
            pool.Clear();
        if (damagePool != null)
            damagePool.Clear();
    }
}
