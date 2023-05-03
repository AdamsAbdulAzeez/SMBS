//using ActiproSoftware.Windows.Controls.Grids.PropertyEditors;
//using ChartUIControls.Controls;
//using ChartUIControls.Controls.Utils;
//using Prism.Mvvm;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace WindowsClient.Shared.Visualisation.ChartUIControl
//{
//    public class YAxisDataAccessor : INotifyPropertyChanged
//    {
//        #region Fields
//        protected Dictionary<string, bool> propertyVisibility;
//        protected Dictionary<string, bool> propertyVisibilityOverrides;
//        protected Dictionary<string, bool> propertyIsReadonly;
//        protected Dictionary<string, bool> propertyIsReadonlyOverrides;
//        protected YAxis yaxis;
//        protected long tickYear = 316224000000000;
//        #endregion

//        #region Properties
//        #region Title
//        private string title = string.Empty;
//        [Category("YAxis")]
//        [Description("YAxis title")]
//        public virtual string Title
//        {
//            get { return title; }
//            set { title = value; OnPropertyChanged("Title"); TitlePropertyChanged(); }
//        }
//        #endregion

//        #region AutomaticTickGeneration
//        private bool automaticTickGeneration;
//        [Category("YAxis")]
//        [Description("Enable automatic axis scaling")]
//        public bool AutomaticTickGeneration
//        {
//            get { return automaticTickGeneration; }
//            set { automaticTickGeneration = value; OnPropertyChanged("AutomaticTickGeneration"); AutomaticTickGenerationPropertyChanged(); }
//        }
//        #endregion

//        #region Ymax
//        private double ymax = 10;
//        [Category("YAxis")]
//        [Description("YAxis maximum value")]
//        public double Ymax
//        {
//            get { return GetYmax(); }
//            set
//            {
//                YmaxPropertyChanged(value, ymax);
//            }
//        }
//        #endregion

//        #region Ymin
//        private double ymin = 1;
//        [Category("YAxis")]
//        [Description("YAxis minimum value")]
//        public double Ymin
//        {
//            get { return GetYmin(); }
//            set { YminPropertyChanged(value, ymin); }
//        }

//        #endregion

//        #region YTick
//        private double yTick = 1;
//        [Category("YAxis")]
//        [Description("YAxis tick value")]
//        public double YTick
//        {
//            get { return GetYtick(); }
//            set { YTickPropertyChanged(value, yTick); }
//        }
//        #endregion

//        #region AxisType
//        //private DataTypeEnum axisType = DataTypeEnum.Linear;
//        //[Category("YAxis")]
//        //[Description("Axis type")]
//        //[Editor(typeof(ComboBoxPropertyEditor), typeof(PropertyEditor)), TypeConverter(typeof(YAxisTypeConverter))]
//        //public DataTypeEnum AxisType
//        //{
//        //    get { return axisType; }
//        //    set { axisType = value; AxisTypePropertyChanged(); }
//        //}
//        #endregion

//        #region AxisOrientation
//        private AxisOrientation axisOrientation = AxisOrientation.Horizontal;
//        //[Category("YAxis label placement")]
//        public AxisOrientation AxisOrientation
//        {
//            get { return axisOrientation; }
//            set { axisOrientation = value; OnPropertyChanged("AxisOrientation"); AxisOrientationPropertyChanged(); }
//        }
//        #endregion

//        #region Numberformat
//        private NumberPrecisionEnum numberFormat = NumberPrecisionEnum.None;
//        //[Category("YAxis")]
//        [Description("YAxis number format")]
//        public NumberPrecisionEnum NumberFormat
//        {
//            get { return numberFormat; }
//            set
//            {
//                numberFormat = value;
//                if (yaxis != null)
//                {
//                    if (yaxis.AxisType == DataTypeEnum.Linear)
//                    {
//                        ((YAxisNumberFormatter)yaxis.AxisFormatter).NumberPrecision = value;
//                    }
//                    yaxis.UpdateYAxis();
//                    yaxis.OnAxisFormatChanged();
//                }
//                OnPropertyChanged("NumberFormat");
//            }
//        }
//        #endregion

//        #region DecimalPlaces
//        private int decimalPlaces = 0;
//        [Category("YAxis")]
//        [Description("YAxis number precision")]
//        public int DecimalPlaces
//        {
//            get { return decimalPlaces; }
//            set
//            {
//                decimalPlaces = OnDecimalPlacesChanged(value);
//                if (yaxis != null)
//                {
//                    if (yaxis.AxisType == DataTypeEnum.Linear)
//                    {
//                        ((YAxisNumberFormatter)yaxis.AxisFormatter).DecimalPlaces = decimalPlaces;
//                    }
//                    else if (yaxis.AxisType == DataTypeEnum.Logarithmic)
//                    {
//                        ((YAxisLogarithmicFormatter)yaxis.AxisFormatter).DecimalPlaces = decimalPlaces;
//                    }
//                    yaxis.UpdateYAxis();
//                    yaxis.OnAxisFormatChanged();
//                }
//                OnPropertyChanged("DecimalPlaces");
//            }
//        }
//        #endregion

