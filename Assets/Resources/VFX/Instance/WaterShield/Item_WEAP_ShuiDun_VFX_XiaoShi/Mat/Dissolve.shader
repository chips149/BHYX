// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Xia/Dissolve"
{
	Properties
	{
		[HDR]_Color0("color", Color) = (1,1,1,1)
		_Float3("Intensity", Float) = 1
		_Float5("Opacity", Float) = 1
		_Float2("Intensity_power", Float) = 1
		_Float4("Opacity_power", Float) = 1
		Main_Tex("主贴图", 2D) = "white" {}
		_U("U", Float) = 0
		_V("V", Float) = 0
		Gradient("Gradient_Tex", 2D) = "white" {}
		[Toggle(_KEYWORD0_ON)] _Keyword0("polar开关", Float) = 0
		_Float6("Gradient_U", Float) = 0
		_Float7("Gradient_V", Float) = 0
		_Float8("极坐标中心X", Float) = 0.5
		_Float9("极坐标中心Y", Float) = 0.5
		_Float10("极坐标X重铺", Float) = 1
		_Float11("极坐标Y重铺", Float) = 1
		_Dissolve("Dissolve", 2D) = "white" {}
		_Diss_U("Diss_U", Float) = 0
		_Diss_V("Diss_V", Float) = 0
		_DissolveIntensity("DissolveIntensity", Range( 0 , 1.05)) = 0.6395373
		_DissolveSoft("DissolveSoft", Range( 0 , 0.5)) = 0.3413005
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
		uniform half _U;
		uniform half _V;
		uniform float4 Main_Tex_ST;
		uniform sampler2D Gradient;
		uniform half _Float6;
		uniform half _Float7;
		uniform half _Float8;
		uniform half _Float9;
		uniform half _Float10;
		uniform half _Float11;
		uniform half _Float2;
		uniform half _Float3;
		uniform half _DissolveSoft;
		uniform sampler2D _Dissolve;
		uniform half _Diss_U;
		uniform half _Diss_V;
		uniform half _DissolveIntensity;
		uniform half _Float4;
		uniform half _Float5;

		void surf( Input i , inout SurfaceOutput o )
		{
			half2 appendResult67 = (half2(_U , _V));
			float2 uvMain_Tex = i.uv_texcoord * Main_Tex_ST.xy + Main_Tex_ST.zw;
			half2 panner66 = ( 1.0 * _Time.y * appendResult67 + uvMain_Tex);
			half4 tex2DNode1 = tex2D( Main_Tex, panner66 );
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
			half2 appendResult74 = (half2(_Diss_U , _Diss_V));
			half2 panner75 = ( 1.0 * _Time.y * appendResult74 + uvMain_Tex);
			half3 desaturateInitialColor77 = tex2D( _Dissolve, panner75 ).rgb;
			half desaturateDot77 = dot( desaturateInitialColor77, float3( 0.299, 0.587, 0.114 ));
			half3 desaturateVar77 = lerp( desaturateInitialColor77, desaturateDot77.xxx, 0.0 );
			half smoothstepResult87 = smoothstep( _DissolveSoft , ( 1.0 - _DissolveSoft ) , saturate( ( ( (desaturateVar77).x + 1.0 ) - ( _DissolveIntensity * 2.0 ) ) ));
			o.Alpha = ( pow( ( _Color0.a * i.vertexColor.a * tex2DNode1.a * smoothstepResult87 ) , _Float4 ) * _Float5 );
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
717;224;1002;502;2219.053;-339.771;1.375038;True;False
Node;AmplifyShaderEditor.RangedFloatNode;72;-2548.029,432.353;Half;False;Property;_Diss_U;Diss_U;17;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;71;-2540.113,516.6216;Half;False;Property;_Diss_V;Diss_V;18;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;74;-2339.949,428.4136;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;73;-2483.275,285.0337;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;75;-2213.65,286.0565;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;46;-1766.408,-643.5799;Half;False;Property;_Float8;极坐标中心X;12;0;Create;False;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;47;-1762.408,-554.5801;Half;False;Property;_Float9;极坐标中心Y;13;0;Create;False;0;0;0;False;0;False;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;70;-2026.089,348.8472;Inherit;True;Property;_Dissolve;Dissolve;16;0;Create;False;0;0;0;False;0;False;-1;c0e0a864124a31642aabe2aa94c5868e;c0e0a864124a31642aabe2aa94c5868e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;49;-1294.449,-446.7846;Half;False;Property;_Float11;极坐标Y重铺;15;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;48;-1289.449,-527.7846;Half;False;Property;_Float10;极坐标X重铺;14;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;45;-1535.793,-625.9449;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DesaturateOpNode;77;-1744.229,383.4159;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;39;-1484.798,-739.7465;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;43;-881.8368,-372.4095;Half;False;Property;_Float7;Gradient_V;11;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-883.8368,-453.4095;Half;False;Property;_Float6;Gradient_U;10;0;Create;False;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;68;-1349.894,170.0397;Half;False;Property;_U;U;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;84;-1626.111,664.2332;Half;False;Property;_DissolveIntensity;DissolveIntensity;20;0;Create;True;0;0;0;False;0;False;0.6395373;0.397;0;1.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;82;-1571.944,746.4604;Half;False;Constant;_Float1;Float 1;20;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;69;-1341.978,255.9859;Half;False;Property;_V;V;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-1558.561,569.074;Half;False;Constant;Dissolve;Dissolve;20;0;Create;False;0;0;0;False;0;False;1;-0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;78;-1583.229,385.4159;Inherit;True;True;False;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;44;-1078.263,-657.9224;Inherit;True;Polar Coordinates;-1;;1;7dab8e02884cf104ebefaa2e788e4162;0;4;1;FLOAT2;0,0;False;2;FLOAT2;0.5,0.5;False;3;FLOAT;1;False;4;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;83;-1300.856,698.4506;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;76;-1336.648,383.4882;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;41;-714.8368,-450.4095;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;67;-1141.814,167.778;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;65;-1285.14,24.39816;Inherit;False;0;1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StaticSwitch;50;-966.0947,-872.3372;Inherit;False;Property;_Keyword0;polar开关;9;0;Create;False;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;9;1;FLOAT2;0,0;False;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT2;0,0;False;6;FLOAT2;0,0;False;7;FLOAT2;0,0;False;8;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;88;-1076.635,705.0656;Half;False;Property;_DissolveSoft;DissolveSoft;21;0;Create;True;0;0;0;False;0;False;0.3413005;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;89;-1092.7,428.3241;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;40;-577.8368,-582.4095;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.PannerNode;66;-1015.515,25.42093;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.VertexColorNode;18;-159.0579,-330.1333;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;17;-333.8069,-174.6886;Half;False;Property;_Color0;color;0;1;[HDR];Create;False;0;0;0;False;0;False;1,1,1,1;1,1,1,1;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;27;-336.5142,-624.2399;Inherit;True;Property;Gradient;Gradient_Tex;8;0;Create;False;0;0;0;False;0;False;-1;ddf7b246cb5b46f47903ed037e3cd8a0;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;86;-664.5823,726.7475;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;1;-754.8089,-1.883759;Inherit;True;Property;Main_Tex;主贴图;5;0;Create;False;0;0;0;False;0;False;-1;c0e0a864124a31642aabe2aa94c5868e;c0e0a864124a31642aabe2aa94c5868e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;90;-766.7517,460.7762;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;11;-456.8191,-0.2860715;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;28;-39.72915,-596.5119;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;19;24.12774,-350.4348;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;22;-101.942,-119.0263;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SmoothstepOpNode;87;-629.6434,444.2632;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;9.608075,380.5085;Half;False;Property;_Float4;Opacity_power;4;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-91.16096,213.8202;Inherit;False;4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;13;529.2224,-106.0146;Half;False;Property;_Float2;Intensity_power;3;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;16;483.0937,-271.1045;Inherit;False;4;4;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode;12;707.259,-196.3878;Inherit;False;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;15;675.6002,2.729165;Half;False;Property;_Float3;Intensity;1;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;25;204.6172,425.3619;Half;False;Property;_Float5;Opacity;2;0;Create;False;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.PowerNode;20;165.4889,256.7112;Inherit;False;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;932.1558,-153.1027;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;677.9519,229.5797;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;1283.905,-43.83617;Half;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;Xia/Dissolve;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Off;2;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;True;0;True;Custom;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;19;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;74;0;72;0
WireConnection;74;1;71;0
WireConnection;75;0;73;0
WireConnection;75;2;74;0
WireConnection;70;1;75;0
WireConnection;45;0;46;0
WireConnection;45;1;47;0
WireConnection;77;0;70;0
WireConnection;78;0;77;0
WireConnection;44;1;39;0
WireConnection;44;2;45;0
WireConnection;44;3;48;0
WireConnection;44;4;49;0
WireConnection;83;0;84;0
WireConnection;83;1;82;0
WireConnection;76;0;78;0
WireConnection;76;1;79;0
WireConnection;41;0;42;0
WireConnection;41;1;43;0
WireConnection;67;0;68;0
WireConnection;67;1;69;0
WireConnection;50;1;39;0
WireConnection;50;0;44;0
WireConnection;89;0;76;0
WireConnection;89;1;83;0
WireConnection;40;0;50;0
WireConnection;40;2;41;0
WireConnection;66;0;65;0
WireConnection;66;2;67;0
WireConnection;27;1;40;0
WireConnection;86;0;88;0
WireConnection;1;1;66;0
WireConnection;90;0;89;0
WireConnection;11;0;1;0
WireConnection;28;0;27;0
WireConnection;19;0;18;0
WireConnection;22;0;17;0
WireConnection;87;0;90;0
WireConnection;87;1;88;0
WireConnection;87;2;86;0
WireConnection;21;0;17;4
WireConnection;21;1;18;4
WireConnection;21;2;1;4
WireConnection;21;3;87;0
WireConnection;16;0;19;0
WireConnection;16;1;22;0
WireConnection;16;2;11;0
WireConnection;16;3;28;0
WireConnection;12;0;16;0
WireConnection;12;1;13;0
WireConnection;20;0;21;0
WireConnection;20;1;23;0
WireConnection;14;0;12;0
WireConnection;14;1;15;0
WireConnection;24;0;20;0
WireConnection;24;1;25;0
WireConnection;0;2;14;0
WireConnection;0;9;24;0
ASEEND*/
//CHKSM=E35F8305D48B1A2E892AD9E75448CFD5FA0F9085