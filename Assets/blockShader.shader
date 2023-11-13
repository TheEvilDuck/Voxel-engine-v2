// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Bumped shader. Differences from regular Bumped one:
// - no Main Color
// - Normalmap uses Tiling/Offset of the Base texture
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Block" {
Properties {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _TextureScale("Texture scale",float) = 1
    _FModX("FMod x",float) = 0.0625
    _FModY("FMod y",float) = 0.0625
    [NoScaleOffset] _BumpMap ("Normalmap", 2D) = "bump" {}
}

SubShader {
    Tags { "RenderType"="Opaque" }
    LOD 250

CGPROGRAM
#pragma surface surf Lambert noforwardadd

sampler2D _MainTex;
float _TextureScale;
float _FModX;
float _FModY;
//sampler2D _BumpMap;

struct Input {
    float2 uv_MainTex;
    float3 worldPos;
    float3 worldNormal;
};

void surf (Input IN, inout SurfaceOutput o) {
    float isUp = abs(IN.worldNormal.y);

    float x = IN.worldPos.x*_TextureScale; 
    float y = IN.worldPos.y*_TextureScale;
    float z = IN.worldPos.z*_TextureScale;

    float2 offset = float2(fmod(z+x*(1-isUp),_FModX),fmod(y+x*isUp,_FModY));

    fixed4 c = tex2D(_MainTex, IN.uv_MainTex+offset);
    o.Albedo = c.rgb;
    o.Alpha = c.a;
    //o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
}
ENDCG
}

FallBack "Mobile/Diffuse"
}