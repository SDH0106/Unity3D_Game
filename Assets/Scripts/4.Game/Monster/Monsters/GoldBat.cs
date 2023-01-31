using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GoldBat : Monster
{
    [SerializeField] Slider monsterHpBar;

    int state = 0;

    float attackTime = 5;
    bool isRush;

    Vector3 rushDir;

    Collider ground;

    void Start()
    {
        StartSetting();
        ground = GameSceneUI.Instance.ground;
    }

    protected override void InitMonsterSetting()
    {
        base.InitMonsterSetting();
        state = 0;
        isRush = false;
        attackTime = 5;
    }

    void Update()
    {
        monsterHpBar.value = 1 - (hp / maxHp);

        if (!isDead && !isFreeze)
        {
            anim.speed = 1f;
            if (!isAttack && state == 0)
            {
                Move();
                attackTime -= Time.deltaTime;
            }

            if (isRush)
            {
                transform.position += rushDir * 8 * Time.deltaTime;
                transform.position = ground.ClosestPoint(transform.position);
            }

            if (attackTime <= 0)
            {
                int rand = Random.Range(0, 2);

                if (rand == 0)
                    Rush();

                else if (rand == 1)
                    Disappear();

                attackTime = 5;
            }

            anim.SetInteger("state", state);
            anim.SetBool("isWalk", isWalk);
            anim.SetBool("isAttack", isAttack);
            
        }

        if(isFreeze)
        {
            anim.speed = 0f;
        }

        BossDead();
    }

    void BossDead()
    {
        if (hp <= 0 || character.isDead)
        {
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
                        gameManager.money += 500;
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
        StartCoroutine(CoolTime());
    }

    IEnumerator CoolTime()
    {
        yield return new WaitForSeconds(0.5f);

        coll.enabled = true;
        isAttack = false;
        isWalk = true;
    }

    void Rush()
    {
        speed = 0;
        rushDir = (character.transform.position - transform.position).normalized;
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
        speed = stat.monsterSpeed * (1 - gameManager.monsterSlow * 0.01f);
        isAttack = false;
    }
}
