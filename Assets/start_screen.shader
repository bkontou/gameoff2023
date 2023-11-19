Shader "Custom/start_screen"
{
Properties
{
_MainTex ("Texture", 2D) = "white" {}
_Color("Color", Color) = (1, 1, 1, 1)
_VisibilityRadius("Visibility Radius Filling", Float) = 0
_FullyFadedInDistance("Fully Faded Distance", Float) = 0
}
SubShader
{
// No culling or depth
Cull Off ZWrite On ZTest Always

Tags
{
"Queue" = "AlphaTest"
"RenderType"="Transparent"
}
Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"
#include "UnityStandardUtils.cginc"

float4 _Color;
float4 _EdgeColor;
float _VisibilityRadius;
float _FullyFadedInDistance;
struct Input
{
    float4 vertex : POSITION;
};

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

v2f vert (appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv  = mul(unity_ObjectToWorld, v.vertex);
    return o;
}

sampler2D _MainTex;

uniform float fisheye_power;
uniform float uniform_kernal_val = 0.1111;

float2 distort(float2 pos)
{
    float theta = atan2(pos.y, pos.x);
    float radius = length(pos);
    radius = pow(radius, 1.3);
    pos.x = radius * cos(theta);
    pos.y = radius * sin(theta);

    return 0.5 * (pos + 1.0);
}



float hash(float2 u)
{
    return frac(sin(7.289 * u.x + 11.23 * u.y) * 23758.5453);
}

float mix(float x, float y, float v)
{
    return (y + x) * v;
}

float3 color_hash(float2 pos)
{    
    float3 hash = float3(-0.01 * frac(sin(7.289 * pos.x + 11.23 * pos.y) * 23758.5453),
                         -0.01 * frac(sin(7.289 * pos.y + 11.23 * pos.x) * 23758.5453),
                          0.05 * frac(sin(7.289 * pos.x * 11.23 * pos.y) * 23758.5453));
    return hash;
}

float4 blur(float2 pos)
{
    float4 color = float4(0, 0, 0, 0);
    const float3 direction = float3(-1.0, 0.0, 1.0);
    for (int x = 0; x < 3; x++)
    {
        for (int y = 0; y < 3; y++)
        {
            float2 offset = float2(direction[x], direction[y]) / _ScreenParams.xy;
            color += tex2D(_MainTex, pos + offset) * 0.1111 + 0.1 * float4(color_hash(pos + offset), 1);
            
        }
    }
    return color;
}

fixed4 frag (v2f i) : SV_Target
{
    float2 xy = 2.0 * i.uv - 1.0;
                
    float d = length(xy);

    if (d >= 1.0 && d <= 1.01)
    {
        return float4(0.5, 0.5, 0.5, 1);
    }
    else if (d > 1.01)
    {
        return float4(1, 1, 1, 0);
    }

    float2 uv = i.uv; //distort(xy);
    
    float4 col = float4(0, 0, 0, 1); //blur(uv); //tex2D(_MainTex, uv);
                
    //float3 hash = color_hash(uv);
    //col.rgb += hash;
    
    //return col;
    return col;
}
ENDCG
}
}
}
