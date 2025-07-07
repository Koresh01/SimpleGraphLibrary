﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Класс, отвечающий за отрисовку графика на текстуре, которую можно прикрепить к UI Image.
/// Позволяет рисовать линии между точками и очищать изображение.
/// </summary>
public class GraphRenderer
{
    /// <summary>
    /// Ширина текстуры в пикселях.
    /// </summary>
    public int Width { get; private set; }

    /// <summary>
    /// Высота текстуры в пикселях.
    /// </summary>
    public int Height { get; private set; }

    /// <summary>
    /// Текстура, на которой осуществляется рисование.
    /// </summary>
    private Texture2D texture;

    /// <summary>
    /// UI Image, в которую встроена текстура.
    /// </summary>
    private Image targetImage;

    /// <summary>
    /// Конструктор, создающий текстуру и прикрепляющий её к UI Image.
    /// </summary>
    /// <param name="imageTarget">Целевой UI Image для отображения графика.</param>
    /// <param name="width">Ширина текстуры (по умолчанию 512).</param>
    /// <param name="height">Высота текстуры (по умолчанию 512).</param>
    public GraphRenderer(Image imageTarget, int width = 512, int height = 512)
    {
        this.targetImage = imageTarget;
        this.Width = width;
        this.Height = height;

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        targetImage.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
        Clear();
    }

    /// <summary>
    /// Очищает текстуру, заливая её указанным цветом (по умолчанию чёрным).
    /// </summary>
    /// <param name="color">Цвет заливки. Если null, используется чёрный.</param>
    public void Clear(Color? color = null)
    {
        Color c = color ?? Color.black;
        Color[] pixels = new Color[Width * Height];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = c;
        texture.SetPixels(pixels);
        texture.Apply();
    }

    /// <summary>
    /// Рисует линии между заданными точками на текстуре.
    /// </summary>
    /// <param name="points">Список точек (в пикселях), соединяемых линиями.</param>
    /// <param name="color">Цвет линий.</param>
    /// <param name="thickness">Толщина линий (не используется в текущей реализации).</param>
    public void DrawGraph(List<Vector2> points, Color color, float thickness = 1f)
    {
        for (int i = 1; i < points.Count; i++)
        {
            DrawLine(points[i - 1], points[i], color);
        }
        texture.Apply();
    }

    /// <summary>
    /// Рисует линию между двумя точками на текстуре с помощью алгоритма Брезенхэма.
    /// </summary>
    /// <param name="p1">Начальная точка линии.</param>
    /// <param name="p2">Конечная точка линии.</param>
    /// <param name="color">Цвет линии.</param>
    private void DrawLine(Vector2 p1, Vector2 p2, Color color)
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
    /// Устанавливает цвет пикселя в пределах границ текстуры.
    /// </summary>
    /// <param name="x">X-координата пикселя.</param>
    /// <param name="y">Y-координата пикселя.</param>
    /// <param name="color">Цвет пикселя.</param>
    private void SetPixelSafe(int x, int y, Color color)
    {
        if (x >= 0 && y >= 0 && x < Width && y < Height)
            texture.SetPixel(x, y, color);
    }
}
