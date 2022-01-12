using System;
using UnityEngine;
using UnityEngine.Perception.Randomization.Parameters;
using UnityEngine.Perception.Randomization.Randomizers;

[Serializable]
[AddRandomizerMenu("Perception/Offset Randomizer")]
public class OffsetRandomizer : Randomizer
{
    public FloatParameter xOffset;
    public FloatParameter yOffset;
    public FloatParameter tilingRange;
    public Texture2DParameter normals;

    static readonly int k_BaseMap = Shader.PropertyToID("_BaseMap");
#if HDRP_PRESENT
        const string k_TutorialHueShaderName = "Shader Graphs/HueShiftOpaque";
        static readonly int k_BaseMap = Shader.PropertyToID("_BaseColorMap");
#endif
    protected override void OnIterationStart()
    {
        var tags = tagManager.Query<OffsetRandomizerTag>();

        foreach (var tag in tags)
        {
            Vector2 offset = new Vector2(xOffset.Sample(), yOffset.Sample());
            float tiling = tilingRange.Sample();
            var renderer = tag.GetComponent<Renderer>();
#if HDRP_PRESENT
                // Choose the appropriate shader texture property ID depending on whether the current material is
                // using the default HDRP/lit shader or the Perception tutorial's HueShiftOpaque shader
                var material = renderer.material;
                material.SetTextureOffset("_BaseColorMap", offset);
#else
            renderer.material.SetTextureOffset(k_BaseMap, offset);
#endif
            renderer.material.mainTextureScale = new Vector2(tiling, tiling);
            renderer.material.SetTexture("_NormalMap", normals.Sample());

        }
    }
}