using UnityEngine;
using MLAgents;

public class MHAgent : Agent
{
    public MHArea area;
    public float maxForce = 100;

    void Start()
    {
    }

    public override void CollectObservations()
    {
        foreach (GameObject cube in area.cubes)
        {
            AddVectorObs(cube.GetComponent<Rigidbody>().velocity.magnitude);
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        SetReward(-1.0f / this.agentParameters.maxStep);

        int bodyIndex = PickFromNChoices(Mathf.Clamp(vectorAction[0], -1, 1), -1, 1, area.numberOfCubes);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[1];
        area.cubes[bodyIndex].GetComponent<Rigidbody>().AddForce(Vector3.Normalize(controlSignal) * maxForce);

        float decision = Mathf.Clamp(vectorAction[2], -1, 1);
        int decisionIndex = -1; //default: undecided
        float separator = 1.0f / area.numberOfCubes;
        if (decision > 0) //decided
        {
            decisionIndex = PickFromNChoices(decision, 0, 1, area.numberOfCubes);
            SetReward(1 - decisionIndex * separator - 2);
            Done();
        }

    }

    public override void AgentReset()
    {
        area.ResetArea();
    }

    public override void AgentOnDone()
    {
        this.AgentReset();
    }


    //Helper methods
    /**
     * Input: a float between min and max
     * 
     */
    private int PickFromNChoices(float flo, float min, float max, int n)
    {

        if (flo < min || flo > max)
        {
            throw new System.ArgumentException("When picking from n choices, it is necessary that" +
                " the float is between the specified min and max");
        }

        //Resize range to 0, 1
        float range = max - min;
        flo = (flo - min) / range;

        float separator = 1.0f / n;
        for (int i = 1; i <= n; i++)
        {
            if (flo <= i * separator) //so i*separator = 1/n, 2/n, 3/n, ... , (n-1)/n, 1
            {
                return i - 1;
            }
        }
        return 0;
    }
}
