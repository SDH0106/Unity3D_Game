using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography;
using System.Xml.Schema;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.Windows.WebCam;

public class GameManager : Singleton<GameManager>       
{
    [SerializeField] Texture2D aimCursor;
    [SerializeField] public Transform bulletStorage;
    [SerializeField] public Transform damageStorage;

    [Header("GameData")]
    [SerializeField] float gameTime;
    [SerializeField] public int money;
    [SerializeField] public int round;

    [Header("StatData")]
    [SerializeField] public float maxHp;
    [HideInInspector] public float percentDamage;
    [HideInInspector] public float physicDamage;
    [HideInInspector] public float magicDamage;
    /*[HideInInspector]*/ public float shortDamage;
    /*[HideInInspector]*/ public float longDamage;
    [SerializeField] public float recoverHp;
    [SerializeField] public float absorbHp;
    [SerializeField] public float defence;
    [SerializeField] public float attackSpeed;
    [SerializeField] public float speed;
    [SerializeField] public float range;
    [SerializeField] public float luck;
    [SerializeField] public float critical;
    [SerializeField] public float avoid;

    #region 특수 패시브
    [HideInInspector] public int[] passiveIntVariables;
    [HideInInspector] public float[] passiveFloatVariables;
    [HideInInspector] public bool[] passiveBoolVariables;
    [HideInInspector] public int dashCount;
    [HideInInspector] public int buffNum;
    [HideInInspector] public int exDmg;
    [HideInInspector] public float salePercent;
    [HideInInspector] public float increaseExp;
    [HideInInspector] public float coinRange;
    [HideInInspector] public float monsterSlow;
    [HideInInspector] public float summonASpd;
    [HideInInspector] public float summonPDmg;
    [HideInInspector] public bool luckCoin;
    [HideInInspector] public bool luckDamage;
    [HideInInspector] public bool luckCritical;
    [HideInInspector] public bool doubleShot;
    [HideInInspector] public bool revive;
    [HideInInspector] public bool ggoGgoSummon;
    [HideInInspector] public bool ilsoonSummon;
    [HideInInspector] public bool wakgoodSummon;
    [HideInInspector] public bool ddilpa;
    [HideInInspector] public bool butterfly;
    [HideInInspector] public bool subscriptionFee;
    [HideInInspector] public bool spawnTree;
    [HideInInspector] public bool dotgu;
    /*[HideInInspector]*/ public bool isReflect;

    #endregion

    [HideInInspector] public float currentGameTime;

    [HideInInspector] public string currentScene;

    UnityEngine.SceneManagement.Scene scene;

    [HideInInspector] public float[] stats;

    [HideInInspector] public bool isPause;
    [HideInInspector] public bool isClear;
    [HideInInspector] public bool isBossDead;

