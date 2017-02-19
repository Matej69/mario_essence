Shader "Entity/Ghost"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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

				o.vertex.y += _Time[1] * 0.035f;

				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				float Xchanged = sin(_Time[1] * i.vertex.y / 60) / 80;
				fixed4 col = tex2D(_MainTex, i.uv + float2(Xchanged,0));
				
				if (col.a != 0) {
					col.rgb += 0.4;
					col.a = 0.2;
				}

				return col;
			}
			ENDCG
		}
	}
}
