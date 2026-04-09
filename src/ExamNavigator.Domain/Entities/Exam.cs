namespace ExamNavigator.Domain.Entities
{
    public sealed class Exam
    {
        public int Id { get; set; }

        public string CodiceMinisteriale { get; set; } = string.Empty;

        public string CodiceInterno { get; set; } = string.Empty;

        public string DescrizioneEsame { get; set; } = string.Empty;

        public int BodyPartId { get; set; }
    }
}
