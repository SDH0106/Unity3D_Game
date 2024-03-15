using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoggingLilpa : MonoBehaviour
{
    [SerializeField] float speed;
    bool isRun;

    float x;
    float z;

    bool isDownUp = false;
    bool isLeftRight = false;

    Vector3 dir;

    Animator anim;
    SpriteRenderer rend;
    GameManager gameManager;
    Logging logging;

    void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        gameManager = GameManager.Instance;
        logging = Logging.Instance;
    }

    void Update()
    {
        //if (!gameManager.isPause)
        if(!logging.isLogging)
        {
            Move();

            anim.SetFloat("moveSpeed", 1 + (speed * 0.1f));
            anim.SetBool("isRun", isRun);
        }
    }

    void Move()
    {
        bool xInput = (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left"))) || (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")));
        bool zInput = (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Up"))) || (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")));

        if (!xInput)
            x = 0;

        if (!zInput)
            z = 0;

        if (zInput)
        {
            if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Up")))
            {
                z = 1;

                if (!isDownUp && Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")))
                    z = -1;
            }

            else if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Down")))
            {
                z = -1;
                isDownUp = true;
            }

            if (Input.GetKeyUp((KeyCode)PlayerPrefs.GetInt("Key_Down")))
                isDownUp = false;
        }

        if (xInput)
        {
            if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Left")))
            {
                x = -1;
                rend.flipX = true;

                if (!isLeftRight && Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")))
                {
                    x = 1;
                    rend.flipX = false;
                }
            }

            else if (Input.GetKey((KeyCode)PlayerPrefs.GetInt("Key_Right")))
            {
                x = 1;
                rend.flipX = false;
                isLeftRight = true;
            }

            if (Input.GetKeyUp((KeyCode)PlayerPrefs.GetInt("Key_Right")))
                isLeftRight = false;
        }


        /*if (ground == null)
            ground = GameSceneUI.Instance.ground;*/

        dir = (Vector3.right * x + Vector3.forward * z).normalized;

        transform.position += dir * speed * Time.deltaTime;

        //transform.position = ground.bounds.ClosestPoint(transform.position);

        if (dir != Vector3.zero)
            isRun = true;

        else if (dir == Vector3.zero)
            isRun = false;
    }
}
