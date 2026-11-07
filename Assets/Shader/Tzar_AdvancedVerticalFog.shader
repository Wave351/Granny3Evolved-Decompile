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
	// Restoration: AssetRipper exported this as a dummy. Approximation of Tzar's vertical fog: each horizontal fog plane
	// is as thick as the distance between it and the scene behind it (depth texture), so it reads as a layer of fog over
	// the floor, thinning softly where it meets walls and furniture. The clouds texture scrolls through it.
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			sampler2D _MainTex; float4 _MainTex_ST;
			UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
			fixed4 _Color;
			float _IntersectionThresholdMax, _Alpha, _Invert, _Translate, _SpeedX, _SpeedY;
			float _Rotation, _RotationSpeed, _OriginX, _OriginY, _Magnitude, _DistortSpeed, _Period, _Offset, _MainDistortAmount;
			struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; float4 screen : TEXCOORD1; float eye : TEXCOORD2; UNITY_FOG_COORDS(3) };
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float2 uv = TRANSFORM_TEX(v.uv, _MainTex);
				if (_Rotation > 0.5)
				{
					float a = _Time.y * _RotationSpeed * 6.2831853;
					float2 c = float2(_OriginX, _OriginY) + 0.5, d = uv - c;
					uv = c + float2(d.x * cos(a) - d.y * sin(a), d.x * sin(a) + d.y * cos(a));
				}
				if (_Translate > 0.5) uv += _Time.y * float2(_SpeedX, _SpeedY);
				o.uv = uv;
				o.screen = ComputeScreenPos(o.pos);
				o.eye = -UnityObjectToViewPos(v.vertex).z;
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				float wave = sin((uv.y * _Period + _Time.y * _DistortSpeed) * 6.2831853 + _Offset);
				uv += (tex2D(_MainTex, uv * 0.5).r - 0.5) * wave * _Magnitude * 0.004 * _MainDistortAmount;
				fixed4 tex = tex2D(_MainTex, uv);
				if (_Invert > 0.5) tex.rgb = 1 - tex.rgb;
				float scene = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screen)));
				float thickness = saturate((scene - i.eye) * _IntersectionThresholdMax);
				float density = saturate(dot(tex.rgb, float3(0.299, 0.587, 0.114)) + _Alpha);
				fixed4 col = fixed4(_Color.rgb * tex.rgb, saturate(_Color.a * 2 * density * thickness));
				col.a *= saturate((i.eye - 0.3) / 1.5);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
