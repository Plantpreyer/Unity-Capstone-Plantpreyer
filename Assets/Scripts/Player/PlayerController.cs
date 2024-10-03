using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
{

    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private bool busy;
    private static Collider2D playerCollider;

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        busy = false;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        if (busy || PlayerManager.Instance.playerBusy)
        {
            myAnimator.SetBool("Idle", true);
        }
        if (!busy && !PlayerManager.Instance.playerBusy) {
            myAnimator.SetBool("Idle", false);
        }
        PlayerInput();
    }

    private void FixedUpdate()
    {
        if (!busy && !PlayerManager.Instance.playerBusy)
        {
            AdjustPlayerFacingDirection();
            Move();
        }
    }

    private void PlayerInput()
    {
        if (!busy && !PlayerManager.Instance.playerBusy)
        {
            movement = playerControls.Movement.Move.ReadValue<Vector2>();

            myAnimator.SetFloat("moveX", movement.x);
            myAnimator.SetFloat("moveY", movement.y);
        } else {
            myAnimator.SetFloat("moveX", 0);
            myAnimator.SetFloat("moveY", 0);
        }
    }

    private void Move()
    {
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection()
    {
        // Vector3 mousePos = Input.mousePosition;
        // Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        // if(mousePos.x < playerScreenPoint.x){
        //     mySpriteRender.flipX = true;
        // } else {
        //     mySpriteRender.flipX = false;
        // }
        if (movement.x > 0.1)
        {
            mySpriteRender.flipX = false;
        }
        else if (movement.x < -0.1)
        {
            mySpriteRender.flipX = true;
        }
    }

    public void ToggleBusy(bool isBusy)
    {
        busy = isBusy;
    }

    public static Collider2D getPlayerCollider(){
        return playerCollider;
    }
}
