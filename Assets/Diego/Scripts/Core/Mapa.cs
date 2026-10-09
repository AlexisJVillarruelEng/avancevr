
namespace Recolector.Core
{
    public class Mapa
    {
        public float LimiteMin = -5f;
        public float LimiteMax = 5f;

        public bool PuedeOcupar(
            float x,
            float z,
            float radio = 0f)
        {
            // Limites del tablero
            if (x - radio < LimiteMin ||
                x + radio > LimiteMax ||
                z - radio < LimiteMin ||
                z + radio > LimiteMax)
            {
                return false;
            }

            // Obstaculo 1
            if (Colision(x, z, radio,
                2f, 3f, -1f, 1f))
                return false;

            // Obstaculo 2
            if (Colision(x, z, radio,
                -3f, -2f, -1f, 1f))
                return false;

            // Obstaculo 3
            if (Colision(x, z, radio,
                -1f, 1f, -3.3f, -2.7f))
                return false;

            // Obstaculo 4
            if (Colision(x, z, radio,
                -1f, 1f, 3.6f, 4.2f))
                return false;

            return true;
        }

        private static bool Colision(
            float x,
            float z,
            float radio,
            float minX,
            float maxX,
            float minZ,
            float maxZ)
        {
            return x >= minX - radio &&
                   x <= maxX + radio &&
                   z >= minZ - radio &&
                   z <= maxZ + radio;
        }
    }
}
