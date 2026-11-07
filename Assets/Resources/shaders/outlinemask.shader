Shader "Custom/Outline Mask" {
	Properties {
		[Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Float) = 0
	}
	// Restored QuickOutline mask: marks the object's own pixels in the stencil buffer so the fill only draws the rim.
	SubShader {
		Tags { "Queue"="Transparent+100" "RenderType"="Transparent" }
		Pass {
			Name "Mask"
			Cull Off
			ZTest [_ZTest]
			ZWrite Off
			ColorMask 0
			Stencil { Ref 1 Pass Replace }
		}
	}
}
