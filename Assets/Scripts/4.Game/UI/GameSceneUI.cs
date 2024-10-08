using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : Singleton<GameSceneUI>
{
    Texture2D cursorAttack;
    Texture2D cursorNormal;
    [SerializeField] public Collider ground;
    [SerializeField] GameObject treePrefab;
    [SerializeField] public GameObject monsterSpawn;
    [SerializeField] GameObject tutoPanel;
    [SerializeField] GameObject selectPanel;
    [SerializeField] GameObject treeShopPanel;

    [Header("HP")]
    [SerializeField] Image chararcterImage;
    [SerializeField] Text hpText;
    [SerializeField] Text maxHpText;
    [SerializeField] Text SheildText;
    [SerializeField] Slider hpBar;
    [SerializeField] Slider sheildBar;

    [Header("EXP")]
    [SerializeField] Text lvText;
    [SerializeField] Slider expBar;

    [Header("COIN")]
    [SerializeField] Text coinText;

    [Header("Wood")]
    [SerializeField] Text woodCount;

    [Header("Time")]
    [SerializeField] Text timeText;

    [Header("Round")]
    [SerializeField] Text roundText;

    [Header("Dash")]
    [SerializeField] GameObject dash;
    [SerializeField] Text dashKey;
    [SerializeField] Image dashImage;
    [SerializeField] Text dashCoolTime;
    [SerializeField] GameObject dashCountParent;
    [SerializeField] Text dashCount;
    [SerializeField] Text maxDashCount;

    [Header("Stat")]
    [SerializeField] GameObject statWindow;
    [SerializeField] Text maxHp;
    [SerializeField] Text reHp;
    [SerializeField] Text apHp;
    [SerializeField] Text def;
    [SerializeField] Text avoid;
    [SerializeField] Text percentDamage;
    [SerializeField] Text wAtk;
    [SerializeField] Text eAtk;
    [SerializeField] Text sAtk;
    [SerializeField] Text lAtk;
    [SerializeField] Text aSpd;
    [SerializeField] Text spd;
    [SerializeField] Text ran;
    [SerializeField] Text luk;
    [SerializeField] Text cri;

    [Header("Text")]
    [SerializeField] GameObject roundClearText;
    [SerializeField] Text bossSceneText;
    [SerializeField] GameObject gameOverUI;
    [SerializeField] TypingText gameOverText;
    [SerializeField] GameObject gameOverWoodUI;
    [SerializeField] TypingText gameOverWoodText;
    [SerializeField] GameObject gameOverIsedolUI;
    [SerializeField] TypingText gameOverIsedolText;
    [SerializeField] Image clearImage;
    [SerializeField] Sprite[] clearIllusts;
    [SerializeField] GameObject clickText;
    [SerializeField] GameObject gameClearUI;
    [SerializeField] TypingText gameClearText;
    [SerializeField] GameObject statCardParent;
    [SerializeField] GameObject chestPassive;

    GameManager gameManager;
    Character character;
    SoundManager soundManager;
    TreeShop treeShop;

    [HideInInspector] public int chestCount;
    [HideInInspector] public int treeShopCount;

    bool bgmChange;

    protected override void Awake()
    {
        base.Awake();
        roundClearText.SetActive(false);
        bossSceneText.gameObject.SetActive(false);
        gameOverUI.SetActive(false);
        gameOverWoodUI.SetActive(false);
        gameOverIsedolUI.SetActive(false);
        gameClearUI.SetActive(false);
        statCardParent.SetActive(false);
        dash.SetActive(false);
        statWindow.SetActive(false);
        chestPassive.SetActive(false);
        clickText.gameObject.SetActive(false);
        clearImage.gameObject.SetActive(false);
        tutoPanel.SetActive(false);
        selectPanel.SetActive(false);
        treeShopPanel.SetActive(false);

        gameManager = GameManager.Instance;
        treeShop = TreeShop.Instance;

        //monsterSpawn.SetActive(false);

        if (gameManager.round == 1)
        {
            if (Convert.ToBoolean(PlayerPrefs.GetInt("GameTuto", 1)))
            {
                monsterSpawn.SetActive(!Convert.ToBoolean(PlayerPrefs.GetInt("GameTuto", 1)));
                tutoPanel.SetActive(Convert.ToBoolean(PlayerPrefs.GetInt("GameTuto", 1)));
            }
        }

        else if(gameManager.round == 10)
        {
            if (Convert.ToBoolean(PlayerPrefs.GetInt("BossTuto", 1)))
            {
                monsterSpawn.SetActive(!Convert.ToBoolean(PlayerPrefs.GetInt("BossTuto", 1)));
                tutoPanel.SetActive(Convert.ToBoolean(PlayerPrefs.GetInt("BossTuto", 1)));
            }
        }
    }

    private void Start()
    {
        character = Character.Instance;
        soundManager = SoundManager.Instance;

        cursorNormal = gameManager.useCursorNormal;
        cursorAttack = gameManager.useCursorAttack;
        Vector2 cursorHotSpot = new Vector3(cursorAttack.width * 0.5f, cursorAttack.height * 0.5f);
        Cursor.SetCursor(cursorAttack, cursorHotSpot, CursorMode.ForceSoftware);

        bgmChange = false;

        if (gameManager.round == 10 || gameManager.round == 20 || gameManager.round == 30)
        {
            soundManager.PlayBGM(2, true);
            soundManager.PlayES("Alert");
            bossSceneText.gameObject.SetActive(true);
            StartCoroutine(BlinkBossSceneText());
        }

        else
            soundManager.PlayBGM(1, true);

        chararcterImage.sprite = character.rend.sprite;

        if (gameManager.round == 1)
            gameManager.gameStartTime = Time.realtimeSinceStartup;

        SpawnTree();

        if (gameManager.spawnTree)
            InvokeRepeating("SpawnOneTree", 10f, 10f);

        chestCount = 0;
        treeShopCount = 1;
    }

    IEnumerator BlinkBossSceneText()
    {
        float fadeTime = 0.5f;
        Color color = bossSceneText.color;

        while (fadeTime > 0f)
        {
            fadeTime = Mathf.Clamp(fadeTime - Time.deltaTime, 0f, 0.5f);
            color.a = fadeTime + 0.5f;
            bossSceneText.color = color;
            yield return null;
        }

        while (fadeTime < 0.5f)
        {
            fadeTime = Mathf.Clamp(fadeTime + Time.deltaTime, 0f, 0.5f);
            color.a = fadeTime + 0.5f;
            bossSceneText.color = color;
            yield return null;
        }

        while (fadeTime > 0f)
        {
            fadeTime = Mathf.Clamp(fadeTime - Time.deltaTime, 0f, 0.5f);
            color.a = fadeTime + 0.5f;
            bossSceneText.color = color;
            yield return null;
        }

        bossSceneText.gameObject.SetActive(false);
    }

    void SpawnTree()
    {
        for (int i = 0; i < 5; i++)
            SpawnOneTree();
    }

    void SpawnOneTree()
    {
        GameObject tree = Instantiate(treePrefab);
        tree.transform.position = TreePos();
    }

    Vector3 TreePos()
    {
        float groundX = ground.bounds.size.x;
        float groundZ = ground.bounds.size.z;
        groundX = UnityEngine.Random.Range((groundX / 2f) * -1f + ground.bounds.center.x, (groundX / 2f) + ground.bounds.center.x);
        groundZ = UnityEngine.Random.Range((groundZ / 2f) * -1f + ground.bounds.center.z, (groundZ / 2f) + ground.bounds.center.z);

        if(Mathf.Abs(groundX) < 3f)
        {
            groundX = groundX < 0f ? groundX - 3f : groundX + 3f;
        }

        if (Mathf.Abs(groundZ) < 3f)
        {
            groundZ = groundZ < 0f ? groundZ - 3f : groundZ + 3f;
        }

        return new Vector3(groundX, 0, groundZ);
    }

    private void Update()
    {
        HpUI();
        ExpUI();
        CoinUI();
        WoodUI();
        RoundUI();
        TimeUI();
        DashUI();
        SettingStatText();

        if (!character.isDead)
        {
            if (gameManager.round != 30)
            {
                if (gameManager.isClear && gameManager.isBossDead)
                {
                    if (!bgmChange)
                    {
                        Vector2 cursorHotSpot = new Vector3(cursorNormal.width * 0.5f, cursorNormal.height * 0.5f);
                        Cursor.SetCursor(cursorNormal, cursorHotSpot, CursorMode.ForceSoftware);

                        soundManager.PlayBGM(5, false);
                        bgmChange = true;
                    }

                    roundClearText.SetActive(true);

                    if (gameManager.spawnTree)
                        CancelInvoke("SpawnOneTree");
                }

                if (roundClearText.GetComponent<TypingText>().isOver == true)
                {
                    roundClearText.SetActive(false);

                    if (character.levelUpCount <= 0 && chestCount <= 0)
                    {
                        if (gameManager.round % 5 == 0 && treeShopCount > 0)
                        {
                            statCardParent.gameObject.SetActive(false);
                            chestPassive.gameObject.SetActive(false);
                            treeShopPanel.SetActive(true);
                        }

                        else 
                        {
                            statCardParent.gameObject.SetActive(false);
                            chestPassive.gameObject.SetActive(false);
                            treeShopPanel.SetActive(false);

                            if (gameManager.woodCount < 5 && !selectPanel.activeSelf)
                                gameManager.ToNextScene("Shop");

                            else if (gameManager.woodCount >= 5)
                                selectPanel.SetActive(true);
                        }
                    }

                    else if (character.levelUpCount > 0)
                    {
                        statCardParent.gameObject.SetActive(true);
                        statWindow.SetActive(true);
                    }

                    else if (character.levelUpCount <= 0 && chestCount > 0)
                    {
                        statCardParent.gameObject.SetActive(false);
                        chestPassive.gameObject.SetActive(true);
                        statWindow.SetActive(true);
                    }
                }
            }

            else if (gameManager.round == 30)
            {
                if (gameManager.isBossDead)
                {
                    if (gameManager.spawnTree)
                        CancelInvoke("SpawnOneTree");

                    gameManager.isClear = true;

                    if (!gameClearUI.activeSelf)
                        gameManager.gameEndTime = Time.realtimeSinceStartup;

                    if (gameManager.woodCount >= gameManager.woodMaxCount)
                    {
                        if (!bgmChange)
                        {
                            Vector2 cursorHotSpot = new Vector3(cursorNormal.width * 0.5f, cursorNormal.height * 0.5f);
                            Cursor.SetCursor(cursorNormal, cursorHotSpot, CursorMode.ForceSoftware);

                            soundManager.PlayBGM(6, false);
                            bgmChange = true;
                        }

                        gameClearUI.SetActive(true);

                        clearImage.sprite = clearIllusts[0];

                        if (gameClearText.isOver)
                        {
                            StartCoroutine(FadeIn());
                            gameClearText.isOver = false;
                        }

                        if (character.characterNum == (int)CHARACTER_NUM.Bagic)
                            PlayerPrefs.SetInt("BagicClear", 1);
                    }

                    else if (gameManager.woodCount < gameManager.woodMaxCount)
                    {
                        gameOverWoodUI.SetActive(true);

                        if (gameOverWoodText.isOver)
                            SceneManager.LoadScene("End");

                        /*if (gameManager.isedolCount != 5)
                        {
                            gameOverWoodUI.SetActive(true);

                            if (gameOverWoodText.isOver)
                                SceneManager.LoadScene("End");
                        }

                        else
                        {
                            gameOverWoodUI.SetActive(true);

                            gameManager.isClear = true;

                            if (gameOverWoodText.isOver)
                            {
                                gameOverWoodUI.SetActive(false);
                                gameOverIsedolUI.SetActive(true);
                            }

                            if (gameOverIsedolText.isOver == true)
                            {
                                clearImage.sprite = clearIllusts[1];
                                StartCoroutine(FadeIn());
                                gameOverIsedolText.isOver = false;
                            }

                            if (clickText.gameObject.activeSelf)
                            {
                                if (Input.GetMouseButtonDown(0))
                                {
                                    if (!bgmChange)
                                    {
                                        soundManager.PlayBGM(6, false);
                                        bgmChange = true;
                                    }

                                    clearImage.gameObject.SetActive(false);
                                    gameOverIsedolUI.SetActive(false);
                                    gameClearUI.SetActive(true);
                                }
                            }

                            if (gameClearText.isOver == true && !soundManager.isPlaying)
                                SceneManager.LoadScene("End");
                        }*/
                    }
                }

                if (clickText.gameObject.activeSelf)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        clearImage.gameObject.SetActive(false);
                        SceneManager.LoadScene("End");
                    }
                }
            }
        }

        else if (character.isDead)
        {
            if (gameManager.spawnTree)
                CancelInvoke("SpawnOneTree");

            if (!gameOverUI.activeSelf || !gameOverIsedolText)
                gameManager.gameEndTime = Time.realtimeSinceStartup;

            if (gameManager.isedolCount != 5)
            {
                gameOverUI.SetActive(true);

                if (gameOverText.isOver == true)
                    SceneManager.LoadScene("End");
            }


            else if (gameManager.isedolCount == 5)
            {
                gameManager.isClear = true;

                if (!clickText.gameObject.activeSelf)
                    gameOverIsedolUI.SetActive(true);

                if (gameOverIsedolText.isOver == true)
                {
                    clearImage.sprite = clearIllusts[1];
                    StartCoroutine(FadeIn());
                    gameOverIsedolText.isOver = false;
                }

                if (character.characterNum == (int)CHARACTER_NUM.Bagic)
                    PlayerPrefs.SetInt("BagicClear", 1);
            }

            if (clickText.gameObject.activeSelf)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (!bgmChange)
                    {
                        soundManager.PlayBGM(6, false);
                        bgmChange = true;
                    }

                    clearImage.gameObject.SetActive(false);
                    gameOverIsedolUI.SetActive(false);
                    gameClearUI.SetActive(true);
                }
            }

            if (gameClearText.isOver == true && !soundManager.isPlaying)
                SceneManager.LoadScene("End");
        }
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(1f);

        float fadeTime = 0f;
        Color imageColor = clearImage.color;
        clearImage.gameObject.SetActive(true);

        do
        {
            fadeTime = Mathf.Clamp(fadeTime + Time.deltaTime, 0f, 2f);
            imageColor.a = fadeTime / 2f;
            clearImage.color = imageColor;
            yield return null;
        }

        while (fadeTime < 2f);

        yield return new WaitForSeconds(3.5f);

        clickText.SetActive(true);
    }

    void SettingStatText()
    {
        maxHp.text = gameManager.maxHp.ToString();
        reHp.text = gameManager.recoverHp.ToString("0.#");
        apHp.text = gameManager.absorbHp.ToString("0.#");
        def.text = gameManager.defence.ToString("0.#");
        avoid.text = gameManager.avoid.ToString("0.#");
        percentDamage.text = gameManager.percentDamage.ToString("0.0#");
        wAtk.text = gameManager.physicDamage.ToString("0.#");
        eAtk.text = gameManager.magicDamage.ToString("0.#");
        sAtk.text = gameManager.shortDamage.ToString("0.#");
        lAtk.text = gameManager.longDamage.ToString("0.#");
        aSpd.text = gameManager.attackSpeed.ToString("0.#");
        spd.text = gameManager.speed.ToString("0.##");
        ran.text = gameManager.range.ToString("0.#");
        luk.text = gameManager.luck.ToString("0.#");
        cri.text = gameManager.critical.ToString("0.#");
    }

    void DashUI()
    {
        if (gameManager.dashCount > 0)
        {
            dash.SetActive(true);
            dashKey.text = KeySetting.keys[KeyAction.DASH].ToString();
            Color color = dashImage.color;
            dashImage.fillAmount = 1;

            if (character.dashCount == 0)
            {
                color.a = 0.5f;
                dashImage.color = color;

                dashCountParent.gameObject.SetActive(false);

                dashCoolTime.gameObject.SetActive(true);
                dashImage.fillAmount = 1 - (character.dashCoolTime / character.initDashCoolTime);
                dashCoolTime.text = character.dashCoolTime.ToString("F2");
            }

            else if (character.dashCount > 0)
            {
                color.a = 1f;
                dashImage.color = color;

                dashCountParent.gameObject.SetActive(true);
                dashCount.text = character.dashCount.ToString();
                maxDashCount.text = gameManager.dashCount.ToString();

                dashCoolTime.gameObject.SetActive(false);
            }
        }
    }

    void HpUI()
    {
        if (gameManager.currentScene == "Game" && character.maxExp != 0)
        {
            maxHpText.text = character.maxHp.ToString();
            hpText.text = character.currentHp.ToString("0.#");

            if (character.shield > 0)
            {
                SheildText.gameObject.SetActive(true);
                SheildText.text = $"+ {character.shield.ToString("0.#")}";
            }

            else
                SheildText.gameObject.SetActive(false);

            hpBar.value = 1 - (character.currentHp / character.maxHp);
            sheildBar.value = (character.shield / character.maxHp);
        }
    }

    void ExpUI()
    {
        if (character.maxExp != 0)
        {
            lvText.text = character.level.ToString();
            expBar.value = 1 - (character.exp / character.maxExp);
        }
    }

    void CoinUI()
    {
        coinText.text = gameManager.money.ToString();
    }

    void WoodUI()
    {
        woodCount.text = gameManager.woodCount.ToString();
    }

    void RoundUI()
    {
        roundText.text = gameManager.round.ToString();
    }

    void TimeUI()
    {
        if (gameManager.currentGameTime >= 5)
            timeText.text = ((int)gameManager.currentGameTime).ToString();

        else
        {
            timeText.color = Color.red;
            timeText.text = (gameManager.currentGameTime).ToString("F2");
        }
    }

    public void OnOffStatWindow()
    {
        statWindow.gameObject.SetActive(!statWindow.gameObject.activeSelf);
    }

    public void SelectScene(string sceneName)
    {
        gameManager.ToNextScene(sceneName);
    }

    public void GetRandomSecne()
    {
        gameManager.woodCount -= 5;

        int rand = UnityEngine.Random.Range(0, 10);
        string nextScene;

        if (rand % 2 == 0)
            nextScene = "Fishing";

        else
            nextScene = "Logging";

        SelectScene(nextScene);
    }
}