//        #region Dateformat
//        private DateFormatEnum dateFormat = DateFormatEnum.Decimal;
//        [Category("YAxis")]
//        [Description("YAxis date format")]
//        public DateFormatEnum DateFormat
//        {
//            get { return dateFormat; }
//            set
//            {
//                dateFormat = value;
//                if (yaxis != null)
//                {
//                    if (yaxis.AxisType == DataTypeEnum.Date)
//                    {
//                        ((YAxisDateFormatter)yaxis.AxisFormatter).DateFormat = value;
//                    }
//                    yaxis.UpdateYAxis();
//                    yaxis.OnAxisFormatChanged();
//                }
//                OnPropertyChanged("DateFormat");
//            }
//        }
//        #endregion

//        #region IsAxisInverted
//        private bool isAxisInverted;
//        [Category("IsAxisInverted")]
//        [Description("Enable axis inversion")]
//        public bool IsAxisInverted
//        {
//            get { return isAxisInverted; }
//            set { isAxisInverted = value; IsAxisInvertedPropertyChanged(); }
//        }

//        #endregion

//        #region IsStackingEnabled
//        private bool isStackingEnabled;
//        [Category("YAxis")]
//        [Description("Enable axis stacking")]
//        public bool IsStackingEnabled
//        {
//            get { return isStackingEnabled; }
//            set { isStackingEnabled = value; IsStackingPropertyChanged(); }
//        }

//        #endregion

//        #region FontColor
//        [Category("YAxis")]
//        [Description("YAxis font color")]
//        public Brush FontColor
//        {
//            get { return yaxis.FontColor; }
//            set
//            {
//                if (value != yaxis.FontColor)
//                {
//                    yaxis.FontColor = value;
//                    OnPropertyChanged("FontColor");
//                }
//            }

//        }
//        #endregion


//        #region FontWeight
//        FontWeightConverter fontWeightConverter = new FontWeightConverter();
//        private string fontWeight;
//        [Category("YAxis")]
//        [Editor(typeof(ComboBoxPropertyEditor), typeof(PropertyEditor)), TypeConverter(typeof(AdhocFontWeightConverter))]
//        [Description("YAxis Font Weight")]
//        public string FontWeight
//        {
//            get { return AxisFontWeights.FirstOrDefault(); }
//            set
//            {
//                fontWeight = value;
//                OnPropertyChanged("FontWeight");
//                //OnPropertyChanged("FontFamily");

//                if (fontWeight != yaxis.FontWeight.ToString())
//                {
//                    yaxis.FontWeight = (FontWeight)fontWeightConverter.ConvertFromString(fontWeight);

//                }

//            }
//        }


//        #endregion

//        #region FontSize
//        private double fontsize = 13;
//        [Category("YAxis")]
//        [Description("YAxis Font Size")]
//        public double FontSize
//        {
//            get { return fontsize; }
//            set
//            {
//                fontsize = value;
//                OnPropertyChanged("FontSize");
//                if (yaxis != null)
//                {
//                    yaxis.FontSize = fontsize;
//                }

//            }

//        }

//        #endregion

//        FontFamilyConverter ffc = new FontFamilyConverter();
//        #region FontFamily
//        //;
//        private string fontFamily;
//        [Category("YAxis")]
//        [Editor(typeof(ComboBoxPropertyEditor), typeof(PropertyEditor)), TypeConverter(typeof(FontListConverter))]
//        [Description("YAxis FontFamily")]
//        public string FontFamily
//        {
//            get { return fontFamily; }
//            set
//            {
//                fontFamily = value;
//                RaisePropertyChanged(() => FontFamily);
//                //OnPropertyChanged("FontFamily");

//                if (fontFamily != yaxis.FontFamily.ToString())
//                {
//                    yaxis.FontFamily = (FontFamily)ffc.ConvertFromString(fontFamily);

//                }
//            }

//        }

//        public IList<string> AxisFontWeights
//        {
//            get
//            {
//                IList<string> listofFontsWeight = new List<string>();
//                foreach (var fontsWeight in FontWrappers.FontWeights)
//                {
//                    listofFontsWeight.Add(fontsWeight.ToString());
//                }
//                return listofFontsWeight.Distinct().ToList();

//            }

//        }


//        public IList<string> AxisFontFamilies
//        {
//            get
//            {
//                IList<string> listofFonts = new List<string>();
//                foreach (var fonts in FontWrappers.GetFontFamilies())
//                {
//                    listofFonts.Add(fonts.ToString());
//                }
//                return listofFonts;
//                //return FontWrappers.GetFontFamilies().Select(a => a.FamilyNames.ToString()).ToList();

//            }

//        }

//        // List<string> IFontFamilies.AxisFontFamilies => throw new NotImplementedException();

//        #endregion

//        #endregion

//        #region Constructor
//        public YAxisDataAccessor()
//        {
//            //this.yaxis = _yaxis;

//            Initialize();

//            //this.title = yaxis.Title;
//            //this.ymin = yaxis.Ymin;
//            //this.ymax = yaxis.Ymax;
//            //this.yTick = yaxis.YTick;
//            //this.axisType = yaxis.AxisType;
//            //this.axisOrientation = yaxis.AxisOrientation;
//            //this.automaticTickGeneration = yaxis.AutomaticTickGeneration;
//        }
//        #endregion

//        #region Methods
//        protected virtual void Initialize()
//        {
//            propertyVisibility = new Dictionary<string, bool>();
//            propertyIsReadonly = new Dictionary<string, bool>();
//            propertyVisibilityOverrides = new Dictionary<string, bool>();
//            propertyIsReadonlyOverrides = new Dictionary<string, bool>();

