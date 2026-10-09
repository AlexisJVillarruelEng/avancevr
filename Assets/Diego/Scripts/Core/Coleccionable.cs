
namespace Recolector.Core
{

public class Coleccionable
{
    public float X { get; }
    public float Z { get; }

    public bool Recogido { get; private set; }

    public Coleccionable(float x, float z)
    {
        X = x;
        Z = z;
    }

    public bool IntentarRecoger(Jugador jugador)
    {
        if (Recogido)
            return false;

        float dx = jugador.X - X;
        float dz = jugador.Z - Z;

        float distanciaCuadrada = dx * dx + dz * dz;
        float radioCuadrado = jugador.Radio * jugador.Radio;

        if (distanciaCuadrada <= radioCuadrado)
        {
            Recogido = true;
            return true;
        }

        return false;
    }

    public void Reiniciar()
    {
        Recogido = false;
    }
}
}
