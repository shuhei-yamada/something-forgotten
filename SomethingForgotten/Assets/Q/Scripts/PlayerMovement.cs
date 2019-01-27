using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
	int GetObjectNum;
    Vector3 Movement;
    Rigidbody PlayerRigidbody;

    [SerializeField] private GameObject _bone;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
		GetObjectNum = 0;
    }

    void Update()
    {
		/* 
        float HorizontalInput = Input.GetAxisRaw("Horizontal");
        float VerticalInput = Input.GetAxisRaw("Vertical");

        Move(HorizontalInput,VerticalInput);*/
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
			GameController.Instance.PlaySe(SoundManager.SeType.ForgottenObjectGet);
			GetObjectNum++;
			if(GetObjectNum >= GameController.Instance.MaxForGottenObject)
			{
				GameController.Instance.GameClear();
			}
            Destroy(other.gameObject);
        }

        if (other.tag == "Bone2")
        {
			GameController.Instance.PlaySe(SoundManager.SeType.ItemGet);
			_bone.SetActive(true);
			Destroy(other.gameObject);
        }
    }
}
