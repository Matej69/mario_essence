using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Shader : MonoBehaviour {
    
    public Material mat;

    void OnRenderImage(RenderTexture _in, RenderTexture _out) {
        Graphics.Blit(_in, _out, mat);
    }

}
