using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>       
{
    [SerializeField] public float gameTime;
    [SerializeField] public int money;
    [SerializeField] public int round;

    [HideInInspector] public float initgameTime;

    public string currentScene;

    UnityEngine.SceneManagement.Scene scene;

    private void Start()
    {
        initgameTime = gameTime;
    }

    private void Update()
    {
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;

        if (currentScene == "Shop")
        {
            Character.Instance.gameObject.SetActive(false);
        }

        else if (currentScene == "Game")
        {
            Character.Instance.gameObject.SetActive(true);
        }

        if (currentScene == "Game")
        {
            gameTime -= Time.deltaTime;

            if (gameTime <= 0)
                gameTime = 0;

            if (money <= 0)
                money = 0;

            ToShopScene();
        }

    }

    void ToShopScene()
    {
        if (gameTime == 0)
        {
            currentScene = "Shop";
            SceneManager.LoadScene(currentScene);
        }
    }
}
