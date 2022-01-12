using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[Serializable]
[AddRandomizerMenu("Perception/Lens Randomizer")]
public class LensDistortionRandomiser : Randomizer
{
    public UnityEngine.Perception.Randomization.Parameters.FloatParameter distortionIntensityParameter;
    public UnityEngine.Perception.Randomization.Parameters.FloatParameter blurIntensityParameter;

    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<LensDistorterRandomiserTag>();

        foreach (var tag in tags)
        {
            var volume = tag.GetComponent<Volume>();

            LensDistortion lensDistortion;
            if(volume.profile.TryGet(out lensDistortion))
            {
                //Adjust your lens here. ie
                lensDistortion.intensity.value = distortionIntensityParameter.Sample();

            }

            MotionBlur motionBlur;
            if(volume.profile.TryGet(out motionBlur))
            {
                //Adjust your lens here. ie
                motionBlur.intensity.value = blurIntensityParameter.Sample();

            }
        }
    }
}