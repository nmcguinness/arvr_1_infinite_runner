using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    [SerializeField] 
    private LayerMask groundLayer;
    [SerializeField] 
    private float runSpeed = 8f;
    [SerializeField] 
    private float jumpHeight = 2f;
    [SerializeField] 
    private Transform[] groundChecks;
    [SerializeField] 
    private Transform[] wallChecks;
    [SerializeField]
    private AudioClip jumpSoundEffect;

    private float gravity = -50f;
    private CharacterController characterController;
    private Animator animator;
    private Vector3 velocity;
    private float horizontalInput = 1;
    private bool isGrounded;
    private bool jumpPressed;
    private float jumpTimer;
    private float jumpGracePeriod = 0.2f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = new Vector3(horizontalInput, 0, Mathf.Abs(horizontalInput) - 1);

        isGrounded = false;
        foreach (var groundCheck in groundChecks)
        {
            if (Physics.CheckSphere(groundCheck.position, 0.1f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                isGrounded = true;
                break;
            }
        }

        if (isGrounded && velocity.y < 0)
            velocity.y = 0;
        else
            velocity.y += gravity * Time.deltaTime;

        var isBlocked = false;
        foreach (var wallCheck in wallChecks)
        {
            if (Physics.CheckSphere(wallCheck.position, 0.1f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                isBlocked = true;
                break;
            }
        }

        if(!isBlocked)
            characterController.Move(new Vector3(horizontalInput * runSpeed, 0, 0) * Time.deltaTime);

        jumpPressed = Input.GetButtonDown("Jump");
        if (jumpPressed)
            jumpTimer = Time.time;

        if (isGrounded && (jumpPressed || jumpTimer > 0 && Time.time < jumpTimer + jumpGracePeriod))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2 * gravity);
            if(jumpSoundEffect != null)
            {
                AudioSource.PlayClipAtPoint(jumpSoundEffect, transform.position, 0.5f);
            }
            jumpTimer = -1;
        }

        characterController.Move(velocity * Time.deltaTime);
        transform.Rotate(new Vector3(0, 180, 0), Space.Self); 
        animator.SetFloat("Speed", horizontalInput);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("VerticalSpeed", velocity.y);
    }
}
