Shader "Tzar/AdvancedVerticalFog" {
	Properties {
		[Header(MAIN)] [Header(Texture and Color)] [Space] _MainTex ("Main Texture", 2D) = "white" {}
		_Color ("Color", Vector) = (1,1,1,0.5)
		[PowerSlider(1.0)] _IntersectionThresholdMax ("Fog Intensity", Range(0, 2)) = 0.5
		[Space] _Alpha ("Alpha", Range(-1, 1)) = 1
		[Toggle] _Invert ("Invert Color", Float) = 0
		[Space] [Header(Cookie)] [Toggle] _UseCookie ("Enable Cookie", Float) = 0
		_Cookie ("Cookie", 2D) = "white" {}
		_CookieStrength ("Cookie Alpha", Range(0, 1)) = 1
		[Space] [Header(Movement)] [Toggle] _Rotation ("Rotate", Float) = 0
		_RotationSpeed ("Rotation Speed", Range(-1, 1)) = 0
		_OriginX ("Origin X", Range(-2, 2)) = 0
		_OriginY ("Origin Y", Range(-2, 2)) = 0
		[Space(30)] [Header(DISTORTION)] [Header(Distortion Texture)] _DistortTex ("Texture", 2D) = "white" {}
		[Space] [Toggle] _UseMainDistort ("Override Texture - Use Main Texture", Float) = 0
		_MainDistortAmount ("Main Texture Distortion Amount", Range(0, 1)) = 1
		[Space] [Toggle] _DistortCookie ("Override Texture - Use Cookie Texture", Float) = 0
		_DistortCookieAmount ("Cookie Distortion Amount", Range(0, 1)) = 1
		[Space] [Toggle] _MainDistort ("Distort Main Texture", Float) = 0
		[Header(Distortion Values)] _Magnitude ("Magnitude", Range(0, 10)) = 0
		[PowerSlider(2.0)] _DistortSpeed ("Speed", Range(0, 2)) = 0
		_Period ("Period", Range(-3, 3)) = 1
		_Offset ("Period Offset", Range(0, 15)) = 0
		[Space] [Header(Distortion Movement)] [Toggle] _DistortionRotation ("Rotate", Float) = 0
		_DistortRotationSpeed ("Rotation Speed", Range(-2, 2)) = 0
		[Space] [Toggle] _Translate ("Move", Float) = 0
		_SpeedX ("X Speed", Range(-0.5, 0.5)) = 0
		_SpeedY ("Y Speed", Range(-0.5, 0.5)) = 0
		[Header(Debug)] [Toggle] _TestDistortion ("Show Distortion Texture", Float) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;
			float4 _MainTex_ST;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct Vertex_Stage_Output
			{
				float2 uv : TEXCOORD0;
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.uv = (input.uv.xy * _MainTex_ST.xy) + _MainTex_ST.zw;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			Texture2D<float4> _MainTex;
			SamplerState sampler_MainTex;
			float4 _Color;

			struct Fragment_Stage_Input
			{
				float2 uv : TEXCOORD0;
			};

			float4 frag(Fragment_Stage_Input input) : SV_TARGET
			{
				return _MainTex.Sample(sampler_MainTex, input.uv.xy) * _Color;
			}

			ENDHLSL
		}
	}
}