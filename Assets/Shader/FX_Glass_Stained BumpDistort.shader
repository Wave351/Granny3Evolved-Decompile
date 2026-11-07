Shader "FX/Glass/Stained BumpDistort" {
	Properties {
		_BumpAmt ("Distortion", Range(0, 128)) = 10
		_MainTex ("Tint Color (RGB)", 2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
	}
	// Restoration: AssetRipper exported this as a dummy. Standard Assets FX/Glass/Stained BumpDistort: grabs the screen
	// behind the glass, shifts it by the normal map and tints it with the main texture.
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Opaque" }
		GrabPass { Name "BASE" Tags { "LightMode" = "Always" } }
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog
			#include "UnityCG.cginc"
			struct appdata { float4 vertex : POSITION; float2 texcoord : TEXCOORD0; };
			struct v2f { float4 vertex : SV_POSITION; float4 uvgrab : TEXCOORD0; float2 uvbump : TEXCOORD1; float2 uvmain : TEXCOORD2; UNITY_FOG_COORDS(3) };
			float _BumpAmt; float4 _BumpMap_ST, _MainTex_ST;
			sampler2D _GrabTexture; float4 _GrabTexture_TexelSize;
			sampler2D _BumpMap, _MainTex;
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvgrab = ComputeGrabScreenPos(o.vertex);
				o.uvbump = TRANSFORM_TEX(v.texcoord, _BumpMap);
				o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}
			half4 frag(v2f i) : SV_Target
			{
				half2 bump = UnpackNormal(tex2D(_BumpMap, i.uvbump)).rg;
				float2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
				i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
				half4 col = tex2Dproj(_GrabTexture, UNITY_PROJ_COORD(i.uvgrab)) * tex2D(_MainTex, i.uvmain);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
