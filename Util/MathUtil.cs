using Microsoft.Xna.Framework;

namespace Construct.Util;
public static class MathUtil
{
    public static float RPMToRad(float rpm)
    {
        return rpm / 3600 * MathHelper.TwoPi;
    }
}
