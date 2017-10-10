Shader "Custom/TransparencyShader" {
	Properties {
		_TransLevel ("Transparent level", Range(0,1)) = 0.5
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" "Queue"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Lambert alpha

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		float _TransLevel;

		void surf (Input IN, inout SurfaceOutput o) {
			// Albedo comes from a texture tinted by color
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.g * _TransLevel;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
