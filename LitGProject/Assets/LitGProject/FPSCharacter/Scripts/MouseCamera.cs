using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCamera : MonoBehaviour
{
    #region Variables

    public float MouseSpeed = 100.0f;
    public Transform PlayerBody;

    float XRotation= 0.0f;

    #endregion

    #region MonoBehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * MouseSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * MouseSpeed;
        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90.0f, 90.0f);
        this.transform.localRotation = Quaternion.Euler(XRotation, 0.0f, 0.0f);
        PlayerBody.Rotate(Vector3.up * mouseX);
    }

    #endregion

    #region Helper Methods

    #endregion
}
