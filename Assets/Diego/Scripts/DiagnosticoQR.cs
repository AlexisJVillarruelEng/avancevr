
using UnityEngine;
using Vuforia;

public class DiagnosticoQR : MonoBehaviour
{
    private ImageTargetBehaviour marcador;

    void Awake()
    {
        marcador = GetComponent<ImageTargetBehaviour>();
    }

    void OnGUI()
    {
        if (marcador == null) return;

        var estado = marcador.TargetStatus;

        GUI.Box(
            new Rect(10, 170, 350, 80),
            "QR: " + estado.Status +
            "\nDetalle: " + estado.StatusInfo
        );
    }
}
