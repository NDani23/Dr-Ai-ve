using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    [SerializeField] GameManager gameManager;

    public float smoothing;
    public float turnSmoothing;

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(50, 800, 0);
        //transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, turnSmoothing);
        transform.LookAt(Vector3.zero);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gameManager.IsGameTime())
        {
            transform.position = Vector3.Lerp(transform.position, player.position, smoothing);
            transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation, turnSmoothing);
            transform.rotation = Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0));
        }
    }
}
