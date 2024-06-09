using Microsoft.ML.Data;

namespace SAKAN.DTO
{
    public class ModelOutput
    {
        public string Label { get; set; }
        public float[] Scores { get; set; }
    }
}
