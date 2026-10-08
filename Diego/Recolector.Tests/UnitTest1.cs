
using Xunit;
using Recolector.Core;

namespace Recolector.Tests;

public class JuegoTests
{
    // PRUEBA 1: Movimiento del jugador
    [Fact]
    public void JugadorPuedeMoverse()
    {
        Jugador jugador = new();
        Mapa mapa = new();

        jugador.Mover(1f, 0f, 1f / 3f, mapa);

        Assert.InRange(jugador.X, 0.99f, 1.01f);
    }

    // PRUEBA 2: Colision con paredes
    [Fact]
    public void JugadorNoAtraviesaPared()
    {
        Jugador jugador = new();
        Mapa mapa = new();

        jugador.Mover(1f, 0f, 1f, mapa);

        Assert.True(jugador.X < 1.75f);
    }

    // PRUEBA 3: Sistema de puntuacion
    [Fact]
    public void RecogerMonedasOtorgaPuntos()
    {
        Juego juego = new(new[]
        {
            new Coleccionable(1f, 0f),
            new Coleccionable(1f, 2f)
        });

        juego.Actualizar(1f, 0f, 1f / 3f);
        juego.Actualizar(0f, 1f, 2f / 3f);

        Assert.Equal(20, juego.Puntos);
    }

    // PRUEBA 4: Condicion de victoria
    [Fact]
    public void RecogerTodasLasMonedasProduceVictoria()
    {
        Juego juego = new(new[]
        {
            new Coleccionable(1f, 0f)
        });

        juego.Actualizar(1f, 0f, 1f / 3f);

        Assert.Equal(EstadoPartida.Ganada, juego.Estado);
    }

    // PRUEBA 5: Condicion de derrota
    [Fact]
    public void TiempoAgotadoProduceDerrota()
    {
        Juego juego = new(new[]
        {
            new Coleccionable(4f, 4f)
        }, 1f);

        juego.Actualizar(0f, 0f, 1f);

        Assert.Equal(EstadoPartida.Perdida, juego.Estado);
        Assert.Equal(0f, juego.TiempoRestante);
    }

    // PRUEBA 6: Reinicio de partida
    [Fact]
    public void ReiniciarRestableceLaPartida()
    {
        Juego juego = new(new[]
        {
            new Coleccionable(1f, 0f)
        });

        juego.Actualizar(1f, 0f, 1f / 3f);

        Assert.Equal(EstadoPartida.Ganada, juego.Estado);
        Assert.Equal(10, juego.Puntos);

        juego.Reiniciar();

        Assert.Equal(EstadoPartida.EnCurso, juego.Estado);
        Assert.Equal(0, juego.Puntos);
        Assert.Equal(60f, juego.TiempoRestante);
        Assert.Equal(0f, juego.Jugador.X);
        Assert.Equal(0f, juego.Jugador.Z);
        Assert.False(juego.Monedas[0].Recogido);
    }

    // PRUEBA 7: Recoleccion durante movimiento rapido
    [Fact]
    public void RecogeMonedaDuranteMovimientoRapido()
    {
        Juego juego = new(new[]
        {
            new Coleccionable(1f, 0f),
            new Coleccionable(-2f, 2f)
        });

        juego.Actualizar(1f, 0f, 0.6f);

        Assert.Equal(10, juego.Puntos);
        Assert.True(juego.Monedas[0].Recogido);
    }
}