//            propertyVisibility.Add("Title", true);
//            propertyVisibility.Add("IsAxisInverted", true);
//            propertyVisibility.Add("AutomaticTickGeneration", true);
//            propertyVisibility.Add("Ymin", true);
//            propertyVisibility.Add("Ymax", true);
//            propertyVisibility.Add("YTick", true);
//            propertyVisibility.Add("AxisType", true);
//            propertyVisibility.Add("AxisOrientation", true);
//            propertyVisibility.Add("DateFormat", false);
//            propertyVisibility.Add("NumberFormat", false);
//            propertyVisibility.Add("DecimalPlaces", false);
//            propertyVisibility.Add("FontColor", true);
//            propertyVisibility.Add("IsStackingEnabled", false);

//            propertyIsReadonly.Add("Title", false);
//            propertyIsReadonly.Add("IsAxisInverted", false);
//            propertyIsReadonly.Add("IsStackingEnabled", false);
//            propertyIsReadonly.Add("AutomaticTickGeneration", false);
//            propertyIsReadonly.Add("Ymin", false);
//            propertyIsReadonly.Add("Ymax", false);
//            propertyIsReadonly.Add("YTick", false);
//            propertyIsReadonly.Add("AxisType", false);
//            propertyIsReadonly.Add("AxisOrientation", false);


//            //overrides
//            propertyVisibilityOverrides.Add("Ymin", true);
//            propertyVisibilityOverrides.Add("Ymax", true);
//            propertyVisibilityOverrides.Add("YTick", true);
//            propertyVisibilityOverrides.Add("DateFormat", false);
//            propertyVisibilityOverrides.Add("NumberFormat", false);

//            //overrides
//            propertyIsReadonlyOverrides.Add("Ymin", false);
//            propertyIsReadonlyOverrides.Add("Ymax", false);
//            propertyIsReadonlyOverrides.Add("YTick", false);
//            propertyIsReadonlyOverrides.Add("DateFormat", false);
//            propertyIsReadonlyOverrides.Add("NumberFormat", false);

//        }

//        public virtual void InvalidateProperties()
//        {
//            if (yaxis != null)
//            {
//                this.title = yaxis.Title;
//                this.isAxisInverted = yaxis.IsAxisInverted;
//                this.isStackingEnabled = yaxis.IsStackingEnabled;
//                this.ymin = yaxis.Ymin;
//                this.ymax = yaxis.Ymax;
//                this.yTick = yaxis.YTick;
//                this.fontFamily = yaxis.FontFamily.ToString();
//                this.fontsize = yaxis.FontSize;
//                this.fontWeight = yaxis.FontWeight.ToString();
//                //this.axisType = yaxis.AxisType;
//                this.axisOrientation = yaxis.AxisOrientation;
//                this.automaticTickGeneration = yaxis.AutomaticTickGeneration;
//                if (yaxis.AxisType == DataTypeEnum.Linear)
//                {
//                    this.numberFormat = ((YAxisNumberFormatter)yaxis.AxisFormatter).NumberPrecision;
//                    this.decimalPlaces = ((YAxisNumberFormatter)yaxis.AxisFormatter).DecimalPlaces;
//                }
//                else if (yaxis.AxisType == DataTypeEnum.Date)
//                {
//                    this.dateFormat = ((YAxisDateFormatter)yaxis.AxisFormatter).DateFormat;
//                }
//                else if (yaxis.AxisType == DataTypeEnum.Logarithmic)
//                {
//                    this.decimalPlaces = ((YAxisLogarithmicFormatter)yaxis.AxisFormatter).DecimalPlaces;
//                }

//                switch (yaxis.AxisType)
//                {
//                    case DataTypeEnum.Date:
//                        if (this.yaxis != null)
//                        {
//                            this.ymax = this.yaxis.Ymax / tickYear;
//                            this.ymin = this.yaxis.Ymin / tickYear;
//                            this.yTick = this.yaxis.YTick / tickYear;
//                        }
//                        SetPropertiesForDateAxis();
//                        break;

//                    case DataTypeEnum.Linear:
//                        SetPropertiesForLinearAxis();
//                        break;

//                    case DataTypeEnum.Logarithmic:
//                        if (this.yaxis != null)
//                        {
//                            this.yTick = 1;
//                        }
//                        SetPropertiesForLogAxis();
//                        break;
//                    case DataTypeEnum.Text:
//                        SetPropertiesForStringAxis();
//                        break;

//                }

//            }

//            OnPropertyChanged("Title");
//            OnPropertyChanged("Ymin");
//            OnPropertyChanged("Ymax");
//            OnPropertyChanged("YTick");
//            OnPropertyChanged("Title");
//            OnPropertyChanged("AxisType");
//            OnPropertyChanged("AxisOrientation");
//            OnPropertyChanged("AutomaticTickGeneration");
//            OnPropertyChanged("DateFormat");
//            OnPropertyChanged("NumberFormat");
//            OnPropertyChanged("DecimalPlaces");
//            OnPropertyChanged("IsAxisInverted");
//        }

//        public void SetYAxis(YAxis axis)
//        {
//            this.yaxis = axis;
//            InvalidateProperties();
//        }

//        public YAxis GetYAxis()
//        {
//            return this.yaxis;
//        }

//        private void FontWeightPropertyChanged()
//        {
//            if (this.yaxis != null)
//            {
//                this.yaxis.FontWeight = (FontWeight)fontWeightConverter.ConvertFrom(FontWeight);
//            }

