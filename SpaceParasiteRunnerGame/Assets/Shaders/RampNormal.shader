Shader "TCA/Ramp Normal"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Normal ("Normal Map", 2D) = "bump" {}
		_Ramp ("Lighting Ramp", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Ramp
		
		sampler2D _Ramp;
		
		half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten)
		{
			half NdotL = dot (s.Normal, lightDir);
			half4 c;
			half diff = NdotL * 0.5 + 0.5;
			half3 ramp = tex2D(_Ramp, float2(diff)).rgb;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = s.Alpha;
			return c;
		}
		
		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _Normal;
		
		struct Input
		{
			float2 uv_MainTex;
			float2 uv_Normal;
		};

		void surf (Input IN, inout SurfaceOutput o)
		{
			half4 c = tex2D(_MainTex, IN.uv_MainTex);
			half4 n = tex2D(_Normal, IN.uv_Normal);
			o.Normal = UnpackNormal(n);
			o.Albedo = c.rgb * _Color.rgb;
			o.Alpha = c.a * _Color.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
