using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(VoxelizedVolume))]
public class VoxelizedVolumeEditor : Editor
{
    Dictionary<float, Color> valueToColorMap = new Dictionary<float, Color>();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Voxelize Volume"))
        {
            var voxelizedVolume = target as VoxelizedVolume;
            VoxelizeVolumeUtils.VoxelizeVolume(voxelizedVolume);
        }
    
        if (GUILayout.Button("Save Volume"))
        {
            var voxelizedVolume = target as VoxelizedVolume;
            VoxelizeVolumeUtils.WriteVolume(voxelizedVolume);
        }
    }

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
    void OnSceneGUI()
    {
        VoxelizedVolume voxelizedVolume = target as VoxelizedVolume;

        float size = voxelizedVolume.HalfSize * 2f;
        float ctValue;

        // Nothing has been voxelised yet.
        if (voxelizedVolume.voxelValues == null)
        {
            return;
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

                    Handles.color = GetColorFromValue(voxelizedVolume, ctValue);

                    Vector3 worldPos = voxelizedVolume.PointToPosition(gridPoint);
                    Handles.DrawWireCube(worldPos, new Vector3(size, size, size));

                }

            }
        }
        Handles.color = Color.red;
        if (voxelizedVolume.TryGetComponent(out MeshCollider meshCollider))
        {
            Bounds bounds = meshCollider.bounds;
            Handles.DrawWireCube(bounds.center, bounds.extents * 2);
        }

    }

    void Start()
    {

    }
}