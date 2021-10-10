// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "ToonWater"
{
	Properties
	{
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_DepthDistance("Depth Distance", Float) = 0
		_FoamRange("Foam Range", Range( 0 , 1)) = 0
		_FoamColor("Foam Color", Color) = (0,0,0,0)
		_NoiseTiling("Noise Tiling", Float) = 0
		_NoiseScale("Noise Scale", Float) = 0
		_DynamicFoamRange("Dynamic Foam Range", Range( 0 , 1)) = 0
		_FoamSpeedX("Foam Speed X", Float) = 0
		_FoamSpeedY("Foam Speed Y", Float) = 0
		_Flowmap("Flowmap", 2D) = "white" {}
		_FlormapIntensity("Flormap Intensity", Range( 0 , 1)) = 0
		_FlowmapTiling("Flowmap Tiling", Float) = 0
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit alpha:fade keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
		};

		uniform float4 _WaterColor;
		uniform float4 _FoamColor;
		UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
		uniform float4 _CameraDepthTexture_TexelSize;
		uniform float _DepthDistance;
		uniform float _FoamRange;
		uniform float _FoamSpeedX;
		uniform float _FoamSpeedY;
		uniform float _NoiseTiling;
		uniform sampler2D _Flowmap;
		uniform float _FlowmapTiling;
		uniform float _FlormapIntensity;
		uniform float _NoiseScale;
		uniform float _DynamicFoamRange;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth2 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth2 = abs( ( screenDepth2 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float2 appendResult25 = (float2(_FoamSpeedX , _FoamSpeedY));
			float3 ase_worldPos = i.worldPos;
			float4 lerpResult27 = lerp( float4( (( ase_worldPos / _NoiseTiling )).xz, 0.0 , 0.0 ) , tex2D( _Flowmap, (( ase_worldPos / _FlowmapTiling )).xz ) , _FlormapIntensity);
			float2 panner21 = ( _Time.y * appendResult25 + lerpResult27.rg);
			float simplePerlin2D9 = snoise( panner21*_NoiseScale );
			simplePerlin2D9 = simplePerlin2D9*0.5 + 0.5;
			float4 lerpResult7 = lerp( _WaterColor , _FoamColor , saturate( ( step( saturate( distanceDepth2 ) , _FoamRange ) + step( simplePerlin2D9 , _DynamicFoamRange ) ) ));
			o.Emission = lerpResult7.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
0;498;1304;501;-88.91269;-128.1173;1;True;False
Node;AmplifyShaderEditor.CommentaryNode;18;-2077.198,612.0521;Inherit;False;1799.258;705.2662;World Position Noise (Dynamic Foam);20;33;32;31;13;11;10;15;16;9;14;21;27;22;25;23;24;30;26;12;34;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;31;-1992.581,976.2241;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;34;-1997.16,1124.667;Inherit;False;Property;_FlowmapTiling;Flowmap Tiling;11;0;Create;True;0;0;0;False;0;False;0;0.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldPosInputsNode;10;-1711.329,652.446;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;12;-1710.753,808.0522;Inherit;False;Property;_NoiseTiling;Noise Tiling;4;0;Create;True;0;0;0;False;0;False;0;2.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;32;-1786.005,994.8303;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;11;-1504.753,671.0522;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;33;-1642.289,985.2021;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;26;-1416.92,993.6714;Inherit;True;Property;_Flowmap;Flowmap;9;0;Create;True;0;0;0;False;0;False;-1;None;d69cca3f601cd894597711f5df765b95;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;24;-1433.927,822.8249;Inherit;False;Property;_FoamSpeedY;Foam Speed Y;8;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;17;-1168.511,325.4794;Inherit;False;877.7353;264.364;Depth Fade (Contact Foam);5;3;2;5;6;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-1432.327,757.0248;Inherit;False;Property;_FoamSpeedX;Foam Speed X;7;0;Create;True;0;0;0;False;0;False;0;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;13;-1361.037,661.424;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;30;-1357.086,1186.443;Inherit;False;Property;_FlormapIntensity;Flormap Intensity;10;0;Create;True;0;0;0;False;0;False;0;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;25;-1257.126,765.8249;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1118.511,412.8207;Inherit;False;Property;_DepthDistance;Depth Distance;1;0;Create;True;0;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;22;-1314.126,907.8249;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;27;-1058.283,983.1484;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.PannerNode;21;-1047.353,663.3951;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-958.3646,806.7792;Inherit;False;Property;_NoiseScale;Noise Scale;5;0;Create;True;0;0;0;False;0;False;0;3.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;2;-907.0766,375.4794;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-756.1146,473.8433;Inherit;False;Property;_FoamRange;Foam Range;2;0;Create;True;0;0;0;False;0;False;0;0.82;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;5;-602.6627,391.8326;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-763.8126,891.0895;Inherit;False;Property;_DynamicFoamRange;Dynamic Foam Range;6;0;Create;True;0;0;0;False;0;False;0;0.152;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;9;-767.5422,672.2943;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;15;-429.9397,664.9542;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;4;-442.7755,379.4244;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-164.4576,476.3937;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;8;-94.96234,-277.0275;Inherit;False;Property;_FoamColor;Foam Color;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;1;-98.42937,-448.6861;Inherit;False;Property;_WaterColor;Water Color;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.07130302,0.4537627,0.631,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;20;-20.49748,472.7947;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-340.5837,77.82721;Inherit;False;Property;_BlinkIntensity;Blink Intensity;13;0;Create;True;0;0;0;False;0;False;0;15.98;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;42;-305.8206,-37.41369;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;41;-458.9279,-47.62085;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;37;-655.7488,-66.13581;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-81.30037,-23.85251;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;7;502.3832,11.95193;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleTimeNode;39;-875.9632,-56.36982;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;75.50188,-46.34446;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;40;-1048.027,-56.36984;Inherit;False;Property;_BlinkSpeed;Blink Speed;12;0;Create;True;0;0;0;False;0;False;0;5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1120.381,-13.42142;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;ToonWater;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;32;0;31;0
WireConnection;32;1;34;0
WireConnection;11;0;10;0
WireConnection;11;1;12;0
WireConnection;33;0;32;0
WireConnection;26;1;33;0
WireConnection;13;0;11;0
WireConnection;25;0;23;0
WireConnection;25;1;24;0
WireConnection;27;0;13;0
WireConnection;27;1;26;0
WireConnection;27;2;30;0
WireConnection;21;0;27;0
WireConnection;21;2;25;0
WireConnection;21;1;22;0
WireConnection;2;0;3;0
WireConnection;5;0;2;0
WireConnection;9;0;21;0
WireConnection;9;1;14;0
WireConnection;15;0;9;0
WireConnection;15;1;16;0
WireConnection;4;0;5;0
WireConnection;4;1;6;0
WireConnection;19;0;4;0
WireConnection;19;1;15;0
WireConnection;20;0;19;0
WireConnection;42;0;41;0
WireConnection;41;0;37;0
WireConnection;37;0;39;0
WireConnection;44;0;42;0
WireConnection;44;1;46;0
WireConnection;7;0;1;0
WireConnection;7;1;8;0
WireConnection;7;2;20;0
WireConnection;39;0;40;0
WireConnection;43;0;8;0
WireConnection;43;1;44;0
WireConnection;0;2;7;0
ASEEND*/
//CHKSM=3CCCBA44EB67B8A99082278C58EA961BB0F2C414