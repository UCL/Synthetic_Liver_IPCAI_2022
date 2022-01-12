using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("Perception/My Light Randomizer")]
public class MyLightRandomizer : Randomizer
{
    public FloatParameter lightIntensityParameter;
    public FloatParameter lightRangeParameter;
    public FloatParameter lightOuterAngleParameter;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<MyLightRandomizerTag>();

        foreach (var tag in tags)
        {
            var light = tag.GetComponent<Light>();
            light.intensity = lightIntensityParameter.Sample();
            light.range = lightRangeParameter.Sample();

            if (light.type == LightType.Spot);
            {
                light.spotAngle = lightOuterAngleParameter.Sample();
            }

        }
    }
}