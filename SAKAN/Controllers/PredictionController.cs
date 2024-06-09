using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SAKAN.DTO;
using SAKAN.Services;
using NumpyDotNet;
using System.Collections.Generic;
using Microsoft.ML;
using System.IO;
using System;

namespace SAKAN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        //private readonly PredictionService _predictionService;

        //public PredictionController(PredictionService predictionService)
        //{
        //    _predictionService = predictionService;
        //}

        //[HttpPost("predict")]
        //public ActionResult<int> Predict([FromBody] List<float> input)
        //{
        //    if (input.Count != 50)
        //    {
        //        return BadRequest("Input array must contain exactly 50 elements.");
        //    }

        //    var inputArray = input.ToArray();
        //    var prediction = _predictionService.Predict(inputArray);
        //    return Ok(prediction);
        //}

        private static PredictionEngine<ModelInput, ModelOutput> _predictionEngine;

        static PredictionController()
        {
            var mlContext = new MLContext();
            var modelPath = "D:\\SAKAN\\SAKAN\\MachineLearning\\model.zip";
            ITransformer model = mlContext.Model.Load(modelPath, out var ModelInput);

            _predictionEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(model);
        }

        [HttpPost("predict")]
        public ActionResult<ModelOutput> Predict([FromBody] ModelInput data)
        {
            if (data == null)
            {
                return BadRequest("Invalid input data");
            }

            var prediction = _predictionEngine.Predict(data);
            return Ok(prediction);
        }
    }
}