//        }

//        public void SetPropertyVisibility(string propertyName, bool visibility)
//        {
//            if (this.propertyVisibility.ContainsKey(propertyName))
//            {
//                this.propertyVisibility[propertyName] = visibility;
//                this.OnPropertyChanged(propertyName);
//            }
//        }

//        public void SetPropertyReadonlyAttribute(string propertyName, bool isReadonly)
//        {
//            if (this.propertyIsReadonly.ContainsKey(propertyName))
//            {
//                this.propertyIsReadonly[propertyName] = isReadonly;
//                this.OnPropertyChanged(propertyName);
//            }
//        }

//        protected void CoerceAxisTypeChange(DataTypeEnum dataType)
//        {
//            switch (dataType)
//            {
//                case DataTypeEnum.Date:
//                    ConfigureAxisForDateScale();
//                    break;

//                case DataTypeEnum.Text:
//                    ConfigureAxisForStringScale();
//                    break;

//                case DataTypeEnum.Logarithmic:
//                    ConfigureAxisForLogarithmicScale();
//                    break;


//                case DataTypeEnum.Linear:
//                    ConfigureAxisForLinearScale();
//                    break;
//            }
//        }

//        protected void ConfigureAxisForLogarithmicScale()
//        {
//            if (yaxis != null)
//            {
//                double ymin = 0;
//                double ymax = 0;
//                ymin = yaxis.Ymin;
//                ymax = yaxis.Ymax;

//                Optimum_Scale scale = new Optimum_Scale(ymin, ymax, ScaleTypes.Logarithmic);
//                if (ymax % 10 != 0)
//                {
//                    this.yaxis.Ymax = scale.getNiceMax();
//                }

//                if (ymin <= 0 || ymin % 10 != 0)
//                {
//                    this.yaxis.Ymin = scale.getNiceMin();
//                }

//                this.Ymax = this.yaxis.Ymax;
//                this.Ymin = this.yaxis.Ymin;
//                yaxis.UpdateYAxis();
//                this.yaxis.OnAxisFormatChanged();
//            }
//        }

//        protected void ConfigureAxisForLinearScale()
//        {
//            if (yaxis != null)
//            {
//                double ymin = 0;
//                double ymax = 0;
//                ymin = yaxis.Ymin;
//                ymax = yaxis.Ymax;

//                Optimum_Scale scale = new Optimum_Scale(ymin, ymax, ScaleTypes.Logarithmic);
//                this.yaxis.YTick = scale.getTickSpacing();
//                this.YTick = this.yaxis.YTick;
//                yaxis.UpdateYAxis();
//                this.yaxis.OnAxisFormatChanged();
//            }
//        }

//        protected void ConfigureAxisForStringScale()
//        {

//        }

//        protected void ConfigureAxisForDateScale()
//        {

//        }

//        protected virtual void TitlePropertyChanged()
//        {
//            if (yaxis != null)
//                this.yaxis.Title = this.Title;
//        }

//        protected void AutomaticTickGenerationPropertyChanged()
//        {
//            if (yaxis != null)
//            {
//                this.yaxis.AutomaticTickGeneration = this.automaticTickGeneration;
//                yaxis.UpdateYAxis();
//                this.yaxis.OnAxisFormatChanged();
//            }
//            if (this.automaticTickGeneration)
//            {
//                this.propertyIsReadonly["Ymin"] = true;
//                this.propertyIsReadonly["Ymax"] = true;
//                this.propertyIsReadonly["YTick"] = true;

//            }
//            else
//            {
//                this.propertyIsReadonly["Ymin"] = false;
//                this.propertyIsReadonly["Ymax"] = false;
//                this.propertyIsReadonly["YTick"] = false;
//            }

//            this.InvalidateProperties();

//            //this.OnPropertyChanged("Ymax");
//            //this.OnPropertyChanged("Ymin");
//            //this.OnPropertyChanged("YTick");
//        }

//        #region X Min calibration
//        private void YminPropertyChanged(double newValue, double oldValue)
//        {
//            ymin = OnYminChanging(newValue, oldValue);
//            this.OnPropertyChanged("Ymin");
//        }

//        private double OnYminChanging(double newValue, double oldValue)
//        {
//            if (newValue == oldValue)
//            {
//                return newValue;
//            }
//            else
//            {
//                double _value = oldValue;
//                switch (yaxis.AxisType)
//                {
//                    case DataTypeEnum.Date:
//                        _value = CalibrateYminForDateAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Linear:
//                        _value = CalibrateYminForNumericalAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Logarithmic:
//                        _value = CalibrateYminForLogarithmicAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Text:
//                        _value = CalibrateYminForStringAxis(newValue, oldValue);
//                        break;
//                }
//                return _value;
//            }
//        }

//        private double CalibrateYminForLogarithmicAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double _newValue = newValue;
//                if (newValue <= 0)
//                {
//                    _newValue = ymin;

