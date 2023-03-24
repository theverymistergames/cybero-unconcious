using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using System;

[Serializable, VolumeComponentMenu("Post-processing/Custom/Wave")]
public sealed class Wave : CustomPostProcessVolumeComponent, IPostProcessComponent {
    public ClampedFloatParameter amplitude = new(0f, 0f, 3f);
    public ClampedFloatParameter frequency = new(0f, 0f, 0.5f);
    public ClampedFloatParameter speed = new(1f, 0f, 5f);

    Material m_Material;

    public bool IsActive() => m_Material != null && amplitude.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    public override void Setup() {
        if (Shader.Find("Hidden/Shader/Wave") != null)
            m_Material = new Material(Shader.Find("Hidden/Shader/Wave"));
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination) {
        if (m_Material == null)
            return;

        m_Material.SetFloat("_Amplitude", amplitude.value);
        m_Material.SetFloat("_Frequency", frequency.value);
        m_Material.SetFloat("_Speed", speed.value);
        
        cmd.Blit(source, destination, m_Material, 0);
    }

    public override void Cleanup() => CoreUtils.Destroy(m_Material);
}