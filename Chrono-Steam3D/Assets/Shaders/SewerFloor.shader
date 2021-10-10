// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SewerFloor"
{
	Properties
	{
		_WaterColor("Water Color", Color) = (0,0,0,0)
		_DepthDistance("Depth Distance", Float) = 0
		_FoamRange("Foam Range", Range( 0 , 1)) = 0
		_FoamColor("Foam Color", Color) = (0,0,0,0)
		_NoiseTiling("Noise Tiling", Float) = 0
		_NoiseScale("Noise Scale", Float) = 1
		_DynamicFoamRange("Dynamic Foam Range", Range( 0 , 1)) = 0.6470588
		_FoamSpeedX("Foam Speed X", Float) = 0
		_FoamSpeedY("Foam Speed Y", Float) = 0
		_Flowmap("Flowmap", 2D) = "white" {}
		_FlormapIntensity("Flormap Intensity", Range( 0 , 1)) = 0.6012609
		_FlowmapTiling("Flowmap Tiling", Float) = 0
		_FloorTexture("Floor Texture", 2D) = "white" {}
		_AlphaFloorTexture("Alpha Floor Texture", 2D) = "white" {}
		[Header(Bricks Tiling)]_Texture_Tiling_X("Texture_Tiling_X", Float) = 1
		[Header(Alpha Tiling)]_Alpha_Tiling_X("Alpha_Tiling_X", Float) = 1
		_Texture_Tiling_Y("Texture_Tiling_Y", Float) = 1
		_Alpha_Tiling_Y("Alpha_Tiling_Y", Float) = 1
		_FloorColor("Floor Color", Color) = (0.0276344,0.254717,0.09352272,0)
		[Header(Bricks Offset)]_Texture_Offset_X("Texture_Offset_X", Float) = 1
		_Texture_Offset_Y("Texture_Offset_Y", Float) = 1
		_Alpha_Offset_Y("Alpha_Offset_Y", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float4 screenPos;
			float3 worldPos;
			float2 uv_texcoord;
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
		uniform sampler2D _FloorTexture;
		uniform float _Texture_Tiling_X;
		uniform float _Texture_Tiling_Y;
		uniform float _Texture_Offset_X;
		uniform float _Texture_Offset_Y;
		uniform float4 _FloorColor;
		uniform sampler2D _AlphaFloorTexture;
		uniform float _Alpha_Tiling_X;
		uniform float _Alpha_Tiling_Y;
		uniform float _Alpha_Offset_Y;


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


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			float screenDepth21 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
			float distanceDepth21 = abs( ( screenDepth21 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthDistance ) );
			float Depth_Fade_Contact_Foam57 = step( saturate( distanceDepth21 ) , _FoamRange );
			float2 appendResult15 = (float2(_FoamSpeedX , _FoamSpeedY));
			float3 ase_worldPos = i.worldPos;
			float4 lerpResult18 = lerp( float4( (( ase_worldPos / _NoiseTiling )).xz, 0.0 , 0.0 ) , tex2D( _Flowmap, (( ase_worldPos / _FlowmapTiling )).xz ) , _FlormapIntensity);
			float2 panner19 = ( _Time.y * appendResult15 + lerpResult18.rg);
			float simplePerlin2D25 = snoise( panner19*_NoiseScale );
			simplePerlin2D25 = simplePerlin2D25*0.5 + 0.5;
			float World_Position_Noise_Dynamic_Foam58 = step( simplePerlin2D25 , _DynamicFoamRange );
			float4 lerpResult37 = lerp( _WaterColor , _FoamColor , saturate( ( Depth_Fade_Contact_Foam57 + World_Position_Noise_Dynamic_Foam58 ) ));
			float4 Water_And_Foam_Addition_And_Lerp62 = lerpResult37;
			float4 appendResult71 = (float4(_Texture_Tiling_X , _Texture_Tiling_Y , 0.0 , 0.0));
			float4 appendResult72 = (float4(_Texture_Offset_X , _Texture_Offset_Y , 0.0 , 0.0));
			float2 uv_TexCoord68 = i.uv_texcoord * appendResult71.xy + appendResult72.xy;
			float4 Bricks66 = ( tex2D( _FloorTexture, uv_TexCoord68 ) * _FloorColor );
			float4 appendResult79 = (float4(_Alpha_Tiling_X , _Alpha_Tiling_Y , 0.0 , 0.0));
			float4 appendResult80 = (float4(_Alpha_Offset_Y , _Alpha_Offset_Y , 0.0 , 0.0));
			float2 uv_TexCoord81 = i.uv_texcoord * appendResult79.xy + appendResult80.xy;
			float4 tex2DNode64 = tex2D( _AlphaFloorTexture, uv_TexCoord81 );
			float4 Bricks_Lerp_Alpha83 = tex2DNode64;
			float4 lerpResult42 = lerp( Water_And_Foam_Addition_And_Lerp62 , Bricks66 , Bricks_Lerp_Alpha83);
			o.Albedo = lerpResult42.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
252;573.6;1226;562;5988.312;-1942.772;3.09655;True;False
Node;AmplifyShaderEditor.CommentaryNode;1;-4690.735,-207.2283;Inherit;False;2131.415;688.7662;World Position Noise (Dynamic Foam);21;58;26;24;25;19;20;18;17;15;12;14;9;10;13;8;7;4;5;6;3;2;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WorldPosInputsNode;2;-4606.118,156.943;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;3;-4610.697,305.386;Inherit;False;Property;_FlowmapTiling;Flowmap Tiling;11;0;Create;True;0;0;0;False;0;False;0;0.73;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-4324.29,-11.22802;Inherit;False;Property;_NoiseTiling;Noise Tiling;4;0;Create;True;0;0;0;False;0;False;0;2.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-4399.542,175.5501;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WorldPosInputsNode;4;-4324.866,-166.8345;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.ComponentMaskNode;8;-4255.826,165.922;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;7;-4118.29,-148.2283;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-4047.464,3.543956;Inherit;False;Property;_FoamSpeedY;Foam Speed Y;8;0;Create;True;0;0;0;False;0;False;0;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;12;-4045.864,-62.25567;Inherit;False;Property;_FoamSpeedX;Foam Speed X;7;0;Create;True;0;0;0;False;0;False;0;0.12;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;9;-4030.457,174.391;Inherit;True;Property;_Flowmap;Flowmap;9;0;Create;True;0;0;0;False;0;False;-1;be60e3b9eac9c4049ac715d15c64f090;be60e3b9eac9c4049ac715d15c64f090;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;13;-3974.573,-157.8564;Inherit;False;True;False;True;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;14;-3984.623,382.162;Inherit;False;Property;_FlormapIntensity;Flormap Intensity;10;0;Create;True;0;0;0;False;0;False;0.6012609;0.1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;11;-4676.212,-521.939;Inherit;False;1114.113;259.8582;Depth Fade (Contact Foam);6;57;27;23;22;21;16;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;16;-4626.212,-434.5978;Inherit;False;Property;_DepthDistance;Depth Distance;1;0;Create;True;0;0;0;False;0;False;0;0.2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;-3870.663,-53.45562;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;17;-3927.663,88.5439;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;-3671.82,163.8681;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-3571.902,-12.50097;Inherit;False;Property;_NoiseScale;Noise Scale;5;0;Create;True;0;0;0;False;0;False;1;3.95;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DepthFade;21;-4414.778,-471.939;Inherit;False;True;False;True;2;1;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;19;-3660.89,-155.8854;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.NoiseGeneratorNode;25;-3381.079,-146.9861;Inherit;True;Simplex2D;True;False;2;0;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;23;-4110.364,-455.5859;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;24;-3377.35,71.80891;Inherit;False;Property;_DynamicFoamRange;Dynamic Foam Range;6;0;Create;True;0;0;0;False;0;False;0.6470588;0.152;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-4263.816,-373.5751;Inherit;False;Property;_FoamRange;Foam Range;2;0;Create;True;0;0;0;False;0;False;0;0.82;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;26;-3043.477,-154.3262;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;27;-3950.476,-467.9941;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;65;-4710.114,1505.635;Inherit;False;1696.168;535.6096;;11;68;72;71;69;73;74;70;66;51;41;52;Bricks;1,1,1,1;0;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;57;-3813.358,-472.9417;Inherit;False;Depth_Fade_Contact_Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;58;-2893.75,-152.6197;Inherit;False;World_Position_Noise_Dynamic_Foam;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;61;-4697.918,617.6353;Inherit;False;1601.196;759.0765;;8;62;60;59;31;28;29;30;37;Water and Foam Addition and Lerp;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;84;-4691.825,2146.43;Inherit;False;1414.875;465.425;;9;75;76;77;78;79;80;81;64;83;Bricks Alpha;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;70;-4692.227,1673.702;Inherit;False;Property;_Texture_Tiling_Y;Texture_Tiling_Y;16;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-4691.227,1590.702;Inherit;False;Property;_Texture_Tiling_X;Texture_Tiling_X;14;1;[Header];Create;True;1;Bricks Tiling;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-4687.746,1860.528;Inherit;False;Property;_Texture_Offset_Y;Texture_Offset_Y;21;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-4686.746,1777.528;Inherit;False;Property;_Texture_Offset_X;Texture_Offset_X;19;1;[Header];Create;True;1;Bricks Offset;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;78;-4640.825,2196.43;Inherit;False;Property;_Alpha_Tiling_X;Alpha_Tiling_X;15;1;[Header];Create;True;1;Alpha Tiling;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;76;-4637.344,2474.255;Inherit;False;Property;_Alpha_Offset_Y;Alpha_Offset_Y;22;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;72;-4439.746,1825.528;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;71;-4444.227,1638.702;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;59;-4622.817,1124.758;Inherit;False;57;Depth_Fade_Contact_Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;60;-4647.918,1247.925;Inherit;False;58;World_Position_Noise_Dynamic_Foam;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;75;-4641.825,2279.43;Inherit;False;Property;_Alpha_Tiling_Y;Alpha_Tiling_Y;17;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-4317.797,1174.74;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;68;-4260.227,1625.702;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;80;-4389.345,2431.255;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;79;-4393.825,2244.43;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ColorNode;30;-4185.607,667.6353;Inherit;False;Property;_WaterColor;Water Color;0;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.07130302,0.4537627,0.631,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;52;-3890.086,1747.373;Inherit;False;Property;_FloorColor;Floor Color;18;0;Create;True;0;0;0;False;0;False;0.0276344,0.254717,0.09352272,0;0.0276344,0.254717,0.09352272,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;81;-4209.824,2231.43;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;29;-4180.448,888.2476;Inherit;False;Property;_FoamColor;Foam Color;3;0;Create;True;0;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;31;-4124.704,1164.733;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;41;-4012.652,1549.379;Inherit;True;Property;_FloorTexture;Floor Texture;12;0;Create;True;0;0;0;False;0;False;-1;e55f75852a3d3994fa7ae36510f2a839;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-3626.085,1724.173;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;64;-3851.087,2250.679;Inherit;True;Property;_AlphaFloorTexture;Alpha Floor Texture;13;0;Create;True;0;0;0;False;0;False;-1;e55f75852a3d3994fa7ae36510f2a839;e55f75852a3d3994fa7ae36510f2a839;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;37;-3626.788,920.5393;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;83;-3520.951,2277.174;Inherit;False;Bricks_Lerp_Alpha;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;66;-3231.599,1831.228;Inherit;False;Bricks;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;62;-3401.49,981.0496;Inherit;False;Water_And_Foam_Addition_And_Lerp;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;82;-709.6514,114.6399;Inherit;False;83;Bricks_Lerp_Alpha;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;63;-795.0345,-64.55292;Inherit;False;62;Water_And_Foam_Addition_And_Lerp;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;67;-653.2037,22.97078;Inherit;False;66;Bricks;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;77;-4636.344,2383.255;Inherit;False;Property;_Alpha_Offset_X;Alpha_Offset_X;20;1;[Header];Create;True;1;Alpha Offset;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;42;-404.464,-3.374106;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;SewerFloor;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;6;0;2;0
WireConnection;6;1;3;0
WireConnection;8;0;6;0
WireConnection;7;0;4;0
WireConnection;7;1;5;0
WireConnection;9;1;8;0
WireConnection;13;0;7;0
WireConnection;15;0;12;0
WireConnection;15;1;10;0
WireConnection;18;0;13;0
WireConnection;18;1;9;0
WireConnection;18;2;14;0
WireConnection;21;0;16;0
WireConnection;19;0;18;0
WireConnection;19;2;15;0
WireConnection;19;1;17;0
WireConnection;25;0;19;0
WireConnection;25;1;20;0
WireConnection;23;0;21;0
WireConnection;26;0;25;0
WireConnection;26;1;24;0
WireConnection;27;0;23;0
WireConnection;27;1;22;0
WireConnection;57;0;27;0
WireConnection;58;0;26;0
WireConnection;72;0;73;0
WireConnection;72;1;74;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;28;0;59;0
WireConnection;28;1;60;0
WireConnection;68;0;71;0
WireConnection;68;1;72;0
WireConnection;80;0;76;0
WireConnection;80;1;76;0
WireConnection;79;0;78;0
WireConnection;79;1;75;0
WireConnection;81;0;79;0
WireConnection;81;1;80;0
WireConnection;31;0;28;0
WireConnection;41;1;68;0
WireConnection;51;0;41;0
WireConnection;51;1;52;0
WireConnection;64;1;81;0
WireConnection;37;0;30;0
WireConnection;37;1;29;0
WireConnection;37;2;31;0
WireConnection;83;0;64;0
WireConnection;66;0;51;0
WireConnection;62;0;37;0
WireConnection;42;0;63;0
WireConnection;42;1;67;0
WireConnection;42;2;82;0
WireConnection;0;0;42;0
ASEEND*/
//CHKSM=2C51D337340186D8C541C878070CFAAD759A5804