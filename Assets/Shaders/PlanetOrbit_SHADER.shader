Shader "Unlit/PlanetOrbit_SHADER" {

    Properties {
        // Color property for material inspector, default to white
        _Color("Color", Color) = (0.2,0.2,1,1)
        _Fade("Fade", float) = 1.0
    }

    SubShader {
        Tags {
            "Queue" = "Transparent"
        }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            fixed4 _Color;
            float _Fade;
            
            struct appdata {
                float4 vertex : POSITION;
            };
            struct v2f {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);
                return o;
            }
            
            // pixel shader
            fixed4 frag(v2f i) : SV_Target {
            
                float distance = abs(i.worldPos.z * _Fade * 0.01);
                float alpha = 1.0-distance;
                    
                return fixed4(_Color.rgb, alpha * _Color.a);
                
            }
            ENDCG
        }
    }
}
