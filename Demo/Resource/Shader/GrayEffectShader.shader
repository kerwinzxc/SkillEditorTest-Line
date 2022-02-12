// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/GrayEffectShader" {
Properties {
        _AlphaMul ("Alpha Factor",Range(0,1)) = 1
		_LightLevel("Light Level",float) = 1
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_BgTex ("Gray Texture",2D) = "white"{}
		_AlphaTex("Alpha texture",2D) = "white"{}
	}
	SubShader 
	{
		Tags { "QUEUE"="Transparent" "RenderType"="Transparent"}
		LOD 200
		Pass
		{
			//Cull Off
			Lighting Off
			Fog {Mode Off}
			Offset -1, -1
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
            #pragma vertex vert
			#pragma fragment frag			
			#include "UnityCG.cginc"

		    uniform sampler2D _MainTex;
			uniform sampler2D _BgTex;
			uniform sampler2D _AlphaTex;
			uniform float _AlphaMul;
			uniform float _LightLevel;

			float4 _MainTex_ST;
			float4 _BgTex_ST;
			float4 _AlphaTex_ST;
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct Input
			{
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				half2 bgTexcoord : TEXCOORD1;
				half2 alphaTexcoord : TEXCOORD2;
				fixed4 color : COLOR;
			};
	
			Input vert (appdata_t v)
			{
				Input o;
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.bgTexcoord = TRANSFORM_TEX(v.texcoord,_BgTex);
				o.alphaTexcoord = TRANSFORM_TEX(v.texcoord,_AlphaTex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.color = v.color;
				return o;
			}

		    fixed4 frag (Input IN) : COLOR
		    {
			  // convert rgb to gray
			  fixed gray = dot(tex2D(_BgTex,IN.bgTexcoord).rgb,fixed3(0.299f,0.578f,0.114f));
			  fixed alpha = tex2D(_AlphaTex,IN.alphaTexcoord).a;
			  fixed4 c = tex2D (_MainTex, IN.texcoord);

			  if((c.r + c.g + c.b) == 0)
			  {
			    c.a = 0;
			  }
			  else
			  {
				c = c * fixed4(1.0f,1.0f,1.0f,gray);
			  }

			  c.a *= _AlphaMul;
			  c *= _LightLevel;
			  return c;
			}
		    ENDCG
		}
	} 
	FallBack "Diffuse"
}
