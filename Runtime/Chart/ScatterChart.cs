using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 散布図は主に2つのデータ次元間の関係を示すために使用されます。
    /// || 散布図は主に2つのデータ次元間の関係を示すために使用されます。
    /// </summary>
    [AddComponentMenu("XCharts/ScatterChart", 17)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class ScatterChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();

            var xAxis = EnsureChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Value;
            xAxis.boundaryGap = false;

            var yAxis = EnsureChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Value;
            yAxis.boundaryGap = false;

            RemoveData();
            Scatter.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// デフォルトのバブルチャート。
        /// || デフォルトのバブルチャート.
        /// </summary>
        public void DefaultBubbleChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.itemStyle.borderWidth = 2f;
            serie.itemStyle.borderColor = theme.GetColor(0);
            serie.itemStyle.opacity = 0.35f;
            serie.symbol.sizeType = SymbolSizeType.FromData;
            serie.symbol.dataScale = 0.3f;
            serie.symbol.maxSize = 30f;
        }
    }
}