using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool CanGoal;
    public float MoveSpeed = 5f;
    Vector3 Movement;
    Rigidbody PlayerRigidbody;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        Init();
    }

    public void Init()
    {
        CanGoal = false;

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
            CanGoal = true;
            Destroy(other.gameObject);
        }

        if(other.tag == "Goal" && CanGoal)
        {
			Debug.Log("GameClear");
        }
    }
}
