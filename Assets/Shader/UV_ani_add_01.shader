// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3020,x:33126,y:32700,varname:node_3020,prsc:2|emission-4376-OUT;n:type:ShaderForge.SFN_TexCoord,id:1838,x:31974,y:32694,varname:node_1838,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Tex2d,id:5510,x:32550,y:32674,ptovrint:False,ptlb:MineTex,ptin:_MineTex,varname:node_5510,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:778902aa4728a4e458eec2902f2d5b57,ntxv:0,isnm:False|UVIN-5954-OUT;n:type:ShaderForge.SFN_Color,id:8489,x:32550,y:32853,ptovrint:False,ptlb:MainColor,ptin:_MainColor,varname:node_8489,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:4376,x:32877,y:32733,varname:node_4376,prsc:2|A-5510-RGB,B-700-OUT,C-3413-RGB,D-829-OUT;n:type:ShaderForge.SFN_VertexColor,id:3413,x:32550,y:33111,varname:node_3413,prsc:2;n:type:ShaderForge.SFN_Add,id:5954,x:32294,y:32788,varname:node_5954,prsc:2|A-1838-UVOUT,B-1327-OUT;n:type:ShaderForge.SFN_ValueProperty,id:103,x:31281,y:32951,ptovrint:False,ptlb:speed_U,ptin:_speed_U,varname:node_103,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:7900,x:31281,y:33158,ptovrint:False,ptlb:speed_v,ptin:_speed_v,varname:node_7900,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Time,id:7281,x:31281,y:33012,varname:node_7281,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7107,x:31477,y:32951,varname:node_7107,prsc:2|A-103-OUT,B-7281-T;n:type:ShaderForge.SFN_Multiply,id:5861,x:31477,y:33114,varname:node_5861,prsc:2|A-7281-T,B-7900-OUT;n:type:ShaderForge.SFN_Append,id:1327,x:31670,y:33056,varname:node_1327,prsc:2|A-7107-OUT,B-5861-OUT;n:type:ShaderForge.SFN_Tex2d,id:6682,x:32567,y:33259,ptovrint:False,ptlb:mengban,ptin:_mengban,varname:node_6682,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:829,x:32862,y:33048,varname:node_829,prsc:2|A-5510-A,B-3413-A,C-6682-R;n:type:ShaderForge.SFN_Multiply,id:700,x:32785,y:32882,varname:node_700,prsc:2|A-8489-RGB,B-779-OUT;n:type:ShaderForge.SFN_Vector1,id:779,x:32550,y:33004,varname:node_779,prsc:2,v1:2;n:type:ShaderForge.SFN_TexCoord,id:281,x:31942,y:32959,varname:node_281,prsc:2,uv:1,uaff:True;n:type:ShaderForge.SFN_Append,id:3306,x:32120,y:33011,varname:node_3306,prsc:2|A-281-Z,B-281-W;proporder:5510-8489-103-7900-6682;pass:END;sub:END;*/

Shader "Unlit/UV_ani_add_01" {
    Properties {
        _MineTex ("MineTex", 2D) = "white" {}
        _MainColor ("MainColor", Color) = (0.5,0.5,0.5,1)
        _speed_U ("speed_U", Float ) = 0
        _speed_v ("speed_v", Float ) = 0
        _mengban ("mengban", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        LOD 100
        Pass {
            Name "FORWARD"
           /* Tags {
                "LightMode"="ForwardBase"
            }*/
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            uniform sampler2D _MineTex; uniform float4 _MineTex_ST;
            uniform float4 _MainColor;
            uniform float _speed_U;
            uniform float _speed_v;
            uniform sampler2D _mengban; uniform float4 _mengban_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
////// Lighting:
////// Emissive:
                float4 node_7281 = _Time;
                float2 node_5954 = (i.uv0+float2((_speed_U*node_7281.g),(node_7281.g*_speed_v)));
                float4 _MineTex_var = tex2D(_MineTex,TRANSFORM_TEX(node_5954, _MineTex));
                float4 _mengban_var = tex2D(_mengban,TRANSFORM_TEX(i.uv0, _mengban));
                float3 emissive = (_MineTex_var.rgb*(_MainColor.rgb*2.0)*i.vertexColor.rgb*(_MineTex_var.a*i.vertexColor.a*_mengban_var.r));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma only_renderers d3d9 d3d11 glcore gles gles3 metal d3d11_9x ps4 psp2 n3ds wiiu 
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
