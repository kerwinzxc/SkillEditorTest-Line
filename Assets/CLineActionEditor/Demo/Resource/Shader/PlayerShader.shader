Shader "Custom/PlayerShader" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	    _BumpMap("Bumpmap", 2D) = "bump" {}
		_Ramp ("Shading Ramp", 2D) = "gray" {}  
        _AmbientColor ("Ambient Color", Color) = (0.1, 0.1, 0.1, 1.0)  
        _SpecularColor ("Specular Color", Color) = (0.12, 0.31, 0.47, 1.0)  
        _Glossiness ("Gloss", Range(1.0,512.0)) = 512.0  
        _RimColor ("Rim Color", Color) = (0.12, 0.31, 0.47, 1.0)  
		_RimPower ("Rim Power", Range(0.5, 8.0)) = 3.0  
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		//#pragma surface surf Lambert
		//#pragma surface surf CustomBlinnPhong
 		#pragma surface surf CustomBlinnPhong exclude_path:prepass
					
		sampler2D _MainTex;  
		sampler2D _BumpMap;  
		sampler2D _Ramp;  
		          
		fixed4 _AmbientColor;  
		fixed4 _SpecularColor;  
		half _Glossiness;  
		  
		fixed4 _RimColor;  
		half _RimPower;  
		  
		struct Input {  
		    float2 uv_MainTex;  
		    float2 uv_BumpMap;  
		    half3 viewDir;     
		}; 

		void surf (Input IN, inout SurfaceOutput o) {
		//<SPAN style="WHITE-SPACE: pre"> </SPAN>fixed c = tex2D (_MainTex, IN.uv_MainTex);
		    fixed4 c = tex2D (_MainTex, IN.uv_MainTex);  
		    o.Albedo = c.rgb;  
		    o.Alpha = c.a; 
		    o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));  
		    fixed rim = 1.0 - saturate (dot (normalize(IN.viewDir), o.Normal));  
		    o.Emission = (_RimColor.rgb * pow (rim, _RimPower));  
		}
		
		inline fixed4 LightingCustomBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 viewDir, fixed atten)   
		{  
		    fixed3 ambient = s.Albedo * _AmbientColor.rgb;  
		    
		    fixed NdotL = saturate(dot (s.Normal, lightDir));   
		    fixed diff = NdotL * 0.5 + 0.5;  
		    fixed3 ramp = tex2D (_Ramp, float2(diff, diff)).rgb;
		    fixed diffuse = s.Albedo * _LightColor0.rgb * ramp;  
		              
		    fixed3 h = normalize (lightDir + viewDir);   
		    float nh = saturate(dot (s.Normal, h));   
		    float specPower = pow (nh, _Glossiness);  
		    fixed3 specular = _LightColor0.rgb * specPower * _SpecularColor.rgb;  
		  
		    fixed4 c;  
		    c.rgb = (ambient + diffuse + specular) * (atten * 2);  
		    c.a = s.Alpha + (_LightColor0.a * _SpecularColor.a * specPower * atten);  		    

		    return c;  			    
		}
		ENDCG
	} 
		  	
	FallBack "Diffuse"
}
