using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Logging : Singleton<Logging>
{
    [SerializeField] RectTransform arrowParent;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI failText;
    
    [HideInInspector] public bool isLogging;

    int count;
    float loggingTime;
    float currentTime;

    bool isFail;
    bool isStart;

    string arrStr;
    KeyCode arrKey;
    KeyCode checkKey;

    void Start()
    {
        isLogging = false;
        isFail = false;
        count = 0;

        loggingTime = 3f;
        currentTime = loggingTime;

        timeText.gameObject.SetActive(false);
        failText.gameObject.SetActive(false);

        arrowParent.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isLogging)
        {
            if (!isFail)
            {
                timeText.text = currentTime.ToString("F2");
                currentTime -= Time.deltaTime;

                if (currentTime <= 0)
                    isFail = true;

                Vector3 arrRot = arrowParent.GetChild(count).rotation.eulerAngles;

                switch (arrRot.z)
                {
                    // up
                    case 0:
                        arrStr = "Key_Up";
                        arrKey = KeyCode.UpArrow;
                        break;

                    // down
                    case 180:
                        arrStr = "Key_Down";
                        arrKey = KeyCode.DownArrow;
                        break;

                    // right
                    case 270:
                        arrStr = "Key_Right";
                        arrKey = KeyCode.RightArrow;
                        break;

                    // left
                    case 90:
                        arrStr = "Key_Left";
                        arrKey = KeyCode.LeftArrow;
                        break;
                }

                isStart = true;

                //if (Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt(arrStr)) || Input.GetKeyDown(arrKey))
                if (checkKey == arrKey || checkKey == (KeyCode)PlayerPrefs.GetInt(arrStr))
                {
                    arrowParent.GetChild(count).gameObject.SetActive(false);
                    count++;
                    isStart = false;
                    checkKey = KeyCode.None;
                }

                else
                {
                    if (checkKey != KeyCode.None && checkKey != (KeyCode)PlayerPrefs.GetInt("Key_Dash"))
                    {
                        isStart = false;
                        checkKey = KeyCode.None;
                        arrowParent.GetChild(count).GetComponent<Image>().color = Color.red;
                        StartCoroutine(ActiveFailText("실패..."));
                    }
                }

                if (count >= arrowParent.childCount)
                {
                    Initialize();
                }
            }

            else
            {
                if (currentTime <= 0)
                {
                    isStart = false;
                    checkKey = KeyCode.None;
                    StartCoroutine(ActiveFailText("시간 초과..."));
                }
            }
        }
    }

    private void OnGUI()
    {
        if (isStart)
        {
            Event keyEvent = Event.current;

            if (keyEvent.isKey)
                checkKey = keyEvent.keyCode;
        }
    }

    public void KeyHit()
    {
        for (int i = 0; i < arrowParent.transform.childCount; i++)
        {
            int rand = Random.Range(0, 3);
            arrowParent.GetChild(i).rotation = Quaternion.Euler(0, 0, rand * 90);
            arrowParent.GetChild(i).GetComponent<Image>().color = Color.white;

            if (!arrowParent.GetChild(i).gameObject.activeSelf)
                arrowParent.GetChild(i).gameObject.SetActive(true);
        }

        arrowParent.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
    }

    void Initialize()
    {
        isLogging = false;
        count = 0;
        currentTime = loggingTime;
        arrowParent.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
    }

    IEnumerator ActiveFailText(string str)
    {
        isFail = true;
        timeText.gameObject.SetActive(false);
        failText.text = str;
        failText.gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        isFail = false;
        failText.gameObject.SetActive(false);
        Initialize();
    }
}
