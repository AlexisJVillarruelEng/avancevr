using System.Linq;
using UnityEngine;
using Recolector.Core;
using Vuforia;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class RecolectorController : MonoBehaviour
{
    [Header("Objetos")]
    public Transform jugadorVisual;
    public GameObject[] monedasVisuales;

    [Header("Realidad aumentada")]
    public ImageTargetBehaviour imageTarget;
    public bool usarInclinacion = false;
    public float sensibilidad = 4f;
    public float zonaMuerta = 0.05f;

    private Juego juego;

    void Start()
    {
        if (jugadorVisual == null ||
            monedasVisuales == null ||
            monedasVisuales.Length == 0 ||
            monedasVisuales.Any(m => m == null))
        {
            Debug.LogError("Faltan referencias del recolector.");
            enabled = false;
            return;
        }

        juego = new Juego(
            monedasVisuales.Select(m =>
                new Coleccionable(
                    m.transform.localPosition.x,
                    m.transform.localPosition.z
                )
            ),
            90f
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
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        reiniciar = Input.GetKeyDown(KeyCode.R);
#endif

        if (reiniciar)
        {
            juego.Reiniciar();
            ActualizarVisuales();
            return;
        }

        if (imageTarget != null)
        {
            Status estado = imageTarget.TargetStatus.Status;

            if (estado != Status.TRACKED &&
                estado != Status.EXTENDED_TRACKED)
                return;
        }

        if (usarInclinacion && imageTarget != null)
        {
            Vector3 pendiente = Vector3.ProjectOnPlane(
                Vector3.down,
                imageTarget.transform.up
            );

            horizontal = Vector3.Dot(
                pendiente, imageTarget.transform.right
            ) * sensibilidad;

            vertical = Vector3.Dot(
                pendiente, imageTarget.transform.forward
            ) * sensibilidad;

            if (Mathf.Abs(horizontal) < zonaMuerta)
                horizontal = 0f;

            if (Mathf.Abs(vertical) < zonaMuerta)
                vertical = 0f;

            horizontal = Mathf.Clamp(horizontal, -1f, 1f);
            vertical = Mathf.Clamp(vertical, -1f, 1f);
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
        Vector3 posicion = jugadorVisual.localPosition;

        posicion.x = juego.Jugador.X;
        posicion.z = juego.Jugador.Z;

        jugadorVisual.localPosition = posicion;

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
            "R: Reiniciar"
        );
    }
}
