
using System.Linq;
using UnityEngine;
using Recolector.Core;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class RecolectorController : MonoBehaviour
{
    [Header("Jugador")]
    public Transform jugadorVisual;

    [Header("Monedas")]
    public GameObject[] monedasVisuales;

    private Juego juego;

    void Start()
    {
        if (jugadorVisual == null ||
            monedasVisuales == null ||
            monedasVisuales.Length == 0 ||
            monedasVisuales.Any(m => m == null))
        {
            Debug.LogError("Faltan objetos por asignar.");
            enabled = false;
            return;
        }

        juego = new Juego(
            monedasVisuales.Select(m =>
                new Coleccionable(
                    m.transform.position.x,
                    m.transform.position.z
                )
            ),
            60f
        );

        ActualizarVisuales();
    }

    void Update()
    {
        if (juego == null) return;

        float horizontal = 0f;
        float vertical = 0f;
        bool reiniciar = false;

#if ENABLE_INPUT_SYSTEM
        Keyboard teclado = Keyboard.current;

        if (teclado != null)
        {
            if (teclado.aKey.isPressed ||
                teclado.leftArrowKey.isPressed)
                horizontal -= 1f;

            if (teclado.dKey.isPressed ||
                teclado.rightArrowKey.isPressed)
                horizontal += 1f;

            if (teclado.wKey.isPressed ||
                teclado.upArrowKey.isPressed)
                vertical += 1f;

            if (teclado.sKey.isPressed ||
                teclado.downArrowKey.isPressed)
                vertical -= 1f;

            reiniciar = teclado.rKey.wasPressedThisFrame;
        }
#elif ENABLE_LEGACY_INPUT_MANAGER
        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow))
            horizontal -= 1f;

        if (Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow))
            horizontal += 1f;

        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow))
            vertical += 1f;

        if (Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow))
            vertical -= 1f;

        reiniciar = Input.GetKeyDown(KeyCode.R);
#endif

        if (reiniciar)
        {
            juego.Reiniciar();
            ActualizarVisuales();
            return;
        }

        juego.Actualizar(
            horizontal,
            vertical,
            Time.deltaTime
        );

        ActualizarVisuales();
    }

    void ActualizarVisuales()
    {
        Vector3 posicion = jugadorVisual.position;

        posicion.x = juego.Jugador.X;
        posicion.z = juego.Jugador.Z;

        jugadorVisual.position = posicion;

        for (int i = 0; i < monedasVisuales.Length; i++)
        {
            monedasVisuales[i].SetActive(
                !juego.Monedas[i].Recogido
            );
        }
    }

    void OnGUI()
    {
        if (juego == null) return;

        GUI.Box(
            new Rect(10, 10, 280, 110),
            "RECOLECTOR PAC-MAN\n" +
            "Puntos: " + juego.Puntos + "\n" +
            "Tiempo: " + juego.TiempoRestante.ToString("F1") + "\n" +
            "Estado: " + juego.Estado + "\n" +
            "WASD: Mover | R: Reiniciar"
        );
    }
}
