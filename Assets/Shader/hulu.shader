// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "hulu"
{
	Properties
	{
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Float0("Float 0", Float) = 0
		_U("U", Float) = 0
		_V("V", Float) = 0
		_Float1("Float 1", Float) = 1
		_Float2("Float 2", Float) = 5
		_TextureSample2("Texture Sample 2", 2D) = "white" {}
		_RD_U("RD_U", Float) = 0
		_RD_V("RD_V", Float) = 0
		_Power("Power", Range( 0 , 1)) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Opaque" }
	LOD 100

		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend One One
		AlphaToMask Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		
		
		Pass
		{
			Name "Unlit"
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				float3 ase_normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform sampler2D _TextureSample0;
			uniform float4 _TextureSample0_ST;
			uniform float _Float0;
			uniform float _Float1;
			uniform float _Float2;
			uniform sampler2D _TextureSample1;
			uniform float _U;
			uniform float _V;
			uniform sampler2D _TextureSample2;
			uniform float _RD_U;
			uniform float _RD_V;
			uniform float _Power;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord2.w = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				float2 uv_TextureSample0 = i.ase_texcoord1.xy * _TextureSample0_ST.xy + _TextureSample0_ST.zw;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(WorldPosition);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 ase_worldNormal = i.ase_texcoord2.xyz;
				float fresnelNdotV2 = dot( ase_worldNormal, ase_worldViewDir );
				float fresnelNode2 = ( _Float0 + _Float1 * pow( 1.0 - fresnelNdotV2, _Float2 ) );
				float2 temp_cast_0 = (( _U * _V )).xx;
				float2 texCoord12 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 temp_cast_2 = (( _RD_U * _RD_V )).xx;
				float2 texCoord21 = i.ase_texcoord1.xy * float2( 1,1 ) + float2( 0,0 );
				float2 panner20 = ( 1.0 * _Time.y * temp_cast_2 + texCoord21);
				float4 lerpResult18 = lerp( float4( texCoord12, 0.0 , 0.0 ) , tex2D( _TextureSample2, panner20 ) , _Power);
				float2 panner10 = ( 1.0 * _Time.y * temp_cast_0 + lerpResult18.rg);
				float4 color16 = IsGammaSpace() ? float4(0.3301886,0.5546281,1,1) : float4(0.08908617,0.2681329,1,1);
				
				
				finalColor = ( ( ( tex2D( _TextureSample0, uv_TextureSample0 ) * fresnelNode2 ) * tex2D( _TextureSample1, panner10 ) ) * color16 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	
}
/*ASEBEGIN
Version=18912
145.3333;50.66667;1552.667;888.3334;1000.563;316.9077;1;True;True
Node;AmplifyShaderEditor.RangedFloatNode;22;-2866.156,417.1447;Inherit;False;Property;_RD_V;RD_V;9;0;Create;True;0;0;0;False;0;False;0;-0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;-2879.489,289.1447;Inherit;False;Property;_RD_U;RD_U;8;0;Create;True;0;0;0;False;0;False;0;0.7;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-2610.822,319.1448;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2728.306,142.265;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;20;-2424.049,256.2887;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;15;-1656.96,449.4566;Inherit;False;Property;_V;V;4;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;19;-2237.168,220.8397;Inherit;True;Property;_TextureSample2;Texture Sample 2;7;0;Create;True;0;0;0;False;0;False;-1;e59a7e6b125b28f4ab00a376f3df6688;4742e5fe56d6eba47b4d49d2b1189cf4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-1637.849,360.509;Inherit;False;Property;_U;U;3;0;Create;True;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;12;-2053.121,56.29951;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;27;-1909.548,349.5984;Inherit;False;Property;_Power;Power;10;0;Create;True;0;0;0;False;0;False;0;0.5;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;18;-1605.546,158.5058;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;6;-1610.516,-27.22019;Inherit;False;Property;_Float2;Float 2;6;0;Create;True;0;0;0;False;0;False;5;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1611.183,-135.8868;Inherit;False;Property;_Float1;Float 1;5;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1629.183,-235.2202;Inherit;False;Property;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;28;-1460.45,346.6264;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;2;-1314.294,-145.6674;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;10;-1322.342,159.5305;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;1;-1360.227,-401.334;Inherit;True;Property;_TextureSample0;Texture Sample 0;0;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;3;-969.3599,-263.534;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;9;-1082.214,123.3613;Inherit;True;Property;_TextureSample1;Texture Sample 1;1;0;Create;True;0;0;0;False;0;False;-1;e59a7e6b125b28f4ab00a376f3df6688;8d2f0c1048159784e9dae2f4928ae0f5;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-715.6165,329.6995;Inherit;False;Constant;_Color0;Color 0;2;0;Create;True;0;0;0;False;0;False;0.3301886,0.5546281,1,1;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-591.4309,-24.24583;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-239.1619,94.53185;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;0,0.6666641;Float;False;True;-1;2;ASEMaterialInspector;100;1;hulu;0770190933193b94aaa3065e307002fa;True;Unlit;0;0;Unlit;2;True;True;4;1;False;-1;1;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;True;0;False;-1;True;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=ForwardBase;False;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;1;True;False;;False;0
WireConnection;24;0;23;0
WireConnection;24;1;22;0
WireConnection;20;0;21;0
WireConnection;20;2;24;0
WireConnection;19;1;20;0
WireConnection;18;0;12;0
WireConnection;18;1;19;0
WireConnection;18;2;27;0
WireConnection;28;0;14;0
WireConnection;28;1;15;0
WireConnection;2;1;4;0
WireConnection;2;2;5;0
WireConnection;2;3;6;0
WireConnection;10;0;18;0
WireConnection;10;2;28;0
WireConnection;3;0;1;0
WireConnection;3;1;2;0
WireConnection;9;1;10;0
WireConnection;8;0;3;0
WireConnection;8;1;9;0
WireConnection;7;0;8;0
WireConnection;7;1;16;0
WireConnection;0;0;7;0
ASEEND*/
//CHKSM=8D821C8BA976E583068EDDE53696004ADAC4A0D5