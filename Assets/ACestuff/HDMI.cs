using UnityEngine;
using UnityEngine.UI;

public class HDMI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public RawImage img = default;
    private WebCamTexture cam;
    
    void Start()
    {
        cam = new WebCamTexture();
        if(!cam.isPlaying) cam.Play();
        img.texture = cam;

    }

}
