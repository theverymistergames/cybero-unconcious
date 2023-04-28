using System;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Boolean", Category = "Logic", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeBoolean : BlueprintNode, IBlueprintOutput<bool> {

        [SerializeField] private bool _value = true;

        public override Port[] CreatePorts() => new[] {
            Port.Output<bool>("Value"),
        };

        bool IBlueprintOutput<bool>.GetOutputPortValue(int port) => port switch {
            0 => _value
        };
    }

}