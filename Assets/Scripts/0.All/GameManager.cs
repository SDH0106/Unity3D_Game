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
using UnityEngine.TextCore.Text;
using UnityEngine.Windows.WebCam;

public class GameManager : Singleton<GameManager>       
{
    [SerializeField] Texture2D aimCursor;
    [SerializeField] public Transform bulletStorage;
    [SerializeField] public Transform damageStorage;
    [SerializeField] GameObject optionUI;

    [Header("GameData")]
    [SerializeField] float gameTime;
    [SerializeField] public int money;
    [SerializeField] public int round;

    [Header("StatData")]
    [SerializeField] public float maxHp;
    [HideInInspector] public float percentDamage;
    [HideInInspector] public float physicDamage;
    [HideInInspector] public float elementDamage;
    [HideInInspector] public float shortDamage;
    [HideInInspector] public float longDamage;
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
    [HideInInspector] public float salePercent;
    [HideInInspector] public float increaseExp;
    [HideInInspector] public float coinRange;
    [HideInInspector] public float monsterSlow;
    [HideInInspector] public bool luckCoin;
    [HideInInspector] public bool luckDamage;
    [HideInInspector] public bool luckCritical;
    [HideInInspector] public bool doubleShot;
    [HideInInspector] public bool revive;
    #endregion

    [HideInInspector] public float currentGameTime;

    [HideInInspector] public string currentScene;

    UnityEngine.SceneManagement.Scene scene;

    [HideInInspector] public float[] stats;

    [HideInInspector] public bool isPause;
    [HideInInspector] public bool isClear;
    [HideInInspector] public bool isBossDead;

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
        increaseExp = 0;
        monsterSlow = 0;
        luckCoin = false;
        luckDamage = false;
        luckCritical = false;
        doubleShot = false;
        revive = false;
    }

    void InitArray()
    {
        stats = new float[15];
        stats[0] = maxHp;
        stats[1] = recoverHp;
        stats[2] = absorbHp;
        stats[3] = defence;
        stats[4] = physicDamage;
        stats[5] = elementDamage;
        stats[6] = shortDamage;
        stats[7] = longDamage;
        stats[8] = attackSpeed;
        stats[9] = speed;
        stats[10] = luck;
        stats[11] = range;
        stats[12] = critical;
        stats[13] = percentDamage;
        stats[14] = avoid;

        passiveIntVariables = new int[10];
        passiveIntVariables[0] = dashCount;

        passiveFloatVariables = new float[10];
        passiveFloatVariables[0] = coinRange;
        passiveFloatVariables[1] = increaseExp;
        passiveFloatVariables[2] = monsterSlow;
        passiveFloatVariables[3] = salePercent;

        passiveBoolVariables = new bool[5];
        passiveBoolVariables[0] = luckCoin;
        passiveBoolVariables[1] = luckDamage;
        passiveBoolVariables[2] = luckCritical;
        passiveBoolVariables[3] = doubleShot;
        passiveBoolVariables[4] = revive;
    }

    void StatArray()
    {
        maxHp = stats[0];
        recoverHp = stats[1];
        absorbHp = stats[2];
        defence = stats[3];
        physicDamage = stats[4];
        elementDamage = stats[5];
        shortDamage = stats[6];
        longDamage  = stats[7];
        attackSpeed = stats[8];
        speed = stats[9];
        luck = stats[10];
        range = stats[11];
        critical = stats[12];
        percentDamage = stats[13];
        avoid = stats[14];
    }

    void IntVariableArray()
    {
        dashCount = passiveIntVariables[0];
    }

    void FloatVariableArray()
    {
        coinRange = passiveFloatVariables[0];
        increaseExp = passiveFloatVariables[1];
        monsterSlow = passiveFloatVariables[2];
        salePercent = passiveFloatVariables[3];
    }

    void BoolVariableArray()
    {
        luckCoin = passiveBoolVariables[0];
        luckDamage = passiveBoolVariables[1];
        luckCritical = passiveBoolVariables[2];
        doubleShot = passiveBoolVariables[3];
        revive = passiveBoolVariables[4];
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        if (currentGameTime == 0)
        {
            isClear = true;
        }

        StatArray();
        IntVariableArray();
        FloatVariableArray();
        BoolVariableArray();
        OnGameScene();
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
