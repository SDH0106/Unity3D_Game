using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoldBat : Monster
{
    [SerializeField] Slider monsterHpBar;

    int state = 0;

    float attackTime = 5;
    float initAttackTime = 5;
    bool isRush;

    Vector3 rushDir;

    Collider ground;

    bool isBerserk;
    //bool isBerserKTime;
    //float berserkTime;
    int dashSpeed;

    void Start()
    {
        StartSetting();
        ground = GameSceneUI.Instance.ground;
        attackTime = 5;
        initAttackTime = 5;
        dashSpeed = 8;
        isBerserk = false;
        //isBerserKTime = false;
        //berserkTime = 60f;
    }

    protected override void InitMonsterSetting()
    {
        base.InitMonsterSetting();
        state = 0;
        isRush = false;
        isBerserk = false;
        //isBerserKTime = false;
        attackTime = 5;
        initAttackTime = 5;
        dashSpeed = 8;
        anim.speed = 1f;
    }

    void Update()
    {
        monsterHpBar.value = 1 - (hp / maxHp);

/*        if(gameManager.currentGameTime <= 0 && !isBerserKTime)
        {
            gameManager.currentGameTime = berserkTime;
        }

        if(isBerserKTime)
        {
            berserkTime = Mathf.Clamp(berserkTime - Time.deltaTime, 0, berserkTime);
        }*/

        if (gameManager.currentGameTime <= 0 && gameManager.round == 30 && !isBerserk)
        {
            isBerserk = true;
            initcolor = new Color(1f, 0.5f, 0.5f, 1f);
            damage = stat.monsterDamage * (1 + Mathf.Floor(gameManager.round / 30)) + Mathf.Floor(gameManager.round / 5) * 2f * 2f;
            speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f) * 1.5f;
            initSpeed = speed;
            initAttackTime = 2.5f;
            dashSpeed = 12;
            rend.color = initcolor;
        }

        if (!isDead && !isFreeze)
        {
            if (!isAttack && state == 0)
            {
                Move();
                attackTime -= Time.deltaTime;
            }

            if (isRush)
            {
                transform.position += rushDir * dashSpeed * Time.deltaTime;
                transform.position = ground.ClosestPoint(transform.position);
            }

            if (attackTime <= 0)
            {
                int rand = Random.Range(0, 2);

                if (rand == 0)
                    Rush();

                else if (rand == 1)
                    Disappear();

                attackTime = initAttackTime;
            }

            anim.SetInteger("state", state);
            anim.SetBool("isWalk", isWalk);
            anim.SetBool("isAttack", isAttack);
            
        }

        BossDead();
    }

    void BossDead()
    {
        if (hp <= 0 || character.isDead)
        {
            anim.speed = 1f;
            rend.color = Color.white;
            isFreeze = false;
            coll.enabled = false;
            isDead = true;
            isAttacked = true;

            anim.SetBool("isAttacked", isAttacked);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    if (hp <= 0)
                    {
                        gameManager.money += 200;
                        SoundManager.Instance.PlayES("LevelUp");
                        character.level++;
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    void Disappear()
    {
        if (isBerserk)
        {
            anim.speed = 2f;
        }
        state = 1;
        isWalk = false;
        coll.enabled = false;
    }

    public void EndDisappear()
    {
        state = 2;
        transform.position = character.transform.position;
    }

    public void StartAppear()
    {
        state = 2;
        isWalk = false;
    }

    public void EndAppear()
    {
        state = 0;
        anim.speed = 1f;
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(0.3f);

        coll.enabled = true;
        isAttack = false;
        isWalk = true;
    }

    void Rush()
    {
        speed = 0;
        rushDir = (character.transform.position - transform.position).normalized;
        rushDir.y = 0;
        isAttack = true;
    }

    public void StartRush()
    {
        isRush = true;
    }

    public void EndRush()
    {
        isRush = false;
        speed = 0;
    }

    public void EndAttack()
    {
        speed = initSpeed;

        isAttack = false;
    }
}
