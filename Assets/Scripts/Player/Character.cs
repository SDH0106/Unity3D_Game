using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Singleton<Character>
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] SpriteRenderer rend;
    [SerializeField] Animator anim;
    [SerializeField] Transform damageStorage;
    [SerializeField] LayerMask coinLayer;

    [Header("Stat")]
    [SerializeField] public float speed;
    [SerializeField] float invincibleTime;
    [SerializeField] public int hp = 10;
    [SerializeField] int attackDamage = 1;
    [SerializeField] public float maxExp;

    [Header("WeaponPos")]
    [SerializeField] GameObject weaponPrefab;
    [SerializeField] Transform[] weaponPoses;
    [SerializeField] public Transform bulletStorage;

    bool isRun, isAttacked, isDead = false;

    [HideInInspector] public int maxHp;
    [HideInInspector] public int money;
    [HideInInspector] public float exp;

    public int AttackDamage => attackDamage;

    public Transform DamageStorage => damageStorage;

    WeaponCardUI weaponCardUI;

    int weaponPosNum;

    // Start is called before the first frame update
    void Start()
    {
        weaponPosNum = 0;
        maxHp = hp;
        money = 0;
        exp = 0f;

        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        weaponCardUI = GetComponent<WeaponCardUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead == false)
        {
            Move();
            //EquipWeapon();
        }

        anim.SetBool("isRun", isRun);

    }

    void ReleaseInvincible()
    {
        isAttacked = false;
    }

    public void EquipWeapon()
    {
        GameObject weapon = Instantiate(weaponPrefab, weaponPoses[weaponPosNum].position, transform.rotation);
        weapon.transform.SetParent(weaponPoses[weaponPosNum]);
        weaponPosNum++;
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        transform.position += new Vector3(x, 0, z) * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            rend.flipX = true;
            isRun = true;
        }

        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            rend.flipX = false;
            isRun = true;
        }

        else if(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            isRun = true;
        }

        else
            isRun = false;
    }

    private IEnumerator PlayerColorBlink()
    {
        Color red = new Color(1, 0, 0, 0.5f);
        Color white = new Color(1, 1, 1, 0.5f);

        for (int i = 0; i < 3; i++)
        {
            rend.color = red;
            yield return new WaitForSeconds(0.1f);

            rend.color = white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void Ondamaged(Collider other)
    {
        if (other.tag == "Monster" && isAttacked == false)
        {
            hp -= other.gameObject.GetComponent<Monster>().AttackDamage;
            StartCoroutine(OnInvincible());
        }

        if (hp <= 0)
        {
            hp = 0;
            isDead = true;

            anim.SetBool("isDead", isDead);

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("PlayerDie"))
            {
                if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
                {
                    Time.timeScale = 0;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Ondamaged(other);
    }

    IEnumerator OnInvincible()
    {
        anim.SetTrigger("isAttacked");
        isAttacked = true;
        StartCoroutine(PlayerColorBlink());

        yield return new WaitForSeconds(invincibleTime);
        rend.color = Color.white;
        isAttacked = false;
    }
    /*    void GetCoin()
        {
            float radius = 2;

            bool isInRadius = Physics.CheckSphere(transform.position, radius, coinLayer);

            if (isInRadius)
            {
                Coin.Instance.MoveCoin(transform.position, radius);
            }


            Collider[] hit = Physics.OverlapSphere(transform.position, radius, coinLayer);

            foreach (Collider c in hit)
            {
                int i = 0;
                if (c.name == "Coin")
                {
                    i++;
                    continue;
                }
                Debug.Log(i);

                Coin.Instance.MoveCoin(transform.position, radius);
            }

            //Debug.Log(money);
        }
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, 2);
        }
        */
}
