Shader "Custom/BlackToTransparent"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Threshold ("Black Threshold", Range(0.0, 1.0)) = 0.1
        _Alpha ("Alpha", Range(0.0, 1.0)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        
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
            float _Threshold;
            float _Alpha;
            
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
                
                // RGBの平均値を計算して黒の判定を行う
                float grayscale = (col.r + col.g + col.b) / 3.0;
                
                // 黒色（閾値以下）の場合は透明にする
                if (grayscale <= _Threshold)
                {
                    col.a = 0.0;
                }
                else
                {
                    col.a *= _Alpha;
                }
                
                return col;
            }
            ENDCG
        }
    }
}