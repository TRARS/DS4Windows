using CustomMacroBase.Helper;
using OpenCvSharp;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models.Local;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Point = OpenCvSharp.Point;
using Rect = OpenCvSharp.Rect;
using SD = System.Drawing;

//对外
namespace CustomMacroBase.PixelMatcher
{
    /// <summary>
    /// OpenCvSharp封装
    /// </summary>
    public partial class OpenCV
    {
        private static readonly Lazy<OpenCV> lazyObject = new(() => new OpenCV());
        public static OpenCV Instance => lazyObject.Value;

        private OpenCV()
        {
            this.Init();
        }
    }
    public partial class OpenCV
    {
        private bool can_update = false;
        private bool task_is_running = false;

        private void Init()
        {
            Mediator.Instance.Register(MessageType.CanUpdateFrames, (para) =>
            {
                can_update = (bool)para;
            });
        }
        private void Print(string str = "")
        {
            Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
        }
        private void UpdateToSnapshotArea(Mat source, string? msg = null)
        {
            if (can_update is false) { return; } //截图区域未展开时不更新

            if (task_is_running is false)
            {
                task_is_running = true;

                Task.Run(async () =>
                {
                    using (source)
                    {
                        await MediatorAsync.Instance.NotifyColleagues(AsyncMessageType.AsyncSnapshot, OpenCvSharp.Extensions.BitmapConverter.ToBitmap(source));

                        if (msg is not null) { Print($"{msg}"); }
                    }
                }).ContinueWith(_ => { task_is_running = false; });
            }
        }
    }
    public partial class OpenCV
    {
        private struct PaddleOcrAllParameter
        {
            public DeviceType device { get; set; }
            public ModelType model { get; set; }

            public PaddleOcrAllParameter(DeviceType d, ModelType m)
            {
                device = d;
                model = m;
            }
        }
        private sealed class PaddleOcrAllManager
        {
            ConcurrentDictionary<string, bool> device2flag = new(); //lock
            ConcurrentDictionary<string, PaddleOcrAll> device2ocrall = new();

            /// <summary>
            /// 获取PaddleOcrAll对象
            /// </summary>
            public PaddleOcrAll? GetPaddleOcrAll(DeviceType _devicetype, ModelType _modeltype)
            {
                var key = $"{_devicetype}_{_modeltype}";

                if (device2flag.ContainsKey(key) is false)
                {
                    device2flag.TryAdd(key, false);
                    {
                        Print($"Creating PaddleOcrAll object: {key}");
                        {
                            device2ocrall.TryAdd(key, CreatePaddleOcrAll(_devicetype, _modeltype));
                        }
                        Print($"Successfully created PaddleOcrAll object: {key}");
                    }
                    device2flag[key] = true;
                }

                device2ocrall.TryGetValue(key, out var result);

                return result;
            }

            /// <summary>
            /// 打印至MacroWindow
            /// </summary>
            private void Print(string str = "")
            {
                Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
            }

            /// <summary>
            /// 创建PaddleOcrAll对象
            /// </summary>
            private PaddleOcrAll CreatePaddleOcrAll(DeviceType _devicetype, ModelType _modeltype)
            {
                var device = _devicetype switch
                {
                    DeviceType.Mkldnn => PaddleDevice.Mkldnn(),
                    DeviceType.Onnx => PaddleDevice.Onnx(),
                    DeviceType.Openblas => PaddleDevice.Openblas(),
                    DeviceType.Gpu => PaddleDevice.Gpu(),
                    _ => throw new NotImplementedException()
                };

                var usegpu = _devicetype == DeviceType.Gpu; //gpu跑够快，用V4模型更准

                var model = _modeltype switch
                {
                    ModelType.EnglishV3 => usegpu ? LocalFullModels.EnglishV4 : LocalFullModels.EnglishV3,
                    ModelType.JapanV3 => usegpu ? LocalFullModels.JapanV4 : LocalFullModels.JapanV3,
                    ModelType.ChineseV3 => usegpu ? LocalFullModels.ChineseV4 : LocalFullModels.ChineseV3,
                    ModelType.TraditionalChineseV3 => LocalFullModels.TraditionalChineseV3, //无V4
                    _ => throw new NotImplementedException()
                };

                var all = new PaddleOcrAll(model, device)
                {
                    AllowRotateDetection = false, /* 允许识别有角度的文字 */
                    Enable180Classification = false, /* 允许识别旋转角度大于90度的文字 */
                };

                return all;
            }
        }

