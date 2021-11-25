// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Fresnel"
{
	Properties
	{
		_FresnelScale("Fresnel Scale", Range( 0 , 2)) = 0.3
		_FresnelPow("Fresnel Pow", Range( 0 , 30)) = 4.352941
		[HDR]_FresnelColor("Fresnel Color", Color) = (0,8,7.780392,0)
		_BaseTextureColorModifier("Base Texture Color Modifier", Color) = (0,0.7168778,0.6971988,0)
		[Header(Texture Properties)][SingleLineTexture]_Material_Base_Texture("Material_Base_Texture", 2D) = "white" {}
		[Header(Texture Properties)][SingleLineTexture]_Material_Metallic_Texture("Material_Metallic_Texture", 2D) = "white" {}
		[SingleLineTexture]_Material_Base_Normal("Material_Base_Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
			INTERNAL_DATA
		};

		uniform sampler2D _Material_Base_Normal;
		uniform float4 _Material_Base_Normal_ST;
		uniform float4 _BaseTextureColorModifier;
		uniform sampler2D _Material_Base_Texture;
		uniform float4 _Material_Base_Texture_ST;
		uniform float4 _FresnelColor;
		uniform float _FresnelScale;
		uniform float _FresnelPow;
		uniform sampler2D _Material_Metallic_Texture;
		uniform float4 _Material_Metallic_Texture_ST;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Material_Base_Normal = i.uv_texcoord * _Material_Base_Normal_ST.xy + _Material_Base_Normal_ST.zw;
			o.Normal = UnpackNormal( tex2D( _Material_Base_Normal, uv_Material_Base_Normal ) );
			float2 uv_Material_Base_Texture = i.uv_texcoord * _Material_Base_Texture_ST.xy + _Material_Base_Texture_ST.zw;
			o.Albedo = saturate( ( _BaseTextureColorModifier * tex2D( _Material_Base_Texture, uv_Material_Base_Texture ) ) ).rgb;
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = WorldNormalVector( i, float3( 0, 0, 1 ) );
			float3 ase_normWorldNormal = normalize( ase_worldNormal );
			float fresnelNdotV1 = dot( ase_normWorldNormal, ase_worldViewDir );
			float fresnelNode1 = ( 0.0 + _FresnelScale * pow( max( 1.0 - fresnelNdotV1 , 0.0001 ), _FresnelPow ) );
			o.Emission = ( _FresnelColor * saturate( fresnelNode1 ) ).rgb;
			float2 uv_Material_Metallic_Texture = i.uv_texcoord * _Material_Metallic_Texture_ST.xy + _Material_Metallic_Texture_ST.zw;
			o.Metallic = tex2D( _Material_Metallic_Texture, uv_Material_Metallic_Texture ).r;
			o.Alpha = 1;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float4 tSpace0 : TEXCOORD2;
				float4 tSpace1 : TEXCOORD3;
				float4 tSpace2 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18900
333;1;1015;528;1424.738;573.9268;2.082191;True;False
Node;AmplifyShaderEditor.RangedFloatNode;3;-1106.121,307.005;Inherit;False;Property;_FresnelPow;Fresnel Pow;2;0;Create;True;0;0;0;False;0;False;4.352941;3.3;0;30;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;2;-1100.178,200.2553;Inherit;False;Property;_FresnelScale;Fresnel Scale;1;0;Create;True;0;0;0;False;0;False;0.3;2;0;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;1;-785.8652,215.7135;Inherit;False;Standard;WorldNormal;ViewDir;True;True;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;12;-661.0273,-606.3959;Inherit;False;Property;_BaseTextureColorModifier;Base Texture Color Modifier;4;0;Create;True;0;0;0;False;0;False;0,0.7168778,0.6971988,0;0.2358491,0.1902368,0.1902368,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;11;-693.8256,-419.9517;Inherit;True;Property;_Material_Base_Texture;Material_Base_Texture;5;2;[Header];[SingleLineTexture];Create;True;1;Texture Properties;0;0;False;0;False;-1;None;681b718a2c8d19e4994b8eb597911ebb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;4;-654.9197,-53.69984;Inherit;False;Property;_FresnelColor;Fresnel Color;3;1;[HDR];Create;True;0;0;0;False;0;False;0,8,7.780392,0;0,5.992157,1.631373,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;10;-536.5469,142.5624;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-359.0283,-479.3952;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-398.4482,26.51307;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;14;-200.2287,-318.9954;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;15;-436.1685,-222.892;Inherit;True;Property;_Material_Base_Normal;Material_Base_Normal;7;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;-1;None;a67347ae533cc284c81b20e39b6bdbaf;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-504.4093,248.5388;Inherit;True;Property;_Material_Metallic_Texture;Material_Metallic_Texture;6;2;[Header];[SingleLineTexture];Create;True;1;Texture Properties;0;0;False;0;False;16;None;681b718a2c8d19e4994b8eb597911ebb;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Fresnel;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;0;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;2;2;0
WireConnection;1;3;3;0
WireConnection;10;0;1;0
WireConnection;13;0;12;0
WireConnection;13;1;11;0
WireConnection;6;0;4;0
WireConnection;6;1;10;0
WireConnection;14;0;13;0
WireConnection;0;0;14;0
WireConnection;0;1;15;0
WireConnection;0;2;6;0
WireConnection;0;3;16;0
ASEEND*/
//CHKSM=1599E4DB820566C8013D29FF58C1B9F168A7DA66