
using System;
using System.Collections.Generic;
using System.Linq;

namespace Recolector.Core
{

public class Juego
{
    public Jugador Jugador { get; } = new();
    public Mapa Mapa { get; } = new();

    public List<Coleccionable> Monedas { get; }

    public int Puntos { get; private set; }
    public float TiempoRestante { get; private set; }
    public EstadoPartida Estado { get; private set; }

    private readonly float duracionInicial;

    public Juego(
        IEnumerable<Coleccionable> monedas,
        float duracion = 60f)
    {
        if (duracion <= 0f || !float.IsFinite(duracion))
            throw new ArgumentException("Duracion invalida");

        if (monedas == null)
            throw new ArgumentNullException(nameof(monedas));

        Monedas = monedas.ToList();

        if (Monedas.Count == 0)
            throw new ArgumentException("Se requieren monedas");

        duracionInicial = duracion;
        TiempoRestante = duracion;
        Estado = EstadoPartida.EnCurso;
    }

    public void Actualizar(
        float horizontal,
        float vertical,
        float deltaTime)
    {
        if (Estado != EstadoPartida.EnCurso ||
            !float.IsFinite(deltaTime) ||
            deltaTime <= 0f)
        {
            return;
        }

        float tiempoDisponible =
            Math.Min(deltaTime, TiempoRestante);

        float tiempoInicial = TiempoRestante;

        // Dividir movimientos grandes en pasos pequeños
        // para detectar monedas durante el recorrido.
        int pasos = Math.Max(
            1,
            (int)MathF.Ceiling(
                MathF.Max(0f, Jugador.Velocidad) *
                tiempoDisponible / 0.1f
            )
        );

        float tiempoPaso = tiempoDisponible / pasos;

        for (int i = 0; i < pasos; i++)
        {
            Jugador.Mover(
                horizontal,
                vertical,
                tiempoPaso,
                Mapa
            );

            // Detectar monedas recogidas
            foreach (var moneda in Monedas)
            {
                if (moneda.IntentarRecoger(Jugador))
                {
                    Puntos += 10;
                }
            }

            // Comprobar victoria
            if (Monedas.All(m => m.Recogido))
            {
                TiempoRestante = Math.Max(
                    0f,
                    tiempoInicial - (i + 1) * tiempoPaso
                );

                Estado = EstadoPartida.Ganada;
                return;
            }
        }

        // Actualizar cronometro
        TiempoRestante = Math.Max(
            0f,
            tiempoInicial - deltaTime
        );

        // Comprobar derrota
        if (TiempoRestante <= 0f)
        {
            Estado = EstadoPartida.Perdida;
        }
    }

    public void Reiniciar()
    {
        Jugador.Reiniciar();

        foreach (var moneda in Monedas)
        {
            moneda.Reiniciar();
        }

        Puntos = 0;
        TiempoRestante = duracionInicial;
        Estado = EstadoPartida.EnCurso;
    }
}
}
