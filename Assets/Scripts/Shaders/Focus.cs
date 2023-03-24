using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Focus")]
public sealed class Focus : CustomPostProcessVolumeComponent, IPostProcessComponent {
    [Tooltip("Controls the intensity of the effect")]
    public ClampedFloatParameter intensity = new(0f, 0f, 1f);
    [Tooltip("Number of blur steps")]
    public ClampedIntParameter steps = new(3, 1, 20);
    [Tooltip("Controls safe zone of effect")]
    public ClampedFloatParameter threshold = new(0f, 0f, 0.5f);
    [Tooltip("Use quadratic function")]
    public BoolParameter quad = new(false);
    [Tooltip("Clamp max distance")]
    public BoolParameter useConstantDistance = new(false);
    
    public ClampedFloatParameter distance = new(5f, 1f, 10f);

    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup() {
        if (Shader.Find("Hidden/Shader/Focus") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/Focus"));
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination) {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetInt("_Steps", steps.value);
        m_Material.SetFloat("_Threshold", threshold.value);
        m_Material.SetFloat("_Distance", distance.value);
        m_Material.SetInt("_Quad", quad.value ? 1 : 0);
        m_Material.SetInt("_UseDistance", useConstantDistance.value ? 1 : 0);
        cmd.Blit(source, destination, m_Material, 0);
    }

    public override void Cleanup() => CoreUtils.Destroy(m_Material);
}