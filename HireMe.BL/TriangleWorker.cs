using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HireMe.BL
{
    /// <summary>
    /// Math calculating with 
    /// </summary>
    public class TriangleWorker : ITriangleWorker
    {
        #region Private fields

        /// <summary>
        /// Repository
        /// </summary>
        // private readonly IRepository _repository;

        #endregion

        #region Constructor

        /// <summary>
        /// If we want to work with files, we can add DI
        /// </summary>
        //public TriangleWorker(IRepository repository)
        //{
        //    this._repository = repository;
        //}

        #endregion

        #region Public methods

        /// <summary>
        /// Implements calculating area of triangle
        /// </summary>
        public double CalculateAreaTriangle(double firstCathetus, double secondCathetus)
        {
            if (firstCathetus < 0)
                throw new ArgumentOutOfRangeException(nameof(firstCathetus),
                    "Doesn't satisfy to restrictions of first cathenus.");
            else if (secondCathetus < 0)
                throw new ArgumentOutOfRangeException(nameof(secondCathetus),
                    "Doesn't satisfy to restrictions  of second cathenus.");

            return (firstCathetus*secondCathetus)/2.0;
        }

        #endregion
    }
}
