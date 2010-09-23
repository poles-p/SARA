using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SARA;
using SARA.Core;
using SARA.SimpleGUI.Image;
using SARA.Astrometry;
using SARA.Fotometry;
using SARA.Utils;
using SARA.Avi;
using SARA.FITS;
using SARA.SimpleGUI.Plot;
using System.Drawing;

namespace SARA.Programs
{
    /// <summary>
    /// Simple program to automatic fotometry.
    /// </summary>
    public class AutoFoto0
    {
        private const string _helloText = "AutoFoto0 v0.2 by Piotr Polesiuk\n\n";

        private ConfigFile _configFile;
        private ImageWindow _imageWindow;

        #region parametrs from config file

        private string _inputFormat;

        private float _aperture1;
        private float _aperture2;
        private float _aperture3;

        private bool _manualTracking;
        private bool _manualTrackingAutoCenter;
        private int _autoCenterTolerance;

        private bool _allowMovableObjects;
        private bool _allowReferenceTracking;
        private int _defaultTolerance;

        private bool _showCalibrationSource;
        private int _waitOnCalibrationSource;

        private bool _showCalibrationImages;
        private int _waitOnCalibrationImages;

        #endregion

        #region other parametrs

        private IEnumerable<FloatMatrix> _objectData;
        private int _objectDataLength;

        #endregion

        /// <summary>
        /// Start program.
        /// </summary>
        /// <param name="configFilePath">
        /// Path to config file that contains program parametrs.
        /// </param>
        public static void Run(string configFilePath)
        {
            AutoFoto0 program = new AutoFoto0(configFilePath);
            if (program._configFile.GetBoolParam("DisplayTextMessages"))
                Console.WriteLine(_helloText);
            program.Run();
        }

