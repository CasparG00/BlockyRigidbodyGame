using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        Dash();
    }

    void FixedUpdate()
    {
        Move();
    }

    void LateUpdate()
    {
        if (IsGrounded())
        {
            Rotate();   
        }
    }
    
    
    Vector3 movement;
    Vector3 controllerDirection;

    void GetInput()
    {
        var h = (Vector3.right + Vector3.back) * Input.GetAxisRaw("Horizontal");
        var v = (Vector3.right + Vector3.forward) * Input.GetAxisRaw("Vertical");

        wishDash = Input.GetButtonDown("Dash");

        movement = Vector3.ClampMagnitude(h + v, 1);
    }

    
    public float acceleration = 100;
    public float counterAcceleration = 10;
    
    void Move()
    {
        if (IsGrounded())
        {
            rb.AddForce(movement * acceleration);
            
            var velocity = rb.velocity;
            velocity.y = 0;
            rb.AddForce(-velocity * counterAcceleration);
        }
    }

    
    public float groundCheckDistance = 0.5f;
    
    bool IsGrounded()
    {
        var up = transform.up;
        return Physics.Raycast(rb.position + up * 0.5f, -up, groundCheckDistance);
    }
    
    
    public float dashForce = 3000;
    public float dashRecoverSpeed = 1f;

    bool wishDash;
    bool canDash = true;
    float currentRecoverTime;
    
    Vector3 dashDirection;
    
    void Dash()
    {
        if (movement != Vector3.zero) dashDirection = movement.normalized;
        
        if (wishDash && canDash)
        {
            currentRecoverTime = 0;
            
            rb.AddForce(dashDirection * dashForce);
            
            canDash = false;
        }

        if (!canDash)
        {
            currentRecoverTime += Time.deltaTime;

            if (currentRecoverTime >= dashRecoverSpeed)
            {
                canDash = true;
            }
        }

        UpdateEnergyValue();
    }
    
    
    public MeshRenderer compass;

    void UpdateEnergyValue()
    {
        compass.material.SetFloat("Energy", Mathf.Lerp(0, dashRecoverSpeed, currentRecoverTime));
    }

    void Rotate()
    {
        if (Cursor.visible)
        {
            var dir = Input.mousePosition - playerCamera.WorldToScreenPoint(transform.position);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.localRotation = Quaternion.AngleAxis(angle - 135, Vector3.down);
        }
        else
        {
            var controllerInput = new Vector3(Input.GetAxis("Controller X"), 0, Input.GetAxis("Controller Y"));
            
            if (controllerInput.magnitude != 0) controllerDirection = controllerInput;
            var euler = Quaternion.LookRotation(controllerDirection).eulerAngles;
            euler.y += 45;
            transform.localRotation = Quaternion.Euler(euler);
        }
    }
}
