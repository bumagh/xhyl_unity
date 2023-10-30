Shader "Unity/RimLight"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "white" {}
    _Diffuse ("Diffuse", Color) = (1,1,1,1)
    _RimColor ("RimColor", Color) = (1,1,1,1)
    _RimPower ("RimPower", Range(0, 1)) = 0
  }
  SubShader
  {
    Tags
    { 
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "RenderType" = "Transparent"
      }
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      //uniform float4 glstate_lightmodel_ambient;
      uniform float4 _LightColor0;
      uniform sampler2D _MainTex;
      uniform float4 _Diffuse;
      uniform float4 _RimColor;
      uniform float _RimPower;
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
          float3 xlv_TEXCOORD3 :TEXCOORD3;
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
          float3 tmpvar_2;
          tmpvar_2 = mul(unity_ObjectToWorld, in_v.vertex).xyz;
          float3x3 tmpvar_3;
          tmpvar_3[0] = conv_mxt4x4_0(unity_WorldToObject).xyz;
          tmpvar_3[1] = conv_mxt4x4_1(unity_WorldToObject).xyz;
          tmpvar_3[2] = conv_mxt4x4_2(unity_WorldToObject).xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.xlv_TEXCOORD1 = (_WorldSpaceLightPos0.xyz - (tmpvar_2 * _WorldSpaceLightPos0.w));
          out_v.xlv_TEXCOORD2 = normalize(mul(in_v.normal, tmpvar_3));
          out_v.xlv_TEXCOORD3 = (_WorldSpaceCameraPos - tmpvar_2);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float3 texColor_2;
          float3 tmpvar_3;
          tmpvar_3 = normalize(in_f.xlv_TEXCOORD2);
          float3 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, in_f.xlv_TEXCOORD0).xyz;
          texColor_2 = tmpvar_4;
          float4 tmpvar_5;
          tmpvar_5.w = 1;
          tmpvar_5.xyz = (((((_LightColor0.xyz * _Diffuse.xyz) * texColor_2) * ((dot(tmpvar_3, normalize(in_f.xlv_TEXCOORD1)) * 0.5) + 0.5)) + ((glstate_lightmodel_ambient * 2).xyz * texColor_2)) + (_RimColor * pow((1 - clamp(dot(tmpvar_3, normalize(in_f.xlv_TEXCOORD3)), 0, 1)), (1 / _RimPower))).xyz);
          tmpvar_1 = tmpvar_5;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "Diffuse"
}
