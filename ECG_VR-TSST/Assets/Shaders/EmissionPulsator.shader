Shader "EmissionPulsator"
{
	Properties
	{
		[HideInInspector] __dirty( "", Int ) = 1
		_AlbedoColor("Albedo Color", Color) = (0,0,0,0)
		_EmissionColor("Emission Color", Color) = (0.6627451,0,0,0)
		_EmissionMultiplier("Emission Multiplier", Range( 0 , 2)) = 1
		_PulseInterval("Pulse Interval", Range(0,60)) = 2
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			fixed filler;
		};

		uniform float4 _AlbedoColor;
		uniform float4 _EmissionColor;
		uniform float _EmissionMultiplier;
		uniform float _PulseInterval;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			o.Albedo = _AlbedoColor.rgb;
			o.Emission = ( lerp( _EmissionColor , _AlbedoColor , sin(_Time.w*1/_PulseInterval)) * _EmissionMultiplier ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
}