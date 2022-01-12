using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCubes : MonoBehaviour
{
    int i = 0;
    float ctValue;
    VoxelizedVolume voxelizedVolume;

    Dictionary<float, Color> valueToColorMap = new Dictionary<float, Color>();


    public float drawDelay = 0.1f;
    public int numCubesToDrawAtOnce = 1;

    public Color GetColorFromValue(VoxelizedVolume voxelizedVolume, float ctValue)
    // Check if we have already stored the voxel colour for this voxel. If not
    // loop through items to find it, and store in dictionary.
    {
        foreach (MeshAndValue mesh in voxelizedVolume.voxelizeObjects)
        {
            if (valueToColorMap.ContainsKey(ctValue))
            {
                return valueToColorMap[ctValue];
            }

            if (mesh.ctValue == ctValue)
            {
                valueToColorMap[ctValue] = mesh.colour;
                return mesh.colour;
            }
        }

        //default case
        return Color.red;

    }


    // Start is called before the first frame update
    void Start()
    {
        voxelizedVolume = gameObject.GetComponent<VoxelizedVolume>();
        VoxelizeVolumeUtils.VoxelizeVolume(voxelizedVolume);

        StartCoroutine(CubeCoroutine());
    }

    IEnumerator CubeCoroutine()
    {

        // Nothing has been voxelised yet.
        if (voxelizedVolume.voxelValues == null)
        {
            yield return new WaitForSeconds(drawDelay);
        }

        for (int x = 0; x < voxelizedVolume.dims; ++x)
        {
            for (int y = 0; y < voxelizedVolume.dims; ++y)
            {
                for (int z = 0; z < voxelizedVolume.dims; ++z)
                {

                    ctValue = voxelizedVolume.voxelValues[x, y, z];

                    if (ctValue == 0) { continue; }

                    Vector3Int gridPoint = new Vector3Int(x, y, z);
                    Vector3 worldPos = voxelizedVolume.PointToPosition(gridPoint);
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    cube.transform.position = worldPos;
                    float size = 2 * voxelizedVolume.HalfSize;
                    cube.transform.localScale = new Vector3(size, size, size);

                    // TODO: Fix colouring
                    // Change cube colour based on CT Value
                    var cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.SetFloat("_Smoothness", 1);

                    cubeRenderer.material.SetColor("_BaseColor", GetColorFromValue(voxelizedVolume, ctValue));

                    // Draw numCubesToDrawAtOnce in one go, creates basic animation
                    i++;
                    if (i % numCubesToDrawAtOnce == 0)
                    {
                        i = 0;
                        yield return new WaitForSeconds(drawDelay);
                    }

                }

            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
