using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform centerOfMass;

    public float motorTorque = 100f;
    public float breakTorque = 100f;
    public float maxSteer = 20f;

    public float Steer { get; set; }
    public float Throttle { get; set; }
    public float Break { get; set; }

    private Rigidbody _rigidbody;

    private Wheel[] wheels;

    private void Start()
    {
        wheels = GetComponentsInChildren<Wheel>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = centerOfMass.localPosition;
    }

    private void FixedUpdate()
    {
        int i = 0;
        foreach (var wheel in wheels)
        {
            wheel.SteerAngle = Steer * maxSteer;

            if(Throttle < 0)
            {
                float dotProuct = Vector3.Dot(transform.forward, _rigidbody.velocity);
                wheel.Torque = (dotProuct < 0 ? Throttle * 0.5f : Throttle * 3f) * motorTorque;
            }
            else
            {
                wheel.Torque = Throttle * motorTorque;
            }

            i++;
            
        }
    }
}
