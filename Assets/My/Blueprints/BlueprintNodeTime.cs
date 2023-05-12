using System;
using MisterGames.Blueprints;
using MisterGames.Character.Motion;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Time", Category = "Time", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeTime : BlueprintNode, IBlueprintOutput<float> {

        private float _startTime = 0;
        
        public override Port[] CreatePorts() => new[] {
            Port.Output<float>("Time"),
            Port.Output<float>("Sine time"),
            Port.Output<float>("Cosine time"),
        };

        public override void OnInitialize(IBlueprintHost host) {
            base.OnInitialize(host);

            _startTime = Time.time;
        }

        float IBlueprintOutput<float>.GetOutputPortValue(int port) => port switch {
            0 => Time.time - _startTime,
            1 => Mathf.Sin(Time.time - _startTime),
            2 => Mathf.Cos(Time.time - _startTime) + 1,
        };
    }

}