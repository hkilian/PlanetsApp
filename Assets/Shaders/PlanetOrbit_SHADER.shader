Shader "Unlit/PlanetOrbit_SHADER" {

    Properties {
        // Color property for material inspector, default to white
        _Color("Color", Color) = (0.2,0.2,1,1)
        _ScaleFade("_ScaleFade", float) = 1.0
        _DistanceFade("_DistanceFade", float) = 1.0
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
            float _ScaleFade;
            float _DistanceFade;
            float _FadeT;
            
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
            
                float fade = lerp(_ScaleFade, _DistanceFade, clamp(_FadeT, 0, 1));
                float distance = abs(i.worldPos.z * pow(fade, 7) * 0.001);
                float alpha = clamp(1.0 - distance, 0, 1);
                    
                return fixed4(_Color.rgb, alpha * _Color.a);
                
            }
            ENDCG
        }
    }
}
