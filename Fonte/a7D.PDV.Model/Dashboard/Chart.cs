using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace a7D.PDV.Model.Dashboard
{
    [DataContract]
    public class Chart
    {
        [DataMember(Name = "type")]
        public _Type Type { get; set; }

        [DataMember(Name = "data")]
        public _Data Data { get; set; }

        [DataMember(Name = "options")]
        public _Options Options { get; set; }
    }

    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum _Type
    {
        [EnumMember(Value = "pie")]
        Pie,

        [EnumMember(Value = "line")]
        Line,

        [EnumMember(Value = "bar")]
        Bar
    }

    [DataContract]
    public partial class _Data
    {
        [DataMember(Name = "labels")]
        public List<string> Labels { get; set; }

        [DataMember(Name = "datasets")]
        public List<_DataSet> Datasets { get; set; }
    }

    [DataContract]
    public partial class _DataSet
    {
        [DataMember(Name = "label")]
        public string Label { get; set; }

        [DataMember(Name = "data")]
        public List<decimal> Data { get; set; }

        [DataMember(Name = "backgroundColor")]
        public object BackgroundColor { get; set; }

        [DataMember(Name = "borderColor")]
        public object BorderColor { get; set; }

        [DataMember(Name = "hoverBackgroundColor")]
        public object HoverBackgroundColor { get; set; }

        [DataMember(Name = "borderWidth")]
        public int BorderWidth { get; set; }

        [DataMember(Name = "lineTension")]
        public decimal LineTension { get; set; }

        [DataMember(Name = "fill")]
        public bool Fill { get; set; } = false;

        [DataMember(Name = "spanGaps", EmitDefaultValue = false)]
        public bool SpanGaps { get; set; }
    }

    [DataContract]
    public partial class _LineDataSet : _DataSet
    {
        [DataMember(Name = "borderColor")]
        public new string BorderColor { get; set; }

        [DataMember(Name = "backgroundColor")]
        public new string BackgroundColor { get; set; }

        [DataMember(Name = "pointRadius")]
        public int PointRadius { get; set; }

        [DataMember(Name = "pointHitRadius")]
        public int PointHitRadius { get; set; }
    }

    [DataContract]
    public partial class _Options
    {

        [DataMember(Name = "responsive")]
        public bool Responsive { get; set; }

        [DataMember(Name = "maintainAspectRatio")]
        public bool MaintainAspectRatio { get; set; }

        [DataMember(Name = "scales")]
        public _Scales Scales { get; set; }

        [DataMember(Name = "title")]
        public _Title Title { get; set; }

        [DataMember(Name = "legend")]
        public _Legend Legend { get; set; }

        [DataMember(Name = "tooltips")]
        public _ToolTips ToolTips { get; set; }

        [DataMember(Name = "hover")]
        public _Hover Hover { get; set; }

        [DataMember(Name = "showAllTooltips", EmitDefaultValue = false)]
        public bool ShowAllTooltips { get; set; } = false;
    }

    [DataContract]
    public partial class _Hover
    {
        [DataMember(Name = "mode")]
        public _InteractionMode Mode { get; set; } = _InteractionMode.Nearest;

        [DataMember(Name = "intersect")]
        public bool Intersect { get; set; }

        [DataMember(Name = "animationDuration")]
        public decimal AnimationDuration { get; set; } = 400;
    }

    [DataContract]
    public partial class _ToolTips
    {
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; } = true;

        [DataMember(Name = "mode")]
        public _InteractionMode Mode { get; set; } = _InteractionMode.Nearest;

        [DataMember(Name = "intersect")]
        public bool Intersect { get; set; } = true;

        [DataMember(Name = "position")]
        public string Position { get; set; } = "average";

        [DataMember(Name = "backgroundColor")]
        public string BackgroundColor { get; set; } = "rgba(0,0,0,0.8)";

        [DataMember(Name = "titleFontFamily")]
        public string TitleFontFamily { get; set; } = "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif";

        [DataMember(Name = "titleFontSize")]
        public decimal TitleFontSize { get; set; } = 12;

        [DataMember(Name = "titleFontStyle")]
        public string TitleFontStyle { get; set; } = "bold";

        [DataMember(Name = "titleFontColor")]
        public string TitleFontColor { get; set; } = "#fff";

        [DataMember(Name = "titleSpacing")]
        public decimal TitleSpacing { get; set; } = 2;

        [DataMember(Name = "titleMarginBottom")]
        public decimal TitleMarginBottom { get; set; } = 6;

        [DataMember(Name = "bodyFontFamily")]
        public string BodyFontFamily { get; set; } = "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif";

        [DataMember(Name = "bodyFontSize")]
        public decimal BodyFontSize { get; set; } = 12;

        [DataMember(Name = "bodyFontStyle")]
        public string BodyFontStyle { get; set; } = "normal";

        [DataMember(Name = "bodyFontColor")]
        public string BodyFontColor { get; set; } = "#fff";

        [DataMember(Name = "bodySpacing")]
        public decimal BodySpacing { get; set; } = 2;

        [DataMember(Name = "footerFontFamily")]
        public string FooterFontFamily { get; set; } = "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif";

        [DataMember(Name = "footerFontSize")]
        public decimal FooterFontSize { get; set; } = 12;

        [DataMember(Name = "footerFontStyle")]
        public string FooterFontStyle { get; set; } = "bold";

        [DataMember(Name = "footerFontColor")]
        public string FooterFontColor { get; set; } = "#fff";

        [DataMember(Name = "footerSpacing")]
        public decimal FooterSpacing { get; set; } = 2;

        [DataMember(Name = "footerMarginTop")]
        public decimal FooterMarginTop { get; set; } = 6;

        [DataMember(Name = "xPadding")]
        public decimal XPadding { get; set; } = 6;

        [DataMember(Name = "yPadding")]
        public decimal YPadding { get; set; } = 6;

        [DataMember(Name = "caretSize")]
        public int CaretSize { get; set; } = 5;

        [DataMember(Name = "cornerRadius")]
        public decimal CornerRadius { get; set; } = 6;

        [DataMember(Name = "multiKeyBackground")]
        public string MultiKeyBackground { get; set; } = "#fff";

        [DataMember(Name = "displayColors")]
        public bool DisplayColors { get; set; } = true;

        [DataMember(Name = "callbacks")]
        public _ToolTipCallbacks Callbacks { get; set; }
    }

    [DataContract]
    public partial class _ToolTipCallbacks
    {
        [DataMember(Name = "beforeTitle")]
        public object beforeTitle { get; set; }

        [DataMember(Name = "label")]
        public object Label { get; set; }
    }

    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum _InteractionMode
    {
        [EnumMember(Value = "point")]
        Point,

        [EnumMember(Value = "nearest")]
        Nearest,

        [Obsolete]
        [EnumMember(Value = "single")]
        Single,

        [Obsolete]
        [EnumMember(Value = "label")]
        Label,

        [EnumMember(Value = "index")]
        Index,

        [Obsolete]
        [EnumMember(Value = "x-axis")]
        XAxis,

        [EnumMember(Value = "dataset")]
        Dataset,

        [EnumMember(Value = "x")]
        X,

        [EnumMember(Value = "y")]
        Y
    }

    [DataContract]
    public partial class _Legend
    {
        [DataMember(Name = "display")]
        public bool Display { get; set; } = true;

        [DataMember(Name = "position")]
        public _Position Position { get; set; } = _Position.Top;

        [DataMember(Name = "fullWidth")]
        public bool FullWidth { get; set; } = true;

        [DataMember(Name = "reverse")]
        public bool Reverse { get; set; } = false;
    }

    [DataContract]
    public partial class _Title
    {
        [DataMember(Name = "display")]
        public bool Display { get; set; } = false;

        [DataMember(Name = "position")]
        public _Position Position { get; set; } = _Position.Top;

        [DataMember(Name = "fullWidth")]
        public bool FullWidth { get; set; } = true;

        [DataMember(Name = "fontSize")]
        public decimal FontSize { get; set; } = 12;

        [DataMember(Name = "fontFamily")]
        public string FontFamily { get; set; } = "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif";

        [DataMember(Name = "fontColor")]
        public string FontColor { get; set; } = "#666";

        [DataMember(Name = "fontStyle")]
        public string FontStyle { get; set; } = "bold";

        [DataMember(Name = "padding")]
        public int Padding { get; set; } = 10;

        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    [DataContract]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum _Position
    {
        [EnumMember(Value = "top")]
        Top,

        [EnumMember(Value = "left")]
        Left,

        [EnumMember(Value = "bottom")]
        Bottom,

        [EnumMember(Value = "right")]
        Right
    }

    [DataContract]
    public partial class _Scales
    {
        [DataMember(Name = "yAxes")]
        public List<_Axis> YAxes { get; set; }

        [DataMember(Name = "xAxes")]
        public List<_Axis> XAxes { get; set; }
    }


    [DataContract]
    public partial class _Axis
    {
        [DataMember(Name = "ticks")]
        public _Ticks Ticks { get; set; }

        [DataMember(Name = "gridLines")]
        public _GridLines GridLines { get; set; }
    }

    [DataContract]
    public partial class _GridLines
    {
        [DataMember(Name = "display")]
        public bool Display { get; set; }
    }

    [DataContract]
    public partial class _Ticks
    {
        [DataMember(Name = "beginAtZero", EmitDefaultValue = false)]
        public bool BeginAtZero { get; set; } = true;

        [DataMember(Name = "callback")]
        public object Callback { get; set; }
    }
}
