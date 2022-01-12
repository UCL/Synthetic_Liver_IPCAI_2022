using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;

/// <summary>
/// Randomizes the look at position of a camera tagged with a LookAtTag
/// </summary>
[Serializable]
[AddRandomizerMenu("Perception/LookAt Randomizer")]
public class LookAtRandomizer : Randomizer
{

    public FloatParameter xPosParameter;
    public FloatParameter yPosParameter;
    public FloatParameter zPosParemeter;
    public Transform target;


    protected override void OnIterationStart()
    {

        var tags = tagManager.Query<LookAtRandomizerTag>();
        foreach (var tag in tags)
        {
        Vector3 targetPosition = target.position;
        Vector3 offset = new Vector3(xPosParameter.Sample(),
                                    yPosParameter.Sample(),
                                    zPosParemeter.Sample());

        
        tag.transform.LookAt(targetPosition + offset, Vector3.right);

        }
    }
}

