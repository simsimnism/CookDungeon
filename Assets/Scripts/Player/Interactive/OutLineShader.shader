Shader "Custom/Outline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1,0,0,1) // 테두리 색상
        _OutlineThickness ("Outline Thickness", Float) = 0.1 // 테두리 두께
    }
    SubShader
    {
        Tags {"Queue"="Overlay"}
        Pass
        {
            Name "OUTLINE"
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite On

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _OutlineThickness;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float4 originalColor = tex2D(_MainTex, uv);

                // 주위 픽셀 샘플링으로 외곽선 검출
                float4 outlineColor = _OutlineColor;
                float4 col = originalColor;
                
                float2 offsets[8] = {
                    float2(-_OutlineThickness, 0), float2(_OutlineThickness, 0),
                    float2(0, -_OutlineThickness), float2(0, _OutlineThickness),
                    float2(-_OutlineThickness, -_OutlineThickness), float2(_OutlineThickness, -_OutlineThickness),
                    float2(-_OutlineThickness, _OutlineThickness), float2(_OutlineThickness, _OutlineThickness)
                };

                for (int j = 0; j < 8; j++)
                {
                    col += tex2D(_MainTex, uv + offsets[j]);
                }

                // 원본 픽셀이 투명할 경우에만 외곽선 색상을 표시
                if (originalColor.a == 0 && col.a > 0)
                {
                    return outlineColor;
                }

                return originalColor;
            }
            ENDCG
        }
    }
}
