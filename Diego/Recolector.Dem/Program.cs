
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Recolector.Core;

Juego CrearJuego() => new Juego(new[]
{
    new Coleccionable(1f, 0f),
    new Coleccionable(1f, 2f),
    new Coleccionable(-2f, -2f),
    new Coleccionable(-3f, 2f),
    new Coleccionable(0f, 3f)
}, 60f);

Juego juego = CrearJuego();

Stopwatch reloj = Stopwatch.StartNew();

double tiempoAnterior = reloj.Elapsed.TotalSeconds;
double ultimaTecla = -100;

float horizontal = 0f;
float vertical = 0f;

Console.Clear();
Console.CursorVisible = false;

try
{
    while (true)
    {
        // Lectura del teclado
        while (Console.KeyAvailable)
        {
            ConsoleKey tecla = Console.ReadKey(true).Key;

            if (tecla == ConsoleKey.Q)
                return;

            if (tecla == ConsoleKey.R)
            {
                juego = CrearJuego();
                horizontal = 0f;
                vertical = 0f;
            }

            switch (tecla)
            {
                case ConsoleKey.W:
                case ConsoleKey.UpArrow:
                    horizontal = 0f;
                    vertical = 1f;
                    break;

                case ConsoleKey.S:
                case ConsoleKey.DownArrow:
                    horizontal = 0f;
                    vertical = -1f;
                    break;

                case ConsoleKey.A:
                case ConsoleKey.LeftArrow:
                    horizontal = -1f;
                    vertical = 0f;
                    break;

                case ConsoleKey.D:
                case ConsoleKey.RightArrow:
                    horizontal = 1f;
                    vertical = 0f;
                    break;
            }

            ultimaTecla = reloj.Elapsed.TotalSeconds;
        }

        // Calcular tiempo transcurrido
        double ahora = reloj.Elapsed.TotalSeconds;

        float deltaTime = (float)(ahora - tiempoAnterior);
        tiempoAnterior = ahora;

        // Detener movimiento si no se presiona una tecla
        if (ahora - ultimaTecla > 0.25)
        {
            horizontal = 0f;
            vertical = 0f;
        }

        // Actualizar el juego
        juego.Actualizar(
            horizontal,
            vertical,
            Math.Min(deltaTime, 0.2f)
        );

        // Dibujar informacion
        Console.SetCursorPosition(0, 0);

        Console.WriteLine("=== RECOLECTOR PAC-MAN ===       ");
        Console.WriteLine($"Puntos: {juego.Puntos}         ");
        Console.WriteLine($"Tiempo: {juego.TiempoRestante:F1} s       ");
        Console.WriteLine($"Estado: {juego.Estado}         ");
        Console.WriteLine();

        // Dibujar tablero
        for (int z = 5; z >= -5; z--)
        {
            for (int x = -5; x <= 5; x++)
            {
                char simbolo = '.';

                // Paredes
                if (!juego.Mapa.PuedeOcupar(x, z, 0.25f))
                {
                    simbolo = '#';
                }

                // Monedas
                if (juego.Monedas.Any(m =>
                    !m.Recogido && m.X == x && m.Z == z))
                {
                    simbolo = '*';
                }

                // Jugador
                if (MathF.Abs(juego.Jugador.X - x) < 0.5f &&
                    MathF.Abs(juego.Jugador.Z - z) < 0.5f)
                {
                    simbolo = 'P';
                }

                Console.Write(simbolo + " ");
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("WASD / Flechas: mover");
        Console.WriteLine("R: reiniciar | Q: salir");

        // Resultado de la partida
        if (juego.Estado == EstadoPartida.Ganada)
        {
            Console.WriteLine("GANASTE!                  ");
        }
        else if (juego.Estado == EstadoPartida.Perdida)
        {
            Console.WriteLine("PERDISTE!                 ");
        }
        else
        {
            Console.WriteLine("                          ");
        }

        Thread.Sleep(100);
    }
}
finally
{
    Console.CursorVisible = true;
}
