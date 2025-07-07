using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Демонстрационный компонент, позволяющий интерактивно добавлять, удалять и очищать точки на графике через UI.
/// Использует компонент GraphRenderer для отрисовки.
/// </summary>
public class GraphExample : MonoBehaviour
{
    [Header("Graph Renderer")]

    /// <summary>
    /// Компонент, отвечающий за отрисовку графика.
    /// </summary>
    public GraphRenderer graphRenderer;

    [Header("UI Elements")]

    /// <summary>
    /// Поле ввода координаты X.
    /// </summary>
    public InputField inputX;

    /// <summary>
    /// Поле ввода координаты Y.
    /// </summary>
    public InputField inputY;

    /// <summary>
    /// Кнопка для добавления новой точки на график.
    /// </summary>
    public Button buttonAdd;

    /// <summary>
    /// Кнопка для удаления последней точки.
    /// </summary>
    public Button buttonRemove;

    /// <summary>
    /// Кнопка для полной очистки графика.
    /// </summary>
    public Button buttonClear;

    /// <summary>
    /// Список точек, добавленных пользователем.
    /// </summary>
    private List<Vector2> data = new List<Vector2>();

    /// <summary>
    /// Подписка на события UI при старте.
    /// </summary>
    void Start()
    {
        buttonAdd.onClick.AddListener(AddPointFromUI);
        buttonRemove.onClick.AddListener(RemoveLastPoint);
        buttonClear.onClick.AddListener(ClearGraph);
        RedrawGraph();
    }

    /// <summary>
    /// Добавляет точку, считанную из UI, в список и перерисовывает график.
    /// </summary>
    private void AddPointFromUI()
    {
        if (float.TryParse(inputX.text, out float x) && float.TryParse(inputY.text, out float y))
        {
            data.Add(new Vector2(x, y));
            RedrawGraph();
        }
        else
        {
            Debug.LogWarning("Неверный формат ввода.");
        }
    }

    /// <summary>
    /// Удаляет последнюю точку и обновляет график.
    /// </summary>
    private void RemoveLastPoint()
    {
        if (data.Count > 0)
        {
            data.RemoveAt(data.Count - 1);
            RedrawGraph();
        }
    }

    /// <summary>
    /// Очищает все данные и график.
    /// </summary>
    private void ClearGraph()
    {
        data.Clear();
        graphRenderer.Clear();
    }

    /// <summary>
    /// Преобразует логические координаты в пиксели и рисует график.
    /// </summary>
    private void RedrawGraph()
    {
        if (data.Count < 2)
        {
            graphRenderer.Clear();
            return;
        }

        List<Vector2> pixelPoints = NormalizeData(data, graphRenderer.width, graphRenderer.height);
        graphRenderer.Clear();
        graphRenderer.DrawGraph(pixelPoints, Color.green);
    }

    /// <summary>
    /// Нормализует координаты в диапазон от 0 до ширины/высоты холста (перевод в пиксели).
    /// </summary>
    /// <param name="data">Список входных точек в логических координатах.</param>
    /// <param name="width">Ширина графика в пикселях.</param>
    /// <param name="height">Высота графика в пикселях.</param>
    /// <returns>Список точек, преобразованных в пиксельные координаты.</returns>
    private List<Vector2> NormalizeData(List<Vector2> data, int width, int height)
    {
        float xMin = data.Min(p => p.x);
        float xMax = data.Max(p => p.x);
        float yMin = data.Min(p => p.y);
        float yMax = data.Max(p => p.y);

        List<Vector2> result = new List<Vector2>();
        foreach (var p in data)
        {
            float x = Mathf.InverseLerp(xMin, xMax, p.x) * width;
            float y = Mathf.InverseLerp(yMin, yMax, p.y) * height;
            result.Add(new Vector2(x, y));
        }
        return result;
    }
}
