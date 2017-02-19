Shader "Entity/glitch"
{
	Properties
	{
		_MainTex ("Main texture", 2D) = "white" {}
		_NoiseTex ("Noise texture", 2D) = "white" {}

	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			Cull Off Lighting Off ZWrite Off Fog{ Mode Off }
			Blend SrcAlpha OneMinusSrcAlpha

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

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 noise = tex2D(_NoiseTex, i.uv);
						
				float moveFactor = 1.2;
				float moveAmount = moveFactor * sin(_Time[0] * 30) / 50 * moveFactor;
				float move = (noise.rgb < 0.5) ? moveAmount : -moveAmount;

				float X = moveFactor * tan(_Time[1] * 20) / 5 * moveFactor / 100;
				
				fixed4 col = tex2D(_MainTex, i.uv + float2(X , move));

				if (tan(_Time[2]) < 0.00001f) {
					col.g += 0.65f / sin(_Time[3]);
					col.b += 0.65f * sin(_Time[3]);
					if (col.g > 0.95) {
						col.rgb = 0;
					}
				}


				
				

			
			return col;
			}
			ENDCG
		}
	}
}
