Shader "Camera/CRT"
{
	Properties
	{
		_MainTex ("MainTexture", 2D) = "white" {}
		_NoiseTex("NoiseTexture", 2D) = "white" {}
		_NoiseAmount("NoiseAmount", Range(0,0.1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			float _NoiseAmount;

			fixed4 frag (v2f IN) : SV_Target
			{
				//Noise
				float2 noise = tex2D(_NoiseTex, IN.uv).xy;
				noise = ((noise * 2) - 1) * _NoiseAmount;

				//Main
				fixed4 col = tex2D(_MainTex, IN.uv + noise);


				return col;
			}
			ENDCG
		}
	}
}
