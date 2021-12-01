using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public CharacterController PlayerCharController;
    public float MovementSpeed = 12.0f;
    public float Gravity = -10.0f;
    public float JumpingHeight = 2.0f;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    Vector3 Velocity;
    bool bIsGrounded;

    #endregion

    #region Monobehaviour Methods

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bIsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        if(bIsGrounded && Velocity.y < 0.0f)
        {
            //to force the player down on the ground to avoid this activating before player is totally on ground
            Velocity.y = -2f; 
        }

        float XInput = Input.GetAxisRaw("Horizontal");
        float ZInput = Input.GetAxisRaw("Vertical");

        Vector3 Movement = Vector3.Normalize( (transform.right * XInput) + (transform.forward * ZInput) );
        PlayerCharController.Move(Movement * MovementSpeed * Time.deltaTime);

        //velocity to reach specific height jumping equation ( v = sqrt( h * -2 * g ) )
        if(Input.GetButtonDown("Jump") && bIsGrounded)
        {
            Velocity.y = Mathf.Sqrt(JumpingHeight * -2.0f * Gravity);
        }

        // Free fall - Vy = g*t ; Y = 1/2*g*(t^2) = 1/2 * (g * t) * t 
        //---> Y = 1/2 * Vy * t
        Velocity.y += (Gravity * Time.deltaTime); 
        PlayerCharController.Move(0.5f * Velocity * Time.deltaTime);
    }

    #endregion

    #region Helper Methods

    #endregion
}
