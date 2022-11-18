using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>       
{
    [SerializeField] public Transform bulletStorage;
    [SerializeField] public Transform damageStorage;

    [Header("GameData")]
    [SerializeField] float gameTime;
    [SerializeField] public int money;
    [SerializeField] public int round;

    [HideInInspector] public float exp;
    [HideInInspector] public float hp;


    [Header("CharacterData")]
    int level;
    [SerializeField] public float maxHp;
    [SerializeField] public float maxExp;
    [HideInInspector] public float totalDamage;
    [HideInInspector] public float physicDamage;
    [HideInInspector] public float elementDamage;
    [HideInInspector] public float defence;
    [HideInInspector] public float absorbHp;
    [HideInInspector] public float recoverHp;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public float range;
    [HideInInspector] public float luck;

    [HideInInspector] public float currentGameTime;

    [HideInInspector] public string currentScene;

    UnityEngine.SceneManagement.Scene scene;

     public int levelUpCount;

    [HideInInspector] public float[] stats;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        stats = new float[10];
        stats[0] = maxHp;
        level = 1;
        //levelUpCount = 0;
        hp = maxHp;
        currentGameTime = gameTime;
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        if(exp ==  maxExp)
        {
            level++;
            levelUpCount++;
            maxExp *= level;
            exp = 0;
        }

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
    }

    void OnGameScene()
    {
        if (currentScene == "Game")
        {
            currentGameTime -= Time.deltaTime;

            if (currentGameTime <= 0)
            {
                currentGameTime = 0;
            }

            if (money <= 0)
                money = 0;
        }
    }

    public void ToShopScene()
    {
        Character.Instance.gameObject.SetActive(true);
        Character.Instance.transform.position = Vector3.zero;
        currentScene = "Shop";
        SceneManager.LoadScene(currentScene);
        hp = maxHp;
        currentGameTime = gameTime;
    }
}
