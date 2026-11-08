// Glowing additive shader for the Hazard Placer's lasers. No lighting and no fog, so lasers stay bright in the dark.
Shader "Hazards/LaserAdditive"
{
	Properties
	{
		_MainTex ("Glow texture", 2D) = "white" {}
		_Color ("Colour", Color) = (1,1,1,1)
		_Intensity ("Intensity", Float) = 1
	}
	SubShader
	{
		Tags { "Queue"="Transparent+10" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
		Blend One One
		ZWrite Off
		Cull Off
		Lighting Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;
			float _Intensity;
			struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };
			struct v2f { float4 pos : SV_POSITION; float2 uv : TEXCOORD0; fixed4 color : COLOR; };
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color * _Color;
				return o;
			}
			fixed4 frag (v2f i) : SV_Target
			{
				fixed3 glow = tex2D(_MainTex, i.uv).rgb;
				return fixed4(glow * i.color.rgb * i.color.a * _Intensity, 1);
			}
			ENDCG
		}
	}
}
