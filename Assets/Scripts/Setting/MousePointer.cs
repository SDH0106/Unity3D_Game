using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointer : MonoBehaviour
{
    Vector3 MousePosition;

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosition.y = 0;
        transform.position = MousePosition;
    }
}
