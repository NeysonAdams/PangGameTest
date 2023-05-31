Shader "Custom/ZShape2D"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Line Color", Color) = (1, 1, 1, 1)
        _LineWidth ("Line Width", Range(0.01, 0.5)) = 0.05
        _Frequency ("Frequency", Range(0.1, 10)) = 1.0
        _Amplitude ("Amplitude", Range(0, 0.5)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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
            fixed4 _Color;
            float _LineWidth;
            float _Frequency;
            float _Amplitude;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float x = i.uv.x - _Amplitude * sin(2.0 * 3.14159 * _Frequency * i.uv.y);
                float dist = abs(x - 0.5);
                float mask = 1.0 - step(_LineWidth, dist);
                fixed4 col = _Color * tex2D(_MainTex, i.uv);
                col.rgb *= mask;
                col.a = mask;
                return col;
            }
            ENDCG
        }
    }
}
