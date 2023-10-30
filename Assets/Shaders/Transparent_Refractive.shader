// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "Transparent/Refractive"
{
  Properties
  {
    _MainTex ("Base (RGB), Alpha (A)", 2D) = "white" {}
    _BumpMap ("Normal Map (RGB)", 2D) = "bump" {}
    _Mask ("Specularity (R), Shininess (G), Refraction (B)", 2D) = "black" {}
    _Color ("Color Tint", Color) = (1,1,1,1)
    _Specular ("Specular Color", Color) = (0,0,0,0)
    _Focus ("Focus", Range(-100, 100)) = -100
    _Shininess ("Shininess", Range(0.01, 1)) = 0.2
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent+1"
      "RenderType" = "Transparent"
    }
    LOD 500
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 500
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          if((in_f.xlv_COLOR0.w<=0))
          {
              discard;
          }
          out_f.color = in_f.xlv_COLOR0;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent+1"
      "RenderType" = "Transparent"
    }
    LOD 400
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _Mask;
      uniform float4 _Color;
      uniform float4 _Specular;
      uniform float _Shininess;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 worldBinormal_1;
          float tangentSign_2;
          float3 worldTangent_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          float3 tmpvar_5;
          tmpvar_5 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_6;
          tmpvar_6[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_6[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_6[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_7;
          tmpvar_7 = normalize(mul(in_v.normal, tmpvar_6));
          float3x3 tmpvar_8;
          tmpvar_8[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_8[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_8[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(mul(tmpvar_8, in_v.tangent.xyz));
          worldTangent_3 = tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_2 = tmpvar_10;
          float3 tmpvar_11;
          tmpvar_11 = (((tmpvar_7.yzx * worldTangent_3.zxy) - (tmpvar_7.zxy * worldTangent_3.yzx)) * tangentSign_2);
          worldBinormal_1 = tmpvar_11;
          float4 tmpvar_12;
          tmpvar_12.x = worldTangent_3.x;
          tmpvar_12.y = worldBinormal_1.x;
          tmpvar_12.z = tmpvar_7.x;
          tmpvar_12.w = tmpvar_5.x;
          float4 tmpvar_13;
          tmpvar_13.x = worldTangent_3.y;
          tmpvar_13.y = worldBinormal_1.y;
          tmpvar_13.z = tmpvar_7.y;
          tmpvar_13.w = tmpvar_5.y;
          float4 tmpvar_14;
          tmpvar_14.x = worldTangent_3.z;
          tmpvar_14.y = worldBinormal_1.z;
          tmpvar_14.z = tmpvar_7.z;
          tmpvar_14.w = tmpvar_5.z;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_12;
          out_v.xlv_TEXCOORD2 = tmpvar_13;
          out_v.xlv_TEXCOORD3 = tmpvar_14;
          out_v.xlv_COLOR0 = in_v.color;
          out_v.xlv_TEXCOORD4 = float3(0, 0, 0);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 worldN_1;
          float4 c_2;
          float3 tmpvar_3;
          float3 worldViewDir_4;
          float3 lightDir_5;
          float3 _unity_tbn_2_6;
          float3 _unity_tbn_1_7;
          float3 _unity_tbn_0_8;
          float4 tmpvar_9;
          float3 tmpvar_10;
          tmpvar_10 = in_f.xlv_TEXCOORD1.xyz;
          _unity_tbn_0_8 = tmpvar_10;
          float3 tmpvar_11;
          tmpvar_11 = in_f.xlv_TEXCOORD2.xyz;
          _unity_tbn_1_7 = tmpvar_11;
          float3 tmpvar_12;
          tmpvar_12 = in_f.xlv_TEXCOORD3.xyz;
          _unity_tbn_2_6 = tmpvar_12;
          float3 tmpvar_13;
          tmpvar_13.x = in_f.xlv_TEXCOORD1.w;
          tmpvar_13.y = in_f.xlv_TEXCOORD2.w;
          tmpvar_13.z = in_f.xlv_TEXCOORD3.w;
          float3 tmpvar_14;
          tmpvar_14 = _WorldSpaceLightPos0.xyz;
          lightDir_5 = tmpvar_14;
          worldViewDir_4 = normalize((_WorldSpaceCameraPos - tmpvar_13));
          tmpvar_9 = in_f.xlv_COLOR0;
          float3 tmpvar_15;
          float3 tmpvar_16;
          float tmpvar_17;
          float tmpvar_18;
          float4 col_19;
          float3 mask_20;
          float3 nm_21;
          float4 tex_22;
          float4 tmpvar_23;
          tmpvar_23 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_22 = tmpvar_23;
          float3 tmpvar_24;
          tmpvar_24 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0).xyz * 2) - 1);
          nm_21 = tmpvar_24;
          float3 tmpvar_25;
          tmpvar_25 = tex2D(_Mask, in_f.xlv_TEXCOORD0).xyz;
          mask_20 = tmpvar_25;
          col_19.xyz = (tmpvar_9.xyz * tex_22.xyz);
          float3 tmpvar_26;
          float _tmp_dvx_2 = (mask_20.z * 0.5);
          tmpvar_26 = float3(_tmp_dvx_2, _tmp_dvx_2, _tmp_dvx_2);
          float3 tmpvar_27;
          tmpvar_27 = lerp(col_19.xyz, _Color.xyz, tmpvar_26);
          col_19.xyz = float3(tmpvar_27);
          col_19.w = ((tmpvar_9.w * _Color.w) * tex_22.w);
          tmpvar_15 = col_19.xyz;
          tmpvar_16 = nm_21;
          tmpvar_17 = (_Shininess * mask_20.y);
          tmpvar_18 = col_19.w;
          c_2.w = 0;
          float tmpvar_28;
          tmpvar_28 = dot(_unity_tbn_0_8, tmpvar_16);
          worldN_1.x = tmpvar_28;
          float tmpvar_29;
          tmpvar_29 = dot(_unity_tbn_1_7, tmpvar_16);
          worldN_1.y = tmpvar_29;
          float tmpvar_30;
          tmpvar_30 = dot(_unity_tbn_2_6, tmpvar_16);
          worldN_1.z = tmpvar_30;
          float3 tmpvar_31;
          tmpvar_31 = normalize(worldN_1);
          worldN_1 = tmpvar_31;
          tmpvar_3 = tmpvar_31;
          c_2.xyz = (tmpvar_15 * in_f.xlv_TEXCOORD4);
          float3 lightDir_32;
          lightDir_32 = lightDir_5;
          float3 viewDir_33;
          viewDir_33 = worldViewDir_4;
          float4 c_34;
          float shininess_35;
          float3 nNormal_36;
          float3 tmpvar_37;
          tmpvar_37 = normalize(tmpvar_3);
          nNormal_36 = tmpvar_37;
          float tmpvar_38;
          tmpvar_38 = ((tmpvar_17 * 250) + 4);
          shininess_35 = tmpvar_38;
          c_34.xyz = (((tmpvar_15 * max(0, dot(nNormal_36, lightDir_32))) + (_Specular.xyz * (pow(max(0, dot((-viewDir_33), (lightDir_32 - (2 * (dot(nNormal_36, lightDir_32) * nNormal_36))))), shininess_35) * mask_20.x))) * _LightColor0.xyz);
          c_34.xyz = (c_34.xyz * 2);
          c_34.w = tmpvar_18;
          c_2 = (c_2 + c_34);
          out_f.color = c_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _MainTex_ST;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform sampler2D _MainTex;
      uniform sampler2D _BumpMap;
      uniform sampler2D _Mask;
      uniform float4 _Color;
      uniform float4 _Specular;
      uniform float _Shininess;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float3 xlv_TEXCOORD4 :TEXCOORD4;
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float3 worldBinormal_1;
          float tangentSign_2;
          float3 worldTangent_3;
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_5;
          tmpvar_5[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_5[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_5[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_6;
          tmpvar_6 = normalize(mul(in_v.normal, tmpvar_5));
          float3x3 tmpvar_7;
          tmpvar_7[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_7[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_7[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_8;
          tmpvar_8 = normalize(mul(tmpvar_7, in_v.tangent.xyz));
          worldTangent_3 = tmpvar_8;
          float tmpvar_9;
          tmpvar_9 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_2 = tmpvar_9;
          float3 tmpvar_10;
          tmpvar_10 = (((tmpvar_6.yzx * worldTangent_3.zxy) - (tmpvar_6.zxy * worldTangent_3.yzx)) * tangentSign_2);
          worldBinormal_1 = tmpvar_10;
          float3 tmpvar_11;
          tmpvar_11.x = worldTangent_3.x;
          tmpvar_11.y = worldBinormal_1.x;
          tmpvar_11.z = tmpvar_6.x;
          float3 tmpvar_12;
          tmpvar_12.x = worldTangent_3.y;
          tmpvar_12.y = worldBinormal_1.y;
          tmpvar_12.z = tmpvar_6.y;
          float3 tmpvar_13;
          tmpvar_13.x = worldTangent_3.z;
          tmpvar_13.y = worldBinormal_1.z;
          tmpvar_13.z = tmpvar_6.z;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_4));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_11;
          out_v.xlv_TEXCOORD2 = tmpvar_12;
          out_v.xlv_TEXCOORD3 = tmpvar_13;
          float4 tmpvar_14;
          tmpvar_14 = mul(unity_ObjectToWorld, in_v.vertex);
          out_v.xlv_TEXCOORD4 = tmpvar_14.xyz;
          out_v.xlv_COLOR0 = in_v.color;
          out_v.xlv_TEXCOORD5 = mul(unity_WorldToLight, tmpvar_14).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 worldN_1;
          float4 c_2;
          float atten_3;
          float3 lightCoord_4;
          float3 tmpvar_5;
          float3 worldViewDir_6;
          float3 lightDir_7;
          float3 _unity_tbn_2_8;
          float3 _unity_tbn_1_9;
          float3 _unity_tbn_0_10;
          float4 tmpvar_11;
          _unity_tbn_0_10 = in_f.xlv_TEXCOORD1;
          _unity_tbn_1_9 = in_f.xlv_TEXCOORD2;
          _unity_tbn_2_8 = in_f.xlv_TEXCOORD3;
          float3 tmpvar_12;
          tmpvar_12 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD4));
          lightDir_7 = tmpvar_12;
          worldViewDir_6 = normalize((_WorldSpaceCameraPos - in_f.xlv_TEXCOORD4));
          tmpvar_11 = in_f.xlv_COLOR0;
          float3 tmpvar_13;
          float3 tmpvar_14;
          float tmpvar_15;
          float tmpvar_16;
          float4 col_17;
          float3 mask_18;
          float3 nm_19;
          float4 tex_20;
          float4 tmpvar_21;
          tmpvar_21 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_20 = tmpvar_21;
          float3 tmpvar_22;
          tmpvar_22 = ((tex2D(_BumpMap, in_f.xlv_TEXCOORD0).xyz * 2) - 1);
          nm_19 = tmpvar_22;
          float3 tmpvar_23;
          tmpvar_23 = tex2D(_Mask, in_f.xlv_TEXCOORD0).xyz;
          mask_18 = tmpvar_23;
          col_17.xyz = (tmpvar_11.xyz * tex_20.xyz);
          float3 tmpvar_24;
          float _tmp_dvx_3 = (mask_18.z * 0.5);
          tmpvar_24 = float3(_tmp_dvx_3, _tmp_dvx_3, _tmp_dvx_3);
          float3 tmpvar_25;
          tmpvar_25 = lerp(col_17.xyz, _Color.xyz, tmpvar_24);
          col_17.xyz = float3(tmpvar_25);
          col_17.w = ((tmpvar_11.w * _Color.w) * tex_20.w);
          tmpvar_13 = col_17.xyz;
          tmpvar_14 = nm_19;
          tmpvar_15 = (_Shininess * mask_18.y);
          tmpvar_16 = col_17.w;
          float4 tmpvar_26;
          tmpvar_26.w = 1;
          tmpvar_26.xyz = in_f.xlv_TEXCOORD4;
          lightCoord_4 = mul(unity_WorldToLight, tmpvar_26).xyz;
          float tmpvar_27;
          float _tmp_dvx_4 = dot(lightCoord_4, lightCoord_4);
          tmpvar_27 = tex2D(_LightTexture0, float2(_tmp_dvx_4, _tmp_dvx_4)).x;
          atten_3 = tmpvar_27;
          float tmpvar_28;
          tmpvar_28 = dot(_unity_tbn_0_10, tmpvar_14);
          worldN_1.x = tmpvar_28;
          float tmpvar_29;
          tmpvar_29 = dot(_unity_tbn_1_9, tmpvar_14);
          worldN_1.y = tmpvar_29;
          float tmpvar_30;
          tmpvar_30 = dot(_unity_tbn_2_8, tmpvar_14);
          worldN_1.z = tmpvar_30;
          float3 tmpvar_31;
          tmpvar_31 = normalize(worldN_1);
          worldN_1 = tmpvar_31;
          tmpvar_5 = tmpvar_31;
          float3 lightDir_32;
          lightDir_32 = lightDir_7;
          float3 viewDir_33;
          viewDir_33 = worldViewDir_6;
          float atten_34;
          atten_34 = atten_3;
          float4 c_35;
          float shininess_36;
          float3 nNormal_37;
          float3 tmpvar_38;
          tmpvar_38 = normalize(tmpvar_5);
          nNormal_37 = tmpvar_38;
          float tmpvar_39;
          tmpvar_39 = ((tmpvar_15 * 250) + 4);
          shininess_36 = tmpvar_39;
          float3 tmpvar_40;
          tmpvar_40 = normalize(lightDir_32);
          lightDir_32 = tmpvar_40;
          c_35.xyz = (((tmpvar_13 * max(0, dot(nNormal_37, tmpvar_40))) + (_Specular.xyz * (pow(max(0, dot((-viewDir_33), (tmpvar_40 - (2 * (dot(nNormal_37, tmpvar_40) * nNormal_37))))), shininess_36) * mask_18.x))) * _LightColor0.xyz);
          c_35.xyz = (c_35.xyz * (atten_34 * 2));
          c_35.w = tmpvar_16;
          c_2 = c_35;
          out_f.color = c_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: Meta
    {
      Name "Meta"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "META"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 400
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4 unity_WorldTransformParams;
      //uniform float4x4 unity_MatrixVP;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
      uniform float4 _Color;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 tangent :TANGENT;
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.color;
          float3 worldBinormal_2;
          float tangentSign_3;
          float3 worldTangent_4;
          float4 vertex_5;
          vertex_5 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_5.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_6;
              if((in_v.vertex.z>0))
              {
                  tmpvar_6 = 0.0001;
              }
              else
              {
                  tmpvar_6 = 0;
              }
              vertex_5.z = tmpvar_6;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_5.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_7;
              if((vertex_5.z>0))
              {
                  tmpvar_7 = 0.0001;
              }
              else
              {
                  tmpvar_7 = 0;
              }
              vertex_5.z = tmpvar_7;
          }
          float4 tmpvar_8;
          tmpvar_8.w = 1;
          tmpvar_8.xyz = vertex_5.xyz;
          float3x3 tmpvar_9;
          tmpvar_9[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_9[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_9[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          float3 tmpvar_10;
          tmpvar_10 = normalize(mul(in_v.normal, tmpvar_9));
          float3x3 tmpvar_11;
          tmpvar_11[0] = conv_mxt4x4_0(unity_ObjectToWorld).xyz;
          tmpvar_11[1] = conv_mxt4x4_1(unity_ObjectToWorld).xyz;
          tmpvar_11[2] = conv_mxt4x4_2(unity_ObjectToWorld).xyz;
          float3 tmpvar_12;
          tmpvar_12 = normalize(mul(tmpvar_11, in_v.tangent.xyz));
          worldTangent_4 = tmpvar_12;
          float tmpvar_13;
          tmpvar_13 = (in_v.tangent.w * unity_WorldTransformParams.w);
          tangentSign_3 = tmpvar_13;
          float3 tmpvar_14;
          tmpvar_14 = (((tmpvar_10.yzx * worldTangent_4.zxy) - (tmpvar_10.zxy * worldTangent_4.yzx)) * tangentSign_3);
          worldBinormal_2 = tmpvar_14;
          float3 tmpvar_15;
          tmpvar_15.x = worldTangent_4.x;
          tmpvar_15.y = worldBinormal_2.x;
          tmpvar_15.z = tmpvar_10.x;
          float3 tmpvar_16;
          tmpvar_16.x = worldTangent_4.y;
          tmpvar_16.y = worldBinormal_2.y;
          tmpvar_16.z = tmpvar_10.y;
          float3 tmpvar_17;
          tmpvar_17.x = worldTangent_4.z;
          tmpvar_17.y = worldBinormal_2.z;
          tmpvar_17.z = tmpvar_10.z;
          out_v.vertex = mul(unity_MatrixVP, tmpvar_8);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = tmpvar_15;
          out_v.xlv_TEXCOORD2 = tmpvar_16;
          out_v.xlv_TEXCOORD3 = tmpvar_17;
          out_v.xlv_COLOR0 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3 = in_f.xlv_COLOR0;
          float3 tmpvar_4;
          float4 col_5;
          float3 mask_6;
          float4 tex_7;
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_7 = tmpvar_8;
          float3 tmpvar_9;
          tmpvar_9 = tex2D(_Mask, in_f.xlv_TEXCOORD0).xyz;
          mask_6 = tmpvar_9;
          col_5.xyz = (tmpvar_3.xyz * tex_7.xyz);
          float3 tmpvar_10;
          float _tmp_dvx_5 = (mask_6.z * 0.5);
          tmpvar_10 = float3(_tmp_dvx_5, _tmp_dvx_5, _tmp_dvx_5);
          float3 tmpvar_11;
          tmpvar_11 = lerp(col_5.xyz, _Color.xyz, tmpvar_10);
          col_5.xyz = float3(tmpvar_11);
          col_5.w = ((tmpvar_3.w * _Color.w) * tex_7.w);
          tmpvar_4 = col_5.xyz;
          tmpvar_2 = tmpvar_4;
          float4 res_12;
          res_12 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_13;
              tmpvar_13.w = 1;
              tmpvar_13.xyz = float3(tmpvar_2);
              res_12.w = tmpvar_13.w;
              float3 tmpvar_14;
              float _tmp_dvx_6 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_14 = clamp(pow(tmpvar_2, float3(_tmp_dvx_6, _tmp_dvx_6, _tmp_dvx_6)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_12.xyz = float3(tmpvar_14);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_15;
              if(int(unity_UseLinearSpace))
              {
                  emission_15 = float3(0, 0, 0);
              }
              else
              {
                  emission_15 = float3(0, 0, 0);
              }
              float4 tmpvar_16;
              tmpvar_16.w = 1;
              tmpvar_16.xyz = float3(emission_15);
              res_12 = tmpvar_16;
          }
          tmpvar_1 = res_12;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent+1"
      "RenderType" = "Transparent"
    }
    LOD 300
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile DIRECTIONAL
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2.w = 1;
          tmpvar_2.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_3;
          tmpvar_3[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_3[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_3[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          tmpvar_1.xyz = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_2));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = normalize(mul(in_v.normal, tmpvar_3));
          out_v.xlv_TEXCOORD2 = tmpvar_1;
          out_v.xlv_COLOR0 = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          float3 lightDir_4;
          float4 tmpvar_5;
          float3 tmpvar_6;
          tmpvar_6 = _WorldSpaceLightPos0.xyz;
          lightDir_4 = tmpvar_6;
          tmpvar_5 = in_f.xlv_COLOR0;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_7;
          float tmpvar_8;
          float4 col_9;
          float3 mask_10;
          float4 tex_11;
          float4 tmpvar_12;
          tmpvar_12 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_11 = tmpvar_12;
          float3 tmpvar_13;
          tmpvar_13 = tex2D(_Mask, in_f.xlv_TEXCOORD0).xyz;
          mask_10 = tmpvar_13;
          col_9.xyz = (tmpvar_5.xyz * tex_11.xyz);
          float3 tmpvar_14;
          float _tmp_dvx_7 = (mask_10.z * 0.5);
          tmpvar_14 = float3(_tmp_dvx_7, _tmp_dvx_7, _tmp_dvx_7);
          float3 tmpvar_15;
          tmpvar_15 = lerp(col_9.xyz, _Color.xyz, tmpvar_14);
          col_9.xyz = float3(tmpvar_15);
          col_9.w = ((tmpvar_5.w * _Color.w) * tex_11.w);
          tmpvar_7 = col_9.xyz;
          tmpvar_8 = col_9.w;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_4;
          float4 c_16;
          float4 c_17;
          float diff_18;
          float tmpvar_19;
          tmpvar_19 = max(0, dot(tmpvar_3, tmpvar_2));
          diff_18 = tmpvar_19;
          c_17.xyz = float3(((tmpvar_7 * tmpvar_1) * diff_18));
          c_17.w = tmpvar_8;
          c_16.w = c_17.w;
          c_16.xyz = c_17.xyz;
          out_f.color = c_16;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile POINT
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4x4 unity_WorldToLight;
      uniform float4 _MainTex_ST;
      //uniform float4 _WorldSpaceLightPos0;
      uniform float4 _LightColor0;
      uniform sampler2D _LightTexture0;
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_COLOR0 :COLOR0;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          float3x3 tmpvar_2;
          tmpvar_2[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_2[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_2[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = normalize(mul(in_v.normal, tmpvar_2));
          float4 tmpvar_3;
          tmpvar_3 = mul(unity_ObjectToWorld, in_v.vertex);
          out_v.xlv_TEXCOORD2 = tmpvar_3.xyz;
          out_v.xlv_COLOR0 = in_v.color;
          out_v.xlv_TEXCOORD3 = mul(unity_WorldToLight, tmpvar_3).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float3 tmpvar_1;
          float3 tmpvar_2;
          float atten_3;
          float3 lightCoord_4;
          float3 tmpvar_5;
          float3 lightDir_6;
          float4 tmpvar_7;
          float3 tmpvar_8;
          tmpvar_8 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_6 = tmpvar_8;
          tmpvar_7 = in_f.xlv_COLOR0;
          tmpvar_5 = in_f.xlv_TEXCOORD1;
          float3 tmpvar_9;
          float tmpvar_10;
          float4 col_11;
          float3 mask_12;
          float4 tex_13;
          float4 tmpvar_14;
          tmpvar_14 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_13 = tmpvar_14;
          float3 tmpvar_15;
          tmpvar_15 = tex2D(_Mask, in_f.xlv_TEXCOORD0).xyz;
          mask_12 = tmpvar_15;
          col_11.xyz = (tmpvar_7.xyz * tex_13.xyz);
          float3 tmpvar_16;
          float _tmp_dvx_8 = (mask_12.z * 0.5);
          tmpvar_16 = float3(_tmp_dvx_8, _tmp_dvx_8, _tmp_dvx_8);
          float3 tmpvar_17;
          tmpvar_17 = lerp(col_11.xyz, _Color.xyz, tmpvar_16);
          col_11.xyz = float3(tmpvar_17);
          col_11.w = ((tmpvar_7.w * _Color.w) * tex_13.w);
          tmpvar_9 = col_11.xyz;
          tmpvar_10 = col_11.w;
          float4 tmpvar_18;
          tmpvar_18.w = 1;
          tmpvar_18.xyz = in_f.xlv_TEXCOORD2;
          lightCoord_4 = mul(unity_WorldToLight, tmpvar_18).xyz;
          float tmpvar_19;
          float _tmp_dvx_9 = dot(lightCoord_4, lightCoord_4);
          tmpvar_19 = tex2D(_LightTexture0, float2(_tmp_dvx_9, _tmp_dvx_9)).x;
          atten_3 = tmpvar_19;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_6;
          tmpvar_1 = (tmpvar_1 * atten_3);
          float4 c_20;
          float4 c_21;
          float diff_22;
          float tmpvar_23;
          tmpvar_23 = max(0, dot(tmpvar_5, tmpvar_2));
          diff_22 = tmpvar_23;
          c_21.xyz = float3(((tmpvar_9 * tmpvar_1) * diff_22));
          c_21.w = tmpvar_10;
          c_20.w = c_21.w;
          c_20.xyz = c_21.xyz;
          out_f.color = c_20;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: Meta
    {
      Name "Meta"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "META"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 300
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      // uniform float4 unity_LightmapST;
      // uniform float4 unity_DynamicLightmapST;
      uniform float4 unity_MetaVertexControl;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
      uniform float4 _Color;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_COLOR0 :COLOR0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR0 :COLOR0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.color;
          float4 vertex_2;
          vertex_2 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_2.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_3;
              if((in_v.vertex.z>0))
              {
                  tmpvar_3 = 0.0001;
              }
              else
              {
                  tmpvar_3 = 0;
              }
              vertex_2.z = tmpvar_3;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_2.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_4;
              if((vertex_2.z>0))
              {
                  tmpvar_4 = 0.0001;
              }
              else
              {
                  tmpvar_4 = 0;
              }
              vertex_2.z = tmpvar_4;
          }
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = vertex_2.xyz;
          out_v.vertex = mul(unity_MatrixVP, tmpvar_5);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3 = in_f.xlv_COLOR0;
          float3 tmpvar_4;
          float4 col_5;
          float3 mask_6;
          float4 tex_7;
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tex_7 = tmpvar_8;
          float3 tmpvar_9;
          tmpvar_9 = tex2D(_Mask, in_f.xlv_TEXCOORD0).xyz;
          mask_6 = tmpvar_9;
          col_5.xyz = (tmpvar_3.xyz * tex_7.xyz);
          float3 tmpvar_10;
          float _tmp_dvx_10 = (mask_6.z * 0.5);
          tmpvar_10 = float3(_tmp_dvx_10, _tmp_dvx_10, _tmp_dvx_10);
          float3 tmpvar_11;
          tmpvar_11 = lerp(col_5.xyz, _Color.xyz, tmpvar_10);
          col_5.xyz = float3(tmpvar_11);
          col_5.w = ((tmpvar_3.w * _Color.w) * tex_7.w);
          tmpvar_4 = col_5.xyz;
          tmpvar_2 = tmpvar_4;
          float4 res_12;
          res_12 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_13;
              tmpvar_13.w = 1;
              tmpvar_13.xyz = float3(tmpvar_2);
              res_12.w = tmpvar_13.w;
              float3 tmpvar_14;
              float _tmp_dvx_11 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_14 = clamp(pow(tmpvar_2, float3(_tmp_dvx_11, _tmp_dvx_11, _tmp_dvx_11)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_12.xyz = float3(tmpvar_14);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_15;
              if(int(unity_UseLinearSpace))
              {
                  emission_15 = float3(0, 0, 0);
              }
              else
              {
                  emission_15 = float3(0, 0, 0);
              }
              float4 tmpvar_16;
              tmpvar_16.w = 1;
              tmpvar_16.xyz = float3(emission_15);
              res_12 = tmpvar_16;
          }
          tmpvar_1 = res_12;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent+1"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent+1"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask RGB
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          col_1 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * in_f.xlv_COLOR0);
          if((col_1.w<=0.01))
          {
              discard;
          }
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