    [HideInInspector] public float gameStartTime;
    [HideInInspector] public float gameEndTime;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //InitSetting();
        InitArray();
        currentGameTime = gameTime;
        isPause = false;
        Time.timeScale = 1;
        isPause = false;
        isClear = false;
        isBossDead = false;
    }

    void InitSetting()
    {
        gameTime = 60;
        money = 0;
        round = 1;
        maxHp = 0;
        recoverHp = 0;
        absorbHp = 0;
        defence = 0;
        attackSpeed = 0;
        speed = 0;
        range = 0;
        luck = 0;
        critical = 5;
        salePercent = 0;
        dashCount = 0;
        buffNum = 0;
        increaseExp = 0;
        monsterSlow = 0;
        luckCoin = false;
        luckDamage = false;
        luckCritical = false;
        doubleShot = false;
        revive = false;
        ggoGgoSummon = false;
        ilsoonSummon = false;
        wakgoodSummon = false;
        subscriptionFee = false;
    }

    void InitArray()
    {
        stats = new float[15];
        stats[0] = maxHp;
        stats[1] = recoverHp;
        stats[2] = absorbHp;
        stats[3] = defence;
        stats[4] = physicDamage;
        stats[5] = magicDamage;
        stats[6] = shortDamage;
        stats[7] = longDamage;
        stats[8] = attackSpeed;
        stats[9] = speed;
        stats[10] = luck;
        stats[11] = range;
        stats[12] = critical;
        stats[13] = percentDamage;
        stats[14] = avoid;

        passiveIntVariables = new int[3];
        passiveIntVariables[0] = dashCount;
        passiveIntVariables[1] = buffNum;
        passiveIntVariables[2] = exDmg;

        passiveFloatVariables = new float[10];
        passiveFloatVariables[0] = coinRange;
        passiveFloatVariables[1] = increaseExp;
        passiveFloatVariables[2] = monsterSlow;
        passiveFloatVariables[3] = salePercent;
        passiveFloatVariables[4] = summonASpd;
        passiveFloatVariables[5] = summonPDmg;

        passiveBoolVariables = new bool[14];
        passiveBoolVariables[0] = luckCoin;
        passiveBoolVariables[1] = luckDamage;
        passiveBoolVariables[2] = luckCritical;
        passiveBoolVariables[3] = doubleShot;
        passiveBoolVariables[4] = revive;
        passiveBoolVariables[5] = ggoGgoSummon;
        passiveBoolVariables[6] = ilsoonSummon;
        passiveBoolVariables[7] = wakgoodSummon;
        passiveBoolVariables[8] = ddilpa;
        passiveBoolVariables[9] = butterfly;
        passiveBoolVariables[10] = subscriptionFee;
        passiveBoolVariables[11] = spawnTree;
        passiveBoolVariables[12] = dotgu;
        passiveBoolVariables[13] = isReflect;
    }

    void StatArray()
    {
        maxHp = stats[0];
        recoverHp = stats[1];
        absorbHp = stats[2];
        defence = stats[3];
        physicDamage = stats[4];
        magicDamage = stats[5];
        shortDamage = stats[6];
        longDamage = stats[7];
        if (!Character.Instance.isBuff)
            attackSpeed = stats[8];
        speed = stats[9];
        luck = stats[10];
        range = stats[11];
        critical = stats[12];
        if (!Character.Instance.isBuff)
            percentDamage = stats[13];
        avoid = stats[14];
    }

    void IntVariableArray()
    {
        dashCount = passiveIntVariables[0];
        buffNum = passiveIntVariables[1];
        exDmg = passiveIntVariables[2];
    }

    void FloatVariableArray()
    {
        coinRange = passiveFloatVariables[0];
        increaseExp = passiveFloatVariables[1];
        monsterSlow = passiveFloatVariables[2];
        salePercent = passiveFloatVariables[3];
        summonASpd = passiveFloatVariables[4];
        summonPDmg = passiveFloatVariables[5];
    }

    void BoolVariableArray()
    {
        luckCoin = passiveBoolVariables[0];
        luckDamage = passiveBoolVariables[1];
        luckCritical = passiveBoolVariables[2];
        doubleShot = passiveBoolVariables[3];
        revive = passiveBoolVariables[4];
        ggoGgoSummon = passiveBoolVariables[5];
        ilsoonSummon = passiveBoolVariables[6];
        wakgoodSummon = passiveBoolVariables[7];
        ddilpa = passiveBoolVariables[8];
        butterfly = passiveBoolVariables[9];
        subscriptionFee = passiveBoolVariables[10];
        spawnTree = passiveBoolVariables[11];
        dotgu = passiveBoolVariables[12];
        isReflect = passiveBoolVariables[13];
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        if (currentGameTime == 0)
        {
            isClear = true;
        }

        if (scene.buildIndex > 1)
        {
            StatArray();
            IntVariableArray();
            FloatVariableArray();
            BoolVariableArray();

            OnGameScene();
        }
    }

    void OnGameScene()
    {
        if (currentScene == "Game")
        {
            Character.Instance.gameObject.SetActive(true);

            if (Character.Instance.currentHp > 0)
                currentGameTime -= Time.deltaTime;

            if (currentGameTime <= 0)
            {
                currentGameTime = 0;
            }

            if (money <= 0)
                money = 0;

            //Cursor.SetCursor(aimCursor, new Vector2(aimCursor.width / 2, aimCursor.height / 2), CursorMode.Auto);
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
    }
}
