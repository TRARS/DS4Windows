using CustomMacroBase.Helper;
using OpenCvSharp;
using Sdcb.PaddleInference;
using Sdcb.PaddleOCR;
using Sdcb.PaddleOCR.Models.Local;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using static CustomMacroBase.PixelMatcher.OpenCV;

namespace CustomMacroBase.PixelMatcher
{
    struct PaddleOcrAllParameter
    {
        public DeviceType device { get; set; }
        public ModelType model { get; set; }

        public PaddleOcrAllParameter(DeviceType d, ModelType m)
        {
            device = d;
            model = m;
        }
    }

    sealed class FrameManager
    {
        bool can_update = false;
        bool task_is_running = false;

        public FrameManager()
        {
            Mediator.Instance.Register(MessageType.CanUpdateFrames, (para) =>
            {
                can_update = (bool)para;
            });
        }

        /// <summary>
        /// 截图区域未展开时不更新
        /// </summary>
        public FrameManager? CanUpdate()
        {
            return can_update ? this : null;
        }

        /// <summary>
        /// 将指定的Mat推给MacroWindow
        /// </summary>
        public void UpdateFrames(Mat source)
        {
            if (task_is_running is false)
            {
                task_is_running = true;

                Task.Run(async () =>
                {
                    await MediatorAsync.Instance.NotifyColleagues(AsyncMessageType.AsyncSnapshot, OpenCvSharp.Extensions.BitmapConverter.ToBitmap(source));
                    source.Dispose();
                }).ContinueWith(_ => { task_is_running = false; });
            }
            else
            {
                source.Dispose();
            }
        }
    }

    sealed class PaddleOcrAllManager
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

        //
        private void Print(string str = "")
        {
            Mediator.Instance.NotifyColleagues(MessageType.PrintNewMessage, str);
        }
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
}
