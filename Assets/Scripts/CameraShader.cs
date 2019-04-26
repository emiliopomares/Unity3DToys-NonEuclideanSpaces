using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CameraShader : MonoBehaviour
{


    private Material material;

    // Creates a private material used to the effect
    void Awake()
    {
        material = new Material(Shader.Find("Stencils/Legacy Shaders/Bumped Specular"));
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {

        Graphics.Blit(source, destination, material);
    }
}