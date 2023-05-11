using System;
using MisterGames.Blueprints;
using UnityEditor.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.UI;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Set Emission", Category = "Material", Color = BlueprintColors.Node.Actions)]
    public sealed class BlueprintNodeSetMaterialEmission : BlueprintNode, IBlueprintEnter {

        private float _intensity;
        private Color _color;
        private Material _sharedMaterial;

        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<GameObject>("Game object"),
            Port.Input<Color>("Color"),
            Port.Input<float>("Intensity"),
            Port.Exit(),
        };

        public override void OnInitialize(IBlueprintHost host) {
            base.OnInitialize(host);
            
            var gameObject = Ports[1].Get<GameObject>();
            
            if (gameObject == null) return;

            var renderer = gameObject.GetComponent<Renderer>();
            var material = renderer.material;
            
            _intensity = material.GetFloat("_EmissiveIntensity");
            _color = material.GetColor("_EmissiveColor") / _intensity;
            _sharedMaterial = renderer.sharedMaterial;
        }

        public void OnEnterPort(int port) {
            if (port != 0) return;

            if (_sharedMaterial == null) return;
            
            var color = Ports[2].Get(defaultValue: _color);
            var intensity = Ports[3].Get(defaultValue: _intensity);
            
            _sharedMaterial.SetColor("_EmissiveColor", color * intensity);

            Ports[4].Call();
        }
    }

}