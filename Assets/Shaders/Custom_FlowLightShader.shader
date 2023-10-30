// Upgrade NOTE: commented out 'float4 unity_DynamicLightmapST', a built-in variable
// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable

Shader "Custom/FlowLightShader"
{
  Properties
  {
    [HideInInspector] _MainTex ("Base (RGB)", 2D) = "white" {}
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
    _FlashColor ("Flash Color", Color) = (1,1,1,1)
    _Angle ("Flash Angle", Range(0, 180)) = 45
    _Width ("Flash Width", Range(0, 1)) = 0.2
    _LoopTime ("Loop Time", float) = 0.5
    _Interval ("Time Interval", float) = 1.5
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    LOD 200
    Pass // ind: 1, name: FORWARD
    {
      Name "FORWARD"
      Tags
      { 
        "LIGHTMODE" = "FORWARDBASE"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 200
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
      //uniform float4 _Time;
      uniform sampler2D _MainTex;
      uniform float4 _FlashColor;
      uniform float _Angle;
      uniform float _Width;
      uniform float _LoopTime;
      uniform float _Interval;
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
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_1 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          float3x3 tmpvar_4;
          tmpvar_4[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_4[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_4[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          tmpvar_2.xyz = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = tmpvar_1;
          out_v.xlv_TEXCOORD1 = normalize(mul(in_v.normal, tmpvar_4));
          out_v.xlv_TEXCOORD2 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 c_1;
          float3 tmpvar_2;
          float tmpvar_3;
          tmpvar_2 = float3(0, 0, 0);
          tmpvar_3 = 0;
          float3 tmpvar_4;
          float tmpvar_5;
          tmpvar_4 = tmpvar_2;
          tmpvar_5 = tmpvar_3;
          float4 texCol_6;
          float4 tmpvar_7;
          tmpvar_7 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          texCol_6 = tmpvar_7;
          float brightness_8;
          brightness_8 = 0;
          float tmpvar_9;
          tmpvar_9 = (0.0174444 * _Angle);
          float tmpvar_10;
          tmpvar_10 = (1 / (sin(tmpvar_9) / cos(tmpvar_9)));
          float tmpvar_11;
          tmpvar_11 = (_Interval + _LoopTime);
          float tmpvar_12;
          tmpvar_12 = ((_Time.y - (float(int((_Time.y / tmpvar_11))) * tmpvar_11)) - _Interval);
          int tmpvar_13;
          tmpvar_13 = (tmpvar_10>0);
          float tmpvar_14;
          if(tmpvar_13)
          {
              tmpvar_14 = 0;
          }
          else
          {
              tmpvar_14 = tmpvar_10;
          }
          float tmpvar_15;
          if(tmpvar_13)
          {
              tmpvar_15 = (1 + tmpvar_10);
          }
          else
          {
              tmpvar_15 = 1;
          }
          float tmpvar_16;
          tmpvar_16 = (tmpvar_14 + ((tmpvar_12 / _LoopTime) * (tmpvar_15 - tmpvar_14)));
          float tmpvar_17;
          tmpvar_17 = (tmpvar_16 - _Width);
          float tmpvar_18;
          tmpvar_18 = (in_f.xlv_TEXCOORD0.x + (in_f.xlv_TEXCOORD0.y * tmpvar_10));
          if(((tmpvar_18>tmpvar_17) && (tmpvar_18<tmpvar_16)))
          {
              brightness_8 = (1 - (abs(((2 * tmpvar_18) - (tmpvar_17 + tmpvar_16))) / _Width));
          }
          tmpvar_4 = (texCol_6.xyz + (_FlashColor.xyz * brightness_8));
          tmpvar_5 = texCol_6.w;
          tmpvar_2 = tmpvar_4;
          tmpvar_3 = tmpvar_5;
          float4 c_19;
          float4 c_20;
          c_20.xyz = float3(0, 0, 0);
          c_20.w = tmpvar_5;
          c_19.w = c_20.w;
          c_19.xyz = c_20.xyz;
          c_1.w = c_19.w;
          c_1.xyz = float3(tmpvar_4);
          out_f.color = c_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: Meta
    {
      Name "Meta"
      Tags
      { 
        "LIGHTMODE" = "META"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      LOD 200
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
      //uniform float4 _Time;
      uniform sampler2D _MainTex;
      uniform float4 _FlashColor;
      uniform float _Angle;
      uniform float _Width;
      uniform float _LoopTime;
      uniform float _Interval;
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
          float2 tmpvar_1;
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
          tmpvar_1 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, tmpvar_5);
          out_v.xlv_TEXCOORD0 = tmpvar_1;
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
          float tmpvar_4;
          tmpvar_3 = float3(0, 0, 0);
          tmpvar_4 = 0;
          float3 tmpvar_5;
          float tmpvar_6;
          tmpvar_5 = tmpvar_3;
          tmpvar_6 = tmpvar_4;
          float4 texCol_7;
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          texCol_7 = tmpvar_8;
          float brightness_9;
          brightness_9 = 0;
          float tmpvar_10;
          tmpvar_10 = (0.0174444 * _Angle);
          float tmpvar_11;
          tmpvar_11 = (1 / (sin(tmpvar_10) / cos(tmpvar_10)));
          float tmpvar_12;
          tmpvar_12 = (_Interval + _LoopTime);
          float tmpvar_13;
          tmpvar_13 = ((_Time.y - (float(int((_Time.y / tmpvar_12))) * tmpvar_12)) - _Interval);
          int tmpvar_14;
          tmpvar_14 = (tmpvar_11>0);
          float tmpvar_15;
          if(tmpvar_14)
          {
              tmpvar_15 = 0;
          }
          else
          {
              tmpvar_15 = tmpvar_11;
          }
          float tmpvar_16;
          if(tmpvar_14)
          {
              tmpvar_16 = (1 + tmpvar_11);
          }
          else
          {
              tmpvar_16 = 1;
          }
          float tmpvar_17;
          tmpvar_17 = (tmpvar_15 + ((tmpvar_13 / _LoopTime) * (tmpvar_16 - tmpvar_15)));
          float tmpvar_18;
          tmpvar_18 = (tmpvar_17 - _Width);
          float tmpvar_19;
          tmpvar_19 = (in_f.xlv_TEXCOORD0.x + (in_f.xlv_TEXCOORD0.y * tmpvar_11));
          if(((tmpvar_19>tmpvar_18) && (tmpvar_19<tmpvar_17)))
          {
              brightness_9 = (1 - (abs(((2 * tmpvar_19) - (tmpvar_18 + tmpvar_17))) / _Width));
          }
          tmpvar_5 = (texCol_7.xyz + (_FlashColor.xyz * brightness_9));
          tmpvar_6 = texCol_7.w;
          tmpvar_3 = tmpvar_5;
          tmpvar_4 = tmpvar_6;
          tmpvar_2 = tmpvar_5;
          float4 res_20;
          res_20 = float4(0, 0, 0, 0);
          if(unity_MetaFragmentControl.x)
          {
              res_20.w = 1;
              float3 tmpvar_21;
              float _tmp_dvx_5 = clamp(unity_OneOverOutputBoost, 0, 1);
              tmpvar_21 = clamp(pow(float3(0, 0, 0), float3(_tmp_dvx_5, _tmp_dvx_5, _tmp_dvx_5)), float3(0, 0, 0), float3(unity_MaxOutputValue, unity_MaxOutputValue, unity_MaxOutputValue));
              res_20.xyz = float3(tmpvar_21);
          }
          if(unity_MetaFragmentControl.y)
          {
              float3 emission_22;
              if(int(unity_UseLinearSpace))
              {
                  emission_22 = tmpvar_2;
              }
              else
              {
                  emission_22 = (tmpvar_2 * ((tmpvar_2 * ((tmpvar_2 * 0.305306) + 0.6821711)) + 0.01252288));
              }
              float4 tmpvar_23;
              tmpvar_23.w = 1;
              tmpvar_23.xyz = float3(emission_22);
              res_20 = tmpvar_23;
          }
          tmpvar_1 = res_20;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
