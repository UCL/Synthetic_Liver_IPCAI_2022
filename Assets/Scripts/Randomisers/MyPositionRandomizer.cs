using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;

/// <summary>
/// Randomizes the position of objects tagged with a PositionRandomizerTag
/// </summary>
[Serializable]
[AddRandomizerMenu("Perception/My Position Randomizer")]
public class MyPositionRandomizer : Randomizer
{

    public FloatParameter xPosParameter;
    public FloatParameter yPosParameter;
    public FloatParameter zPosParemeter;
    //public GameObject inside;
    // public GameObject outside;

    protected override void OnIterationStart()
    {

        var tags = tagManager.Query<MyPositionRandomizerTag>();
        foreach (var tag in tags)
        {
            Vector3 new_position = new Vector3(
                xPosParameter.Sample(),
                yPosParameter.Sample(),
                zPosParemeter.Sample());

            // Vector3 closest_inside = inside.GetComponent<Collider>().ClosestPoint(new_position);
            // if (closest_inside != new_position) {
            //     Debug.Log("Outside sphere");
            // }
            // Vector3 closest_outside = outside.GetComponent<Collider>().ClosestPoint(new_position);
            // if (closest_inside == new_position) {
            //     Debug.Log("Inside Liver");
            // }


            tag.transform.position = new_position;
        }
    }
}

