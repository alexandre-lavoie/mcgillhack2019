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
    public float beta = 0;

    public override void ResetArea()
    {
        resetCubes();
    }

    void Start()
    {
        createCubes();
    }

    float randomMass()
    {
        return Mathf.Clamp(50 * beta + 100 * Random.Range(0f, 1f - beta), 50, 1000);
    }

    void resetCubes()
    {
        if(cubes.Count == numberOfCubes)
        {
            for (int i = 0; i < numberOfCubes; i++)
            {
                cubes[i].transform.position = new Vector3(0, i * 5.0f + 1.5f, 0);
                cubes[i].transform.rotation = Quaternion.Euler(Vector3.zero);
                cubes[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                cubes[i].GetComponent<Rigidbody>().mass = randomMass();
            }

            cubes.Sort((x, y) => ((y.GetComponent<Rigidbody>().mass > x.GetComponent<Rigidbody>().mass) ? 1 : -1));
        } else
        {
            createCubes();
        }

    }

    void createCubes()
    {
        for (int i = 0; i < numberOfCubes; i++)
        {
            GameObject temporaryCube = Instantiate(cubePrefab, new Vector3(0, i * 5.0f + 1.5f,0), Quaternion.Euler(Vector3.zero));

            temporaryCube.GetComponent<Rigidbody>().mass = randomMass();

            temporaryCube.transform.parent = this.transform;

            cubes.Add(temporaryCube);
        }

        cubes.Sort((x,y) => ((y.GetComponent<Rigidbody>().mass > x.GetComponent<Rigidbody>().mass) ? 1 : -1));
    }
}
