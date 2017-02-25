Shader "Camera/Nocturno"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_CRTLines("CRT LINES", 2D) = "white" {}
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
			sampler2D _CRTLines;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);

			
				if (col.a != 0 && col.g < 0.65)
						col.rgb = 0;

				if (col.b == 1) {
					col.r = 0.4;
					col.g = 0;
					col.b = 0;
				}

				fixed4 lines = tex2D(_CRTLines, i.uv);
				if (lines.a < 0.15) {
					col.r += 0.1f;
					col.g += 0.1f;
					col.b += 0.1f;
				}


				return col;
			}
			ENDCG
		}
	}
}
