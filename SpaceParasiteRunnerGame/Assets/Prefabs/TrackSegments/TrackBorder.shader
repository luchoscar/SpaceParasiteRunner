Shader "Custom/TrackBorder" {
	Properties {
		_Color ("Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_MainTex ("Base (RGBA)", 2D) = "white" {}
		_PlayerPos ("Player Pos", Vector) = (0, 0, 0, 0)
	}
	SubShader {
		Tags { "Queue"="Transparent" }
		LOD 200
			
		Blend SrcAlpha OneMinusSrcAlpha
		Cull off
		
		CGPROGRAM
		#pragma surface surf Lambert alpha vertex:vert
		
		sampler2D _MainTex;
		float4 _Color;
		float3 _PlayerPos;
		float _MaxDist;
		
		struct Input {
			float3 pos;
			float2 uv_MainTex;
		};

		void vert (inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input,o);
			o.pos = v.vertex.xyz;
      	}
      	
		void surf (Input IN, inout SurfaceOutput o) {
			float3 pointDist = IN.pos - _PlayerPos;
			float pointDistMag = length(pointDist);
			pointDist = (pointDistMag / _MaxDist);
			pointDist = saturate(pointDist);
			pointDist = min(0.25, pointDist);
			half4 c = tex2D (_MainTex, IN.uv_MainTex) * half4(_Color);
			o.Albedo = c.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
