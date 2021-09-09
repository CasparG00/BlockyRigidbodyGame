using UnityEngine;

public class MoveMouse : MonoBehaviour
{
    bool isUsingMouse;

    void Update()
    {
        if (!isUsingMouse)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                isUsingMouse = true;
            }
        }
        else
        {
            if (Input.GetAxis("Controller X") != 0 || Input.GetAxis("Controller Y") != 0)
            {
                isUsingMouse = false;
            }
        }

        Cursor.visible = isUsingMouse;
    }
}
