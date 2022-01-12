using System.Linq;
using System.IO;
using UnityEditor;
using UnityEngine;



//TODO: Need to set colliders as convex?

public static class VoxelizeVolumeUtils
{
    static int voxelizingLayer;
    static LayerMask layermask;

    public static void WriteVolume(VoxelizedVolume voxelizedVolume)
    {

        string filePath = voxelizedVolume.filename;
        using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
        {

            for (int x = 0; x < voxelizedVolume.dims; x++)
            {
                for (int y = 0; y < voxelizedVolume.dims; y++)
                {
                    for (int z = 0; z < voxelizedVolume.dims; z++)
                    {
                        writer.Write(voxelizedVolume.voxelValues[x, y, z]);
                    }

                }
            }

            writer.Close();
        }

    }

    public static bool isPointInsideCollider(MeshCollider meshCollider, Vector3 target)
    {
        // https://answers.unity.com/questions/163864/test-if-point-is-in-collider.html?childToView=394300#answer-394300
        // Check if a point is inside a mesh collider. Works for any 3d object (e..g concave meshes)

        Vector3 point;
        Vector3 start = new Vector3(0, 100, 0); //TODO: Set this based on the point? i.e. start + (100,100,100)
        float directionStepIncrementFactor = 10000.0f;
        Vector3 direction = target - start;
        direction.Normalize();
        int face_hits = 0;
        point = start;

        while (point != target)
        {
            RaycastHit hit;
            if (Physics.Linecast(point, target, out hit, layermask))
            {
                face_hits++;
                point = hit.point + (direction / directionStepIncrementFactor); // Move the point to hit.point and push it forward slightly, to move it through the skin of the mesh
            }

            else
            {
                point = target;
            }
        }

        while (point != start)
        { // try to return to where we came from, this will make sure we see all back faces too
            RaycastHit hit;
            if (Physics.Linecast(point, start, out hit, layermask))
            {
                face_hits++;
                point = hit.point + (-direction / directionStepIncrementFactor);
            }
            else
            {
                point = start;
            }
        }

        if (face_hits % 2 == 0)
        {
            return false;
        }

        else
        {
            return true;
        }

    }

    public static void VoxelizeVolume(VoxelizedVolume voxelizedVolume)
    // Based on https://bronsonzgeb.com/index.php/2021/05/15/simple-mesh-voxelization-in-unity/
    {

        if (!voxelizedVolume.TryGetComponent(out BoxCollider boxCollider))
        {
            boxCollider = voxelizedVolume.gameObject.AddComponent<BoxCollider>();
        }

        Bounds bounds = boxCollider.bounds;
        Vector3 minExtents = bounds.center - bounds.extents;

        voxelizedVolume.HalfSize = bounds.extents[0] / voxelizedVolume.dims;
        float halfSize = voxelizedVolume.HalfSize;
        Vector3 count = bounds.extents / halfSize;

        int xGridSize = Mathf.CeilToInt(count.x);
        int yGridSize = Mathf.CeilToInt(count.y);
        int zGridSize = Mathf.CeilToInt(count.z);

        voxelizedVolume.voxelValues = new float[xGridSize, yGridSize, zGridSize];

        voxelizedVolume.LocalOrigin = voxelizedVolume.transform.InverseTransformPoint(minExtents);

        // Fill array with empty space, then we iterate over all items and fill in the real voxels.
        for (int x = 0; x < xGridSize; ++x)
        {
            for (int y = 0; y < yGridSize; ++y)
            {
                for (int z = 0; z < zGridSize; ++z)
                {
                    voxelizedVolume.voxelValues[x, y, z] = 0;
                }
            }
        }

        foreach (MeshAndValue meshAndValue in voxelizedVolume.voxelizeObjects)
        {
            GameObject target = meshAndValue.mesh;
            float ctValue = meshAndValue.ctValue;

            if (!target)
            {
                Debug.Log("No mesh specified in Voxelise Objects list");
            }

            // Move object to new layer, so that we only have one collider on that layer
            // REstore old layer value afterwards
            voxelizingLayer = voxelizedVolume.voxelisingLayer;
            layermask = 1 << voxelizingLayer;

            int originalLayer = target.layer;
            target.layer = voxelizingLayer;

            if (!target.TryGetComponent(out MeshCollider meshCollider))
            {
                meshCollider = target.gameObject.AddComponent<MeshCollider>();
            }

            for (int x = 0; x < xGridSize; ++x)
            {
                for (int y = 0; y < yGridSize; ++y)
                {
                    for (int z = 0; z < zGridSize; ++z)
                    {

                        Vector3 pos = voxelizedVolume.PointToPosition(new Vector3Int(x, y, z));
                        if ( // Voxelise the edge and/or the inside of the mesh
                            (Physics.CheckBox(pos, new Vector3(halfSize, halfSize, halfSize), Quaternion.identity, layermask) && voxelizedVolume.voxeliseEdgeOfObject)
                            ||
                            (isPointInsideCollider(meshCollider, pos) && voxelizedVolume.voxeliseInsideObject)
                        )
                        {
                            voxelizedVolume.voxelValues[x, y, z] = ctValue;
                        }

                    }
                }

            }
            // Move object back to original layer. TODO: Wrap this in some kind of decorator (or whatever c# equivalent is)
            target.layer = originalLayer;
        }


    }
}