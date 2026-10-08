namespace Recolector.Core;

public class Mapa
{
    public float LimiteMin = -5f;
    public float LimiteMax = 5f;

    public bool PuedeOcupar(float x, float z, float radio = 0f)
    {
        // Limites del tablero
        if (x - radio < LimiteMin ||
            x + radio > LimiteMax ||
            z - radio < LimiteMin ||
            z + radio > LimiteMax)
        {
            return false;
        }

        // Obstaculo rectangular
        if (x >= 2f - radio && x <= 3f + radio &&
            z >= -1f - radio && z <= 1f + radio)
        {
            return false;
        }

        return true;
    }
}
