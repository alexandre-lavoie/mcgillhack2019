using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class MHArea : Area
{
    public MHAgent mhAgent;
    public List<GameObject> cubes;
    public GameObject cubePrefab;
    public int numberOfCubes = 100;
    public float beta = 0;
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

    float randomMass()
    {
        return 50 * beta + 100 * Random.Range(0f, 1f - beta);
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
            GameObject temporaryCube = Instantiate(cubePrefab, new Vector3(0, i * 5.0f + 0.51f,0), cubePrefab.transform.rotation);

            temporaryCube.GetComponent<Rigidbody>().mass = randomMass();

            cubes.Add(temporaryCube);
        }

        cubes.Sort((x,y) => ((y.GetComponent<Rigidbody>().mass > x.GetComponent<Rigidbody>().mass) ? 1 : -1));
    }
}
