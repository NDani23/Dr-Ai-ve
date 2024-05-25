using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.MLAgents;
using static UnityEngine.GraphicsBuffer;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.Barracuda;
using Unity.Mathematics;
using Unity.MLAgents.Policies;

public class Player : Agent
{

    private CarController carController;
    [SerializeField] private Rigidbody carRB;
    [SerializeField] private Transform carTransform;
    [SerializeField] private TrackScript Track;
    [SerializeField] private InputController inputController;
    [SerializeField] private GameManager gameManager;

    private bool drive = false;
    private int nextCheckPoint = 0;

    private float time = 0.0f;

    public void StartDrive()
    {
        drive = true;
    }

    public void Update()
    {
        if (!drive) return;
        time += Time.deltaTime;
    }

    public override void Initialize()
    {
        //drive = true;
        carController = GetComponent<CarController>();
    }

    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(0f, 0.5f, -5f);
        nextCheckPoint = 0;
        time = 0.0f;
        carRB.velocity = Vector3.zero;
        carRB.angularVelocity = Vector3.zero;
        transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 CrossVec = Vector3.Cross(carTransform.forward, carTransform.up);

        Vector3 GeneralDir = Track.GetCheckPointTransform(nextCheckPoint).forward
                                              + Track.GetCheckPointTransform(nextCheckPoint + 1).forward
                                              + Track.GetCheckPointTransform(nextCheckPoint + 2).forward
                                              + Track.GetCheckPointTransform(nextCheckPoint + 3).forward
                                              + Track.GetCheckPointTransform(nextCheckPoint + 4).forward;

        sensor.AddObservation(carRB.velocity.magnitude);
        sensor.AddObservation(Vector3.Magnitude(GeneralDir));

        GeneralDir = Vector3.Normalize(GeneralDir);
        sensor.AddObservation(Vector3.Dot(carTransform.forward, GeneralDir));
        sensor.AddObservation(Vector3.Dot(CrossVec, GeneralDir));

    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        if (!drive) return;

        AddReward(-10f / MaxStep);

        if (carRB.velocity.magnitude < 10.0f)
        {
            AddReward(-0.05f);
        }

        carController.Throttle = actions.ContinuousActions[0];
        carController.Steer = actions.ContinuousActions[1];
    }


    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[1] = inputController.SteerInput;
        continuousActions[0] = inputController.ThrottleInput;
    }

    private void OnCollisionEnter(Collision collision)
    {
        AddReward(-(collision.relativeVelocity.magnitude * 0.05f + Mathf.Abs(Vector3.Dot(collision.contacts[0].normal, carTransform.forward)) * 5.0f));
    }

    private void OnCollisionStay(Collision collision)
    {
        AddReward(-0.01f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="CheckPoint")
        {
            int newID = Track.CrossCheckPoint(nextCheckPoint, other.transform);

            if(newID != nextCheckPoint)
            {
                if(gameManager != null && newID == 0)
                {
                    gameManager.setLapTime(time, GetComponent<BehaviorParameters>().BehaviorType == BehaviorType.HeuristicOnly ? 1 : 0);
                    time = 0.0f;
                }

                nextCheckPoint = newID;
                AddReward(2f);
            }


        }
    }
}
