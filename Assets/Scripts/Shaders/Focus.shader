Shader "Hidden/Shader/Focus"
{
    Properties
    {
        // This property is necessary to make the CommandBuffer.Blit bind the source texture to _MainTex
        _MainTex("Main Texture", 2DArray) = "grey" {}
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO

    };

    Varyings Vert(Attributes input)
    {
        Varyings output;

        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);

        return output;
    }

    // List of properties to control your post process effect
    float _Intensity;
    float _Threshold;
    float _Distance;
    int _Steps;
    int _Quad;
    int _UseDistance;
    
    TEXTURE2D_X(_MainTex);
    
    // half4 MainFragment(Varyings vri) : SV_Target {
    //
    //     float2 screenUVs = vri.screenPos.xy / vri.screenPos.w;
    //     float zRaw = SampleSceneDepth(screenUVs);
    //     float z01 = Linear01Depth(SampleCameraDepth(screenUVs), _ZBufferParams);
    //     float zEye = LinearEyeDepth(SampleCameraDepth(screenUVs), _ZBufferParams);
    //     return zRaw;
    // }
    
    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        const float offset[] = {0.0, 1.0, 2.0, 3.0, 4.0};
        float depth = LinearEyeDepth(SampleCameraDepth(input.texcoord), _ZBufferParams) * 0.001 * _Intensity;

        float3 outColor = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord).xyz;

        for (int i = 1; i < _Steps; i++) {
            outColor +=
                SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord + float2(depth * offset[i], depth * offset[i])).xyz +
                SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord - float2(depth * offset[i], depth * offset[i])).xyz +
                SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord + float2(-depth * offset[i], depth * offset[i])).xyz +
                SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord - float2(depth * offset[i], -depth * offset[i])).xyz;      
        }

        
// for (int i = 1; i < 5; i++) {
//     FragmentColor +=
//         SceneTexture.Sample(PointSample, ppIn.UV + float2(hstep*offset[i], vstep*offset[i]))*weight[i] +
//         SceneTexture.Sample(PointSample, ppIn.UV - float2(hstep*offset[i], vstep*offset[i]))*weight[i];      
//     }
        
        return float4(outColor / (_Steps + 1), 1);
    }

    ENDHLSL

    SubShader
    {
        Pass
        {
            Name "ZoomBlur"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }

    Fallback Off
}