using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody thisRigidBody;
    public float jumpPower = 10;
    public float jumpInterval = 0.5f;
    private float jumpCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update cooldown
        jumpCooldown -= Time.deltaTime;
        bool isGameActive = GameManager.Instance.isGameActive();
        bool canJump = jumpCooldown <= 0 && isGameActive;

        //Jump
        if (canJump)
        {        
            bool jumpInput = Input.GetKey(KeyCode.Space);
            if (jumpInput)
            {
                Jump();
            }
        }

        // Toggle gravity
        thisRigidBody.useGravity = isGameActive;
    }

    void OnCollisionEnter(Collision other)
    {
        OnCustomCollisionEnter(other.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        OnCustomCollisionEnter(other.gameObject);
    }

    private void OnCustomCollisionEnter(GameObject other)
    {
        bool isSensor = other.CompareTag("Sensor");
        if (isSensor)
        {

            GameManager.Instance.score++;
            UnityEngine.Debug.Log("Score: " + GameManager.Instance.score);
        }
        else
        {
            GameManager.Instance.EndGame();
        }
    }

    private void Jump()
    {
        //Reset Cooldown
        jumpCooldown = jumpInterval;

        // Apply force
        thisRigidBody.velocity = Vector3.zero;
        thisRigidBody.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
    }
}
