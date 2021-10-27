﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineBehavior : MonoBehaviour
{
    public float forward = 30;
    public float back;

    public float rotation = 10;
    public float floating = 0.5f;
    public float minVel = 0;
    public float maxVel = 10;
    public float minAngVel = 0;
    public float maxAngVel = 1;
    public float chargeRate = 50f; //rate of increase per second

    float charge = 0f; //percent

    new Rigidbody rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();

        rigidbody.position = new Vector3(rigidbody.position.x, floating, rigidbody.position.z);
        rigidbody.mass = 5;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(charge);
        rigidbody = this.GetComponent<Rigidbody>();
        var position = rigidbody.position;
        var direction = transform.forward * forward;

        rigidbody.AddForce(direction);

        if (Input.GetKey(KeyCode.Space)) //スペースキーが押されたとき
        {
            if (charge <= 100)
            {
                charge += chargeRate * Time.deltaTime; //時間に応じてチャージ
                Debug.Log(charge);
            }
            rigidbody.AddForce(-direction * charge/100);
        }
        else
        {
            //スペースキーが押されていない時，マシンが浮き，前進方向に力を受ける
            rigidbody.position = new Vector3(position.x, floating, position.z);

            rigidbody.AddForce(direction*charge); //チャージに応じてダッシュ
            charge = 0; //reset
        }

        if (Input.GetKey(KeyCode.DownArrow)) //↓キーが押されたとき
        {

        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rigidbody.AddTorque(new Vector3(0, -rotation, 0)); //←キーが押されたとき，マシンは時計回りのトルクを受ける
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            rigidbody.angularVelocity = new Vector3(); //←キーが押されていない時，マシンの角速度を0にする
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            rigidbody.AddTorque(new Vector3(0, rotation, 0)); //→キーが押されたとき，マシンは反時計回りのトルクを受ける
        }
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            rigidbody.angularVelocity = new Vector3(); //→キーが押されていない時，マシンの角速度を0にする
        }

        //最大速度，最大角速度でクリッピング
        var Vel = Mathf.Clamp(rigidbody.velocity.magnitude, minVel, maxVel);
        rigidbody.velocity = Vel * rigidbody.velocity.normalized;
        var angVel = Mathf.Clamp(rigidbody.angularVelocity.magnitude, minAngVel, maxAngVel);
        rigidbody.angularVelocity = angVel * rigidbody.angularVelocity.normalized;
    }
}