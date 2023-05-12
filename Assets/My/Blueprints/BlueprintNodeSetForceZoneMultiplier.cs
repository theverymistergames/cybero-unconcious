using System;
using MisterGames.Blueprints;
using MisterGames.Character.Motion;
using UnityEngine;

namespace MisterGames.BlueprintLib {

    [Serializable]
    [BlueprintNodeMeta(Name = "Set force zone multiplier", Category = "Character", Color = BlueprintColors.Node.Actions)]
    public sealed class BlueprintNodeSetForceZoneMultiplier : BlueprintNode, IBlueprintEnter {
        
        private float _multiplier = 0;
        
        public override Port[] CreatePorts() => new[] {
            Port.Enter(),
            Port.Input<CharacterForceZone>("Zone"),
            Port.Input<float>("Multiplier"),
        };

        public void OnEnterPort(int port) {
            if (port != 0) return;

            var zone = Ports[1].Get<CharacterForceZone>();
            var multiplier = Ports[2].Get(_multiplier);

            zone.forceMultiplier = multiplier;
        }
    }

}