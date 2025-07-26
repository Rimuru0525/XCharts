using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 簡易ローソク足チャートは、構成要素や設定を簡素化することでパフォーマンスを向上させたローソク足チャートの簡易モードです。
    /// || 簡易ローソク足チャートは、構成要素や設定を簡素化することで高いパフォーマンスを実現します。
    /// </summary>
    [AddComponentMenu("XCharts/SimplifiedCandlestickChart", 28)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class SimplifiedCandlestickChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            SimplifiedCandlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < GetSerie(0).dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}