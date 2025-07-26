using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 平行座標は、高次元の幾何学を可視化し、多変量データを分析する一般的な方法です。
    /// || 平行座標は、座標軸に垂直な平行線を描画してデータを表示する可視化チャートの一種です。
    /// </summary>
    [AddComponentMenu("XCharts/ParallelChart", 25)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class ParallelChart : BaseChart
    {
        protected override void DefaultChart()
        {
            RemoveData();
            AddChartComponent<ParallelCoord>();

            for (int i = 0; i < 3; i++)
            {
                var valueAxis = AddChartComponent<ParallelAxis>();
                valueAxis.type = Axis.AxisType.Value;
            }
            var categoryAxis = AddChartComponent<ParallelAxis>();
            categoryAxis.type = Axis.AxisType.Category;
            categoryAxis.position = Axis.AxisPosition.Right;
            categoryAxis.data = new List<string>() { "x1", "x2", "x3", "x4", "x5" };

            Parallel.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}