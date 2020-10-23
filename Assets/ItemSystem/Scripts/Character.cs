using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 1f;

    private CharacterController characterController;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (!characterController)
        {
            Debug.LogWarning("Character script cannot find attached CharacterController");
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 totalMovement = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            totalMovement += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            totalMovement -= Vector3.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            totalMovement -= Vector3.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            totalMovement += Vector3.right;
        }

        totalMovement *= speed;

        characterController.Move(totalMovement);
    }
}
