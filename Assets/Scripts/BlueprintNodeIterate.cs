using System;
using MisterGames.Blueprints;
using MisterGames.Blueprints.Meta;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Iterate", Category = "GameObject", Color = BlueprintColors.Node.Data)]
    public sealed class BlueprintNodeIterate : BlueprintNode
    {
        private bool _value = true;
        
        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<Array>("Array"),
        };

        public void OnEnterPort(int port) {
            if (port != 0) return;

            var gameObject = Ports[1].Get<GameObject>();
            var value = Ports[2].Get(_value);

            gameObject.SetActive(value);
        }
    }

}
