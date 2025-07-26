using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// リングチャートは、各項目の割合や項目間の関係を表示するために主に使用されます。
    /// || リングチャートは、各項目の割合や項目同士の関係を可視化します。
    /// </summary>
    [AddComponentMenu("XCharts/RingChart", 20)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class RingChart : BaseChart
    {
        protected override void DefaultChart()
        {
            GetChartComponent<Tooltip>().type = Tooltip.Type.Line;
            RemoveData();
            Ring.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// デフォルトの複数リングチャート。
        /// || デフォルトの複数リングチャート。
        /// </summary>
        public void DefaultMultipleRingChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.label.show = false;
            AddData(0, UnityEngine.Random.Range(30, 90), 100, "data2");
            AddData(0, UnityEngine.Random.Range(30, 90), 100, "data3");
        }
    }
}