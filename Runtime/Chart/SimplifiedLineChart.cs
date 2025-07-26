using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 簡易折れ線グラフは、構成要素や設定を簡素化することでパフォーマンスを向上させた折れ線グラフの簡易モードです。
    /// || 簡易折れ線グラフは、構成要素や設定を簡素化することで高いパフォーマンスを実現します。
    /// </summary>
    [AddComponentMenu("XCharts/SimplifiedLineChart", 26)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class SimplifiedLineChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            SimplifiedLine.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < GetSerie(0).dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}