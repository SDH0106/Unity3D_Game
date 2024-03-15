using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoggingTree : MonoBehaviour
{
    [SerializeField] GameObject buttonUI;
    [SerializeField] TextMeshPro keyText;

    Logging logging;
    bool isContact;

    private void Start()
    {
        logging = Logging.Instance;

        buttonUI.SetActive(false);

        isContact = false;

        keyText.text = ((KeyCode)PlayerPrefs.GetInt("Key_Dash")).ToString();
    }

    private void Update()
    {
        if (isContact)
        {
            if (Input.GetKeyDown((KeyCode)PlayerPrefs.GetInt("Key_Dash")))
            {
                buttonUI.SetActive(false);
                logging.isLogging = true;
                isContact = false;

                logging.KeyHit();
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
