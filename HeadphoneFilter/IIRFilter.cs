public class IIRFilter
{
    private double[] _aCoefficients;
    private double[] _bCoefficients;
    private double[] _buffer;

    public IIRFilter(double[] aCoefficients, double[] bCoefficients)
    {
        _aCoefficients = aCoefficients;
        _bCoefficients = bCoefficients;
        _buffer = new double[Math.Max(aCoefficients.Length, bCoefficients.Length)];
    }

    public double Filter(double input)
    {
        double output = _bCoefficients[0] * input + _buffer[0];
        for (int i = 1; i < _bCoefficients.Length; i++)
        {
            _buffer[i - 1] = _bCoefficients[i] * input + _aCoefficients[i] * output + _buffer[i];
        }
        _buffer[_bCoefficients.Length - 1] = _aCoefficients[0] * output;

        return output;
    }
}
