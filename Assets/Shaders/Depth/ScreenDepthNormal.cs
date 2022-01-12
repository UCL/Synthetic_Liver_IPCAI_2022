using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenDepthNormal : MonoBehaviour
{

    public Camera Cam;
    public Material Mat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Cam == null)
        {
            Cam = this.GetComponent<Camera>();
            Cam.depthTextureMode = DepthTextureMode.DepthNormals;
        }

        if (Mat == null)
        {
            //assign shader Hidden/ScreenDepthShader
            Mat = new Material(Shader.Find("Hidden/ScreenDepthShader"));
        }

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        // render source to screen
        //Graphics.Blit(source, destination);

        //render source to screen with Shader
        Graphics.Blit(source, destination, Mat);
        Debug.Log(1);

    }
}
