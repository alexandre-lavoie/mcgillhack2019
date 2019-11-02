using UnityEngine;
using MLAgents;

public class MHAgent : Agent
{
    public MHArea area;
    Rigidbody[] bodies;
    public float maxForce = 10;

    void Start()
    {
        bodies = new Rigidbody[area.numberOfCubes];
        foreach (GameObject cube in area.cubes)
        {
            bodies[0] = cube.GetComponent<Rigidbody>();
        }
    }

    public override void CollectObservations()
    {
        foreach (Rigidbody body in bodies)
        {
            AddVectorObs(body.velocity);
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        SetReward(1.0f / this.agentParameters.maxStep);

        int bodyIndex = PickFromNChoices(Mathf.Clamp(vectorAction[0], -1, 1), -1, 1, bodies.Length);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[1];
        controlSignal.z = vectorAction[2];
        controlSignal.y = vectorAction[3];
        bodies[bodyIndex].AddForce(Vector3.Normalize(controlSignal) * maxForce);

        float decision = Mathf.Clamp(vectorAction[4], -1, 1);
        int decisionIndex = -1; //default: undecided
        float separator = 1.0f / area.numberOfCubes;
        if (decision >= 0) //decided
        {
            decisionIndex = PickFromNChoices(decision, 0, 1, area.numberOfCubes);
            SetReward(1 - decisionIndex * separator);
            Done();
        }

    }

    public override void AgentReset()
    {
    }

    public override void AgentOnDone()
    {
    }


    //Helper methods
    /**
     * Input: a float between min and max
     * 
     */
    private int PickFromNChoices(float flo, float min, float max, int n)
    {
        int pick = -1; //Not picked yet

        if (flo < min || flo > max)
        {
            throw new System.ArgumentException("When picking from n choices, it is necessary that" +
                " the float is between the specified min and max");
        }

        //Resize range to 0, 1
        flo -= min; max -= min;
        flo /= max;

        float separator = 1.0f / area.numberOfCubes;
        for (int i = 1; i <= n; i++)
        {
            if (flo <= i * separator) //so i*separator = 1/n, 2/n, 3/n, ... , (n-1)/n, 1
            {
                pick = i - 1;
            }
        }
        return pick;
    }
}
