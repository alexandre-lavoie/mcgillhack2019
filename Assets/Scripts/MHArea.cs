﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using MathNet.Numerics.Distributions;

public class MHArea : Area
{
    public MHAgent mhAgent;
    public List<GameObject> cubes;
    public GameObject cubePrefab;
    public int numberOfCubes = 2;
    public float beta = 0;
    public string logString = "";

    public override void ResetArea()
    {
        resetCubes();
        //Debug.Log(logString);
        logString = "";
    }

    float randomMass(bool high)
    {
        Beta betaDist;
        if (high)
        {
            betaDist = new Beta(beta, 2);
        }
        else
        {
            betaDist = new Beta(2, beta);
        }
        return (9f * (float)betaDist.Sample()) + 1f;
    }

    void resetCubes()
    {
        if(cubes.Count == numberOfCubes)
        {
            int highIndex = Random.Range(0, numberOfCubes);
            for (int i = 0; i < numberOfCubes; i++)
            {
                cubes[i].transform.position = new Vector3(this.transform.position.x, i * 5.0f + 1f, this.transform.position.z);
                cubes[i].transform.rotation = Quaternion.Euler(Vector3.zero);
                cubes[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                cubes[i].GetComponent<Rigidbody>().mass = randomMass(i==highIndex);
            }
        } else
        {
            createCubes();
        }
    }

    void createCubes()
    {
        int highIndex = Random.Range(0, numberOfCubes);
        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject temporaryCube = Instantiate(cubePrefab, new Vector3(this.transform.position.x, i * 5.0f + 1f, this.transform.position.z), Quaternion.Euler(Vector3.zero));

            temporaryCube.GetComponent<Rigidbody>().mass = randomMass(i==highIndex);

            temporaryCube.transform.parent = this.transform;

            cubes.Add(temporaryCube);
        }
    }

    public Dictionary<int, int> getAnswerKey()
    {
        List<int> indices = new List<int>();

        for (int i=0; i<numberOfCubes; i++)
        {
            indices.Add(i);
        }

        indices.Sort((x, y) => ((cubes[y].GetComponent<Rigidbody>().mass > cubes[x].GetComponent<Rigidbody>().mass) ? 1 : -1));

        Dictionary<int, int> reversedIndices = new Dictionary<int, int>();

        for (int i = 0; i < numberOfCubes; i++)
        {
            reversedIndices.Add(indices[i], i);
        }
        // Returns a Dictionary where you give it the object index as a key and returns mass ranking.
        return reversedIndices;
    }
}
