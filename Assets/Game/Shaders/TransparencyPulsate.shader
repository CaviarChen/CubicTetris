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

        _PointLightColor("Point Light Color", Color) = (0, 0, 0)
        _PointLightPosition("Point Light Position", Vector) = (0.0, 0.0, 0.0)
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
            float4 worldVertex : TEXCOORD0;
            float3 worldNormal : TEXCOORD1;
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

            uniform float3 _PointLightColor;
            uniform float3 _PointLightPosition;

            // Implementation of the vertex shader
            vertOut vert(vertIn v)
            {
                vertOut o;
                v.vertex = v.vertex * (1 + sin(_Time.y * _Speed) * _SizeRange);
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Convert Vertex position and corresponding normal into world coords.
                // Note that we have to multiply the normal by the transposed inverse of the world
                // transformation matrix (for cases where we have non-uniform scaling; we also don't
                // care about the "fourth" dimension, because translations don't affect the normal)
                float4 worldVertex = mul(unity_ObjectToWorld, v.vertex);
                float3 worldNormal = normalize(mul(transpose((float3x3)unity_WorldToObject), v.normal.xyz));

                o.worldVertex = worldVertex;
                o.worldNormal = worldNormal;

                return o;
            }

            // Implementation of the fragment shader
            fixed4 frag(vertOut v) : SV_Target
            {
                // add diffuse light
                float3 interpNormal = normalize(v.worldNormal);
                float3 L = normalize(_PointLightPosition - v.worldVertex.xyz);
                float LdotN = dot(L, interpNormal);
                float3 dif = _PointLightColor.rgb * _ObjectColor.rgb * saturate(LdotN);


                float4 returnColor = float4(0.0f, 0.0f, 0.0f, 0.0f);
                returnColor.rgb = _ObjectColor.rgb + dif.rgb;
                returnColor.a = _Transparency + sin(_Time.y * _Speed) * _TransparencyRange;
                return returnColor;
            }

            ENDCG
        }
    }
}
