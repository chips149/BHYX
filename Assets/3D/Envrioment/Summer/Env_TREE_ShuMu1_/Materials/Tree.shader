// Made with Amplify Shader Editor v1.9.9.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Tree"
{
	Properties
	{
		_TextureSample0( "Texture Sample 0", 2D ) = "white" {}
		_Texture0( "Texture 0", 2D ) = "white" {}
		_Normal( "Normal", Float ) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+0" }
		Cull Back
		CGPROGRAM
		#pragma target 3.5
		#define ASE_VERSION 19903
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		float4 _Texture0_TexelSize;
		uniform float _Normal;
		uniform sampler2D _TextureSample0;
		uniform float4 _TextureSample0_ST;


		float3 CombineSamplesSharp128_g1( float S0, float S1, float S2, float Strength )
		{
			{
			    float3 va = float3( 0.13, 0, ( S1 - S0 ) * Strength );
			    float3 vb = float3( 0, 0.13, ( S2 - S0 ) * Strength );
			    return normalize( cross( va, vb ) );
			}
		}


		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float localCalculateUVsSharp110_g1 = ( 0.0 );
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float2 temp_output_85_0_g1 = uv_Texture0;
			float2 UV110_g1 = temp_output_85_0_g1;
			float4 TexelSize110_g1 = _Texture0_TexelSize;
			float2 UV0110_g1 = float2( 0,0 );
			float2 UV1110_g1 = float2( 0,0 );
			float2 UV2110_g1 = float2( 0,0 );
			{
			{
			    UV110_g1.y -= TexelSize110_g1.y * 0.5;
			    UV0110_g1 = UV110_g1;
			    UV1110_g1 = UV110_g1 + float2( TexelSize110_g1.x, 0 );
			    UV2110_g1 = UV110_g1 + float2( 0, TexelSize110_g1.y );
			}
			}
			float4 break134_g1 = tex2D( _Texture0, UV0110_g1 );
			float S0128_g1 = break134_g1.r;
			float4 break136_g1 = tex2D( _Texture0, UV1110_g1 );
			float S1128_g1 = break136_g1.r;
			float4 break138_g1 = tex2D( _Texture0, UV2110_g1 );
			float S2128_g1 = break138_g1.r;
			float temp_output_91_0_g1 = _Normal;
			float Strength128_g1 = temp_output_91_0_g1;
			float3 localCombineSamplesSharp128_g1 = CombineSamplesSharp128_g1( S0128_g1 , S1128_g1 , S2128_g1 , Strength128_g1 );
			o.Normal = localCombineSamplesSharp128_g1;
			float2 uv_TextureSample0 = i.uv_texcoord * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
			o.Albedo = tex2D( _TextureSample0, uv_TextureSample0 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19903
Node;AmplifyShaderEditor.TexturePropertyNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;4;-800,112;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;0;False;0;False;None;b82539cbf72ade44995198919ef11b1e;False;white;Auto;Texture2D;False;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;5;-506.4007,344.5;Inherit;False;Property;_Normal;Normal;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;2;-384,144;Inherit;False;Normal From Texture;-1;;1;9728ee98a55193249b513caf9a0f1676;13,149,0,147,0,143,0,141,0,139,0,151,0,137,0,153,0,159,0,157,0,155,0,135,0,108,0;4;87;SAMPLER2D;0;False;85;FLOAT2;0,0;False;74;SAMPLERSTATE;0;False;91;FLOAT;1.5;False;2;FLOAT3;40;FLOAT3;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;1;-864,-112;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;b82539cbf72ade44995198919ef11b1e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.StandardSurfaceOutputNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;0;0,0;Float;False;True;-1;3;AmplifyShaderEditor.MaterialInspector;0;0;Standard;Tree;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;;0;False;;False;0;False;;0;False;;False;0;Opaque;0.5;True;True;0;False;Opaque;;Geometry;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;2;87;4;0
WireConnection;2;91;5;0
WireConnection;0;0;1;0
WireConnection;0;1;2;40
ASEEND*/
//CHKSM=C89660D3D23363400CFDEDEB9974CA2E7ABE7365