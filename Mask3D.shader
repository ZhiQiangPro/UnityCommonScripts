Shader "Masked/Mask" {
SubShader { 
 Tags { "QUEUE"="Geometry+10" }
 Pass {
  Tags { "QUEUE"="Geometry+10" }
  ColorMask 0
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityShaderVariables.cginc"
#pragma multi_compile_fog
#include "UnityCG.cginc"
#define USING_FOG (defined(FOG_LINEAR) || defined(FOG_EXP) || defined(FOG_EXP2))

// uniforms

// vertex shader input data
struct appdata {
  float3 pos : POSITION;
  half4 color : COLOR;
};

// vertex-to-fragment interpolators
struct v2f {
  fixed4 color : COLOR0;
  #if USING_FOG
    fixed fog : TEXCOORD0;
  #endif
  float4 pos : SV_POSITION;
};

// vertex shader
v2f vert (appdata IN) {
  v2f o;
  half4 color = IN.color;
  float3 eyePos = mul (UNITY_MATRIX_MV, float4(IN.pos,1)).xyz;
  half3 viewDir = 0.0;
  o.color = saturate(color);
  // compute texture coordinates
  // fog
  #if USING_FOG
    float fogCoord = length(eyePos.xyz); // radial fog distance
    UNITY_CALC_FOG_FACTOR(fogCoord);
    o.fog = saturate(unityFogFactor);
  #endif
  // transform position
  o.pos = mul(UNITY_MATRIX_MVP, float4(IN.pos,1));
  return o;
}

// fragment shader
fixed4 frag (v2f IN) : SV_Target {
  fixed4 col;
  col = IN.color;
  // fog
  #if USING_FOG
    col.rgb = lerp (unity_FogColor.rgb, col.rgb, IN.fog);
  #endif
  return col;
}
ENDCG
 }
}
}
