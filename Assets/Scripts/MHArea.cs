using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class MHArea : Area
{
    public MHAgent mhAgent;
    public List<GameObject> cubes;
    public GameObject cubePrefab;
    public int numberOfCubes = 2;
    public override void ResetArea()
    {
        createEnvironment();
    }

    void Start()
    {
        createEnvironment();
    }

    void createEnvironment()
    {
        createCubes();
    }
    void createCubes()
    {
        if (cubes.Count > 0)
        {
            foreach (GameObject cube in cubes)
            {
                Destroy(cube);
            }
        }

        cubes = new List<GameObject>();

        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject temporaryCube = Instantiate(cubePrefab, Vector3.zero, cubePrefab.transform.rotation);

            cubes.Add(temporaryCube);
        }
    }
}
