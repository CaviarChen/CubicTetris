// Author: Zijun Chen
Shader "Custom/TransparencyPulsate"
{
    Properties
    {
        _ObjectColor("Object Color", Color) = (0, 0, 0)
        _Transparency("Transparency", Range(0.0,1.0)) = 0.5
        _Speed("Speed", float) = 1.0
        _SizeRange("Size Range", float) = 0.4
        _TransparencyRange("Transparency Range", float) = 0.1
    }

    CGINCLUDE
        #include "UnityCG.cginc"

        struct vertIn
        {
            float4 vertex : POSITION;
            float4 normal : NORMAL;
            float4 color : COLOR;
        };

        struct vertOut
        {
            float4 vertex : SV_POSITION;
            float4 color : COLOR;
        };

    ENDCG


    SubShader
    {
        // prevent some issues
        Tags{ "DisableBatching" = "True" }

        ZWrite Off
        // use alpha blending
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            uniform float3 _ObjectColor;
            uniform float _Transparency;
            uniform float _Speed;
            uniform float _SizeRange;
            uniform float _TransparencyRange;

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                v.vertex = v.vertex * (1 + sin(_Time.y * _Speed) * _SizeRange);
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target
            {
                float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
                returnColor.rgb = _ObjectColor.rgb;
                returnColor.a = _Transparency + sin(_Time.y * _Speed) * _TransparencyRange;
                return returnColor;
            }

            ENDCG
        }
    }
}
