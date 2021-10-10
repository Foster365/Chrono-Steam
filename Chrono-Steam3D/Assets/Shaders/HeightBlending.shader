// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "HeightBlending"
{
	Properties
	{
		_EdgeLength ( "Edge length", Range( 2, 50 ) ) = 2
		[Header(Heightmap)][SingleLineTexture]_Heightmap("Heightmap", 2D) = "white" {}
		[IntRange]_HeightmapTiling("Heightmap Tiling", Range( 1 , 5)) = 1
		_HeightmapColor("Heightmap Color", Color) = (0.6509434,0.1814199,0.09518512,0)
		_Mud("Mud", 2D) = "white" {}
		[Header()]_HeightmapOffsetX("Heightmap Offset X", Float) = 0
		[Header()]_HeightmapOffsetY("Heightmap Offset Y", Float) = 0
		_MudHeight("Mud Height", Float) = 0
		_HeightmapVertexOffset("Heightmap Vertex Offset", Float) = 0
		[Header(Blending)]_BlendingGradient("Blending Gradient", Float) = 0
		_BlendingHardness("Blending Hardness", Float) = 3.74
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#include "Tessellation.cginc"
		#pragma target 4.6
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows vertex:vertexDataFunc tessellate:tessFunction 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform sampler2D _Heightmap;
		uniform float _HeightmapTiling;
		uniform float _HeightmapOffsetX;
		uniform float _HeightmapOffsetY;
		uniform float _BlendingHardness;
		uniform float _BlendingGradient;
		uniform float _MudHeight;
		uniform float _HeightmapVertexOffset;
		uniform float4 _HeightmapColor;
		uniform sampler2D _Mud;
		uniform float4 _Mud_ST;
		uniform float _EdgeLength;

		float4 tessFunction( appdata_full v0, appdata_full v1, appdata_full v2 )
		{
			return UnityEdgeLengthBasedTess (v0.vertex, v1.vertex, v2.vertex, _EdgeLength);
		}

		void vertexDataFunc( inout appdata_full v )
		{
			float2 temp_cast_0 = (_HeightmapTiling).xx;
			float2 appendResult11 = (float2(_HeightmapOffsetX , _HeightmapOffsetY));
			float2 uv_TexCoord3 = v.texcoord.xy * temp_cast_0 + appendResult11;
			float4 tex2DNode1 = tex2Dlod( _Heightmap, float4( uv_TexCoord3, 0, 0.0) );
			float MudMask46 = saturate( ( saturate( pow( ( ( 1.0 - tex2DNode1.r ) * _BlendingHardness ) , _BlendingGradient ) ) + v.color.r ) );
			v.vertex.xyz += ( ( tex2DNode1.r + saturate( ( MudMask46 - _MudHeight ) ) ) * float3(0,1,0) * _HeightmapVertexOffset );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 temp_cast_0 = (_HeightmapTiling).xx;
			float2 appendResult11 = (float2(_HeightmapOffsetX , _HeightmapOffsetY));
			float2 uv_TexCoord3 = i.uv_texcoord * temp_cast_0 + appendResult11;
			float4 tex2DNode1 = tex2D( _Heightmap, uv_TexCoord3 );
			float2 uv_Mud = i.uv_texcoord * _Mud_ST.xy + _Mud_ST.zw;
			float MudMask46 = saturate( ( saturate( pow( ( ( 1.0 - tex2DNode1.r ) * _BlendingHardness ) , _BlendingGradient ) ) + i.vertexColor.r ) );
			float4 lerpResult43 = lerp( ( _HeightmapColor * tex2DNode1.r ) , tex2D( _Mud, uv_Mud ) , MudMask46);
			o.Albedo = lerpResult43.rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
264;443.2;1059;360;1033.105;-364.0221;2.455762;True;False
Node;AmplifyShaderEditor.CommentaryNode;16;-1109.43,-430.5093;Inherit;False;1300.001;766.8289;;6;8;9;4;11;3;1;Bricks;0.9528302,0.6188895,0.4359648,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;8;-1059.43,-113.4775;Inherit;False;Property;_HeightmapOffsetX;Heightmap Offset X;9;1;[Header];Create;True;1;;0;0;False;0;False;0;0.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;9;-1058.43,-39.47746;Inherit;False;Property;_HeightmapOffsetY;Heightmap Offset Y;10;1;[Header];Create;True;1;;0;0;False;0;False;0;1.53;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;11;-823.4303,-87.47746;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1028.031,-211.7697;Inherit;False;Property;_HeightmapTiling;Heightmap Tiling;6;1;[IntRange];Create;True;0;0;0;False;0;False;1;2;1;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;3;-677.1318,-176.5235;Inherit;True;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-420.2844,-117.8485;Inherit;True;Property;_Heightmap;Heightmap;5;2;[Header];[SingleLineTexture];Create;True;1;Heightmap;0;0;False;0;False;-1;e55f75852a3d3994fa7ae36510f2a839;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;39;115.1081,571.9976;Inherit;False;1397.662;502.1641;;10;46;42;41;40;29;27;28;32;33;20;Mud Mask;0.4234823,0.9056604,0.4058384,1;0;0
Node;AmplifyShaderEditor.OneMinusNode;20;216.2056,622.5365;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;33;165.108,724.492;Inherit;False;Property;_BlendingHardness;Blending Hardness;14;0;Create;True;0;0;0;False;0;False;3.74;1.15;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;28;353.3105,772.3651;Inherit;False;Property;_BlendingGradient;Blending Gradient;13;1;[Header];Create;True;1;Blending;0;0;False;0;False;0;2.48;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;32;377.3485,621.9976;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;27;526.3486,640.9976;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;4.34;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;29;675.5269,638.7436;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;40;682.3119,880.2471;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;41;920.0891,757.7932;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;42;1065.138,753.9738;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;46;1286.994,753.6365;Inherit;False;MudMask;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;50;817.1097,67.32089;Inherit;False;1049.239;478.8267;;9;13;15;14;35;36;37;48;49;38;Vertex Offset;0.5309274,0.8317378,0.9150943,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;37;867.6282,238.2491;Inherit;False;Property;_MudHeight;Mud Height;11;0;Create;True;0;0;0;False;0;False;0;0.79;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;48;867.1097,151.9239;Inherit;False;46;MudMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;36;1087.292,188.4337;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CommentaryNode;51;1050.411,-593.0263;Inherit;False;822.8079;518.2717;;5;17;45;6;43;47;Color Blending;0.8715186,0.4493592,0.9622642,1;0;0
Node;AmplifyShaderEditor.ColorNode;6;1100.411,-543.0263;Inherit;False;Property;_HeightmapColor;Heightmap Color;7;0;Create;True;0;0;0;False;0;False;0.6509434,0.1814199,0.09518512,0;0.9528302,0.2047404,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;49;1355.457,117.3209;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;38;1285.709,185.1503;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;14;1422.701,275.5722;Inherit;False;Constant;_Vector0;Vector 0;5;0;Create;True;0;0;0;False;0;False;0,1,0;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SimpleAddOpNode;35;1436.342,152.9167;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;17;1131.548,-304.7546;Inherit;True;Property;_Mud;Mud;8;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;45;1366.201,-433.1249;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;15;1389.48,430.1476;Inherit;False;Property;_HeightmapVertexOffset;Heightmap Vertex Offset;12;0;Create;True;0;0;0;False;0;False;0;0.57;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;47;1425.838,-223.7609;Inherit;False;46;MudMask;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;1704.348,269.0348;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;43;1608.219,-333.143;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2045.844,3.633239;Float;False;True;-1;6;ASEMaterialInspector;0;0;Standard;HeightBlending;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;True;2;2;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;0;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;11;0;8;0
WireConnection;11;1;9;0
WireConnection;3;0;4;0
WireConnection;3;1;11;0
WireConnection;1;1;3;0
WireConnection;20;0;1;1
WireConnection;32;0;20;0
WireConnection;32;1;33;0
WireConnection;27;0;32;0
WireConnection;27;1;28;0
WireConnection;29;0;27;0
WireConnection;41;0;29;0
WireConnection;41;1;40;1
WireConnection;42;0;41;0
WireConnection;46;0;42;0
WireConnection;36;0;48;0
WireConnection;36;1;37;0
WireConnection;49;0;1;1
WireConnection;38;0;36;0
WireConnection;35;0;49;0
WireConnection;35;1;38;0
WireConnection;45;0;6;0
WireConnection;45;1;1;1
WireConnection;13;0;35;0
WireConnection;13;1;14;0
WireConnection;13;2;15;0
WireConnection;43;0;45;0
WireConnection;43;1;17;0
WireConnection;43;2;47;0
WireConnection;0;0;43;0
WireConnection;0;11;13;0
ASEEND*/
//CHKSM=57AF8D91F5E4648B6A5C1EB30A5C469F0D0345DE