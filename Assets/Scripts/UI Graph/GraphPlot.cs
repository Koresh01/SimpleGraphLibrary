using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;


/// <summary>
/// ����� ��� �������� � ���������� ������� �������, � ����� ��� �� ��������� �� UI-����������� ����� GraphRenderer.
/// </summary>
public class GraphPlot
{
    /// <summary>
    /// ������ ���� ����������� ����� ������� (� ���������������� �����������).
    /// </summary>
    private List<Vector2> points = new();

    /// <summary>
    /// ������, ���������� �� ��������� ������� �� ��������.
    /// </summary>
    private GraphRenderer renderer;

    /// <summary>
    /// ������ ����� ��������� �������, ������������� ������������� ����������.
    /// </summary>
    /// <param name="imageTarget">UI Image, � ������� ����� ��������� ������.</param>
    /// <param name="width">������ �������� � �������� (�� ��������� 512).</param>
    /// <param name="height">������ �������� � �������� (�� ��������� 512).</param>
    public GraphPlot(Image imageTarget, int width = 512, int height = 512)
    {
        renderer = new GraphRenderer(imageTarget, width, height);
    }

    /// <summary>
    /// ��������� ����� �� ������ � �������������� ���.
    /// </summary>
    /// <param name="x">�������� �� ��� X.</param>
    /// <param name="y">�������� �� ��� Y.</param>
    public void AddPoint(float x, float y)
    {
        points.Add(new Vector2(x, y));
        Redraw();
    }

    /// <summary>
    /// ������� ��������� ����������� ����� � ������� � �������������� ���.
    /// </summary>
    public void RemoveLast()
    {
        if (points.Count > 0)
        {
            points.RemoveAt(points.Count - 1);
            Redraw();
        }
    }

    /// <summary>
    /// ������� ��� ����� � ������� ����������� �������.
    /// </summary>
    public void Clear()
    {
        points.Clear();
        renderer.Clear();
    }

    /// <summary>
    /// �������������� ������ �� ������ ������� �����. ���� ����� ������ ���� � ������ ������� ������.
    /// </summary>
    private void Redraw()
    {
        if (points.Count < 2)
        {
            renderer.Clear();
            return;
        }

        var pixelPoints = NormalizePoints(points, renderer.Width, renderer.Height);
        renderer.Clear();
        renderer.DrawGraph(pixelPoints, Color.green);
    }

    /// <summary>
    /// ����������� ������ ���������������� ��������� � ���������� �������� ��� ��������� �� ��������.
    /// </summary>
    /// <param name="data">������ ����� � ���������������� �����������.</param>
    /// <param name="width">������ �������� � ��������.</param>
    /// <param name="height">������ �������� � ��������.</param>
    /// <returns>������ ����� � ���������� �����������.</returns>
    private List<Vector2> NormalizePoints(List<Vector2> data, int width, int height)
    {
        float xMin = data.Min(p => p.x);
        float xMax = data.Max(p => p.x);
        float yMin = data.Min(p => p.y);
        float yMax = data.Max(p => p.y);

        return data.Select(p =>
        {
            float x = Mathf.InverseLerp(xMin, xMax, p.x) * width;
            float y = Mathf.InverseLerp(yMin, yMax, p.y) * height;
            return new Vector2(x, y);
        }).ToList();
    }
}
