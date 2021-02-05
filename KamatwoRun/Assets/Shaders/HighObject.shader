Shader "Custom/High"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _MinDist("MinimumDistance",Float) = 0.1
        _MaxDist("MaximumDistance",Float) = 30.0
        _MinAlpha("MinimumAlpha",Range(0.0,1.0)) = 0.0
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
            LOD 200

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            //#pragma surface surf Standard fullforwardshadows alpha
            #pragma surface surf Lambert alpha

            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;

            struct Input
            {
                float2 uv_MainTex;
                float3 worldPos;
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;
            float _MinDist;
            float _MaxDist;
            float _MinAlpha;

            //// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            //// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            //// #pragma instancing_options assumeuniformscaling
            //UNITY_INSTANCING_BUFFER_START(Props)
            //    // put more per-instance properties here
            //UNITY_INSTANCING_BUFFER_END(Props)

            void surf(Input IN, inout SurfaceOutput o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;
                // Metallic and smoothness come from slider variables
                float2 cameraPos_XZ = _WorldSpaceCameraPos.xz;
                float2 worldPos_XZ = IN.worldPos.xz;
                float dist = length(cameraPos_XZ - worldPos_XZ);
                dist = clamp(dist, _MinDist, _MaxDist);
                float t = (dist - _MinDist) / (_MaxDist - _MinDist);
                o.Alpha = lerp(_MinAlpha, 1.0, t);
            }
            ENDCG
        }
            FallBack "Diffuse"
}
