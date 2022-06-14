namespace Diploma_Master.Objects
{
    /// <summary>
    /// Класс объектов типа "Популяция". Содержит номер поколения, номер популяции среди поколения,
    /// экземпляр "Решения", значение целевой функции и флаг соответствия ограничениям
    /// </summary>
    public class PopulationObject
    {
        public int GenerationNumber { get; set; }

        public int IndividualNumber { get; set; }

        public SolutionObject Solution { get; set; }

        public float ObjectiveFunctionValue { get; set; }

        public bool ConstraintCheckResult { get; set; }
    }
}
