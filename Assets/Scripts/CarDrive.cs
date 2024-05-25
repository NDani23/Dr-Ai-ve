
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDrive : MonoBehaviour
{
    public WheelCollider frontDriverW, frontPassangerW;
    public WheelCollider rearDriverW, rearPassangerW;
    public Transform frontDriverT, frontPassangerT;
    public Transform rearDriverT, rearPassangetT;
    public float maxSteerAngle = 30;
    public float motorForce = 50;

    private float m_horizontalInput;
    private float m_verticalInput;
    private float m_steeringAngle;

    public void GetInput()
    {
        m_horizontalInput = Input.GetAxis("Horizontal");
        m_verticalInput = Input.GetAxis("Vertical");

    }

    private void Steer()
    {
        m_steeringAngle = maxSteerAngle * m_horizontalInput;
       // Mathf.Lerp(frontDriverW.steerAngle, m_steeringAngle, 0.9f);
        frontDriverW.steerAngle = Mathf.Lerp(frontDriverW.steerAngle, m_steeringAngle, 0.9f);
        frontPassangerW.steerAngle = Mathf.Lerp(frontPassangerW.steerAngle, m_steeringAngle, 0.9f);
    }

    private void Accelerate()
    {
        rearDriverW.motorTorque = -m_verticalInput * motorForce;
        rearPassangerW.motorTorque = -m_verticalInput * motorForce;

        //frontDriverW.motorTorque = -m_verticalInput * motorForce;
        //frontPassangerW.motorTorque = -m_verticalInput * motorForce;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontDriverW, frontDriverT);
        UpdateWheelPose(frontPassangerW, frontPassangerT);
        UpdateWheelPose(rearDriverW, rearDriverT);
        UpdateWheelPose(rearDriverW, rearDriverT);

    }

    private void UpdateWheelPose(WheelCollider _collider, Transform _transform)
    {
        Vector3 _pos = _transform.position;
        Quaternion _quat = _transform.rotation;

        _collider.GetWorldPose(out _pos, out _quat);

        _transform.position = _pos;
        _transform.rotation = _quat;
    }

    private void Start()
    {
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        Debug.Log(collider.bounds.size);
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPoses();
    }

    //private void OnCollisionStay(Collision collision)
    //{

    //    ContactPoint[] contacts = new ContactPoint[collision.contactCount];
    //    int points = collision.GetContacts(contacts);
    //    for (int i = 0; i < collision.contactCount; i++)
    //    {
    //        //Debug.Log(contacts[i].otherCollider.gameObject.transform.up);
    //        if (contacts[i].normal.y != 1)
    //        {
    //            //Debug.Log("Collision!");
    //        }
    //    }

    //}
}
