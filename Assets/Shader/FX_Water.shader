Shader "FX/Water" {
	Properties {
		_WaveScale ("Wave scale", Range(0.02, 0.15)) = 0.063
		_ReflDistort ("Reflection distort", Range(0, 1.5)) = 0.44
		_RefrDistort ("Refraction distort", Range(0, 1.5)) = 0.4
		_RefrColor ("Refraction color", Vector) = (0.34,0.85,0.92,1)
		[NoScaleOffset] _Fresnel ("Fresnel (A) ", 2D) = "gray" {}
		[NoScaleOffset] _BumpMap ("Normalmap ", 2D) = "bump" {}
		WaveSpeed ("Wave speed (map1 x,y; map2 x,y)", Vector) = (19,9,-16,-7)
		[NoScaleOffset] _ReflectiveColor ("Reflective color (RGB) fresnel (A) ", 2D) = "" {}
		_HorizonColor ("Simple water horizon color", Vector) = (0.172,0.463,0.435,1)
		[HideInInspector] _ReflectionTex ("Internal Reflection", 2D) = "" {}
		[HideInInspector] _RefractionTex ("Internal Refraction", 2D) = "" {}
	}
	// Restoration: AssetRipper exported this as a dummy (solid white). Standard Assets FX/Water in its "simple" mode, which
	// is what it draws without the Water.cs reflection/refraction cameras (this game has none).
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			sampler2D _ReflectiveColor;
			fixed4 _HorizonColor, _RefrColor;
			float4 WaveSpeed; float _WaveScale;
			sampler2D _BumpMap;
			struct appdata { float4 vertex : POSITION; float3 normal : NORMAL; };
			struct v2f { float4 pos : SV_POSITION; float2 bump0 : TEXCOORD0; float2 bump1 : TEXCOORD1; float3 viewDir : TEXCOORD2; UNITY_FOG_COORDS(3) };
			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				// WaterBasic.cs / Water.cs normally feed _WaveScale4 and _WaveOffset each frame; computed here instead.
				float4 scale4 = float4(_WaveScale, _WaveScale, _WaveScale * 0.4, _WaveScale * 0.45);
				float4 offset = frac(WaveSpeed * scale4 * _Time.y / 20.0);
				float4 wpos = mul(unity_ObjectToWorld, v.vertex);
				float4 temp = wpos.xzxz * scale4 + offset;
				o.bump0 = temp.xy;
				o.bump1 = temp.wz;
				o.viewDir = normalize(WorldSpaceViewDir(v.vertex)).xzy;
				UNITY_TRANSFER_FOG(o, o.pos);
				return o;
			}
			float Fresnel(v2f i)
			{
				half3 b1 = UnpackNormal(tex2D(_BumpMap, i.bump0)).rgb;
				half3 b2 = UnpackNormal(tex2D(_BumpMap, i.bump1)).rgb;
				return saturate(dot(i.viewDir, (b1 + b2) * 0.5));
			}
			fixed4 frag(v2f i) : SV_Target
			{
				half f = Fresnel(i);
				half4 water = tex2D(_ReflectiveColor, float2(f, f));
				fixed4 col;
				col.rgb = lerp(water.rgb * _RefrColor.rgb, _HorizonColor.rgb, water.a);
				col.a = _HorizonColor.a;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
