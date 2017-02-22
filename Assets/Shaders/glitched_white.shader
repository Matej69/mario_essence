Shader "Entity/Glitched_white"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_NoiseTex ("Noise texture", 2D) = "white" {}
	}

		SubShader
	{
		Tags
	{
		
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
		
	}
	
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		CGPROGRAM
		
#pragma surface surf Lambert vertex:vert nofog keepalpha
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA


	sampler2D _MainTex;
	sampler2D _NoiseTex;
	fixed4 _Color;
	sampler2D _AlphaTex;
	float _AlphaSplitEnabled;


	struct Input
	{
		float2 uv_MainTex;
		fixed4 color;
	};






	void vert(inout appdata_full v, out Input o)
	{

		UNITY_INITIALIZE_OUTPUT(Input, o);
		o.color = v.color * _Color;
	}







	fixed4 SampleSpriteTexture(float2 uv)
	{
		fixed4 noise = tex2D(_NoiseTex, uv);

		float moveFactor = 1.2;
		float moveAmount = moveFactor * sin(_Time[0] * 30) / 50 * moveFactor;
		float move = (noise.rgb < 0.5) ? moveAmount : -moveAmount;

		float X = moveFactor * tan(_Time[1] * 20) / 5 * moveFactor / 100;

		fixed4 col = tex2D(_MainTex, uv + float2(X, move));

		if (tan(_Time[2]) < 0.00001f) {
			col.g += 0.65f / sin(_Time[3]);
			col.b += 0.65f * sin(_Time[3]);
			if (col.g > 0.95) {
				col.rgb = 0;
			}
		}

		return col;
	}







	void surf(Input IN, inout SurfaceOutput o)
	{
		
		fixed4 c = SampleSpriteTexture(IN.uv_MainTex) * IN.color;
		o.Albedo = c.rgb * c.a * 20;
		o.Alpha = c.a;
		
	}




	ENDCG
	}

		Fallback "Transparent/VertexLit"
}
