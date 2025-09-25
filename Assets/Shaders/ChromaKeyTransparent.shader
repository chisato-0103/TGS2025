Shader "Custom/ChromaKeyTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ChromaKey ("Chroma Key Color", Color) = (0, 1, 0, 1)  // デフォルトはグリーン
        _Threshold ("Threshold", Range(0, 1)) = 0.1
        _Smoothing ("Smoothing", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
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
            float4 _MainTex_ST;
            fixed4 _ChromaKey;
            float _Threshold;
            float _Smoothing;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // クロマキーカラーとの差を計算
                float chromaDist = distance(col.rgb, _ChromaKey.rgb);

                // アルファ値を計算（スムージング付き）
                float alpha = smoothstep(_Threshold, _Threshold + _Smoothing, chromaDist);

                // 元のカラーを保持し、アルファのみを調整
                col.a *= alpha;

                return col;
            }
            ENDCG
        }
    }
}