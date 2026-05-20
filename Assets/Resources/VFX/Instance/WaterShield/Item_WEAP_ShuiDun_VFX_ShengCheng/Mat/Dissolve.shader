// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Xia/Dissolve"
{
	Properties
	{
		[HDR]_Color0("color", Color) = (1,1,1,1)
		_Float3("Intensity", Float) = 0
		_Float5("Opacity", Float) = 0
		_Float2("Intensity_power", Float) = 0
		_Float4("Opacity_power", Float) = 0
		Main_Tex("主贴图", 2D) = "white" {}
		_Float0("Main_U", Float) = 0
		_Float1("Main_V", Float) = 0
		Gradient("Gradient_Tex", 2D) = "white" {}
		[Toggle(_KEYWORD0_ON)] _Keyword0("polar开关", Float) = 0
		_Float6("Gradient_U", Float) = 0
		_Float7("Gradient_V", Float) = 0
		_Float8("极坐标中心X", Float) = 0
		_Float9("极坐标中心Y", Float) = 0
		_Float10("极坐标X重铺", Float) = 0
		_Float11("极坐标Y重铺", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _KEYWORD0_ON
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
		};

		uniform half4 _Color0;
		uniform sampler2D Main_Tex;
		uniform float4 Main_Tex_ST;
		uniform half _Float0;
		uniform half _Float1;
		uniform sampler2D Gradient;
		uniform half _Float6;
		uniform half _Float7;
		uniform half _Float8;
		uniform half _Float9;
		uniform half _Float10;
		uniform half _Float11;
		uniform half _Float2;
		uniform half _Float3;
		uniform half _Float4;
		uniform half _Float5;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uvMain_Tex = i.uv_texcoord * Main_Tex_ST.xy + Main_Tex_ST.zw;
			half2 appendResult3 = (half2(_Float0 , _Float1));
			half4 tex2DNode1 = tex2D( Main_Tex, ( uvMain_Tex + ( appendResult3 * _Time.y ) ) );
			half2 appendResult41 = (half2(_Float6 , _Float7));
			half2 appendResult45 = (half2(_Float8 , _Float9));
			half2 CenteredUV15_g1 = ( i.uv_texcoord - appendResult45 );
			half2 break17_g1 = CenteredUV15_g1;
			half2 appendResult23_g1 = (half2(( length( CenteredUV15_g1 ) * _Float10 * 2.0 ) , ( atan2( break17_g1.x , break17_g1.y ) * ( 1.0 / 6.28318548202515 ) * _Float11 )));
			#ifdef _KEYWORD0_ON
				half2 staticSwitch50 = appendResult23_g1;
			#else
				half2 staticSwitch50 = i.uv_texcoord;
			#endif
			half2 panner40 = ( 1.0 * _Time.y * appendResult41 + staticSwitch50);
			half3 temp_cast_0 = (_Float2).xxx;
			o.Emission = ( pow( ( (i.vertexColor).rgb * (_Color0).rgb * (tex2DNode1).rgb * (tex2D( Gradient, panner40 )).rgb ) , temp_cast_0 ) * _Float3 );
			o.Alpha = ( pow( ( _Color0.a * i.vertexColor.a * tex2DNode1.a ) , _Float4 ) * _Float5 );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Lambert keepalpha fullforwardshadows noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd 

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
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				half4 color : COLOR0;
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
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18912
7;410;1163;601;3399.217;116.9477;3.281474;True;False
Node;AmplifyShaderEditor.CommentaryNode;26;-1453.03,-155.0524;Inherit;False;699.2326;425.1592;;7;5;4;8;3;6;2;9;UV组件;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1766.408,-643.5799;Half;False;Property;_Float8;极坐标中心X;12;0;Create;False;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1762.408,-554.5801;Half;False;Property;_Float9;极坐标中心Y;13;0;Create;False;0;0;0;False;0;False;0;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-1403.03,54.15643;Half;False;Property;_Float0;Main_U;6;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;49;-1294.449,-446.7846;Half;False;Property;_Float11;极坐标Y重铺;15;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-1398.639,144.1085;Half;False;Property;_Float1;Main_V;7;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1289.449,-527.7846;Half;False;Property;_Float10;极坐标X重铺;14;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;45;-1535.793,-625.9449;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1484.798,-739.7465;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;3;-1258.821,45.63403;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;8;-1242.386,159.1068;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;43;-881.8368,-372.4095;Half;False;Property;_Float7;Gradient_V;11;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-883.8368,-453.4095;Half;False;Property;_Float6;Gradient_U;10;0;Create;False;0;0;0;False;0;False;0;-0.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;44;-1078.263,-657.9224;Inherit;False;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-1099.487,22.81011;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.StaticSwitch;50;-966.0947,-872.3372;Inherit;False;Property;_Keyword0;polar开关;9;0;Create;False;0;0;0;False;0;False;0;0;1;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;-714.8368,-450.4095;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-1216.605,-105.0524;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;9;-905.7978,-12.05688;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;40;-577.8368,-582.4095;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;17;-333.8069,-174.6886;Half;False;Property;_Color0;color;0;1;[HDR];Create;False;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;-754.8089,-1.883759;Inherit;True;Property;Main_Tex;主贴图;5;0;Create;False;0;0;0;False;0;False;-1;c0e0a864124a31642aabe2aa94c5868e;21c3e84b89a6ea44cbbff8e0112ae703;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;18;-159.0579,-330.1333;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-336.5142,-624.2399;Inherit;True;Property;Gradient;Gradient_Tex;8;0;Create;False;0;0;0;False;0;False;-1;None;5a26b1146f15d20438533c932eb11980;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;11;-456.8191,-0.2860715;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;19;24.12774,-350.4348;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;28;-39.72915,-596.5119;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;22;-101.942,-119.0263;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;23;9.608075,380.5085;Half;False;Property;_Float4;Opacity_power;4;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;632.7942,-101.2197;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;13;579.6833,70.59825;Half;False;Property;_Float2;Intensity_power;3;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-91.16096,213.8202;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;15;840.4388,108.6969;Half;False;Property;_Float3;Intensity;1;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;20;165.4889,256.7112;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;267.0172,338.2619;Half;False;Property;_Float5;Opacity;2;0;Create;False;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;12;786.3142,-18.09287;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;1012.893,10.05391;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;466.5518,253.4497;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1283.905,-43.83617;Half;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;Xia/Dissolve;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;16;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;45;0;46;0
WireConnection;45;1;47;0
WireConnection;3;0;4;0
WireConnection;3;1;5;0
WireConnection;44;1;39;0
WireConnection;44;2;45;0
WireConnection;44;3;48;0
WireConnection;44;4;49;0
WireConnection;6;0;3;0
WireConnection;6;1;8;0
WireConnection;50;1;39;0
WireConnection;50;0;44;0
WireConnection;41;0;42;0
WireConnection;41;1;43;0
WireConnection;9;0;2;0
WireConnection;9;1;6;0
WireConnection;40;0;50;0
WireConnection;40;2;41;0
WireConnection;1;1;9;0
WireConnection;27;1;40;0
WireConnection;11;0;1;0
WireConnection;19;0;18;0
WireConnection;28;0;27;0
WireConnection;22;0;17;0
WireConnection;16;0;19;0
WireConnection;16;1;22;0
WireConnection;16;2;11;0
WireConnection;16;3;28;0
WireConnection;21;0;17;4
WireConnection;21;1;18;4
WireConnection;21;2;1;4
WireConnection;20;0;21;0
WireConnection;20;1;23;0
WireConnection;12;0;16;0
WireConnection;12;1;13;0
WireConnection;14;0;12;0
WireConnection;14;1;15;0
WireConnection;24;0;20;0
WireConnection;24;1;25;0
WireConnection;0;2;14;0
WireConnection;0;9;24;0
ASEEND*/
//CHKSM=2F02381D9CF371D90672EE99CA69D95C0FAA182A