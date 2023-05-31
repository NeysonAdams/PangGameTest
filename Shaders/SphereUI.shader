Shader "Custom/SphereUI"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BumpMap("Bumpmap", 2D) = "bump" {}
        _SpecularPower("Specular Power", Range(1, 100)) = 50
        _ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.5
        _Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType" = "TransparentCutout" "Queue" = "Transparent" }
        LOD 100

        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            sampler2D _BumpMap;
            float _SpecularPower;
            float _ShadowIntensity;
            fixed4 _Color;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.uv);
                clip(tex.a - 0.01);

                float2 centeredUV = i.uv * 2 - 1;
                float radialGradient = 1 - dot(centeredUV, centeredUV);
                radialGradient = smoothstep(0, 1, radialGradient);

                fixed4 bump = tex2D(_BumpMap, i.uv);
                float3 lightDirection = normalize(float3(1, 1, 0.5));
                float3 normal = normalize(bump.rgb * 2 - 1);
                float diffuse = max(0, dot(lightDirection, normal));
                float3 reflection = reflect(lightDirection, normal);
                float specular = pow(max(0, -reflection.z), _SpecularPower);

                float shadow = 1 - radialGradient * _ShadowIntensity;

                return float4((tex.rgb * _Color.rgb * diffuse + specular * shadow) * radialGradient, 1.0);
            }
            ENDCG
        }
    }
}