//                }
//                else
//                {
//                    if (newValue >= ymax)
//                    {
//                        double exp = Math.Floor(Math.Log10(ymax));
//                        _newValue = Math.Pow(10, exp - 1);
//                    }
//                    else
//                    {
//                        double exp = Math.Floor(Math.Log10(ymax));
//                        double newValueTemp = (int)Math.Floor(Math.Log10(newValue));
//                        double temp = Math.Pow(10, exp - 1);
//                        if (newValueTemp > exp)
//                        {
//                            _newValue = temp;
//                        }
//                        else
//                        {
//                            double i = exp;
//                            double min = 1;
//                            do
//                            {
//                                i -= 1;
//                                min = Math.Pow(10, i);
//                            } while (newValueTemp < i);
//                            _newValue = min;
//                        }
//                    }
//                }
//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymin = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateYminForNumericalAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;
//                if (newValue >= ymax)
//                {
//                    _newValue = ymax - xtick;
//                }
//                else
//                {
//                    if (newValue + xtick > ymax)
//                    {
//                        _newValue = ymax - xtick;
//                    }
//                    else
//                    {
//                        if (((ymax - ymin) / xtick) > 100)
//                        {
//                            _newValue = ymax - (100 * xtick);
//                        }
//                    }
//                }


//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymin = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateYminForDateAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                //tick values are stored in the yaxis but year and month values are displayed
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;

//                //we cannot have negative values for date
//                if (newValue < 0)
//                {
//                    newValue = (-1) * newValue;
//                }
//                double _newValue = newValue;

//                //determine if valid ymin is passed. A date min must have a decimal value
//                //;so we address this issue first
//                double month = 0;
//                double year = Math.Floor(newValue);
//                GetYearMonth(newValue, out year, out month);

//                double newYminTicks = 0;

//                //if the year path is < 1900 or greater 2200
//                if (year < 1900)
//                {
//                    year = 1900; month = 0;
//                }
//                else
//                {
//                    if (year > 2200)
//                    {
//                        year = 2200;
//                        month = 0;
//                    }
//                }

//                //after resolving the year and month, we compare it to our previous values using ticks
//                newYminTicks = (year + (month)) * tickYear;

//                if (newYminTicks > ymax || ymax - xtick < newYminTicks)
//                {
//                    newYminTicks = ymax - xtick;
//                }
//                else
//                {
//                    if (((ymax - newYminTicks) / xtick) > 100)
//                    {
//                        newYminTicks = (ymax) - (100 * xtick);
//                    }
//                }

//                //we need to convert back to Year.Month format
//                _newValue = (newYminTicks / tickYear);

//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymin = newYminTicks;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();

//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateYminForStringAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;
//                if (newValue >= ymax)
//                {
//                    _newValue = ymax - xtick;
//                }
//                else
//                {
//                    if (newValue + xtick > ymax)
//                    {
//                        _newValue = ymax - xtick;
//                    }
//                    else
//                    {
//                        if (((ymax - ymin) / xtick) > 100)
//                        {
//                            _newValue = ymax - (100 * xtick);
//                        }
//                    }
//                }


//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymin = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }
//        #endregion

//        #region X Max calibration
//        private void YmaxPropertyChanged(double newValue, double oldValue)
//        {
//            ymax = OnYmaxChanging(newValue, oldValue);
//            this.OnPropertyChanged("Ymax");
//        }

//        private double OnYmaxChanging(double newValue, double oldValue)
//        {
//            if (newValue == oldValue)
//            {
//                return newValue;
//            }
//            else
//            {
//                double _value = oldValue;
//                switch (yaxis.AxisType)
//                {
//                    case DataTypeEnum.Date:
//                        _value = CalibrateYmaxForDateAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Linear:
//                        _value = CalibrateYmaxForNumericalAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Logarithmic:
//                        _value = CalibrateYmaxForLogarithmicAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Text:
//                        _value = CalibrateYmaxForStringAxis(newValue, oldValue);
//                        break;
//                }
//                return _value;
//            }
//        }

//        private double CalibrateYmaxForLogarithmicAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double _newValue = newValue;
//                if (newValue <= 0)
//                {
//                    //max value cannot be less than 1 and must be greater than min value
//                    double exp = Math.Floor(Math.Log10(ymin));
//                    _newValue = Math.Pow(10, exp + 1);

//                }
//                else
//                {
//                    //we cannot have a value like 60 as the max value.The next logical log value will be
//                    //selected. So 100 will be selected. That's what we try to accomplish here
//                    double exp = Math.Floor(Math.Log10(ymin));
//                    double newValueTemp = (int)Math.Floor(Math.Log10(newValue));
//                    double temp = Math.Pow(10, exp + 1);
//                    if (newValueTemp < exp)
//                    {
//                        _newValue = temp;
//                    }
//                    else
//                    {
//                        double i = exp;
//                        double max = 0;
//                        do
//                        {
//                            i += 1;
//                            max = Math.Pow(10, i);
//                        } while (newValueTemp > i);
//                        _newValue = max;
//                        //_newValue = newValueTemp;
//                    }
//                }
//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymax = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateYmaxForNumericalAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;
//                if (newValue <= ymin)
//                {
//                    _newValue = ymin + xtick;
//                }
//                else
//                {
//                    if (newValue - xtick < ymin)
//                    {
//                        _newValue = ymin + xtick;
//                    }
//                    else
//                    {
//                        if (((ymax - ymin) / xtick) > 100)
//                        {
//                            _newValue = ymin + (100 * xtick);
//                        }
//                    }
//                }


//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymax = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateYmaxForDateAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                //tick values are stored in the yaxis but year and month values are displayed
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                if (newValue < 0)
//                {
//                    newValue = (-1) * newValue;
//                }
//                double _newValue = newValue;

