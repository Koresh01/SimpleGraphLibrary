using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Компонент для отрисовки графика на текстуре, прикреплённой к UI-элементу Image.
/// Позволяет очищать изображение, рисовать линии между точками и отображать графики.
/// </summary>
public class GraphRenderer : MonoBehaviour
{
    /// <summary>
    /// UI-элемент Image, в котором отображается сгенерированная текстура графика.
    /// </summary>
    public Image targetImage;

    /// <summary>
    /// Ширина текстуры графика (в пикселях).
    /// </summary>
    public int width = 512;

    /// <summary>
    /// Высота текстуры графика (в пикселях).
    /// </summary>
    public int height = 512;

    /// <summary>
    /// Текстура, на которой выполняется отрисовка графика.
    /// </summary>
    private Texture2D texture;

    /// <summary>
    /// Инициализирует текстуру и прикрепляет её к спрайту Image при запуске.
    /// </summary>
    private void Awake()
    {
        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
        Clear();
    }

    /// <summary>
    /// Очищает график, заполняя его заданным цветом (по умолчанию — чёрный).
    /// </summary>
    /// <param name="color">Цвет, которым будет заполнена текстура. По умолчанию чёрный.</param>
    public void Clear(Color? color = null)
    {
        Color c = color ?? Color.black;
        Color[] pixels = new Color[width * height];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = c;
        texture.SetPixels(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Отрисовывает график, соединяя заданные точки линиями указанного цвета и толщины.
    /// </summary>
    /// <param name="points">Список точек, между которыми нужно провести линии.</param>
    /// <param name="color">Цвет линий графика.</param>
    /// <param name="thickness">Толщина линий (на данный момент не используется, только для расширения).</param>
    public void DrawGraph(List<Vector2> points, Color color, float thickness = 1f)
    {
        for (int i = 1; i < points.Count; i++)
        {
            DrawLine(points[i - 1], points[i], color, thickness);
        }
        texture.Apply();
    }

    /// <summary>
    /// Рисует линию между двумя точками на текстуре.
    /// Использует алгоритм Брезенхема для дискретной отрисовки.
    /// </summary>
    /// <param name="p1">Начальная точка линии.</param>
    /// <param name="p2">Конечная точка линии.</param>
    /// <param name="color">Цвет линии.</param>
    /// <param name="thickness">Толщина линии (не используется в текущей реализации).</param>
    private void DrawLine(Vector2 p1, Vector2 p2, Color color, float thickness = 1f)
    {
        int x0 = Mathf.RoundToInt(p1.x);
        int y0 = Mathf.RoundToInt(p1.y);
        int x1 = Mathf.RoundToInt(p2.x);
        int y1 = Mathf.RoundToInt(p2.y);

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = (x0 < x1) ? 1 : -1;
        int sy = (y0 < y1) ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            SetPixelSafe(x0, y0, color);

            if (x0 == x1 && y0 == y1) break;
            int e2 = 2 * err;
            if (e2 > -dy) { err -= dy; x0 += sx; }
            if (e2 < dx) { err += dx; y0 += sy; }
        }
    }

    /// <summary>
    /// Безопасно устанавливает пиксель в заданную позицию, проверяя границы текстуры.
    /// </summary>
    /// <param name="x">Координата X пикселя.</param>
    /// <param name="y">Координата Y пикселя.</param>
    /// <param name="color">Цвет пикселя.</param>
    private void SetPixelSafe(int x, int y, Color color)
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
            texture.SetPixel(x, y, color);
    }
}
