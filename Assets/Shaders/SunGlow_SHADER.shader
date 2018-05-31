Shader "Unlit/SunGlow_SHADER"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Main Color", Color) = (1,1,1,1)
	}
	SubShader
	{
    
     Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     LOD 100
     
     ZWrite Off
     Blend SrcAlpha OneMinusSrcAlpha 
     
     Color [_Color]

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"
			
			     fixed4 _Color;


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
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				
				// Get the distance to the center (0,0,0)
				//float dis = distance(i.vertex.xyz, float3(0,0,0)) * 0.001;
				
				//float2 offset = float2(_SinTime.z, 0.0) * 0.1;
				
				
				//fixed4 offsetTex  = tex2D(_MainTex, i.uv + float2(_SinTime.x * dis, _SinTime.x * dis));
				//float2 offset = float2(offsetTex.a, offsetTex.a) * 0.1;
				
			    float dis = distance(i.uv, float2(0.5, 0.5)) * 2.5;

				fixed4 col = fixed4(_Color.r, _Color.g, _Color.b, (1-dis)*0.5);
				return col;
				
			}
			ENDCG
		}
	}
}