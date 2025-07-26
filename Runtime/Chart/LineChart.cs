using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 折れ線グラフは、データポイントを折れ線で結んだチャートで、データの変化傾向を表示するために使用されます。
    /// 直交座標系と極座標系の両方で使用できます。
    /// ||折れ線グラフは、データポイントを折れ線で結んだチャートで、データの変化傾向を表示します。
    /// 直交座標系と極座標系の両方で使用でき、areaStyleを設定するとエリアチャートとしても描画できます。
    /// </summary>
    [AddComponentMenu("XCharts/LineChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class LineChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }

        /// <summary>
        /// デフォルトのエリア折れ線グラフ.
        /// || デフォルトのエリア折れ線グラフ.
        /// </summary>
        public void DefaultAreaLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.EnsureComponent<AreaStyle>();
        }

        /// <summary>
        /// デフォルトのスムーズ折れ線グラフ.
        /// || デフォルトのスムーズ折れ線グラフ.
        /// </summary>
        public void DefaultSmoothLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.lineType = LineType.Smooth;
        }

        /// <summary>
        /// デフォルトのスムーズエリア折れ線グラフ.
        /// || デフォルトのスムーズエリア折れ線グラフ.
        /// </summary>
        public void DefaultSmoothAreaLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.EnsureComponent<AreaStyle>();
            serie.lineType = LineType.Smooth;
        }

        /// <summary>
        /// デフォルトの積み上げ折れ線グラフ.
        /// || デフォルトの積み上げ折れ線グラフ.
        /// </summary>
        public void DefaultStackLineChart()
        {
            CheckChartInit();
            var serie1 = GetSerie(0);
            if (serie1 == null) return;
            serie1.stack = "stack1";
            var serie2 = Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            serie2.stack = "stack1";
        }

        /// <summary>
        /// デフォルトの積み上げエリア折れ線グラフ.
        /// || デフォルトの積み上げエリア折れ線グラフ.
        /// </summary>
        public void DefaultStackAreaLineChart()
        {
            CheckChartInit();
            var serie1 = GetSerie(0);
            if (serie1 == null) return;
            serie1.EnsureComponent<AreaStyle>();
            serie1.stack = "stack1";
            var serie2 = Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            serie2.EnsureComponent<AreaStyle>();
            serie2.stack = "stack1";
        }

        /// <summary>
        /// default step line chart.
        /// || 默认阶梯折线图。
        /// </summary>
        public void DefaultStepLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.lineType = LineType.StepMiddle;
        }

        /// <summary>
        /// default dash line chart.
        /// || 默认虚线折线图。
        /// </summary>
        public void DefaultDashLineChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            if (serie == null) return;
            serie.lineType = LineType.Normal;
            serie.lineStyle.type = LineStyle.Type.Dashed;
        }

        /// <summary>
        /// default time line chart.
        /// || 默认时间折线图。
        /// </summary>
        public void DefaultTimeLineChart()
        {
            CheckChartInit();
            var xAxis = GetChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Time;
        }

        /// <summary>
        /// default logarithmic line chart.
        /// || 默认对数轴折线图。
        /// </summary>
        public void DefaultLogLineChart()
        {
            CheckChartInit();
            var yAxis = GetChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Log;
        }
    }
}