//                //determine if valid ymin is passed. A date min must have a decimal value
//                //;so we address this issue first
//                double month = 0;
//                double year = Math.Floor(newValue);
//                GetYearMonth(newValue, out year, out month);

//                double newYmayTicks = 0;

//                //if the year path is < 1900 or greater 2200
//                if (year < 1900)
//                {
//                    year = 1900; month = 0;
//                }
//                else
//                {
//                    if (year > 2200)
//                    {
//                        year = 2200;
//                        month = 0;
//                    }
//                }

//                //after resolving the year and month, we compare it to our previous values using ticks
//                newYmayTicks = (year + (month)) * tickYear;

//                if (newYmayTicks < ymin || ymin + xtick > newYmayTicks)
//                {
//                    newYmayTicks = ymin + xtick;
//                }
//                else
//                {
//                    if ((newYmayTicks - ymin) / xtick > 100)
//                    {
//                        newYmayTicks = ymin + (xtick * 100);
//                    }
//                }

//                //we need to convert back to Year.Month format
//                _newValue = (newYmayTicks / tickYear);

//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymax = newYmayTicks;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();

//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateYmaxForStringAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;
//                if (newValue <= ymin)
//                {
//                    _newValue = ymin + xtick;
//                }
//                else
//                {
//                    if (newValue - xtick < ymin)
//                    {
//                        _newValue = ymin + xtick;
//                    }
//                    else
//                    {
//                        if (((ymax - ymin) / xtick) > 100)
//                        {
//                            _newValue = ymin + (100 * xtick);
//                        }
//                    }
//                }


//                yaxis.ReCalibrateAxis = false;
//                yaxis.Ymax = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }
//        #endregion

//        #region X Tick calibration
//        private void YTickPropertyChanged(double newValue, double oldValue)
//        {
//            yTick = OnYTickChanging(newValue, oldValue);
//            this.OnPropertyChanged("YTick");
//        }

//        private double OnYTickChanging(double newValue, double oldValue)
//        {
//            if (newValue == oldValue)
//            {
//                return newValue;
//            }
//            else
//            {
//                double _value = oldValue;
//                switch (yaxis.AxisType)
//                {
//                    case DataTypeEnum.Date:
//                        _value = CalibrateXtickForDateAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Linear:
//                        _value = CalibrateXtickForNumericalAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Logarithmic:
//                        _value = CalibrateXtickForLogarithmicAxis(newValue, oldValue);
//                        break;

//                    case DataTypeEnum.Text:
//                        _value = CalibrateXtickForStringAxis(newValue, oldValue);
//                        break;
//                }
//                return _value;
//            }
//        }

//        private double CalibrateXtickForLogarithmicAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                //we can't specify tick values for Log table so move on
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;

//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateXtickForNumericalAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;
//                if (newValue == 0)
//                {
//                    _newValue = oldValue;
//                }
//                else
//                {
//                    if (newValue > (ymax - ymin))
//                    {
//                        _newValue = ymax - ymin;
//                    }
//                    else
//                    {
//                        if (((ymax - ymin) / newValue) > 100)
//                        {
//                            _newValue = (ymax - ymin) / 100.0;
//                        }
//                    }

//                }


//                yaxis.ReCalibrateAxis = false;
//                yaxis.YTick = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateXtickForDateAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                //tick values are stored in the yaxis but year and month values are displayed
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                if (newValue < 0)
//                {
//                    newValue = (-1) * newValue;
//                }
//                double _newValue = newValue;

//                //determine if valid ymin is passed. A date min must have a decimal value
//                //;so we address this issue first
//                double month = 0;
//                double year = Math.Floor(newValue);
//                GetYearMonth(newValue, out year, out month);

//                double newYminTicks = 0;

//                //after resolving the year and month, we compare it to our previous values using ticks
//                newYminTicks = (year + (month)) * tickYear;

//                if (newYminTicks > ymax - ymin)
//                {
//                    newYminTicks = ymax - ymin;
//                }
//                else
//                {
//                    if (newYminTicks < ((ymax - ymin) / 100))
//                    {
//                        newYminTicks = ((ymax - ymin) / 100);
//                    }
//                }

//                //we need to convert back to Year.Month format
//                _newValue = (newYminTicks / tickYear);

//                yaxis.ReCalibrateAxis = false;
//                yaxis.YTick = newYminTicks;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisPropertyChanged();

//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }

//        private double CalibrateXtickForStringAxis(double newValue, double oldValue)
//        {
//            if (yaxis != null)
//            {
//                double ymax = this.yaxis.Ymax;
//                double ymin = this.yaxis.Ymin;
//                double xtick = this.yaxis.YTick;
//                double _newValue = newValue;
//                if (newValue == 0)
//                {
//                    _newValue = oldValue;
//                }
//                else
//                {
//                    if (newValue > (ymax - ymin))
//                    {
//                        _newValue = ymax - ymin;
//                    }
//                    else
//                    {
//                        if (((ymax - ymin) / newValue) > 100)
//                        {
//                            _newValue = (ymax - ymin) / 100.0;
//                        }
//                    }

//                }


//                yaxis.ReCalibrateAxis = false;
//                yaxis.YTick = _newValue;
//                yaxis.ReCalibrateAxis = true;
//                yaxis.UpdateYAxis();
//                yaxis.OnAxisFormatChanged();
//                return _newValue;
//            }
//            else
//            {
//                return oldValue;
//            }
//        }
//        #endregion