        static void Main(string[] args)
        {
            Console.WriteLine(_helloText);

            // Test command line arguments and start.
            if (args.Length != 1)
                Console.WriteLine("Usage :\n>AutoFoto0 <configFile>\n\n");
            else
            {
                try
                {
                    AutoFoto0 program = new AutoFoto0(args[0]);
                    program.Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private AutoFoto0(string configFilePath)
        {
            // open config file
            _configFile = new ConfigFile(configFilePath);

            // read parametrs
            ReadParametrs();

            // initialize image window
            _imageWindow = new ImageWindow();
        }

        private void Run()
        {
            // read data
            {
                string[] dataPaths = _configFile.GetPathsParam("Data");
                _objectData = GetSequence(dataPaths);
                if ((_configFile.GetBoolParam("UseFlat") && _configFile.GetBoolParam("UseFlat2"))
                    || (_configFile.GetBoolParam("UseDark") && _configFile.GetBoolParam("UseDark2")))
                    _objectDataLength = GetSequenceLength(dataPaths);
            }

            // dark processing
            _objectData = Debias(_objectData, _objectDataLength, "");

            // flat processing
            if (_configFile.GetBoolParam("UseFlat"))
            {
                string[] flatPaths = _configFile.GetPathsParam("Flat");
                IEnumerable<FloatMatrix> flatData = GetSequence(flatPaths);
                int flatDataLength =
                    (_configFile.GetBoolParam("UseFlatDark") && _configFile.GetBoolParam("UseFlatDark2"))
                        ? GetSequenceLength(flatPaths)
                        : 0;

                flatData = Debias(flatData, flatDataLength, "Flat");

                FloatMatrix masterFlat = _showCalibrationSource
                        ? CalcMeanAndShow(flatData, "Flat")
                        : BasicReduction.MasterBias(flatData);

                masterFlat = BasicReduction.NormImage(masterFlat);

                ShowMaster(masterFlat, "Master Flat");

                if (_configFile.GetBoolParam("UseFlat2"))
                {
                    string[] flat2Paths = _configFile.GetPathsParam("Flat2");
                    IEnumerable<FloatMatrix> flat2Data = GetSequence(flat2Paths);
                    int flat2DataLength =
                        (_configFile.GetBoolParam("UseFlat2Dark") && _configFile.GetBoolParam("UseFlat2Dark2"))
                            ? GetSequenceLength(flat2Paths)
                            : 0;

                    flat2Data = Debias(flat2Data, flat2DataLength, "Flat2");

                    FloatMatrix masterFlat2 = _showCalibrationSource
                            ? CalcMeanAndShow(flat2Data, "Flat2")
                            : BasicReduction.MasterBias(flat2Data);

                    masterFlat2 = BasicReduction.NormImage(masterFlat2);

                    ShowMaster(masterFlat2, "Master Flat2");

                    _objectData = Deflat2(_objectData, _objectDataLength, masterFlat, masterFlat2);
                }
                else
                    _objectData = BasicReduction.Deflat(_objectData, masterFlat);
            }

            // Show first image
            _imageWindow.ShowImage(_objectData.First());
            _imageWindow.ImgForm.Text = "First image";

            Console.Write("Number of objects : ");
            int numberOfObjects = Int32.Parse(Console.ReadLine());

            StarTrackerByMax[] stars = new StarTrackerByMax[numberOfObjects];
            List<Vector2D>[] results = new List<Vector2D>[numberOfObjects];
            Aperture3R[] fotometry = new Aperture3R[numberOfObjects];
            for (int i = 0; i < numberOfObjects; i++)
            {
                stars[i] = new StarTrackerByMax(new Vector2D());
                results[i] = new List<Vector2D>();
                fotometry[i] = new Aperture3R(_aperture1, _aperture2, _aperture3, stars[i]);
            }

            int id = 0;

            foreach (FloatMatrix image in _objectData)
            {
                // show image
                _imageWindow.ShowImage(image);
                _imageWindow.ImgForm.Text = "Image " + id.ToString();

                // track star and do fotometry
                if (id == 0 || _manualTracking)
                {
                    for (int i = 0; i < numberOfObjects; i++)
                    {
                        Console.WriteLine("========== Object {0} ==========", i);
                        Console.WriteLine("Select object");
                        stars[i].Position = _imageWindow.ImgForm.SelectPoint();
                        if (_manualTrackingAutoCenter)
                        {
                            stars[i].Tolerance = _autoCenterTolerance;
                            stars[i].Track(image);
                            _imageWindow.ImgForm.Refresh();
                        }
                        if (id == 0)
                            _imageWindow.ImgForm.AddStar(stars[i]);

                        if (!_manualTracking)
                        {
                            if (_defaultTolerance == -1)
                            {
                                Console.Write("Tolerance : ");
                                stars[i].Tolerance = (float)Int32.Parse(Console.ReadLine());
                            }
                            else
                                stars[i].Tolerance = _defaultTolerance;

                            if (i != 0 && _allowMovableObjects)
                            {
                                Console.Write("Movable? (y/n) : ");
                                stars[i].Movable = Console.ReadLine().Trim().ToLower() == "y";
                            }

                            if (i != 0 && _allowReferenceTracking)
                            {
                                Console.Write("Reference object (press return if no ref. obj) : ");
                                string refObj = Console.ReadLine();
                                if (refObj != "")
                                    stars[i].Reference = stars[Int32.Parse(refObj)];
                            }
                        }
                        results[i].Add(new Vector2D((float)id, fotometry[i].GetResult(image).Magnitudo));
                    }
                }
                else
                {
                    for (int i = 0; i < numberOfObjects; i++)
                    {
                        stars[i].Track(image);
                        results[i].Add(new Vector2D((float)id, fotometry[i].GetResult(image).Magnitudo));
                    }

                    _imageWindow.ImgForm.Refresh();
                }

                id++;
            }

            if (_configFile.GetBoolParam("ShowPlot"))
            {
                PlotForm plot = new PlotForm();

                plot.MinX = 0;
                plot.MaxX = (float)id;
                plot.MinY = 0.0f;
                plot.MaxY = -5.0f;

                foreach (var data in results)
                    plot.Add(new DataSet(data.ToArray(), new PlotPointCross(10, Color.Blue)));

                plot.ShowDialog();
            }

            if (_configFile.GetBoolParam("SaveResults"))
            {
                IEnumerator<Vector2D>[] res = new IEnumerator<Vector2D>[results.Length];
                for (int i = 0; i < res.Length; i++)
                {
                    res[i] = results[i].GetEnumerator();
                    res[i].Reset();
                    if (!res[i].MoveNext())
                        res[i] = null;
                }

                StreamWriter writer = File.CreateText(_configFile.GetStringParam("OutputFile"));

                for (int num = 0; num < id; num++)
                {
                    writer.Write(num);
                    for (int i = 0; i < res.Length; i++)
                    {
                        if (res[i] != null && res[i].Current.X == (float)num)
                        {
                            writer.Write("\t{0}", res[i].Current.Y);
                            if (!res[i].MoveNext())
                                res[i] = null;
                        }
                        else
                            writer.Write("\t---------");
                    }
                    writer.WriteLine();
                }

                writer.Close();
            }
        }

        private void ReadParametrs()
        {
            _inputFormat = _configFile.GetStringParam("InputFormat");

            _aperture1 = _configFile.GetFloatParam("Aperture1");
            _aperture2 = _configFile.GetFloatParam("Aperture2");
            _aperture3 = _configFile.GetFloatParam("Aperture3");

            _manualTracking = _configFile.GetBoolParam("ManualTracking");
            _manualTrackingAutoCenter = _configFile.GetBoolParam("ManualTrackingAutoCenter");
            _autoCenterTolerance = _configFile.GetIntParam("AutoCenterTolerance");

            _allowMovableObjects = _configFile.GetBoolParam("AllowMovableObjects");
            _allowReferenceTracking = _configFile.GetBoolParam("AllowReferenceTracking");
            _defaultTolerance = _configFile.GetIntParam("DefaultTolerance");

            _showCalibrationSource = _configFile.GetBoolParam("ShowCalibrationSource");
            _waitOnCalibrationSource = _configFile.GetIntParam("WaitOnCalibrationSource");

            _showCalibrationImages = _configFile.GetBoolParam("ShowCalibrationImages");
            _waitOnCalibrationImages = _configFile.GetIntParam("WaitOnCalibrationImages");
        }

        private int GetSequenceLength(string[] paths)
        {
            switch (_inputFormat.ToUpper())
            {
                case "AUTO":
                    {
                        int result = 0;
                        foreach (string path in paths)
                        {
                            string pu = path.ToUpper();
                            if (pu.EndsWith(".FITS") || pu.EndsWith(".FTS") || pu.EndsWith(".FIT"))
                                result++;
                            else if (pu.EndsWith(".AVI"))
                                result += (new AviFile(path)).GetVideoStream().Length;
                            else
                                throw new ArgumentException("Unknown file extension : " + path);
                        }
                        return result;
                    }
                case "FITS":
                    return paths.Length;
                case "AVI":
                    {
                        int result = 0;
                        foreach (string path in paths)
                            result += (new AviFile(path)).GetVideoStream().Length;
                        return result;
                    }
                default:
                    throw new ArgumentException("Unknown file format : " + _inputFormat);
            }
        }

        private IEnumerable<FloatMatrix> GetSequence(string[] paths)
        {
            switch (_inputFormat.ToUpper())
            {
                case "AUTO":
                    foreach (string path in paths)
                    {
                        string pu = path.ToUpper();
                        if (pu.EndsWith(".FITS") || pu.EndsWith(".FTS") || pu.EndsWith(".FIT"))
                            yield return (new FitsFile(path)).Data.ToFloatMatrix();
                        else if (pu.EndsWith(".AVI"))
                            foreach (FloatMatrix frame in (new AviFile(path)).GetVideoStream().GetAllFrames())
                                yield return frame;
                        else
                            throw new ArgumentException("Unknown file extension : " + path);
                    }
                    break;
                case "FITS":
                    foreach (string path in paths)
                        yield return (new FitsFile(path)).Data.ToFloatMatrix();
                    break;
                case "AVI":
                    foreach (string path in paths)
                        foreach (FloatMatrix frame in (new AviFile(path)).GetVideoStream().GetAllFrames())
                            yield return frame;
                    break;
                default:
                    throw new ArgumentException("Unknown file format : " + _inputFormat);
            }
        }

        private FloatMatrix CalcMeanAndShow(IEnumerable<FloatMatrix> source, string title)
        {
            FloatMatrix result = new FloatMatrix(new DataMatrix<float>(source.First().Dimensions));
            int n = 0;

            foreach (FloatMatrix image in source)
            {
                n++;
                _imageWindow.ShowImage(image);
                _imageWindow.ImgForm.Text = title + " " + n.ToString();

                result.Add(image);

                Wait(_waitOnCalibrationSource);
            }

            result.Divide((float)n);

            return result;
        }

        private void Wait(int miliseconds)
        {
            if (miliseconds < 0)
            {
                Console.WriteLine("Press return key to continue ...");
                Console.ReadLine();
            }
            else
                System.Threading.Thread.Sleep(miliseconds);
        }

        private void ShowMaster(FloatMatrix image, string title)
        {
            if (_showCalibrationImages)
            {
                _imageWindow.ShowImage(image);
                _imageWindow.ImgForm.Text = title;

                Wait(_waitOnCalibrationImages);
            }
        }

        private IEnumerable<FloatMatrix> Debias2(IEnumerable<FloatMatrix> source, int length, FloatMatrix bias1, FloatMatrix bias2)
        {
            if (length <= 1)
            {
                FloatMatrix bias = bias1 * 0.5f;
                bias.Add(bias2 * 0.5f);

                foreach (FloatMatrix image in source)
                {
                    yield return image - bias;
                }
            }
            else
            {
                float a = 1.0f / (float)(length - 1);
                int n = 0;

                foreach (FloatMatrix image in source)
                {
                    float c = (float)n;

                    FloatMatrix bias = bias2 * c;
                    bias.Add(bias1 * (1.0f - c));

                    yield return image - bias;
                }
            }
        }

        private IEnumerable<FloatMatrix> Deflat2(IEnumerable<FloatMatrix> source, int length, FloatMatrix flat1, FloatMatrix flat2)
        {
            if (length <= 1)
            {
                FloatMatrix flat = flat1 * 0.5f;
                flat.Add(flat2 * 0.5f);

                foreach (FloatMatrix image in source)
                {
                    yield return image / flat;
                }
            }
            else
            {
                float a = 1.0f / (float)(length - 1);
                int n = 0;

                foreach (FloatMatrix image in source)
                {
                    float c = (float)n;

                    FloatMatrix flat = flat2 * c;
                    flat.Add(flat1 * (1.0f - c));

                    yield return image / flat;
                }
            }
        }

        private IEnumerable<FloatMatrix> Debias(IEnumerable<FloatMatrix> source, int length, string sequenceName)
        {
            if (_configFile.GetBoolParam("Use" + sequenceName + "Dark"))
            {
                IEnumerable<FloatMatrix> darkData = GetSequence(_configFile.GetPathsParam(sequenceName + "Dark"));

                FloatMatrix masterDark = _showCalibrationSource
                    ? CalcMeanAndShow(darkData, sequenceName + "Dark")
                    : BasicReduction.MasterBias(darkData);

                ShowMaster(masterDark, "Master " + sequenceName + "Dark");

                // dark 2 processing
                if (_configFile.GetBoolParam("Use" + sequenceName + "Dark2"))
                {
                    IEnumerable<FloatMatrix> dark2Data = GetSequence(_configFile.GetPathsParam(sequenceName + "Dark2"));

                    FloatMatrix masterDark2 = _showCalibrationSource
                        ? CalcMeanAndShow(dark2Data, sequenceName + "Dark2")
                        : BasicReduction.MasterBias(dark2Data);

                    ShowMaster(masterDark2, "Master " + sequenceName + "Dark2");

                    return Debias2(source, length, masterDark, masterDark2);
                }
                else
                    return BasicReduction.Debias(source, masterDark);
            }

            return source;
        }
    }
}
