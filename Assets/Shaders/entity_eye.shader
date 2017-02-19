Shader "Entity/entity_eye"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" {}
		_EyeTex ("Eye texture", 2D) = "white" {}
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
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _EyeTex;

			fixed4 frag (v2f i) : SV_Target
			{
			
				i.uv.x += 0.3;
				fixed4 noise = tex2D(_EyeTex, i.uv);
				
				fixed4 fin = tex2D(_MainTex, i.uv);



				return noise;
			}
			ENDCG
		}
	}


}
