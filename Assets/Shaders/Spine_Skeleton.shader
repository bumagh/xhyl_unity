Shader "Spine/Skeleton"
{
  Properties
  {
    _Cutoff ("Shadow alpha cutoff", Range(0, 1)) = 0.1
    [NoScaleOffset] _MainTex ("Main Texture", 2D) = "black" {}
    [Toggle(_STRAIGHT_ALPHA_INPUT)] _StraightAlphaInput ("Straight Alpha Texture", float) = 0
    [HideInInspector] _StencilRef ("Stencil Reference", float) = 1
    [Enum(UnityEngine.Rendering.CompareFunction)] [HideInInspector] _StencilComp ("Stencil Comparison", float) = 8
    [HideInInspector] _OutlineWidth ("Outline Width", Range(0, 8)) = 3
    [HideInInspector] _OutlineColor ("Outline Color", Color) = (1,1,0,1)
    [HideInInspector] _OutlineReferenceTexWidth ("Reference Texture Width", float) = 1024
    [HideInInspector] _ThresholdEnd ("Outline Threshold", Range(0, 1)) = 0.25
    [HideInInspector] _OutlineSmoothness ("Outline Smoothness", Range(0, 1)) = 1
    [MaterialToggle(_USE8NEIGHBOURHOOD_ON)] [HideInInspector] _Use8Neighbourhood ("Sample 8 Neighbours", float) = 1
    [HideInInspector] _OutlineMipLevel ("Outline Mip Level", Range(0, 3)) = 0
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: Normal
    {
      Name "Normal"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 255
        WriteMask 255
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
      Fog
      { 
        Mode  Off
      } 
      Blend One OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_COLOR :COLOR;
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
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_COLOR = in_v.color;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 texColor_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          texColor_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3 = (texColor_1 * in_f.xlv_COLOR);
          out_f.color = tmpvar_3;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
    Pass // ind: 2, name: Caster
    {
      Name "Caster"
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "LIGHTMODE" = "SHADOWCASTER"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
        "SHADOWSUPPORT" = "true"
      }
      Cull Off
      Offset 1, 1
      Stencil
      { 
        Ref 0
        ReadMask 255
        WriteMask 255
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
      Fog
      { 
        Mode  Off
      } 
      Blend One OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile SHADOWS_DEPTH
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 unity_LightShadowBias;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform sampler2D _MainTex;
      uniform float _Cutoff;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.xyz = in_v.texcoord.xyz;
          tmpvar_1.w = in_v.color.w;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          float4 clipPos_4;
          clipPos_4.xyw = tmpvar_2.xyw;
          clipPos_4.z = (tmpvar_2.z + clamp((unity_LightShadowBias.x / tmpvar_2.w), 0, 1));
          clipPos_4.z = lerp(clipPos_4.z, max(clipPos_4.z, (-tmpvar_2.w)), unity_LightShadowBias.y);
          out_v.vertex = clipPos_4;
          out_v.xlv_TEXCOORD1 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = tex2D(_MainTex, in_f.xlv_TEXCOORD1.xy);
          float x_2;
          x_2 = ((tmpvar_1.w * in_f.xlv_TEXCOORD1.w) - _Cutoff);
          if((x_2<0))
          {
              discard;
          }
          out_f.color = float4(0, 0, 0, 0);
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
