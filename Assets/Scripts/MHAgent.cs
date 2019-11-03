using UnityEngine;
using MLAgents;
using System.Collections.Generic;

public class MHAgent : Agent
{
    public MHArea area;
    public float maxForce = 100;
    private string logString = "";

    void Start()
    {
    }

    public override void CollectObservations()
    {
        foreach (GameObject cube in area.cubes)
        {
            AddVectorObs(cube.GetComponent<Rigidbody>().velocity.x);
        }
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        if(GetStepCount() > this.agentParameters.maxStep / 2.0f)
        {
            AddReward(-1.0f / this.agentParameters.maxStep);
        } else
        {
            AddReward(1.0f / this.agentParameters.maxStep);
        }

        int bodyIndex = PickFromNChoices(Mathf.Clamp(vectorAction[0], -1, 1), -1, 1, area.numberOfCubes);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = vectorAction[1];
        area.cubes[bodyIndex].GetComponent<Rigidbody>().AddForce(Vector3.Normalize(controlSignal) * maxForce);

        area.logString += bodyIndex + ", ";

        float decision = Mathf.Clamp(vectorAction[2], -1, 1);
        float separator = 1.0f / area.numberOfCubes;
        if (decision > 0) //decided
        {
            int decisionIndex = PickFromNChoices(decision, 0, 1, area.numberOfCubes);
            Dictionary<int, int> answerKey = area.getAnswerKey();
            // If the selected object index is the heaviest, thus 0 rank, reward agent.
            if (answerKey[decisionIndex] == 0)
            {
                AddReward(1);
            } else
            {
                // Else remove according to the how wrong it is.
                AddReward(-answerKey[decisionIndex]);
            }
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
