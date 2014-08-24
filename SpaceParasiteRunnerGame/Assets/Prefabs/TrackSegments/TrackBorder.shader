Shader "Custom/TrackBorder" {
	Properties {
		_Color ("Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_MainTex ("Base (RGBA)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 200
			
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		
		Pass {
			ZWrite On
			ColorMask 0
		}
		
		CGPROGRAM
		#pragma surface surf Lambert alpha
		
		sampler2D _MainTex;
		float4 _Color;
		float3 _PlayerPos;
		float _MaxDist;
		
		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