        private readonly PaddleOcrAllManager paddleOcrAllManager = new();
    }
    public partial class OpenCV
    {
        public enum TextType
        {
            Number = 0,
            Word = 1,
        }
        public enum DeviceType
        {
            Mkldnn = 0,
            Onnx,
            Openblas,
            Gpu
        }
        public enum ModelType
        {
            EnglishV3 = 0,
            JapanV3,
            ChineseV3,
            TraditionalChineseV3
        }
    }

    //对外公开方法
    public partial class OpenCV
    {
        public int MatchColor(Bitmap bmpA, int argb, Rectangle? rect, int? tolerance)
        {
            try
            {
                using (Mat refMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmpA)) //大图
                {
                    return MatchColorBase(refMat, argb, rect, tolerance);
                }
            }
            catch (Exception ex) { Print($"{ex.Message}"); }
            return 0;
        }

        public SD.Point? MatchImage(Bitmap bmpA, Bitmap bmpB, Rectangle? rect, double? tolerance)
        {
            try
            {
                using (Mat refMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmpA)) //大图
                using (Mat tplMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmpB)) //小图
                {
                    return MatchImageBase(refMat, tplMat, rect, tolerance);
                }
            }
            catch (Exception ex) { Print($"{ex.Message}"); }
            return null;
        }
        public SD.Point? MatchImage(Bitmap bmpA, string pathB, Rectangle? rect, double? tolerance)
        {
            try
            {
                using (Mat refMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmpA)) //大图
                using (Mat tplMat = new Mat(pathB)) //小图
                {
                    return MatchImageBase(refMat, tplMat, rect, tolerance);
                }
            }
            catch (Exception ex) { Print($"{ex.Message}"); }
            return null;
        }

        public string MatchNumber(Bitmap bmpA, Rectangle rect, bool isWhiteText, DeviceType deviceType, ModelType language, double zoomratio)
        {
            try
            {
                using (Mat refMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmpA)) //大图
                {
                    return MatchTextBase(refMat, rect, isWhiteText, deviceType, language, zoomratio, TextType.Number);
                }
            }
            catch (Exception ex) { Print($"{ex.Message}"); }
            return string.Empty;
        }

        public string MatchText(Bitmap bmpA, Rectangle rect, bool isWhiteText, DeviceType deviceType, ModelType language, double zoomratio)
        {
            try
            {
                using (Mat refMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(bmpA)) //大图
                {
                    return MatchTextBase(refMat, rect, isWhiteText, deviceType, language, zoomratio, TextType.Word);
                };
            }
            catch (Exception ex) { Print($"{ex.Message}"); }
            return string.Empty;
        }
    }
}

//对内
namespace CustomMacroBase.PixelMatcher
{
    //MatchColorBase
    public partial class OpenCV
    {
        private int MatchColorBase(Mat _refMat, int argb, Rectangle? _cropRect, int? tolerance)
        {
            int result = 0;
            Color targetColor = Color.FromArgb(argb);

            using (_refMat = _refMat.CvtColor(ColorConversionCodes.BGRA2BGR))
            using (Mat flowMat = new())
            {
                Rectangle crop = (_cropRect ?? new(0, 0, _refMat.Width, _refMat.Height));//缺省范围设定为全图（但全图找色无意义）
                if (_cropRect is not null)
                {
                    _refMat = _refMat.Clone(new Rect(crop.X, crop.Y, crop.Width, crop.Height));//裁剪指定范围
                }

                tolerance ??= 20;

                Scalar minScalar = new Scalar()
                {
                    Val0 = Math.Clamp(targetColor.B - (tolerance ?? default), 0, 255),
                    Val1 = Math.Clamp(targetColor.G - (tolerance ?? default), 0, 255),
                    Val2 = Math.Clamp(targetColor.R - (tolerance ?? default), 0, 255),
                };
                Scalar maxScalar = new Scalar()
                {
                    Val0 = Math.Clamp(targetColor.B + (tolerance ?? default), 0, 255),
                    Val1 = Math.Clamp(targetColor.G + (tolerance ?? default), 0, 255),
                    Val2 = Math.Clamp(targetColor.R + (tolerance ?? default), 0, 255),
                };

                SaveToFlow(_refMat, flowMat);//储存流程
                Cv2.InRange(_refMat, minScalar, maxScalar, _refMat);//取出指定颜色（前景白背景黑）
                SaveToFlow(_refMat.CvtColor(ColorConversionCodes.GRAY2BGR), flowMat);//储存流程

                this.UpdateToSnapshotArea(flowMat.Clone());

                //unsafe
                //{
                //    byte* ptr = (byte*)_refMat.Ptr(0).ToPointer();
                //    int hit_count = 0;
                //    for (int i = 0; i < (_refMat.Width * _refMat.Height * 1); i += 1) //Gray单通道
                //    {
                //        if (ptr[i] == 255) { hit_count++; }
                //    }
                //    result = hit_count;
                //}

                result = Cv2.CountNonZero(_refMat);
            }

            return result;
        }
    }

