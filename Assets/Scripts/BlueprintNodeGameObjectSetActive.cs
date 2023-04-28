using System;
using MisterGames.Blueprints;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Set Active", Category = "Gameobject", Color = BlueprintColors.Node.Actions)]
    public sealed class BlueprintNodeGameObjectSetActive : BlueprintNode, IBlueprintEnter {
        
        private bool _value = true;
        
        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<GameObject>("GameObject"),
            Port.Input<bool>("Value"),
        };

        public void OnEnterPort(int port) {
            if (port != 0) return;

            var gameObject = Ports[1].Get<GameObject>();
            var value = Ports[2].Get(_value);

            gameObject.SetActive(value);
        }
    }

}