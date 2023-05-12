using System;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Vector3 Split", Category = "Transform", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeSplitVector3 : BlueprintNode, IBlueprintOutput<float> {

        private Vector3 vector;

        public override Port[] CreatePorts() => new[] {
            Port.Input<Vector3>("Vector3"),
            Port.Output<float>("x"),
            Port.Output<float>("y"),
            Port.Output<float>("z"),
        };

        float IBlueprintOutput<float>.GetOutputPortValue(int port) => port switch {
            1 => Ports[0].Get(vector).x,
            2 => Ports[0].Get(vector).y,
            3 => Ports[0].Get(vector).z,
            _ => default
        };
    }

}