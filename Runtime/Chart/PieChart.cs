using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 円グラフは主に異なるカテゴリの割合を表示するために使用されます。各円弧の長さはデータの量の割合を表します。
    /// </summary>
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class PieChart : BaseChart
    {
        protected override void DefaultChart()
        {
            var legend = EnsureChartComponent<Legend>();
            legend.show = true;

            RemoveData();
            Pie.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// デフォルトのラベル付き円グラフ。
        /// </summary>
        public void DefaultLabelPieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }

        /// <summary>
        /// デフォルトのドーナツグラフ。
        /// </summary>
        public void DefaultDonutPieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.radius[0] = 0.20f;
            serie.radius[1] = 0.28f;
        }

        /// <summary>
        /// デフォルトのラベル付きドーナツグラフ。
        /// </summary>
        public void DefaultLabelDonutPieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.radius[0] = 0.20f;
            serie.radius[1] = 0.28f;
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }

        /// <summary>
        /// デフォルトのローズダイアグラム（半径基準）。
        /// </summary>
        public void DefaultRadiusRosePieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.pieRoseType = RoseType.Radius;
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }

        /// <summary>
        /// デフォルトのエリアローズダイアグラム（面積基準）。
        /// </summary>
        public void DefaultAreaRosePieChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.pieRoseType = RoseType.Area;
            serie.EnsureComponent<LabelStyle>();
            serie.EnsureComponent<LabelLine>();
        }
    }
}