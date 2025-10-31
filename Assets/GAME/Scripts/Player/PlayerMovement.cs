using System;
using System.Collections;
using System.Collections.Generic;
using sb.eventbus;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private FloatingJoystick floatingJoystick;
    [SerializeField] private PlayerAnimationsController playerAnimationsController;

    private EventListener<NewAreaOpenedEvent> newAreaOpened;
    private Vector3 input;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        newAreaOpened = new EventListener<NewAreaOpenedEvent>(CloseJoystick);
        EventBus<NewAreaOpenedEvent>.AddListener(newAreaOpened);
    }

    private void OnDisable()
    {
        EventBus<NewAreaOpenedEvent>.RemoveListener(newAreaOpened);
    }

    void Update()
    {
        input.z = -floatingJoystick.Horizontal;
        input.x = floatingJoystick.Vertical;
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.z);

        if (moveDirection.magnitude > 0.1f)
        {
            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
            
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 0.2f);
        }
        playerAnimationsController.ChangeAnimation("Walk", moveDirection.magnitude);
    }

    private void CloseJoystick(NewAreaOpenedEvent e)
    {
        if(e.areaType != FillableAreaTypes.Board) return;
        
        floatingJoystick.gameObject.SetActive(false);
    }
}
