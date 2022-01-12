using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
[RequireComponent(typeof(Volume))]
[RequireComponent(typeof(BoxCollider))]

public class MeshAndValue
{
    public GameObject mesh;
    public float ctValue;
    [Tooltip("Colour used when drawing voxels in GUI, no relevance to saved data")]
    public Color colour;
}

public class VoxelizedVolume : MonoBehaviour
{
    public MeshAndValue[] voxelizeObjects;

    public int dims = 32;
    public bool voxeliseInsideObject = true;
    public bool voxeliseEdgeOfObject = true;

    [Tooltip("Objects will be moved to this layer during voxelisation, then moved back to original layer.")]
    public int voxelisingLayer = 27;
    public string filename;

    [HideInInspector]
    public float HalfSize;


    [HideInInspector]
    public float[,,] voxelValues;

    [HideInInspector]
    public Vector3 LocalOrigin;

    public Vector3 PointToPosition(Vector3Int point)
    {
        float size = HalfSize * 2f;
        Vector3 pos = new Vector3(HalfSize + point.x * size, HalfSize + point.y * size, HalfSize + point.z * size);
        return LocalOrigin + transform.TransformPoint(pos);
    }

}
