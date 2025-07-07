# Simple Graph Library for Unity
Библиотека для построения графиков в Unity UI Canvas.

<img src="https://github.com/user-attachments/assets/f21531a4-3d0c-410f-880f-cdf0385f82a2" width="600"/>
<img src="https://github.com/user-attachments/assets/49e6f547-4d12-4f9c-975d-72a10aced6bb" width="300"/>


# Пример использования
```C#
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Пример использования системы графика: добавление, удаление и очистка через UI.
/// </summary>
public class GraphExample : MonoBehaviour
{
    [Header("Graph placeholder:")]
    public Image graphImage;

    [Header("UI buttons:")]
    public InputField inputX;
    public InputField inputY;

    public Button buttonAdd;
    public Button buttonRemove;
    public Button buttonClear;

    private GraphPlot plot;

    void Start()
    {
        plot = new GraphPlot(graphImage, 128, 128);   # Обратите внимание, у вас есть возможность выбирать разрешение текстуры на которой рисуется график. Это разрешение влияет лишь на качество линий графика. График будет в любом случае растягиваться под размеры вашего Image на Canvas, опираясь на данные которые вы туда введете

        buttonAdd.onClick.AddListener(() =>
        {
            if (float.TryParse(inputX.text, out float x) && float.TryParse(inputY.text, out float y))
            {
                plot.AddPoint(x, y);
            }
        });

        buttonRemove.onClick.AddListener(() => plot.RemoveLast());
        buttonClear.onClick.AddListener(() => plot.Clear());
    }
}

```

# Какое разрешение для текстуры графика можно выбрать
К примеру на вашем графике будет 60 000 точек (x,y). Тогда вот зависимость скорости отрисовки графика от разрешения выбранной вами текстуры.

```Python
import matplotlib.pyplot as plt
import numpy as np

# Примерные разрешения текстуры (ширина = высота)
resolutions = [128, 256, 512, 1024, 2048]

# Количество точек
points_count = 60000

# Предположим, что время отрисовки зависит от:
# 1. количества точек (линейно)
# 2. размера текстуры (влияет на SetPixel, особенно при более высоком разрешении)
# Формула: time = base_time * points * (resolution_factor)
# resolution_factor условно зависит от площади текстуры (w*h)

base_time = 1e-6  # базовая стоимость одной операции (условно)

times = []
for res in resolutions:
    texture_area = res * res
    resolution_factor = texture_area / (256 * 256)  # нормируем относительно 256x256
    time = base_time * points_count * resolution_factor
    times.append(time)

# Построим график
plt.figure(figsize=(10, 6))
plt.plot(resolutions, times, marker='o', linestyle='-', color='green')
plt.title('Зависимость времени отрисовки GraphPlot от разрешения текстуры')
plt.xlabel('Разрешение текстуры (ширина = высота, px)')
plt.ylabel('Время отрисовки (условные единицы)')
plt.grid(True)
plt.xticks(resolutions)
plt.show()
```

![image](https://github.com/user-attachments/assets/396b24b7-cbaf-4490-8ebd-58f14b4ec083)
Вот график, показывающий, как увеличивается время отрисовки GraphPlot при росте разрешения текстуры, если на графике 60 000 точек:

При низком разрешении (128–256 px) отрисовка выполняется быстро, потому что меньше операций SetPixel.

При высоком разрешении (1024–2048 px) нагрузка возрастает экспоненциально по площади текстуры.

Вывод:
Выбор слишком большого разрешения без реальной необходимости приводит к значительному увеличению времени отрисовки и нагрузке на CPU/GPU, особенно при большом числе точек. 
