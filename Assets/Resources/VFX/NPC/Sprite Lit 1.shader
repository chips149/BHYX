// Made with Amplify Shader Editor v1.9.8.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "enemy"
{
	Properties
	{
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_dissolvetextrue("dissolvetextrue", 2D) = "white" {}
		[HDR]_Color0("Color 0", Color) = (1,0.4486697,0,0)
		_BIan("BIan", Float) = 0.05
		_Diissolve_In("Diissolve_In", Range( 0 , 1.5)) = 0.07549236
		_Texture0("Texture 0", 2D) = "white" {}
		_Nooal("Nooal", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#pragma target 3.0
		#define ASE_VERSION 19801
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _Nooal;
		uniform float4 _Nooal_ST;
		uniform sampler2D _Texture0;
		uniform float4 _Texture0_ST;
		uniform float _Diissolve_In;
		uniform sampler2D _dissolvetextrue;
		uniform float4 _dissolvetextrue_ST;
		uniform float _BIan;
		uniform float4 _Color0;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_Nooal = i.uv_texcoord * _Nooal_ST.xy + _Nooal_ST.zw;
			o.Normal = tex2D( _Nooal, uv_Nooal ).rgb;
			float2 uv_Texture0 = i.uv_texcoord * _Texture0_ST.xy + _Texture0_ST.zw;
			float4 tex2DNode48 = tex2D( _Texture0, uv_Texture0 );
			float2 uv_dissolvetextrue = i.uv_texcoord * _dissolvetextrue_ST.xy + _dissolvetextrue_ST.zw;
			float4 tex2DNode57 = tex2D( _dissolvetextrue, uv_dissolvetextrue );
			float temp_output_61_0 = step( _Diissolve_In , ( tex2DNode57.r + _BIan ) );
			float temp_output_62_0 = ( temp_output_61_0 - step( _Diissolve_In , tex2DNode57.r ) );
			float4 lerpResult51 = lerp( tex2DNode48 , ( tex2DNode48.a * temp_output_62_0 * _Color0 ) , temp_output_62_0);
			float temp_output_53_0 = ( tex2DNode48.a * temp_output_61_0 );
			float4 appendResult54 = (float4((lerpResult51).rgb , temp_output_53_0));
			float4 SpriteColor3 = ( appendResult54 * temp_output_53_0 );
			o.Albedo = SpriteColor3.xyz;
			o.Alpha = 1;
			float SpriteAlpha4 = temp_output_53_0;
			clip( SpriteAlpha4 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "AmplifyShaderEditor.MaterialInspector"
}
/*ASEBEGIN
Version=19801
Node;AmplifyShaderEditor.CommentaryNode;65;-2784,-1472;Inherit;False;1926.768;1427.458;溶解;15;54;52;53;51;64;63;62;48;61;60;67;59;57;58;78;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;57;-2752,-656;Inherit;True;Property;_dissolvetextrue;dissolvetextrue;3;0;Create;True;0;0;0;False;0;False;-1;None;77365e6ac6add044883a10b1e84d5607;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RangedFloatNode;58;-2688,-224;Inherit;False;Property;_BIan;BIan;9;0;Create;True;0;0;0;False;0;False;0.05;0.05;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;59;-2464,-384;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;67;-2720,-800;Inherit;False;Property;_Diissolve_In;Diissolve_In;10;0;Create;True;0;0;0;False;0;False;0.07549236;0;0;1.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;60;-2384,-768;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;61;-2256,-384;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;78;-2528,-1136;Inherit;True;Property;_Texture0;Texture 0;11;0;Create;True;0;0;0;False;0;False;None;bc5732324f1d3c1489a23942bd143136;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleSubtractOpNode;62;-2032,-800;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;48;-2192,-1136;Inherit;True;Property;_maintTextrue;maintTextrue;2;0;Create;True;0;0;0;False;0;False;-1;None;af2b066dbbe6f1340af8f17aea40b916;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;63;-2032,-320;Inherit;False;Property;_Color0;Color 0;7;1;[HDR];Create;True;0;0;0;False;0;False;1,0.4486697,0,0;1,0.4486697,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;64;-1760,-496;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.LerpOp;51;-1600,-1024;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;53;-1376,-384;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;52;-1344,-720;Inherit;True;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.DynamicAppendNode;54;-1104,-528;Inherit;True;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;68;-800,-272;Inherit;True;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;3;-576,-272;Float;True;SpriteColor;-1;True;1;0;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;4;-736,384;Float;True;SpriteAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WorldSpaceLightDirHlpNode;69;-1008,-928;Inherit;False;False;1;0;FLOAT;0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.RangedFloatNode;70;-720,-720;Inherit;False;Property;_dot;dot;6;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DotProductOpNode;71;-544,-960;Inherit;True;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;72;-800,-1232;Inherit;False;Property;_viewdir;view dir;1;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;73;-800,-1168;Inherit;False;Property;_bias;bias;2;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;74;-784,-1104;Inherit;False;Property;_power;power;4;0;Create;True;0;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;76;-288,-944;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FresnelNode;75;-544,-1232;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;77;-80,-1008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.NegateNode;41;-1312,256;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;43;-1136,48;Float;False;Property;_FLIPFACE;FLIP FACE;8;0;Create;True;0;0;0;False;0;False;0;0;0;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SwitchByFaceNode;42;-1136,192;Inherit;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StaticSwitch;40;-864,32;Float;False;Property;_TWOSIDEDILLUMINATION;TWO SIDED ILLUMINATION;5;0;Create;True;0;0;0;False;0;False;0;1;1;True;;Toggle;2;Key0;Key1;Create;True;True;All;9;1;FLOAT3;0,0,0;False;0;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT3;0,0,0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;12;-560,32;Float;False;Normal;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;45;-480,-400;Inherit;False;Constant;_Float0;Float 0;4;0;Create;True;0;0;0;False;0;False;0.5;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;5;-336,-272;Inherit;True;3;SpriteColor;1;0;OBJECT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;6;-368,384;Inherit;True;4;SpriteAlpha;1;0;OBJECT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;13;-320,32;Inherit;False;12;Normal;1;0;OBJECT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-288,-480;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector3Node;36;-1568,32;Float;False;Constant;_Vector1;Vector 1;2;0;Create;True;0;0;0;False;0;False;0,0,1;0,0,0;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;79;-464,144;Inherit;True;Property;_Nooal;Nooal;12;0;Create;True;0;0;0;False;0;False;-1;None;f11758077c8b6344281f1323ec451a55;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;2,1.3;Float;False;True;-1;2;AmplifyShaderEditor.MaterialInspector;0;0;Standard;enemy;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;;0;False;;False;0;False;;0;False;;False;0;Masked;0.5;True;True;0;False;TransparentCutout;;AlphaTest;All;12;all;True;True;True;True;0;False;;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;False;2;15;10;25;False;0.5;True;0;0;False;;0;False;;0;0;False;;0;False;;0;False;;0;False;;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;0;-1;-1;-1;0;False;0;0;False;;-1;0;False;;0;0;0;False;0.1;False;;0;False;;False;17;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;16;FLOAT4;0,0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;59;0;57;1
WireConnection;59;1;58;0
WireConnection;60;0;67;0
WireConnection;60;1;57;1
WireConnection;61;0;67;0
WireConnection;61;1;59;0
WireConnection;62;0;61;0
WireConnection;62;1;60;0
WireConnection;48;0;78;0
WireConnection;64;0;48;4
WireConnection;64;1;62;0
WireConnection;64;2;63;0
WireConnection;51;0;48;0
WireConnection;51;1;64;0
WireConnection;51;2;62;0
WireConnection;53;0;48;4
WireConnection;53;1;61;0
WireConnection;52;0;51;0
WireConnection;54;0;52;0
WireConnection;54;3;53;0
WireConnection;68;0;54;0
WireConnection;68;1;53;0
WireConnection;3;0;68;0
WireConnection;4;0;53;0
WireConnection;71;0;69;0
WireConnection;71;1;70;0
WireConnection;76;0;71;0
WireConnection;75;4;72;0
WireConnection;75;1;73;0
WireConnection;75;3;74;0
WireConnection;77;0;75;0
WireConnection;77;1;76;0
WireConnection;41;0;36;0
WireConnection;43;1;36;0
WireConnection;43;0;41;0
WireConnection;42;0;36;0
WireConnection;42;1;41;0
WireConnection;40;1;43;0
WireConnection;40;0;42;0
WireConnection;12;0;40;0
WireConnection;44;0;45;0
WireConnection;0;0;5;0
WireConnection;0;1;79;0
WireConnection;0;10;6;0
ASEEND*/
//CHKSM=8A7AA236901C4A88771244DB270E3E5D463E4CE1