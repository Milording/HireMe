using System;

namespace HireMe.BL
{
    public interface ITriangleWorker
    {
        /// <summary>
        /// Calculate area of triangle.
        /// </summary>
        /// <param name="firstCathetus">Cathenus</param>
        /// <param name="secondCathetus">Cathenus</param>
        /// <returns>Area</returns>
        double CalculateAreaTriangle(double firstCathetus, double secondCathetus);
    }
}
