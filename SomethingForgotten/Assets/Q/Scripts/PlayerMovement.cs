using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    Vector3 Movement;
    Rigidbody PlayerRigidbody;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");

        Move(HorizontalInput,VerticalInput);
    }

    void Move(float HorizontalInput,float VerticalInput)
    {
        Movement.Set(HorizontalInput,0,VerticalInput);
        Movement = Movement.normalized * MoveSpeed * Time.deltaTime;
        PlayerRigidbody.MovePosition(transform.position + Movement);
    }

    private void OnTriggerEnter(Collider other){
        if(other.tag == "ForgottenObject")
        {
			GameController.Instance.GameClear();
            // Destroy(other.gameObject);
        }
    }
}
