// adapted from: https://www.shadertoy.com/view/WdSGz1
// TODO: check out this one as well: https://github.com/andydbc/unity-frosted-glass/blob/master/Assets/FrostedGlass/Shaders/FrostedGlass.shader

Shader "Unlit/frosted-glass"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            /*
            float stepfunc(float x) {
                return (sign(x) + 1.0 / 2.0);
            }

            float square(float2 pos) {
                return (stepfunc(pos.x + 1.0) * stepfunc(1.0 - pos.x)) * (stepfunc(pos.y + 1.0) * stepfunc(1.0 - pos.y));
            }*/

            float2 dist(float2 pos) {
                float2 texcoord = (pos - 0.5) * 1.5;
                return pos * tex2D(_MainTex, texcoord).xy * 0.01;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);

                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);

                //return col;

                return tex2D(_MainTex, dist(i.uv)) * 1.7;
            }
            ENDCG
        }
    }
}
