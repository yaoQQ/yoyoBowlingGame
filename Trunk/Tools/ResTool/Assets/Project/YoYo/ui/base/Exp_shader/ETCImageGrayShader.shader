// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/ETCImageGrayShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		[PerRendererData] _AlphaTex("Sprite Alpha Texture", 2D) = "white" {}
	_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		//---Add---
		// Change the brightness of the Sprite
		_GrayScale("GrayScale", Float) = 1
		//---Add---
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

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile __ UNITY_UI_ALPHACLIP
#include "UnityCG.cginc"
#include "UnityUI.cginc"

	struct appdata_t
	{
		float4 vertex   : POSITION;
		float4 color    : COLOR;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f
	{
		float4 vertex   : SV_POSITION;
		fixed4 color : COLOR;
		half2 texcoord  : TEXCOORD0;
		float4 worldPosition :TEXCOORD1;
	};

	fixed4 _Color;
	fixed4 _TextureSampleAdd;
	float4 _ClipRect;
    

	v2f vert(appdata_t IN)
	{
		v2f OUT;
		OUT.vertex = UnityObjectToClipPos(IN.vertex);
		OUT.worldPosition=IN.vertex;
		OUT.texcoord = IN.texcoord;
		OUT.color = IN.color * _Color;
#ifdef PIXELSNAP_ON
		OUT.vertex = UnityPixelSnap(OUT.vertex);
#endif

		return OUT;
	}

	sampler2D _MainTex;
	sampler2D _AlphaTex;
	float _GrayScale;

	fixed4 frag(v2f IN) : SV_Target
	{
	                         
    fixed4 colorTex = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
    
  
    fixed AlphaTexAlpha = tex2D(_AlphaTex, IN.texcoord).r + _TextureSampleAdd.a;

  
    fixed4 color = fixed4(colorTex.rgb, colorTex.a * AlphaTexAlpha);
	
    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                
    #ifdef UNITY_UI_ALPHACLIP
    clip (color.a - 0.001);
    #endif
	
	//---Add--
	float cc = (color.r * 0.299 + color.g * 0.518 + color.b * 0.184);
	cc *= _GrayScale;
	color.r = color.g = color.b = cc;
	//---Add--
    //color.rgb = Luminance(color.rgb);

    return color;
	
	}
		ENDCG
	}
	}
}