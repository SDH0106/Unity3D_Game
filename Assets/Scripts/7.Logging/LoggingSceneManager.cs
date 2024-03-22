using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoggingSceneManager : Singleton<LoggingSceneManager>
{
    [SerializeField] Text timeText;
    [SerializeField] Text roundText;
    [SerializeField] Text woodCountText;
    [SerializeField] Text coinText;
    [SerializeField] GameObject nextRoundPanel;
    [SerializeField] GameObject treeParent;
    [SerializeField] GameObject[] treePrefabs;
    [SerializeField] Collider ground;

    GameManager gameManager;
    Logging logging;

    float time;
    float currentTime;
    [HideInInspector] public bool isEnd;

    private void Start()
    {
        gameManager = GameManager.Instance;
        logging = Logging.Instance;

        time = 60f;
        currentTime = time;
        isEnd = false;;
        timeText.text = time.ToString("F2");
        woodCountText.text = gameManager.woodCount.ToString();
        roundText.text = gameManager.round.ToString();
        coinText.text = gameManager.money.ToString();
        nextRoundPanel.SetActive(isEnd);

        TreeInstance();
    }

    private void Update()
    {
        if (treeParent.transform.childCount > 0)
        {
            if (currentTime > 0)
            {
                currentTime = Mathf.Clamp(currentTime - Time.deltaTime, 0f, time);
                timeText.text = currentTime.ToString("F2");
            }

            else if (currentTime == 0f && !isEnd)
            {
                isEnd = true;
                treeParent.SetActive(false);
                nextRoundPanel.SetActive(isEnd);
                currentTime = -100f;
            }
        }

        else
        {
            isEnd = true;
            treeParent.SetActive(false);
            nextRoundPanel.SetActive(isEnd);
            currentTime = -100f;
        }


        woodCountText.text = gameManager.woodCount.ToString();
        coinText.text = gameManager.money.ToString();
    }

    List<int> posX, posZ;

    void TreeInstance()
    {
        posX = new List<int>();
        posZ = new List<int>();

        for (int i = -19; i <= 19; i += 2)
            posX.Add(i);

        for (int i = -12; i <= 12; i += 2)
            posZ.Add(i);

        for (int i = 0; i < 30; i++)
        {
            int rand = Random.Range(0, 100);
            GameObject tree;

            if (rand < 95)
                tree = Instantiate(treePrefabs[0]);

            else
                tree = Instantiate(treePrefabs[1]);

            tree.transform.SetParent(treeParent.transform);

            int randX = Random.Range(0, posX.Count - 1);
            int randZ = Random.Range(0, posZ.Count - 1);

            tree.transform.position = ground.bounds.ClosestPoint(new Vector3(posX[randX], 0, posZ[randZ]));
        }
    }

    public void ToGameScene()
    {
        gameManager.ToNextScene("Shop");
    }
}
