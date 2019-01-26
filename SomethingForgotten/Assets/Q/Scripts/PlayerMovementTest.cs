using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementTest : MonoBehaviour
{
    public float MoveSpeed = 5f;
    int GetObjectNum;
    Vector3 Movement;
    Rigidbody PlayerRigidbody;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        GetObjectNum = 0;
    }

    void Update()
    {
        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");

        Move(HorizontalInput,VerticalInput);
    }

    void Move(float HorizontalInput, float VerticalInput)
    {
        Movement.Set(HorizontalInput, 0, VerticalInput);
        Movement = Movement.normalized * MoveSpeed * Time.deltaTime;
        PlayerRigidbody.MovePosition(transform.position + Movement);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ForgottenObject")
        {
            GetObjectNum++;
            if (GetObjectNum >= GameController.Instance.MaxForGottenObject)
            {
                GameController.Instance.GameClear();
            }
            Destroy(other.gameObject);
        }
    }
}
