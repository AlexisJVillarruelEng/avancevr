
using System;

namespace Recolector.Core
{

public class Jugador
{
    public float X { get; private set; }
    public float Z { get; private set; }

    public float Velocidad { get; set; } = 3f;
    public float Radio { get; } = 0.25f;

    public void Mover(
        float horizontal,
        float vertical,
        float deltaTime,
        Mapa mapa)
    {
        if (deltaTime <= 0f) return;

        float magnitud = MathF.Sqrt(
            horizontal * horizontal +
            vertical * vertical
        );

        if (magnitud > 1f)
        {
            horizontal /= magnitud;
            vertical /= magnitud;
        }

        float dx = horizontal * Velocidad * deltaTime;
        float dz = vertical * Velocidad * deltaTime;

        float distancia = MathF.Sqrt(dx * dx + dz * dz);

        int pasos = Math.Max(
            1,
            (int)MathF.Ceiling(distancia / 0.1f)
        );

        for (int i = 0; i < pasos; i++)
        {
            float nuevoX = X + dx / pasos;
            float nuevoZ = Z + dz / pasos;

            if (mapa.PuedeOcupar(nuevoX, Z, Radio))
                X = nuevoX;

            if (mapa.PuedeOcupar(X, nuevoZ, Radio))
                Z = nuevoZ;
        }
    }

    public void Reiniciar()
    {
        X = 0f;
        Z = 0f;
    }
}
}
