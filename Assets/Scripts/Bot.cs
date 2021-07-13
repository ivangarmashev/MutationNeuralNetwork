﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bot : MonoBehaviour
{
    public float speed;//Speed Multiplier
    public float rotation;//Rotation multiplier
    public LayerMask raycastMask;//Mask for the sensors

    private float[] input = new float[5];//input to the neural network
    public NeuralNetwork network;

    public int position;//Checkpoint number on the course
    public bool collided;//To tell if the car has crashed
    private string checkRow = "CheckPoint";
    private string backupRow = "CheckPoint2";

    private void Start()
    {
        if(gameObject.layer == LayerMask.NameToLayer("Bot2"))
        {
            backupRow = checkRow;
            checkRow = "CheckPoint2";
        }
    }


    void FixedUpdate()//FixedUpdate is called at a constant interval
    {

        if (!collided)//if the car has not collided with the wall, it uses the neural network to get an output
        {
            for (int i = 0; i < 5; i++)//draws five debug rays as inputs
            {
                Vector3 newVector = Quaternion.AngleAxis(i * 45 - 90, new Vector3(0, 1, 0)) * transform.right;//calculating angle of raycast
                RaycastHit hit;
                Ray Ray = new Ray(transform.position, newVector);

                if (Physics.Raycast(Ray, out hit, 10, raycastMask))
                {
                    input[i] = (10 - hit.distance) / 10;//return distance, 1 being close
                    Debug.DrawLine(Ray.origin, hit.point);
                }
                else
                {
                    input[i] = 0;//if nothing is detected, will return 0 to network
                }
            }

            float[] output = network.FeedForward(input);//Call to network to feedforward
        
            transform.Rotate(0, output[0] * rotation, 0, Space.World);//controls the cars movement
            transform.position += this.transform.right * output[1] * speed;//controls the cars turning
        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer(checkRow))//check if the car passes a gate
        {
            GameObject[] checkPoints = GameObject.FindGameObjectsWithTag(checkRow);
            for (int i = 0; i < checkPoints.Length; i++)
            {
                if (collision.collider.gameObject == checkPoints[i] && i == (position + 1 + checkPoints.Length) % checkPoints.Length)
                {
                    position++;//if the gate is one ahead of it, it increments the position, which is used for the fitness/performance of the network
                    //Debug.Log("Checkpoint");
                    break;
                }
            }
        }
        else if (collision.collider.gameObject.layer != gameObject.layer
            && collision.collider.gameObject.layer != LayerMask.NameToLayer("Death")
            && collision.collider.gameObject.layer != LayerMask.NameToLayer(backupRow))
        {
            collided = true;//stop operation if car has collided
            gameObject.layer = LayerMask.NameToLayer("Death");
        }
    }


    public void UpdateFitness()
    {
        network.fitness = position;//updates fitness of network for sorting
    }
}
