using System;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Set Euler Angles", Category = "Transform", Color = BlueprintColors.Node.Actions)]
    public sealed class BlueprintNodeSetEulerAngles : BlueprintNode, IBlueprintEnter {

        [SerializeField] private Vector3 _angles;

        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<Transform>("Transform"),
            Port.Input<Vector3>("Rotation"),
            Port.Exit(),
        };

        public void OnEnterPort(int port) {
            if (port != 0) return;

            var transform = Ports[1].Get<Transform>();
            var rotation = Ports[2].Get(_angles);

            transform.eulerAngles = rotation;

            Ports[3].Call();
        }
    }

}