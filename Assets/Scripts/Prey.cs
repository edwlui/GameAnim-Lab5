using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Prey : MonoBehaviour
{
    [SerializeField] FoV FoV;
    [SerializeField] int numberRays = 180;
    [SerializeField] float regularSpeed = 3.0f;
    [SerializeField] float fleeSpeed = 5.0f;
    [SerializeField] float currentSpeed = 0.0f;
    float targetTime, elapsedTime;
    bool predatorFound = false;
    RaycastHit hit;

    private void Start()
    {
        NewHeadingAndTime();
        currentSpeed = regularSpeed;
    }

    private void FixedUpdate()
    {
        // If predator is found, run the other direction and increase speed, reset predator
        if (predatorFound && targetTime >= elapsedTime && hit.collider.tag == "Predator")
        {
            transform.LookAt(hit.transform.position);
            transform.rotation = Quaternion.LookRotation(-transform.forward);
            //transform.rotation = Quaternion.Inverse(transform.rotation);
            if (currentSpeed < fleeSpeed)
            {
                currentSpeed += 1.0f;
            }
            transform.Translate(Vector3.forward * (regularSpeed * 2.0f) * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            predatorFound = false;
        }
        else
        {
            // if predator isnt found, return to regular state and decrease speed if needed
            // or if in regular state, do an update of frame
            // runs for target time chosen by newheadingandtime
            if (targetTime >= elapsedTime)
            {
                if (currentSpeed > regularSpeed)
                {
                    currentSpeed -= 0.5f;
                }
                transform.Translate(Vector3.forward * regularSpeed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
            }
            // Get new heading and time
            else
            {
                NewHeadingAndTime();
            }
            LookForPredator();
        }
    }

    // Looks for predator via raycasts and tags
    private void LookForPredator()
    {
        for (int i = 0; i <= numberRays; i++)
        {
            Vector3 currentRay = FoV.angleToVector3Heading((-FoV.viewAngle / 2) + ((FoV.viewAngle / numberRays) * i));
            if (Physics.Raycast(FoV.transform.position, currentRay, out hit, FoV.viewRange))
            {
                if (hit.collider.tag == "Predator")
                {
                    Debug.Log("Predator found");
                    elapsedTime = 0;
                    targetTime = 3.0f;
                    predatorFound = true;
                    break;
                }
                else
                {
                    predatorFound = false;
                }
            }
        }
    }

    // Get random heading and time to wander
    private void NewHeadingAndTime()
    {
        elapsedTime = 0.0f;
        targetTime = UnityEngine.Random.Range(0.5f, 3.0f);
        transform.eulerAngles = new Vector3(0, UnityEngine.Random.Range(0, 360), 0);
    }

    // If collides with wall, avoid
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            transform.rotation = Quaternion.LookRotation(-transform.forward);
            //transform.eulerAngles = new Vector3(0, -transform.forward.y, 0);
            //transform.rotation = Quaternion.Inverse(transform.rotation);
        }
    }
}
