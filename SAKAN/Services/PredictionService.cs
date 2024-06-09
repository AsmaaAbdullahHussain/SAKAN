using Microsoft.ML;
using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.Transforms.Onnx;
using SAKAN.DTO;
using System.Collections.Generic;
using System.Linq;

namespace SAKAN.Services
{
    public class PredictionService
    {


        private readonly InferenceSession _session;
        private readonly string _inputName;
        public PredictionService(string modelPath)
        {
            _session = new InferenceSession(modelPath);
            // Get the correct input name from the model
            _inputName = _session.InputMetadata.Keys.First();
        }

        public int Predict(float[] input)
        {
            var inputTensor = new DenseTensor<float>(input, new[] { 1, input.Length });

            var inputsDictionary = new List<NamedOnnxValue>
            {
                NamedOnnxValue.CreateFromTensor(_inputName, inputTensor)
            };

            using (var results = _session.Run(inputsDictionary))
            {
                var output = results.First().AsEnumerable<int>().ToList();
                return output.First(); // Assuming the model outputs a single cluster number
            }
        }


        //private readonly MLContext _mlContext;
        //private readonly string _modelPath;


        //public PredictionService(string modelPath)
        //{
        //    _mlContext = new MLContext();
        //    _modelPath = modelPath;
        //}

        //public PredictionEngine<InputModelNumpy, ModelOutput> LoadModel()
        //{
        //    var pipeline = _mlContext.Transforms.ApplyOnnxModel(_modelPath);
        //    var model = pipeline.Fit(_mlContext.Data.LoadFromEnumerable(new[] { new InputModelNumpy() }));
        //    return _mlContext.Model.CreatePredictionEngine<InputModelNumpy, ModelOutput>(model);
        //}



    }
}