    //MatchImageBase
    public partial class OpenCV
    {
        private SD.Point? MatchImageBase(Mat _refMat, Mat _tplMat, Rectangle? _cropRect, double? _tolerance, Mat _mask = null)
        {
            SD.Point? result = null;

            // 検索対象の画像とテンプレート画像
            using (_refMat = _refMat.CvtColor(ColorConversionCodes.BGRA2BGR))
            using (_tplMat = _tplMat.CvtColor(ColorConversionCodes.BGRA2BGR))
            using (Mat resMat = new())
            {
                //必要な範囲だけ切り出して計算量を減らす
                Mat original;
                Rectangle crop = (_cropRect ?? new());
                if (_cropRect is not null)
                {
                    if ((crop.Width < _tplMat.Width) || (crop.Height < _tplMat.Height)) { return result; }

                    original = _refMat.Clone().CvtColor(ColorConversionCodes.BGRA2BGR);
                    _refMat = _refMat.Clone(new Rect(crop.X, crop.Y, crop.Width, crop.Height));
                }
                else
                {
                    original = _refMat.Clone();
                }
                //類似度
                _tolerance ??= 0.8;

                // テンプレートマッチ
                Cv2.MatchTemplate(_refMat, _tplMat, resMat, TemplateMatchModes.CCoeffNormed, _mask);

                // しきい値の範囲に絞る
                Cv2.Threshold(resMat, resMat, 0.8, 1.0, ThresholdTypes.Tozero);

                // 類似度が最大/最小となる画素の位置を調べる
                Cv2.MinMaxLoc(resMat, out _, //minval
                                      out double maxval,
                                      out _, //minloc
                                      out Point maxloc);

                // しきい値で判断
                if (maxval >= _tolerance)
                {
                    result = new(crop.X + maxloc.X, crop.Y + maxloc.Y);

                    ////// 最も見つかった場所に赤枠を表示
                    Cv2.Rectangle(original,
                                  new Rect(crop.X + maxloc.X, crop.Y + maxloc.Y, _tplMat.Width, _tplMat.Height),
                                  new Scalar(0, 0, 255), 2);
                    //
                }

                this.UpdateToSnapshotArea(original.Clone());//
            }

            return result;
        }
    }

    //MatchTextBase
    public partial class OpenCV
    {
        //const string TessData = @".\tessdata";
        //const string CharWhitelist = @"1234567890";
        //OCRTesseract tesseract = OCRTesseract.Create(TessData, "eng", CharWhitelist);//OpenCvSharp自带的比较拉胯，不予采用

        //tesseract.Run(cropMat, out var outputText, out _, out _, out _, ComponentLevels.Word);//OpenCvSharp自带的比较拉胯，不予采用
        //return outputText;

        //缩放倍率
        List<Action<Mat, Mat, double>> PreActionResize = new()
        {
            (x,y,r)=>{ Cv2.Resize(x, y, new OpenCvSharp.Size(), r, r, InterpolationFlags.Lanczos4);},//放大or缩小
        };

        //黑底白字输入
        List<Action<Mat, Mat>> PreActionWhiteText = new()
        {
            //(x,y)=>{ Cv2.Resize(x, y, new OpenCvSharp.Size(), 2, 2, InterpolationFlags.Lanczos4);},//放大
        };
        //白底黑字输入
        List<Action<Mat, Mat>> PreActionBlackText = new()
        {
            //(x,y)=>{ Cv2.Resize(x, y, new OpenCvSharp.Size(), 2, 2, InterpolationFlags.Lanczos4);},//放大
            (x,y)=>{ Cv2.BitwiseNot(x, y);},//黑白翻转，用以将白底黑字转为黑底白字
        };

