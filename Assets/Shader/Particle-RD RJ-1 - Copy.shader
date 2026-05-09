// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "eff_Shaders/Particles/RD RJ 1"
{
    Properties
    {
        [HDR]_HDR("HDR", Color) = (1,1,1,0)
        [HDR]_ditu("ditu", 2D) = "white" {}
        [Header(RD Setting)]_raodong("raodong", 2D) = "white" {}
        _raodong_power("raodong_power", Float) = 1
        _U_panner("U_panner", Float) = 0
        _V_panner("V_panner", Float) = 0
        [Header(RJ Setting)]_tex_rj("tex_rj", 2D) = "white" {}
        _rjU_panner("rjU_panner", Float) = 0
        _rjV_panner("rjV_panner", Float) = 0
        [Header(Mask Setting)]_mask("mask", 2D) = "white" {}
        _mask_U_panner("mask_U_panner", Float) = 0
        _mask_V_panner("mask_V_panner", Float) = 0

    }

    Category
    {
        SubShader
        {
		LOD 0

            Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
            Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            ZTest LEqual
            
            Pass
            {
                CGPROGRAM
                
                
                
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 3.0
                #pragma multi_compile_instancing
                #include "UnityShaderVariables.cginc"
                #define ASE_NEEDS_FRAG_COLOR


                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex: POSITION;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    half4 ase_color : COLOR;
                    float4 ase_texcoord : TEXCOORD0;
                    float4 ase_texcoord1 : TEXCOORD1;
                };

                struct v2f
                {
                    float4 vertex: SV_POSITION;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                    float4 ase_color : COLOR;
                    float4 ase_texcoord1 : TEXCOORD1;
                    float4 ase_texcoord2 : TEXCOORD2;
                };

                //Don't delete this comment
                // uniform sampler2D_float _CameraDepthTexture;
                
                uniform half4 _HDR;
                uniform sampler2D _ditu;
                uniform half4 _ditu_ST;
                uniform half _raodong_power;
                uniform sampler2D _raodong;
                uniform half _U_panner;
                uniform half4 _raodong_ST;
                uniform half _V_panner;
                SamplerState sampler_ditu;
                uniform sampler2D _tex_rj;
                uniform half _rjU_panner;
                uniform half4 _tex_rj_ST;
                uniform half _rjV_panner;
                uniform sampler2D _mask;
                SamplerState sampler_mask;
                uniform half _mask_U_panner;
                uniform half _mask_V_panner;
                uniform half4 _mask_ST;


                v2f vert(appdata_t v )
                {
                    v2f o;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_TRANSFER_INSTANCE_ID(v, o);
                    o.ase_color = v.ase_color;
                    o.ase_texcoord1.xy = v.ase_texcoord.xy;
                    o.ase_texcoord2 = v.ase_texcoord1;
                    
                    //setting value to unused interpolator channels and avoid initialization warnings
                    o.ase_texcoord1.zw = 0;

                    v.vertex.xyz +=  float3(0, 0, 0) ;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    return o;
                }

                fixed4 frag(v2f i ): SV_Target
                {
                    fixed4 col;
                    UNITY_SETUP_INSTANCE_ID(i);
                    half2 uv_ditu = i.ase_texcoord1.xy * _ditu_ST.xy + _ditu_ST.zw;
                    half2 uv_raodong = i.ase_texcoord1.xy * _raodong_ST.xy + _raodong_ST.zw;
                    half4 appendResult16 = (half4(( ( _U_panner * _Time.y ) + uv_raodong.x ) , ( uv_raodong.y + ( _V_panner * _Time.y ) ) , 0.0 , 0.0));
                    half4 appendResult36 = (half4(i.ase_texcoord2.x , i.ase_texcoord2.y , 0.0 , 0.0));
                    half4 tex2DNode2 = tex2D( _ditu, ( half4( ( half3( uv_ditu ,  0.0 ) + ( _raodong_power * (tex2D( _raodong, appendResult16.xy )).rgb ) ) , 0.0 ) + appendResult36 ).xy );
                    
                    half2 uv_tex_rj = i.ase_texcoord1.xy * _tex_rj_ST.xy + _tex_rj_ST.zw;
                    half2 appendResult103 = (half2(( ( _rjU_panner * _Time.y ) + uv_tex_rj.x ) , ( uv_tex_rj.y + ( _rjV_panner * _Time.y ) )));
                    half clampResult107 = clamp( ( tex2D( _tex_rj, appendResult103 ).r - i.ase_texcoord2.z ) , 0.0 , 1.0 );
                    half4 appendResult56 = (half4(( _Time.y * _mask_U_panner ) , ( _Time.y * _mask_V_panner ) , 0.0 , 0.0));
                    half2 uv_mask = i.ase_texcoord1.xy * _mask_ST.xy + _mask_ST.zw;
                    

                    col.rgb = ( i.ase_color * ( _HDR * tex2DNode2 ) ).rgb;
                    col.a = ( ( i.ase_color.a * tex2DNode2.a * ceil( clampResult107 ) ) * tex2D( _mask, ( appendResult56 + half4( uv_mask, 0.0 , 0.0 ) ).xy ).r );
                    return col;
                }
                ENDCG
                
            }
        }
        Fallback "VertexLit"
    }
    CustomEditor "ASEMaterialInspector"
	
}
/*ASEBEGIN
Version=18400
-2048;52.8;2048;1037;3555.236;1030.671;2.438476;True;False
Node;AmplifyShaderEditor.CommentaryNode;112;-2384.21,-512.5558;Inherit;False;1378.292;800.3441;RD;13;111;16;15;11;14;5;7;13;12;10;9;6;113;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TexturePropertyNode;111;-2344.904,-438.1298;Inherit;True;Property;_raodong;raodong;2;0;Create;True;0;0;False;1;Header(RD Setting);False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;13;-2005.374,69.68745;Inherit;False;Property;_V_panner;V_panner;5;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;12;-2033.168,189.619;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-2021.046,-357.7704;Inherit;False;Property;_U_panner;U_panner;4;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;9;-2048.84,-237.8388;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;113;-2101.793,-129.9378;Inherit;False;1;0;SAMPLER2D;;False;1;SAMPLER2D;0
Node;AmplifyShaderEditor.CommentaryNode;109;-1926.651,364.711;Inherit;False;1324.885;755.6678;RJ;12;103;102;101;100;98;99;105;95;97;96;94;44;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;7;-1837.37,-259.117;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1966,-113.8961;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-1828.367,78.31921;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;105;-1880.031,427.0904;Inherit;True;Property;_tex_rj;tex_rj;6;0;Create;True;0;0;False;1;Header(RJ Setting);False;None;None;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;95;-1506.201,509.7126;Inherit;False;Property;_rjU_panner;rjU_panner;7;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;11;-1692.256,-158.5355;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;97;-1585.559,1010.692;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-1557.764,890.7597;Inherit;False;Property;_rjV_panner;rjV_panner;8;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-1688.808,-2.254427;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;94;-1533.996,628.644;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-1344.881,922.1133;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;100;-1329.194,517.3442;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;98;-1519.007,732.8383;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;16;-1528.355,-74.33453;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleAddOpNode;102;-1198.89,646.1454;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-1201.423,812.933;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;6;-1332.333,-436.6431;Inherit;True;Property;_raodong1;raodong1;2;0;Create;True;0;0;False;1;Header(RD Setting);False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-900.2407,-301.1447;Inherit;False;Property;_raodong_power;raodong_power;3;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;103;-1061.168,717.6903;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;21;-918.0908,-184.1063;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.CommentaryNode;86;-878.3568,35.2238;Inherit;False;506.8846;286.1415;kekongUV;2;36;104;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;55;-507.839,789.7048;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;53;-498.581,594.7571;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-675.7512,-198.1118;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;104;-819.7516,122.9069;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-715.6831,-434.7895;Inherit;False;0;2;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;58;-511.3347,904.5942;Inherit;False;Property;_mask_V_panner;mask_V_panner;11;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-512.5196,686.3217;Inherit;False;Property;_mask_U_panner;mask_U_panner;10;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;44;-914.8996,428.5569;Inherit;True;Property;_tex_rj1;tex_rj1;6;0;Create;True;0;0;False;0;False;-1;None;61c0b9c0523734e0e91bc6043c72a490;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DynamicAppendNode;36;-543.2709,147.7395;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;106;-456.2693,426.3486;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;62;-288.2626,820.4706;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;-287.8046,609.6829;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;-473.6144,-220.7783;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;93;-278.3049,154.5646;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT4;0,0,0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-52.0416,664.9641;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.ClampOpNode;107;-261.3497,427.2003;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;61;-67.41318,852.0914;Inherit;False;0;49;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;30;-36.00261,-142.5227;Inherit;False;Property;_HDR;HDR;0;1;[HDR];Create;True;0;0;False;0;False;1,1,1,0;0,0.5774764,0.6898702,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;24;255.3737,-234.6684;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;59;170.8281,675.4515;Inherit;False;2;2;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0,0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SamplerNode;2;-88.18362,125.371;Inherit;True;Property;_ditu;ditu;1;1;[HDR];Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CeilOpNode;108;12.2164,423.8587;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;49;327.2009,646.4333;Inherit;True;Property;_mask;mask;9;0;Create;True;0;0;False;1;Header(Mask Setting);False;-1;None;5b99eaadd59a03046948aff90d1b59c4;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;27;244.4093,22.30006;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;26;483.1819,313.8322;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;604.8815,2.690045;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;673.2148,314.6641;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;110;882.5743,5.04503;Half;False;True;-1;2;ASEMaterialInspector;0;11;eff_Shaders/Particles/RD RJ 1;82ad3bcc521a6ec49a178c583a2f939d;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;3;True;2;5;False;-1;10;False;-1;2;5;False;-1;10;False;-1;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;True;2;False;-1;True;3;False;-1;False;True;4;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;IgnoreProjector=True;PreviewType=Plane;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;0;VertexLit;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;113;0;111;0
WireConnection;7;0;10;0
WireConnection;7;1;9;0
WireConnection;5;2;113;0
WireConnection;14;0;13;0
WireConnection;14;1;12;0
WireConnection;11;0;7;0
WireConnection;11;1;5;1
WireConnection;15;0;5;2
WireConnection;15;1;14;0
WireConnection;99;0;96;0
WireConnection;99;1;97;0
WireConnection;100;0;95;0
WireConnection;100;1;94;0
WireConnection;98;2;105;0
WireConnection;16;0;11;0
WireConnection;16;1;15;0
WireConnection;102;0;100;0
WireConnection;102;1;98;1
WireConnection;101;0;98;2
WireConnection;101;1;99;0
WireConnection;6;0;111;0
WireConnection;6;1;16;0
WireConnection;103;0;102;0
WireConnection;103;1;101;0
WireConnection;21;0;6;0
WireConnection;19;0;20;0
WireConnection;19;1;21;0
WireConnection;44;0;105;0
WireConnection;44;1;103;0
WireConnection;36;0;104;1
WireConnection;36;1;104;2
WireConnection;106;0;44;1
WireConnection;106;1;104;3
WireConnection;62;0;55;0
WireConnection;62;1;58;0
WireConnection;51;0;53;0
WireConnection;51;1;57;0
WireConnection;17;0;18;0
WireConnection;17;1;19;0
WireConnection;93;0;17;0
WireConnection;93;1;36;0
WireConnection;56;0;51;0
WireConnection;56;1;62;0
WireConnection;107;0;106;0
WireConnection;59;0;56;0
WireConnection;59;1;61;0
WireConnection;2;1;93;0
WireConnection;108;0;107;0
WireConnection;49;1;59;0
WireConnection;27;0;30;0
WireConnection;27;1;2;0
WireConnection;26;0;24;4
WireConnection;26;1;2;4
WireConnection;26;2;108;0
WireConnection;23;0;24;0
WireConnection;23;1;27;0
WireConnection;48;0;26;0
WireConnection;48;1;49;1
WireConnection;110;0;23;0
WireConnection;110;1;48;0
ASEEND*/
//CHKSM=59C03A4229718D40872621701FCBD90CF1A8CC45