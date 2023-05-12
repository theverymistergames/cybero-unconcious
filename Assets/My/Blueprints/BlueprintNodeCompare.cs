using System;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Compare Float", Category = "Logic", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeCompare : BlueprintNode {

        [SerializeField] private float _x = 0;
        [SerializeField] private float _y = 0;
        [SerializeField] private bool useAbs = false;
        [SerializeField] private float _tolerance = 0.00001f;

        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<float>("x"),
            Port.Input<float>("y"),
            Port.Exit("x < y"),
            Port.Exit("x == y"),
            Port.Exit("x > y"),
        };

        public void OnEnterPort(int port) {
            if (port != 0) return;

            var x = Ports[1].Get(_x);
            var y = Ports[1].Get(_y);

            if (useAbs) {
                if (Math.Abs(x) < Math.Abs(y)) {
                    Ports[3].Call();
                } else if (Math.Abs(Math.Abs(x) - Math.Abs(y)) < _tolerance) {
                    Ports[4].Call();
                } else {
                    Ports[5].Call();
                }
            } else {
                if (x < y) {
                    Ports[3].Call();
                } else if (Math.Abs(x - y) < _tolerance) {
                    Ports[4].Call();
                } else {
                    Ports[5].Call();
                }
            }
        }
    }

}