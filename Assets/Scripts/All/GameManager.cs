using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>       
{
    [Header("GameData")]
    [SerializeField] float gameTime;
    [SerializeField] public int money;
    [SerializeField] public int round;

    [HideInInspector] public int hp;
    [HideInInspector] public int exp;
    [HideInInspector] public int damage;
    [HideInInspector] public int characterDamage;
    [HideInInspector] public int weaponDamage;
    [HideInInspector] public int elementDamage;
    [HideInInspector] public float defence;
    [HideInInspector] public float absorbHp;
    [HideInInspector] public float recoverHp;
    [HideInInspector] public float attackSpeed;
    [HideInInspector] public float speed;
    [HideInInspector] public float range;
    [HideInInspector] public float luck;

    [Header("CharacterData")]
    [SerializeField] public int maxHp;
    [SerializeField] public int maxExp;

    [HideInInspector] public float currentGameTime;

    [HideInInspector] public string currentScene;

    UnityEngine.SceneManagement.Scene scene;

    private void Start()
    {
        hp = maxHp;
        DontDestroyOnLoad(gameObject);
        currentGameTime = gameTime;
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        if (currentScene == "Game")
        {
            //Character.Instance.gameObject.SetActive(true);
            currentGameTime -= Time.deltaTime;

            if (currentGameTime <= 0)
            {
                currentGameTime = 0;
                ToShopScene();
            }

            if (money <= 0)
                money = 0;
        }

        else if(currentScene != "StartTitle")
        {
            //Character.Instance.gameObject.SetActive(false);
        }
    }

    void ToShopScene()
    {
        Character.Instance.transform.position = Vector3.zero;
        currentScene = "Shop";
        SceneManager.LoadScene(currentScene);
        hp = maxHp;
        currentGameTime = gameTime;
    }
}