        //通用流程（老头环找数字）
        List<Action<Mat, Mat>> PreActionNumber = new()
        {
            (x,y)=>{ Cv2.EqualizeHist(x, y); },//直方图均衡化（使黑纸白字下的白字突出）
            (x,y)=>{ Cv2.Dilate(x, y, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(2, 2)));},//膨胀（白吃黑）
            (x,y)=>{ Cv2.InRange(x, new Scalar(200, 200, 200), new Scalar(255, 255, 255), y);},//取出白色
            (x,y)=>{ Cv2.Erode(x, y, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(2, 2)));},//腐蚀（黑吃白）
            //(x,y)=>{ Cv2.BitwiseNot(x, y);},//黑白翻转
            //(x,y)=>{ },
            //(x,y)=>{ },
            //(x,y)=>{ Cv2.MedianBlur(x, y, 5); },//中值滤波
            //(x,y)=>{ Cv2.GaussianBlur(x, y, new OpenCvSharp.Size(3,3), 0); },//高斯滤波
            //(x,y)=>{ Cv2.Dilate(x, y, new Mat(new OpenCvSharp.Size(3, 3), MatType.CV_8UC1));},//膨胀（白吃黑）（该写法有概率失败）
            //(x,y)=>{ Cv2.Erode(x, y, new Mat(new OpenCvSharp.Size(3, 3), MatType.CV_8UC1)); }//腐蚀（黑吃白）（该写法有概率失败）
            //(x,y)=>{ Cv2.Threshold(x, y, 0, 255, ThresholdTypes.Otsu);},//大津二值化
        };

        //通用流程2（xb3找日文）
        List<Action<Mat, Mat>> PreActionText = new()
        {
            //(x,y)=>{ Cv2.Threshold(x, y, 0, 255, ThresholdTypes.Otsu);},//大津二值化
            //(x,y)=>{ Cv2.Dilate(x, y, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(2, 2)));},//膨胀（白吃黑）
            (x,y)=>{ Cv2.InRange(x, new Scalar(160, 160, 160), new Scalar(255, 255, 255), y);},//取出白色
            //(x,y)=>{ Cv2.Erode(x, y, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(2, 2)));},//腐蚀（黑吃白）
        };

        //private string MatchTextBase(Mat _refMat, Rectangle _cropRect, bool isWhiteText, string language, string whitelist, double zoomratio, TextType textType)
        //{
        //    //判断是白字还是黑字
        //    var pre_white_or_black = isWhiteText switch
        //    {
        //        true => PreActionWhiteText,
        //        false => PreActionBlackText
        //    };
        //    //判断是数字还是文字
        //    var pre_number_or_word = textType switch
        //    {
        //        TextType.Number => PreActionNumber,
        //        TextType.Word => PreActionText,
        //        _ => throw new InvalidOperationException()
        //    };
        //    //储存字符串
        //    List<string> resultStrList = new();

        //    using (Mat cropMat = _refMat.Clone(new Rect(_cropRect.X, _cropRect.Y, _cropRect.Width, _cropRect.Height)).CvtColor(ColorConversionCodes.BGRA2GRAY))
        //    using (Mat flowMat = new())
        //    {
        //        //1.预处理
        //        foreach (var action in PreActionResize)//缩放 对应流程
        //        {
        //            action.Invoke(cropMat, cropMat, zoomratio);
        //            SaveToFlow(cropMat, flowMat);//储存流程
        //        }
        //        foreach (var action in pre_white_or_black)//白字/黑字 对应流程
        //        {
        //            action.Invoke(cropMat, cropMat);
        //            SaveToFlow(cropMat, flowMat);//储存流程
        //        }
        //        foreach (var action in pre_number_or_word)//数字/常规字 对应流程
        //        {
        //            action.Invoke(cropMat, cropMat);
        //            SaveToFlow(cropMat, flowMat);//储存流程
        //        }

        //        using (Mat blackMat = cropMat.Clone())//黑底白字，用来膨胀找矩形
        //        using (Mat whiteMat = cropMat.Clone())//白底黑字，用来画矩形
        //        {
        //            Cv2.Dilate(blackMat, blackMat, Cv2.GetStructuringElement(MorphShapes.Ellipse, new OpenCvSharp.Size(16, 16)), null, 3);//膨胀3次
        //            Cv2.FindContours(blackMat, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);//找到轮廓

        //            Cv2.BitwiseNot(cropMat, cropMat);//获得白底黑字
        //            Cv2.BitwiseNot(whiteMat, whiteMat);//获得白底黑字

        //            Cv2.CvtColor(cropMat, cropMat, ColorConversionCodes.GRAY2BGR);//cropMat转为彩图
        //            Cv2.CvtColor(flowMat, flowMat, ColorConversionCodes.GRAY2BGR);//resultMat转为彩图

        //            int count = 0;
        //            foreach (var contour in contours)
        //            {
        //                var biggestContourRect = Cv2.BoundingRect(contour);//获得包含轮廓的最小矩形
        //                resultStrList.Add(TesseractGetText(whiteMat.Clone(biggestContourRect), language, whitelist));

        //                if (count > 2) { count = 0; }
        //                switch (count)
        //                {
        //                    case 0: Cv2.Rectangle(cropMat, biggestContourRect, new Scalar(0, 0, 255), 2); break;//把矩形画上去
        //                    case 1: Cv2.Rectangle(cropMat, biggestContourRect, new Scalar(0, 255, 0), 2); break;
        //                    case 2: Cv2.Rectangle(cropMat, biggestContourRect, new Scalar(255, 0, 0), 2); break;
        //                }
        //                count++;
        //            }

        //            SaveToFlow(cropMat, flowMat);//储存流程 
        //        }

        //        UpdateManager?.CanUpdate()?.UpdateFrames(flowMat.Clone());//展示流程(用Clone以防止flowMat被过早地回收)

        //        return string.Join("\n", resultStrList.ToArray());
        //    }
        //}
    }

    //MatchTextBase_PaddleSharp
    public partial class OpenCV
    {
        private string MatchTextBase(Mat _refMat, Rectangle _cropRect, bool isWhiteText, DeviceType _deviceType, ModelType _modelType, double zoomratio, TextType textType = TextType.Word)
        {
            var text = string.Empty;

            {
                //白字黑字
                var pre_white_or_black = isWhiteText switch
                {
                    true => PreActionWhiteText,
                    false => PreActionBlackText
                };

                if (paddleOcrAllManager.GetPaddleOcrAll(_deviceType, _modelType) is PaddleOcrAll all)
                {
                    using (all.Clone())
                    using (Mat cropMat = _refMat.Clone(new Rect(_cropRect.X, _cropRect.Y, _cropRect.Width, _cropRect.Height)).CvtColor(ColorConversionCodes.BGRA2BGR))
                    using (Mat flowMat = new())
                    {
                        foreach (var action in PreActionResize)//缩放 对应流程
                        {
                            action.Invoke(cropMat, cropMat, zoomratio);
                            SaveToFlow(cropMat, flowMat); //储存流程
                        }
                        foreach (var action in pre_white_or_black)//白字/黑字 对应流程
                        {
                            action.Invoke(cropMat, cropMat);
                            SaveToFlow(cropMat, flowMat);//储存流程
                        }

                        this.UpdateToSnapshotArea(flowMat.Clone());

                        //PaddleOCR读取文本
                        switch (textType)
                        {
                            case TextType.Word:
                                {
                                    text = all.Run(cropMat.Clone()).Text;
                                    break;
                                }
                            case TextType.Number:
                                {
                                    var input = all.Run(cropMat.Clone()).Text;
                                    var match = Regex.Match(input, @"\d+", RegexOptions.Singleline);
                                    text = match.Success ? match.Value : string.Empty;
                                    break;
                                }
                            default: throw new NotImplementedException();
                        }
                    }
                }
            }

            return text;
        }
    }

    //内部专用方法
    public partial class OpenCV
    {
        //OCR读取文本
        //private string TesseractGetText(Mat input, string language, string? whitelist = null)
        //{
        //    using (var tesseract = new TesseractEngine(@".\tessdata", language))//"eng"
        //    {
        //        if (whitelist is not null && (whitelist.Trim().Length > 0))
        //        {
        //            tesseract.SetVariable("tessedit_char_whitelist", whitelist);
        //        }//@"1234567890"

        //        using (var ms = new MemoryStream())
        //        {
        //            OpenCvSharp.Extensions.BitmapConverter.ToBitmap(input).Save(ms, SD.Imaging.ImageFormat.Png);

        //            using (var img = Pix.LoadFromMemory(ms.ToArray()))
        //            using (var page = tesseract.Process(img, PageSegMode.Auto))//
        //            {
        //                return page.GetText();
        //            }
        //        }
        //    }
        //}

        //将流程储存至单个Mat以便展示
        private void SaveToFlow(Mat input, Mat flow)
        {
            if (flow.Width == 0 || flow.Height == 0)
            {
                Cv2.CopyTo(input, flow);
            }
            else
            {
                Cv2.VConcat(flow, input, flow);
            }
        }

        // コントラスト調整
        private void contrust(Mat src, Mat dst, float contrust)
        {
            byte[] lut = new byte[256];
            for (int i = 0; i < lut.Length; i++)
            {
                lut[i] = (byte)(255 / (1.0f + Math.Exp(-contrust * (i - 128) / 255)));
            }
            Cv2.LUT(src, lut, dst);
        }

        // ガンマ調整
        private void ganma(Mat src, Mat dst, float ganma)
        {
            byte[] lut = new byte[256];
            for (int i = 0; i < lut.Length; i++)
            {
                lut[i] = (byte)(Math.Pow((float)i / 255, 1.0f / ganma) * 255);
            }
            Cv2.LUT(src, lut, dst);
        }
    }
}