//        private int OnDecimalPlacesChanged(int value)
//        {
//            int result = value;

//            if (value < 0)
//            {
//                result = 0;
//            }
//            else if (decimalPlaces > 12)
//            {
//                result = 12;
//            }
//            else
//            {
//                result = value;
//            }

//            return result;
//        }

//        protected double GetYmax()
//        {
//            double _ymax = Math.Round(ymax, 1);

//            //if (yaxis != null)
//            //{
//            //    if (yaxis.AxisType == DataTypeEnum.Date)
//            //    {
//            //        ymax = yaxis. / tickYear;
//            //    }
//            //}

//            return ymax;
//        }

//        protected double GetYmin()
//        {
//            double _ymin = Math.Round(ymin, 1);

//            //if (yaxis != null)
//            //{
//            //    if (yaxis.AxisType == DataTypeEnum.Date)
//            //    {
//            //        _ymin = _ymin / tickYear;
//            //    }
//            //}

//            return ymin;
//        }

//        protected double GetYtick()
//        {
//            double _ytick = yTick;

//            if (yaxis != null)
//            {
//                //if (yaxis.AxisType == DataTypeEnum.Date)
//                //{
//                //    _xtick = _xtick / tickYear;
//                //}
//                //else
//                //{


//                //}
//                if (yaxis.AxisType == DataTypeEnum.Logarithmic)
//                {
//                    _ytick = 1;
//                }
//            }

//            return _ytick;
//        }

//        protected void GetYearMonth(double number, out double year, out double month)
//        {
//            month = 1;
//            year = Math.Floor(number);
//            month = (number - year);

//        }

//        //protected void AxisTypePropertyChanged()
//        //{
//        //    //this.CoerceAxisTypeChange(this.AxisType);
//        //    if (this.yaxis != null)
//        //    {
//        //        this.yaxis.AxisType = this.AxisType;
//        //        this.ymax = this.yaxis.Ymax;
//        //        this.ymin = this.yaxis.Ymin;
//        //        this.yTick = this.yaxis.YTick;

//        //    }
//        //    switch (yaxis.AxisType)
//        //    {
//        //        case DataTypeEnum.Date:
//        //            if (this.yaxis != null)
//        //            {
//        //                this.ymax = this.yaxis.Ymax / tickYear;
//        //                this.ymin = this.yaxis.Ymin / tickYear;
//        //                this.yTick = this.yaxis.YTick / tickYear;
//        //            }
//        //            SetPropertiesForDateAxis();
//        //            break;

//        //        case DataTypeEnum.Linear:
//        //            SetPropertiesForLinearAxis();
//        //            break;

//        //        case DataTypeEnum.Logarithmic:
//        //            if (this.yaxis != null)
//        //            {
//        //                this.yTick = 1;
//        //            }
//        //            SetPropertiesForLogAxis();
//        //            break;
//        //        case DataTypeEnum.Text:
//        //            SetPropertiesForStringAxis();
//        //            break;

//        //    }

//        //    OnPropertyChanged("AxisType");
//        //    OnPropertyChanged("Ymin");
//        //    OnPropertyChanged("Ymax");
//        //    OnPropertyChanged("YTick");
//        //    OnPropertyChanged("NumberFormat");
//        //    OnPropertyChanged("DateFormat");
//        //}

//        protected void SetPropertiesForStringAxis()
//        {
//            //property visibility
//            propertyVisibilityOverrides["Ymax"] = true;
//            propertyVisibilityOverrides["Ymin"] = true;
//            propertyVisibilityOverrides["YTick"] = true;
//            propertyVisibilityOverrides["DateFormat"] = false;
//            propertyVisibilityOverrides["NumberFormat"] = false;
//            propertyVisibilityOverrides["DecimalPlaces"] = false;
//            //property readonly
//            propertyIsReadonlyOverrides["Ymax"] = true;
//            propertyIsReadonlyOverrides["Ymin"] = true;
//            propertyIsReadonlyOverrides["YTick"] = true;
//            propertyIsReadonlyOverrides["DateFormat"] = true;
//            propertyIsReadonlyOverrides["NumberFormat"] = true;
//        }

//        protected void SetPropertiesForLogAxis()
//        {
//            //property visibility
//            propertyVisibilityOverrides["Ymax"] = true;
//            propertyVisibilityOverrides["Ymin"] = true;
//            propertyVisibilityOverrides["YTick"] = false;
//            propertyVisibilityOverrides["DateFormat"] = false;
//            //propertyVisibilityOverrides["NumberFormat"] = false;
//            propertyVisibilityOverrides["DecimalPlaces"] = true;
//            //property readonly
//            propertyIsReadonlyOverrides["YTick"] = true;
//            if (yaxis != null)
//            {
//                if (yaxis.AutomaticTickGeneration)
//                {
//                    propertyIsReadonlyOverrides["Ymax"] = true;
//                    propertyIsReadonlyOverrides["Ymin"] = true;
//                }
//                else
//                {
//                    propertyIsReadonlyOverrides["Ymax"] = false;
//                    propertyIsReadonlyOverrides["Ymin"] = false;
//                }
//            }
//            else
//            {
//                propertyIsReadonlyOverrides["Ymax"] = false;
//                propertyIsReadonlyOverrides["Ymin"] = false;
//            }
//            propertyIsReadonlyOverrides["DateFormat"] = true;
//            propertyIsReadonlyOverrides["NumberFormat"] = true;
//        }

