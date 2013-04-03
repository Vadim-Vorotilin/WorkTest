Shader "Custom/CustomTransparent" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass {
			Cull Front
			Color (1,0,0,1)
			SetTexture [_MainTex] { 
				Combine texture * primary
			} 
		}
		
		Pass {
			Cull Back
			Color (0,1,0,1)
			SetTexture [_MainTex] { 
				Combine texture * primary
			} 
		} 
	} 
}
