using System;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Vector3", Category = "Transform", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeVector3 : BlueprintNode, IBlueprintOutput<Vector3> {

        [SerializeField] private float _x;
        [SerializeField] private float _y;
        [SerializeField] private float _z;

        public override Port[] CreatePorts() => new[] {
            Port.Input<float>("x"),
            Port.Input<float>("y"),
            Port.Input<float>("z"),
            Port.Output<Vector3>("Vector3"),
        };

        Vector3 IBlueprintOutput<Vector3>.GetOutputPortValue(int port) => port switch {
            3 => new Vector3(Ports[0].Get(_x), Ports[1].Get(_y), Ports[2].Get(_z)),
            _ => default
        };
    }

}