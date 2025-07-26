using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// レーダーチャートは、サッカー選手の様々な属性の分析など、多変量データを表示するために主に使用されます。レーダーコンポーネントに依存しています。
    /// || レーダーチャートは、サッカー選手の様々な属性の分析など、多変量データを表示するために主に使用されます。レーダーコンポーネントに依存しています。
    /// </summary>
    [AddComponentMenu("XCharts/RadarChart", 16)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class RadarChart : BaseChart
    {
        protected override void DefaultChart()
        {
            RemoveData();
            RemoveChartComponents<RadarCoord>();
            AddChartComponent<RadarCoord>();
            Radar.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// デフォルトの円形レーダーチャート.
        /// || デフォルトの円形レーダーチャート.
        /// </summary>
        public void DefaultCircleRadarChart()
        {
            CheckChartInit();
            var radarCoord = GetChartComponent<RadarCoord>();
            radarCoord.shape = RadarCoord.Shape.Circle;
        }
    }
}