// Made with Amplify Shader Editor v1.9.9.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Xia/Fre_Dissolve_Q"
{
	Properties
	{
		[Toggle( _RORA_ON )] _RorA( "R or A", Float ) = 0
		[HDR] _Color( "Color", Color ) = ( 1, 1, 1, 1 )
		_MainTex( "MainTex", 2D ) = "white" {}
		_Main_U( "Main_U", Float ) = 0
		_Main_V( "Main_V", Float ) = 0
		_Mask( "Mask", 2D ) = "white" {}
		_Mask_U( "Mask_U", Float ) = 0
		_Mask_V( "Mask_V", Float ) = 0
		_DissolveTex( "DissolveTex", 2D ) = "white" {}
		_Dissolve_U( "Dissolve_U", Float ) = 0
		_Dissolve_V( "Dissolve_V", Float ) = 0
		_DissolveInstenity( "DissolveInstenity", Range( 0, 1.05 ) ) = 0.3022233
		_DissolveSoft( "DissolveSoft", Range( -0.5, 0.5 ) ) = 0.07647059
		_FresnelRange( "FresnelRange", Range( 0, 5 ) ) = 1.896656
		_VertexOffectTex( "VertexOffectTex", 2D ) = "white" {}
		_VertexOffectInstenity( "VertexOffectInstenity", Float ) = 0
		_VertexOffect_U( "VertexOffect_U", Float ) = 0
		_VertexOffect_V( "VertexOffect_V", Float ) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Custom"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma shader_feature_local _RORA_ON
		#define ASE_VERSION 19903
		#pragma surface surf Unlit keepalpha noshadow noambient novertexlights nolightmap  nodynlightmap nodirlightmap nofog nometa noforwardadd vertex:vertexDataFunc 
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float3 worldPos;
			half3 worldNormal;
		};

		uniform sampler2D _VertexOffectTex;
		uniform half _VertexOffect_U;
		uniform half _VertexOffect_V;
		uniform float4 _VertexOffectTex_ST;
		uniform half _VertexOffectInstenity;
		uniform sampler2D _MainTex;
		uniform half _Main_U;
		uniform half _Main_V;
		uniform float4 _MainTex_ST;
		uniform half4 _Color;
		uniform half _DissolveSoft;
		uniform sampler2D _DissolveTex;
		uniform half _Dissolve_U;
		uniform half _Dissolve_V;
		uniform float4 _DissolveTex_ST;
		uniform half _DissolveInstenity;
		uniform sampler2D _Mask;
		uniform half _Mask_U;
		uniform half _Mask_V;
		uniform float4 _Mask_ST;
		uniform half _FresnelRange;

		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			half2 appendResult99 = (half2(_VertexOffect_U , _VertexOffect_V));
			float2 uv_VertexOffectTex = v.texcoord.xy * _VertexOffectTex_ST.xy + _VertexOffectTex_ST.zw;
			half2 panner101 = ( 1.0 * _Time.y * appendResult99 + uv_VertexOffectTex);
			half3 ase_normalOS = v.normal.xyz;
			v.vertex.xyz += ( tex2Dlod( _VertexOffectTex, half4( panner101, 0, 0.0) ) * half4( ase_normalOS , 0.0 ) * _VertexOffectInstenity ).rgb;
			v.vertex.w = 1;
		}

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			half2 appendResult15 = (half2(_Main_U , _Main_V));
			float2 uv_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			half2 panner14 = ( 1.0 * _Time.y * appendResult15 + uv_MainTex);
			half4 tex2DNode2 = tex2D( _MainTex, panner14 );
			o.Emission = ( (i.vertexColor).rgb * (tex2DNode2).rgb * (_Color).rgb );
			#ifdef _RORA_ON
				half staticSwitch9 = tex2DNode2.a;
			#else
				half staticSwitch9 = tex2DNode2.r;
			#endif
			half2 appendResult46 = (half2(_Dissolve_U , _Dissolve_V));
			float2 uv_DissolveTex = i.uv_texcoord * _DissolveTex_ST.xy + _DissolveTex_ST.zw;
			half2 panner48 = ( 1.0 * _Time.y * appendResult46 + uv_DissolveTex);
			half smoothstepResult26 = smoothstep( _DissolveSoft , ( 1.0 - _DissolveSoft ) , saturate( ( ( tex2D( _DissolveTex, panner48 ).r + 1.0 ) - ( _DissolveInstenity * 2.0 ) ) ));
			half2 appendResult40 = (half2(_Mask_U , _Mask_V));
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			half2 panner42 = ( 1.0 * _Time.y * appendResult40 + uv_Mask);
			half3 desaturateInitialColor35 = tex2D( _Mask, panner42 ).rgb;
			half desaturateDot35 = dot( desaturateInitialColor35, float3( 0.299, 0.587, 0.114 ));
			half3 desaturateVar35 = lerp( desaturateInitialColor35, desaturateDot35.xxx, 1.0 );
			float3 ase_positionWS = i.worldPos;
			half3 ase_viewVectorWS = ( _WorldSpaceCameraPos.xyz - ase_positionWS );
			half3 ase_viewDirWS = normalize( ase_viewVectorWS );
			half3 ase_normalWS = i.worldNormal;
			half3 ase_normalWSNorm = normalize( ase_normalWS );
			half dotResult73 = dot( ase_viewDirWS , ase_normalWSNorm );
			o.Alpha = ( i.vertexColor.a * _Color.a * staticSwitch9 * smoothstepResult26 * ( tex2DNode2.a * (desaturateVar35).x ) * pow( saturate( abs( dotResult73 ) ) , _FresnelRange ) );
		}

		ENDCG
	}
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19903
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;43;-2656,1344;Inherit;False;520.3367;370.0016;;5;48;47;46;45;44;UV组件;0.2264151,0.2264151,0.2264151,1;0;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;37;-2378.556,511.2154;Inherit;False;520.3367;370.0016;;5;42;41;40;39;38;UV组件;0.2264151,0.2264151,0.2264151,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;45;-2608,1584;Inherit;False;Property;_Dissolve_V;Dissolve_V;10;0;Create;True;0;0;0;False;0;False;0;-0.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;44;-2608,1520;Inherit;False;Property;_Dissolve_U;Dissolve_U;9;0;Create;True;0;0;0;False;0;False;0;1.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;39;-2325.556,765.2173;Inherit;False;Property;_Mask_V;Mask_V;7;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;38;-2328.556,691.2172;Inherit;False;Property;_Mask_U;Mask_U;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;46;-2448,1536;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;47;-2576,1392;Inherit;False;0;18;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;34;-1706.498,154.9232;Inherit;False;520.3367;370.0016;;5;17;16;15;10;14;UV组件;0.2264151,0.2264151,0.2264151,1;0;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;33;-2096,1344;Inherit;False;1050.556;555.334;;11;27;25;21;19;18;22;20;32;28;26;23;溶解;0,0,0,1;0;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;40;-2175.097,705.4877;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;41;-2305.462,561.2155;Inherit;False;0;29;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;48;-2336,1392;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;95;-944.2354,1830.095;Inherit;False;1204;611;;5;105;104;103;102;96;点偏移;0.8207547,0.120016,0.120016,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;16;-1656.498,334.925;Inherit;False;Property;_Main_U;Main_U;3;0;Create;True;0;0;0;False;0;False;0;1.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ViewDirInputsCoordNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;71;-2297.143,905.9592;Inherit;False;World;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;17;-1653.498,408.925;Inherit;False;Property;_Main_V;Main_V;4;0;Create;True;0;0;0;False;0;False;0;-2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldNormalVector, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;72;-2326.944,1062.475;Inherit;False;True;1;0;FLOAT3;0,0,1;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;42;-2063.219,574.1669;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;32;-2048,1696;Half;False;Property;_DissolveInstenity;DissolveInstenity;11;0;Create;True;0;0;0;False;0;False;0.3022233;0;0;1.05;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;20;-1904,1584;Half;False;Constant;_Float0;Float 0;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;23;-1968,1792;Half;False;Constant;_Float1;Float 1;7;0;Create;True;0;0;0;False;0;False;2;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;18;-2016,1392;Inherit;True;Property;_DissolveTex;DissolveTex;8;0;Create;True;0;0;0;False;0;False;-1;None;916fc6038bcae604fa26b544d6546808;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.CommentaryNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;96;-894.2354,1960.095;Inherit;False;520.3367;370.0016;;5;101;100;99;98;97;UV组件;0.2264151,0.2264151,0.2264151,1;0;0
Node;AmplifyShaderEditor.DotProductOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;73;-2079.844,949.1373;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;15;-1503.039,349.1955;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;10;-1633.404,204.9232;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;29;-1836.223,712.2802;Inherit;True;Property;_Mask;Mask;5;0;Create;True;0;0;0;False;0;False;-1;None;c3eda942da823234f97213bff567f9b6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleAddOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;19;-1712,1488;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;22;-1728,1616;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;97;-862.2354,2120.095;Inherit;False;Property;_VertexOffect_U;VertexOffect_U;16;0;Create;True;0;0;0;False;0;False;0;1.9;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;98;-862.2354,2216.095;Inherit;False;Property;_VertexOffect_V;VertexOffect_V;17;0;Create;True;0;0;0;False;0;False;0;-0.28;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.AbsOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;70;-1858.756,938.3702;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;14;-1391.161,217.8747;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DesaturateOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;35;-1510.914,752.1383;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT;1;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;21;-1568,1536;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;99;-686.2354,2152.095;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;100;-846.2354,1992.095;Inherit;False;0;102;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;1;-1068.535,-75.82004;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;2;-1138.574,217.4316;Inherit;True;Property;_MainTex;MainTex;2;0;Create;True;0;0;0;False;0;False;-1;None;ae657e267152caa4ba26c841985674e9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;74;-1689.869,932.6243;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;77;-1743.283,1167.283;Half;False;Property;_FresnelRange;FresnelRange;13;0;Create;True;0;0;0;False;0;False;1.896656;1.03;0;5;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;36;-1338.807,748.1359;Inherit;False;True;False;False;False;1;0;FLOAT3;0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;5;-1248,528;Half;False;Property;_Color;Color;1;1;[HDR];Create;True;0;0;0;False;0;False;1,1,1,1;1.690993,9.487002,14.33962,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;27;-1616,1648;Half;False;Property;_DissolveSoft;DissolveSoft;12;0;Create;True;0;0;0;False;0;False;0.07647059;0.07647059;-0.5;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;28;-1328,1664;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;25;-1408,1536;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;101;-574.2354,2024.095;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;92;-810.9076,221.9365;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.PowerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;76;-1358.874,951.3233;Inherit;True;False;2;0;FLOAT;0;False;1;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.StaticSwitch, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;9;-810.8329,302.6573;Inherit;False;Property;_RorA;R or A;0;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT;0;False;0;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;93;-898.6314,493.8795;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;91;-794.4762,-39.60657;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;94;-711.6624,609.202;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;26;-1136,1472;Inherit;True;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;102;-366.2354,1976.095;Inherit;True;Property;_VertexOffectTex;VertexOffectTex;14;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;False;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.NormalVertexDataNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;103;-286.2354,2168.095;Inherit;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;104;-318.2354,2328.095;Half;False;Property;_VertexOffectInstenity;VertexOffectInstenity;15;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;3;-463.0352,97.73277;Inherit;True;3;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;4;-240,960;Inherit;True;6;6;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;105;81.76465,2056.095;Inherit;False;3;3;0;COLOR;0,0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode, AmplifyShaderEditor, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null;0;496,1024;Half;False;True;-1;2;AmplifyShaderEditor.MaterialInspector;0;0;Unlit;Xia/Fre_Dissolve_Q;False;False;False;False;True;True;True;True;True;True;True;True;False;False;False;False;False;False;False;False;False;Back;2;False;;0;False;;False;0;False;;0;False;;False;0;Custom;0.5;True;False;0;True;Custom;;Transparent;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;False;2;5;False;;10;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;18;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;46;0;44;0
WireConnection;46;1;45;0
WireConnection;40;0;38;0
WireConnection;40;1;39;0
WireConnection;48;0;47;0
WireConnection;48;2;46;0
WireConnection;42;0;41;0
WireConnection;42;2;40;0
WireConnection;18;1;48;0
WireConnection;73;0;71;0
WireConnection;73;1;72;0
WireConnection;15;0;16;0
WireConnection;15;1;17;0
WireConnection;29;1;42;0
WireConnection;19;0;18;1
WireConnection;19;1;20;0
WireConnection;22;0;32;0
WireConnection;22;1;23;0
WireConnection;70;0;73;0
WireConnection;14;0;10;0
WireConnection;14;2;15;0
WireConnection;35;0;29;0
WireConnection;21;0;19;0
WireConnection;21;1;22;0
WireConnection;99;0;97;0
WireConnection;99;1;98;0
WireConnection;2;1;14;0
WireConnection;74;0;70;0
WireConnection;36;0;35;0
WireConnection;28;0;27;0
WireConnection;25;0;21;0
WireConnection;101;0;100;0
WireConnection;101;2;99;0
WireConnection;92;0;2;0
WireConnection;76;0;74;0
WireConnection;76;1;77;0
WireConnection;9;1;2;1
WireConnection;9;0;2;4
WireConnection;93;0;5;0
WireConnection;91;0;1;0
WireConnection;94;0;2;4
WireConnection;94;1;36;0
WireConnection;26;0;25;0
WireConnection;26;1;27;0
WireConnection;26;2;28;0
WireConnection;102;1;101;0
WireConnection;3;0;91;0
WireConnection;3;1;92;0
WireConnection;3;2;93;0
WireConnection;4;0;1;4
WireConnection;4;1;5;4
WireConnection;4;2;9;0
WireConnection;4;3;26;0
WireConnection;4;4;94;0
WireConnection;4;5;76;0
WireConnection;105;0;102;0
WireConnection;105;1;103;0
WireConnection;105;2;104;0
WireConnection;0;2;3;0
WireConnection;0;9;4;0
WireConnection;0;11;105;0
ASEEND*/
//CHKSM=C1C5FCE79AA5E724EF760F47BF625CCC682A3330