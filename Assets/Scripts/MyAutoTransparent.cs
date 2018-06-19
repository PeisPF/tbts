using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAutoTransparent : MonoBehaviour
{
    private Shader m_OldShader = null;
    private Color m_OldColor = Color.black;
    private float m_Transparency = 0.3f;
    private const float m_TargetTransparancy = 0.3f;
    private const float m_FallOff = 0.1f; // returns to 100% in 0.1 sec

    private Material nonTransparentMaterial;
    private Material transparentMaterial;
    public bool beTransparent;

    public Material getTransparentMaterial() {
        Material m = new Material(Shader.Find("Standard"));
        m.SetFloat("_Mode", 2);
        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        m.SetInt("_ZWrite", 0);
        m.DisableKeyword("_ALPHATEST_ON");
        m.EnableKeyword("_ALPHABLEND_ON");
        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        m.renderQueue = 3000;
        return m;
    }

    private void Start()
    {
        nonTransparentMaterial = GetComponent<Renderer>().material;
        transparentMaterial = getTransparentMaterial();
    }

    public void BeTransparent()
    {

        beTransparent = true;
        // reset the transparency;
        /*m_Transparency = m_TargetTransparancy;
        //
        if (m_OldShader == null)
        {
            // Save the current shader
            m_OldShader = GetComponent<Renderer>().material.shader;
            m_OldColor = GetComponent<Renderer>().material.color;
            GetComponent<Renderer>().material = getTransparentMaterial();
        }*/
    }
    void Update()
    {
        /*if (m_Transparency < 1.0f)
        {
            Color C = GetComponent<Renderer>().material.color;
            C.a = m_Transparency;
            GetComponent<Renderer>().material.color = C;
        }
        else
        {
            // Reset the shader
            GetComponent<Renderer>().material.shader = m_OldShader;
            GetComponent<Renderer>().material.color = m_OldColor;
            // And remove this script
            Destroy(this);
        }

        m_Transparency += ((1.0f - m_TargetTransparancy) * Time.deltaTime) / m_FallOff;*/
        
        if (beTransparent) {
            this.GetComponent<Renderer>().material = transparentMaterial;
            Color color = this.GetComponent<Renderer>().material.color;
            color.a = 0.3f;
            this.GetComponent<Renderer>().material.color = color;
            beTransparent = false;
        }
        else
        {
            this.GetComponent<Renderer>().material = nonTransparentMaterial;
            Destroy(this);
        }

    }
}