//        protected void SetPropertiesForLinearAxis()
//        {
//            //property visibility
//            propertyVisibilityOverrides["Ymax"] = true;
//            propertyVisibilityOverrides["Ymin"] = true;
//            propertyVisibilityOverrides["YTick"] = true;
//            propertyVisibilityOverrides["DateFormat"] = false;
//            //propertyVisibilityOverrides["NumberFormat"] = true;
//            propertyVisibilityOverrides["DecimalPlaces"] = true;
//            //property readonly
//            if (yaxis != null)
//            {
//                if (yaxis.AutomaticTickGeneration)
//                {
//                    propertyIsReadonlyOverrides["Ymax"] = true;
//                    propertyIsReadonlyOverrides["Ymin"] = true;
//                    propertyIsReadonlyOverrides["YTick"] = true;
//                }
//                else
//                {
//                    propertyIsReadonlyOverrides["Ymax"] = false;
//                    propertyIsReadonlyOverrides["Ymin"] = false;
//                    propertyIsReadonlyOverrides["YTick"] = false;
//                }
//            }
//            else
//            {
//                propertyIsReadonlyOverrides["Ymax"] = false;
//                propertyIsReadonlyOverrides["Ymin"] = false;
//                propertyIsReadonlyOverrides["YTick"] = false;
//            }
//            propertyIsReadonlyOverrides["DateFormat"] = true;
//            propertyIsReadonlyOverrides["NumberFormat"] = false;
//        }

//        protected void SetPropertiesForDateAxis()
//        {
//            //property visibility
//            propertyVisibilityOverrides["Ymax"] = true;
//            propertyVisibilityOverrides["Ymin"] = true;
//            propertyVisibilityOverrides["YTick"] = true;
//            propertyVisibilityOverrides["DateFormat"] = true;
//            propertyVisibilityOverrides["NumberFormat"] = false;
//            propertyVisibilityOverrides["DecimalPlaces"] = false;
//            //property readonly
//            if (yaxis != null)
//            {
//                if (yaxis.AutomaticTickGeneration)
//                {
//                    propertyIsReadonlyOverrides["Ymax"] = true;
//                    propertyIsReadonlyOverrides["Ymin"] = true;
//                    propertyIsReadonlyOverrides["YTick"] = true;
//                }
//                else
//                {
//                    propertyIsReadonlyOverrides["Ymax"] = false;
//                    propertyIsReadonlyOverrides["Ymin"] = false;
//                    propertyIsReadonlyOverrides["YTick"] = false;
//                }
//            }
//            else
//            {
//                propertyIsReadonlyOverrides["Ymax"] = false;
//                propertyIsReadonlyOverrides["Ymin"] = false;
//                propertyIsReadonlyOverrides["YTick"] = false;
//            }
//            propertyIsReadonlyOverrides["DateFormat"] = false;
//            propertyIsReadonlyOverrides["NumberFormat"] = true;
//        }

//        protected void AxisOrientationPropertyChanged()
//        {
//            if (this.yaxis != null)
//            {
//                this.yaxis.AxisOrientation = this.AxisOrientation;
//            }
//        }

//        protected void IsAxisInvertedPropertyChanged()
//        {
//            if (this.yaxis != null)
//            {
//                this.yaxis.IsAxisInverted = this.isAxisInverted;
//                this.yaxis.UpdateYAxis();
//                this.yaxis.OnAxisFormatChanged();
//            }
//            OnPropertyChanged("IsAxisInverted");
//        }

//        protected void IsStackingPropertyChanged()
//        {
//            if (this.yaxis != null)
//            {
//                this.yaxis.IsStackingEnabled = this.IsStackingEnabled;
//                this.yaxis.UpdateYAxis();
//                this.yaxis.OnAxisFormatChanged();
//            }
//            OnPropertyChanged("IsStackingEnabled");
//        }
//        #endregion

//        #region IPropertyVisibility
//        public bool GetPropertyVisibility(string propertyName)
//        {
//            if (this.propertyVisibility.ContainsKey(propertyName))
//            {
//                if (this.propertyVisibilityOverrides.ContainsKey(propertyName))
//                {
//                    return propertyVisibilityOverrides[propertyName];
//                }
//                else
//                {
//                    return propertyVisibility[propertyName];
//                }
//            }
//            else
//            {
//                if (this.propertyVisibilityOverrides.ContainsKey(propertyName))
//                {
//                    return propertyVisibilityOverrides[propertyName];
//                }
//                else
//                {
//                    return true;
//                }
//            }
//        }
//        #endregion

//        #region IPropertyReadOnly
//        public bool GetPropertyReadOnly(string propertyName)
//        {
//            if (this.propertyIsReadonly.ContainsKey(propertyName))
//            {
//                if (this.propertyIsReadonlyOverrides.ContainsKey(propertyName))
//                {
//                    return propertyIsReadonlyOverrides[propertyName];
//                }
//                else
//                {
//                    return propertyIsReadonly[propertyName];
//                }

//            }
//            else
//            {
//                if (this.propertyIsReadonlyOverrides.ContainsKey(propertyName))
//                {
//                    return propertyIsReadonlyOverrides[propertyName];
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }
//        #endregion

//        #region INotifyPropertyChanged
//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void OnPropertyChanged(string propertyName)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
//            }
//        }
//        #endregion
//    }
//}
