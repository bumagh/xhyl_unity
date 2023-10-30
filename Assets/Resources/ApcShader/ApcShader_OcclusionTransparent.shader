// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "ApcShader/OcclusionTransparent"
{
  Properties
  {
    _Color ("Main Color", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
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
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
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
          float3 tmpvar_5;
          tmpvar_5 = _WorldSpaceLightPos0.xyz;
          lightDir_4 = tmpvar_5;
          tmpvar_3 = in_f.xlv_TEXCOORD1;
          float4 tmpvar_6;
          tmpvar_6 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          float tmpvar_7;
          if((tmpvar_6.w>=1))
          {
              tmpvar_7 = 1;
          }
          else
          {
              tmpvar_7 = 0.2;
          }
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_4;
          float4 c_8;
          float4 c_9;
          float diff_10;
          float tmpvar_11;
          tmpvar_11 = max(0, dot(tmpvar_3, tmpvar_2));
          diff_10 = tmpvar_11;
          c_9.xyz = ((tmpvar_6.xyz * tmpvar_1) * diff_10);
          c_9.w = tmpvar_7;
          c_8.w = c_9.w;
          c_8.xyz = c_9.xyz;
          out_f.color = c_8;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDADD"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
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
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
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
          float3 tmpvar_7;
          tmpvar_7 = normalize((_WorldSpaceLightPos0.xyz - in_f.xlv_TEXCOORD2));
          lightDir_6 = tmpvar_7;
          tmpvar_5 = in_f.xlv_TEXCOORD1;
          float4 tmpvar_8;
          tmpvar_8 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color);
          float tmpvar_9;
          if((tmpvar_8.w>=1))
          {
              tmpvar_9 = 1;
          }
          else
          {
              tmpvar_9 = 0.2;
          }
          float4 tmpvar_10;
          tmpvar_10.w = 1;
          tmpvar_10.xyz = in_f.xlv_TEXCOORD2;
          lightCoord_4 = mul(unity_WorldToLight, tmpvar_10).xyz;
          float tmpvar_11;
          float _tmp_dvx_0 = dot(lightCoord_4, lightCoord_4);
          tmpvar_11 = tex2D(_LightTexture0, float2(_tmp_dvx_0, _tmp_dvx_0)).x;
          atten_3 = tmpvar_11;
          tmpvar_1 = _LightColor0.xyz;
          tmpvar_2 = lightDir_6;
          tmpvar_1 = (tmpvar_1 * atten_3);
          float4 c_12;
          float4 c_13;
          float diff_14;
          float tmpvar_15;
          tmpvar_15 = max(0, dot(tmpvar_5, tmpvar_2));
          diff_14 = tmpvar_15;
          c_13.xyz = ((tmpvar_8.xyz * tmpvar_1) * diff_14);
          c_13.w = tmpvar_9;
          c_12.w = c_13.w;
          c_12.xyz = c_13.xyz;
          out_f.color = c_12;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 3, name: Meta
    {
      Name "Meta"
      Tags
      { 
        "LIGHTMODE" = "META"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
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
      uniform float4 _Color;
      uniform float4 unity_MetaFragmentControl;
      uniform float unity_OneOverOutputBoost;
      uniform float unity_MaxOutputValue;
      uniform float unity_UseLinearSpace;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
          float4 texcoord2 :TEXCOORD2;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float3 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 vertex_1;
          vertex_1 = in_v.vertex;
          if(unity_MetaVertexControl.x)
          {
              vertex_1.xy = ((in_v.texcoord1.xy * unity_LightmapST.xy) + unity_LightmapST.zw);
              float tmpvar_2;
              if((in_v.vertex.z>0))
              {
                  tmpvar_2 = 0.0001;
              }
              else
              {
                  tmpvar_2 = 0;
              }
              vertex_1.z = tmpvar_2;
          }
          if(unity_MetaVertexControl.y)
          {
              vertex_1.xy = ((in_v.texcoord2.xy * unity_DynamicLightmapST.xy) + unity_DynamicLightmapST.zw);
              float tmpvar_3;
              if((vertex_1.z>0))
              {
                  tmpvar_3 = 0.0001;
              }
              else
              {
                  tmpvar_3 = 0;
              }
              vertex_1.z = tmpvar_3;
          }
          float4 tmpvar_4;
          tmpvar_4.w = 1;
          tmpvar_4.xyz = vertex_1.xyz;
          out_v.vertex = mul(unity_MatrixVP, tmpvar_4);
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 tmpvar_2;
          float3 tmpvar_3;
          tmpvar_3 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color).xyz;
          tmpvar_2 = tmpvar_3;
          float4 res_4;
          res_4 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              float4 tmpvar_5;
              tmpvar_5.w = 1;
              tmpvar_5.xyz = float3(tmpvar_2);
              res_4.w = tmpvar_5.w;
              float3 tmpvar_6;
              float _tmp_dvx_1 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_6 = clamp(pow(tmpvar_2, float3(_tmp_dvx_1, _tmp_dvx_1, _tmp_dvx_1)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_4.xyz = float3(tmpvar_6);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_7;
              if(int(unity_UseLinearSpace))
              {
                  emission_7 = float3(0, 0, 0);
              }
              else
              {
                  emission_7 = float3(0, 0, 0);
              }
              float4 tmpvar_8;
              tmpvar_8.w = 1;
              tmpvar_8.xyz = float3(emission_7);
              res_4 = tmpvar_8;
          }
          tmpvar_1 = res_4;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
