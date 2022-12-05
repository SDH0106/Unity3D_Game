using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Xml.Schema;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>       
{
    [SerializeField] Texture2D aimCursor;
    [SerializeField] public Transform bulletStorage;
    [SerializeField] public Transform damageStorage;

    [Header("GameData")]
    [SerializeField] float gameTime;
    [SerializeField] public int money;
    [SerializeField] public int round;

    [HideInInspector] public float exp;
    [HideInInspector] public float hp;


    [Header("CharacterData")]
    public int level;
    [SerializeField] public float maxHp;
    [SerializeField] public float maxExp;
    [HideInInspector] public float totalDamage;
    [HideInInspector] public float physicDamage;
    [HideInInspector] public float elementDamage;
    [SerializeField] public float recoverHp;
    [SerializeField] public float absorbHp;
    [SerializeField] public float defence;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float speed;
    [SerializeField] public float range;
    [SerializeField] public float luck;
    [SerializeField] public float critical;

    [HideInInspector] public float currentGameTime;

    [HideInInspector] public string currentScene;

    UnityEngine.SceneManagement.Scene scene;

     public int levelUpCount;

    [HideInInspector] public float[] stats;

    float recoverTime = 3;

    [HideInInspector] public bool isPause = false;
    [HideInInspector] public bool isClear = false;
    [HideInInspector] public bool isBossDead = false;

    int beforeLevel;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        beforeLevel = level;
        DontDestroyOnLoad(gameObject);
        //InitStatSetting();
        InitArray();

        currentGameTime = gameTime;
        hp = maxHp;

        isPause = false;
        Time.timeScale = 1;
    }

    void InitStatSetting()
    {
        level = 1;
        levelUpCount = 0;
        maxHp = 20;
        maxExp = 10;
        recoverHp = 0;
        absorbHp = 0;
        defence = 1;
        attackSpeed = 1;
        speed = 2;
        range = 1;
        luck = 0;
        critical = 10;
    }

    void InitArray()
    {
        stats = new float[11];
        stats[0] = maxHp;
        stats[1] = recoverHp;
        stats[2] = absorbHp;
        stats[3] = defence;
        stats[4] = physicDamage;
        stats[5] = elementDamage;
        stats[6] = attackSpeed;
        stats[7] = speed;
        stats[8] = luck;
        stats[9] = range;
        stats[10] = critical;
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        if (exp ==  maxExp)
        {
            SoundManager.Instance.PlayES("LevelUp");
            level++;
            maxExp *= level;
            exp = 0;
        }

        if(level != beforeLevel)
        {
            levelUpCount = level - beforeLevel;
        }

        if (hp > maxHp)
        {
            hp = maxHp;
        }

        if (currentGameTime == 0)
            isClear = true;

        StatArray();
        OnGameScene();
    }

    void StatArray()
    {
        maxHp = stats[0];
        recoverHp = stats[1];
        absorbHp = stats[2];
        defence = stats[3];
        physicDamage = stats[4];
        elementDamage = stats[5];
        attackSpeed = stats[6];
        speed = stats[7];
        luck = stats[8];
        range = stats[9];
        critical = stats[10];
    }

    void OnGameScene()
    {
        if (currentScene == "Game" && hp > 0)
        {
            currentGameTime -= Time.deltaTime;

            if (currentGameTime <= 0)
            {
                currentGameTime = 0;
            }

            if (money <= 0)
                money = 0;

            AutoRecoverHp();

            //Cursor.SetCursor(aimCursor, new Vector2(aimCursor.width / 2, aimCursor.height / 2), CursorMode.Auto);
        }
    }

    void AutoRecoverHp()
    {
        if (recoverHp > 0 && hp != maxHp)
        {
            recoverTime -= Time.deltaTime;
            if(recoverTime <= 0)
            {
                recoverTime = 3;
                hp += recoverHp;
            }
        }
    }

    public void ToShopScene()
    {
        Character.Instance.gameObject.SetActive(true);
        Character.Instance.transform.position = Vector3.zero;
        currentScene = "Shop";
        SceneManager.LoadScene(currentScene);
        currentGameTime = gameTime;
        isClear = false;
        beforeLevel = level;
    }
}
