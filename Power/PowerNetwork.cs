using System.Collections.Generic;
using Terraria.ModLoader;

namespace Construct.Power;

public class PowerNetwork
{
    public static List<PowerNetwork> networks = new();

    public List<Component> components = new();
    public float lastForce = 0;
    public float force = 0;
    public float rpm = 15f;

    public PowerNetwork()
    {
        networks.Add(this);
    }
}

public class PowerNetworkSystem : ModSystem
{
    public override void PostUpdateEverything()
    {
        int i = 0;
        foreach (PowerNetwork network in PowerNetwork.networks)
        {
            network.rpm += network.force / 60;
            network.lastForce = network.force;
            network.force = 0;
            //Main.NewText($"RPM: {network.rpm}");
            foreach (var comp in network.components)
            {
                //Main.NewText($"ADV: {comp.advantage}");
                comp.rotation += Util.MathUtil.RPMToRad(network.rpm) / comp.advantage;

            }
            i++;
        }
    }
}
