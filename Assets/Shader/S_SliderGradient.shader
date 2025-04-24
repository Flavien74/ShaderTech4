Shader "Unlit/S_SliderGradient"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Value ("Slider Value", Range(0, 1)) = 0.5
        _StartColor ("Start Color", Color) = (0,1,0,0.5)
        _EndColor ("End Color", Color) = (1,0,0,1)
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha


        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                fixed4 color : COLOR;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Value;
            float4 _StartColor;
            float4 _EndColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //fixed4 newColor = i.color * tint;

                fixed4 tex = tex2D(_MainTex, i.uv);
                fixed4 tint = lerp(_StartColor.rgba, _EndColor.rgba, _Value);
                return tex*tint;
            }
            ENDCG
        }
    }
}
