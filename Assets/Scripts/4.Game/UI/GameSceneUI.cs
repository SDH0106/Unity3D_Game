using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSceneUI : Singleton<GameSceneUI>
{
    [SerializeField] public Collider ground;
    [SerializeField] GameObject treePrefab;
    [SerializeField] Camera subCam;

    [Header("HP")]
    [SerializeField] Image chararcterImage;
    [SerializeField] Text hpText;
    [SerializeField] Text maxHpText;
    [SerializeField] Slider hpBar;

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

    [HideInInspector] public int chestCount;

    bool bgmChange;

    protected override void Awake()
    {
        base.Awake();
        roundClearText.SetActive(false);
        gameOverUI.SetActive(false);
        gameOverWoodUI.SetActive(false);
        gameOverIsedolUI.SetActive(false);
        gameClearUI.SetActive(false);
        statCardParent.SetActive(false);
        dash.SetActive(false);
        statWindow.SetActive(false);
        chestPassive.SetActive(false);
        subCam.gameObject.SetActive(false);
        clickText.gameObject.SetActive(false);
        clearImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        character = Character.Instance;
        soundManager = SoundManager.Instance;

        bgmChange = false;

        if (gameManager.round == 10 || gameManager.round == 20 || gameManager.round == 30)
            soundManager.PlayBGM(2, true);

        else
            soundManager.PlayBGM(1, true);

        chararcterImage.sprite = character.rend.sprite;

        if (gameManager.round == 1)
            gameManager.gameStartTime = Time.realtimeSinceStartup;

        SpawnTree();
        if (gameManager.spawnTree)
            Invoke("SpawnOneTree", 10f);
    }

    void SpawnTree()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject tree = Instantiate(treePrefab);
            float groundX = ground.bounds.size.x;
            float groundZ = ground.bounds.size.z;
            groundX = UnityEngine.Random.Range((groundX / 2) * -1 + ground.bounds.center.x, groundX / 2 + ground.bounds.center.x);
            groundZ = UnityEngine.Random.Range((groundZ / 2) * -1 + ground.bounds.center.z, groundZ / 2 + ground.bounds.center.z);

            tree.transform.position = new Vector3(groundX, 0, groundZ);
        }
    }

    void SpawnOneTree()
    {
        GameObject tree = Instantiate(treePrefab);
        float groundX = ground.bounds.size.x;
        float groundZ = ground.bounds.size.z;
        groundX = UnityEngine.Random.Range((groundX / 2) * -1 + ground.bounds.center.x, groundX / 2 + ground.bounds.center.x);
        groundZ = UnityEngine.Random.Range((groundZ / 2) * -1 + ground.bounds.center.z, groundZ / 2 + ground.bounds.center.z);

        tree.transform.position = new Vector3(groundX, 0, groundZ);
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
                        soundManager.PlayBGM(5, false);
                        bgmChange = true;
                    }
                    roundClearText.SetActive(true);
                }

                if (roundClearText.GetComponent<TypingText>().isOver == true)
                {
                    roundClearText.SetActive(false);

                    if (character.levelUpCount <= 0 && chestCount <= 0)
                    {
                        gameManager.ToShopScene();
                    }

                    if (character.levelUpCount > 0)
                    {
                        statCardParent.gameObject.SetActive(true);
                        statWindow.SetActive(true);
                    }

                    if (character.levelUpCount <= 0 && chestCount > 0)
                    {
                        statCardParent.gameObject.SetActive(false);
                        chestPassive.gameObject.SetActive(true);
                        statWindow.SetActive(true);
                    }
                }
            }

            else if (gameManager.round == 30)
            {
                if (gameManager.isClear && gameManager.isBossDead)
                {
                    if (!gameClearUI.activeSelf)
                        gameManager.gameEndTime = Time.realtimeSinceStartup;

                    if (gameManager.woodCount >= gameManager.woodMaxCount)
                    {
                        if (!bgmChange)
                        {
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
                    }
                }

                if (clickText.gameObject.activeSelf)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        subCam.gameObject.SetActive(true);
                        clearImage.gameObject.SetActive(false);
                        SceneManager.LoadScene("End");
                    }
                }
            }
        }

        else if (character.isDead)
        {
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
                    subCam.gameObject.SetActive(true);

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
            {
                SceneManager.LoadScene("End");
            }
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

        yield return new WaitForSeconds(5f);

        clickText.SetActive(true);
    }

    void SettingStatText()
    {
        maxHp.text = character.maxHp.ToString();
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
        spd.text = character.speed.ToString("0.#");
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
            hpBar.value = 1 - (character.currentHp / character.maxHp);
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
        if (gameManager.currentGameTime >= 1)
            timeText.text = ((int)gameManager.currentGameTime).ToString();

        else
        {
            timeText.color = Color.red;
            timeText.text = (gameManager.currentGameTime).ToString("F2");
        }
    }

    public void TitleScene()
    {
        subCam.transform.position = Camera.main.transform.position;
        subCam.transform.rotation = Camera.main.transform.rotation;
        subCam.gameObject.SetActive(true);
    }
}
