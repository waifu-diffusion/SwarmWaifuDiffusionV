using System.IO;
using Newtonsoft.Json.Linq;
using SwarmUI.Builtin_ComfyUIBackend;
using SwarmUI.Core;
using SwarmUI.Text2Image;
using SwarmUI.Utils;

namespace WaifuTeam.SwarmExtensions;

public class SwarmWaifuDiffusionVExtension : Extension
{
    public static string NodeFolder;

    public static T2IRegisteredParam<string> SamplingMode, Schedule;
    public static List<string> SamplingModes = ["edm", "v_prediction"];
    public static List<string> Schedules = ["Tan", "XLTC", "XL10"];
    public static T2IRegisteredParam<bool> EnableRefiner, UseXLTC;
    public static T2IRegisteredParam<double> TanScaling, SigmaMin, SigmaMax;

    public override void OnPreInit()
    {
        NodeFolder = Path.Join(FilePath, "WaifuNodes");
        ComfyUISelfStartBackend.CustomNodePaths.Add(NodeFolder);
        Logs.Init($"Adding {NodeFolder} to CustomNodePaths");
    }

    public override void OnInit()
    {
        T2IParamGroup wdVGroup = new("WaifuEDM", Toggles: true, Open: false, OrderPriority: -8);

        EnableRefiner = T2IParamTypes.Register<bool>(new(
            Name: "Enable Refiner",
            Description: "[WaifuEDM] Enable for refiner model",
            Default: "true",
            Group: wdVGroup,
            FeatureFlag: "comfyui",
            OrderPriority: 10,
            ChangeWeight: 10
        ));

        SamplingMode = T2IParamTypes.Register<string>(new(
            Name: "Sampling Mode",
            Description: "[WaifuEDM] Sampling Mode",
            Default: "edm",
            Group: wdVGroup,
            FeatureFlag: "comfyui",
            GetValues: (_) => SamplingModes,
            IsAdvanced: true,
            ChangeWeight: 10,
            OrderPriority: 20
        ));
        Schedule = T2IParamTypes.Register<string>(new(
            Name: "Schedule Type",
            Description: "[WaifuEDM] Schedule Type",
            Default: "XLTC",
            Group: wdVGroup,
            FeatureFlag: "comfyui",
            GetValues: (_) => Schedules,
            IsAdvanced: true,
            ChangeWeight: 10,
            OrderPriority: 20
        ));
        TanScaling = T2IParamTypes.Register<double>(new(
            Name: "Tan Scaling",
            Description: "[WaifuEDM] Tan Schedule Scaling",
            Default: "1.6",
            Group: wdVGroup,
            FeatureFlag: "comfyui",
            IsAdvanced: true,
            ChangeWeight: 10,
            OrderPriority: 40
        ));

        SigmaMin = T2IParamTypes.Register<double>(new(
            Name: "Sigma Min",
            Description: "[WaifuEDM] Sigma Min",
            Default: "0.001",
            IgnoreIf: "0.001",
            Min: 0.0,
            Max: 1000.0,
            Step: 0.001,
            ViewMin: 0.001,
            ViewMax: 1000.0,
            ViewType: ParamViewType.SLIDER,
            Group: wdVGroup,
            FeatureFlag: "comfyui",
            IsAdvanced: true,
            ChangeWeight: 10,
            OrderPriority: 50
        ));
        SigmaMax = T2IParamTypes.Register<double>(new(
            Name: "Sigma Max",
            Description: "[WaifuEDM] Sigma Max",
            Default: "1000.0",
            IgnoreIf: "1000.0",
            Min: 0.0,
            Max: 1000.0,
            Step: 0.001,
            ViewMin: 0.001,
            ViewMax: 1000.0,
            ViewType: ParamViewType.SLIDER,
            Group: wdVGroup,
            FeatureFlag: "comfyui",
            IsAdvanced: true,
            ChangeWeight: 10,
            OrderPriority: 60
        ));

        WorkflowGenerator.AddModelGenStep(g =>
            {
                if (g.UserInput.TryGet(SamplingMode, out string samplingMode))
                {
                    if (g.LoadingModelType == "Base" || g.UserInput.Get(EnableRefiner, false))
                    {
                        string waifuNode = g.CreateNode("ModelSamplingWaifuEDM", new JObject()
                        {
                            ["model"] = g.LoadingModel,
                            ["sampling"] = samplingMode,
                            ["schedule"] = g.UserInput.Get(Schedule, "Tan"),
                            ["sigma_min"] = g.UserInput.Get(SigmaMin, 0.001),
                            ["sigma_max"] = g.UserInput.Get(SigmaMax, 1000.0),
                            ["tan_scaling"] = g.UserInput.Get(TanScaling, 1.6),
                        });
                        g.LoadingModel = [waifuNode, 0];
                    }
                }
            },
            priority: -20);

    }
}
