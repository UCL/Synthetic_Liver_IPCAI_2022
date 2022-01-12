using System;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers.SampleRandomizers.Tags;

namespace UnityEngine.Perception.Randomization.Randomizers.SampleRandomizers
{
    /// <summary>
    /// Randomizes the material of objects tagged with a MaterialRandomizerTag
    /// </summary>
    [Serializable]
    [AddRandomizerMenu("Perception/Material Randomizer")]
    public class MaterialRandomizer : Randomizer
    {

        /// <summary>
        /// The list of materials to sample and apply to target objects
        /// </summary>
        [Tooltip("The list of materials to sample and apply to target objects.")]
        public MaterialParameter material;
        public FloatParameter tilingRange;
        public ColorRgbParameter colorParameter;

        /// <summary>
        /// Randomizes the material of tagged objects at the start of each scenario iteration
        /// </summary>
        protected override void OnIterationStart()
        {
            var tags = tagManager.Query<MaterialRandomizerTag>();
            foreach (var tag in tags)
            {
                var renderer = tag.GetComponent<Renderer>();

                Material new_material = material.Sample();
                renderer.material = new_material;

                if (!(new_material.name=="Liver8k")) {
                    float tiling  = tilingRange.Sample();
                    renderer.material.SetFloat("_Tiling", tiling);
                }

                renderer.material.SetColor("_BrightnessTint", colorParameter.Sample());
            }
        }
    }
}