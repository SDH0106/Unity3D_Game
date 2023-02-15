using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPanelClose : MonoBehaviour
{
    [SerializeField] GameObject optionPanel;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionPanel != null)
                optionPanel.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}
