using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMaterial : MonoBehaviour
{

    public Material mat;

    Shader shader;
    // Start is called before the first frame update
    void Start()
    {
            shader = Shader.Find("HDRP/Nature/SpeedTree8");

    }

    // Update is called once per frame
    void Update()
    {
    //GetComponent<MeshRenderer>().material.shader = shader;
    GetComponent<MeshRenderer>().material = mat;
    }
}
