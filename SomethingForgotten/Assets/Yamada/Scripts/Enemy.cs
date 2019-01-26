using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{

	[SerializeField] private Text _text;

	private float _direction = 1f;

	void Start()
	{
		Observable.Interval(TimeSpan.FromSeconds(1)).TakeUntilDestroy(this).Subscribe(_ =>
		{
			var value = Random.Range(0, 2);

			Debug.Log(value.ToString());

			if (value == 1)
			{
				_direction = 1;
			}
			else
			{
				_direction = -1;
			}
		});
	}

	void Update()
	{
		TakeRayCast();

		Rotate();
	}

	private void TakeRayCast()
	{
//Rayの作成　　　　　　　↓Rayを飛ばす原点　　　↓Rayを飛ばす方向
		var ray = new Ray(transform.position,  transform.forward);

		//Rayが当たったオブジェクトの情報を入れる箱
		RaycastHit hit;

		//Rayの飛ばせる距離
		var distance = 10;

		//Rayの可視化    ↓Rayの原点　　　　↓Rayの方向　　　　　　　　　↓Rayの色
		Debug.DrawLine(ray.origin, ray.direction * distance, Color.red);

		//もしRayにオブジェクトが衝突したら
		//                  ↓Ray  ↓Rayが当たったオブジェクト ↓距離
		if (Physics.Raycast(ray, out hit, distance))
		{
			//Rayが当たったオブジェクトのtagがPlayerだったら
			if (hit.collider.tag == "Player")
			{
				Debug.Log("RayがPlayerに当たった");
				_text.gameObject.SetActive(true);
			}
		}
		//else
		//{
		//	_text.gameObject.SetActive(false);
		//}
	}

	private void Rotate()
	{
		transform.Rotate(new Vector3(0, 1 * _direction, 0));
	}
}
