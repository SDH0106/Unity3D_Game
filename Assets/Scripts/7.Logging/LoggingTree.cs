using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoggingTree : MonoBehaviour
{
    [SerializeField] GameObject buttonUI;
    [SerializeField] TextMeshPro keyText;
    [SerializeField] int treeNum;

    Logging logging;
    bool isContact;
    bool isKeyPush;


    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
        logging = Logging.Instance;

        buttonUI.SetActive(false);

        isContact = false;
        isKeyPush = false;

        keyText.text = ((KeyCode)PlayerPrefs.GetInt("Key_Dash")).ToString();
    }

    private void Update()
    {
        if (isContact)
        {
            if (!isKeyPush)
            {
                if (Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt("Key_Dash")))
                {
                    buttonUI.SetActive(false);
                    logging.isLogging = true;
                    if (treeNum == 0)
                        logging.loggingTime = 5f;

                    else
                        logging.loggingTime = 3f;

                    logging.currentTime = logging.loggingTime;
                    isKeyPush = true;

                    logging.KeyHit();
                }
            }

            else
            {
                if (!logging.isLogging)
                {
                    isContact = false;

                    if(!logging.isFail)
                    {
                        if (treeNum == 0)
                            gameManager.woodCount += 5;

                        else
                            gameManager.money += 200;
                    }
                    
                    logging.isLogging = false;
                    logging.isFail = false;

                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            buttonUI.SetActive(true);
            isContact = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            buttonUI.SetActive(false);
            isContact = false;
        }
    }
}
