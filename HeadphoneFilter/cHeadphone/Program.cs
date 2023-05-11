// public class IIRFilter which is a class implementing Direct Form II IIR filter.
// public class Programfilter which is a class implementing the Main function which is used to test the IIR filter.

public class IIRFilter
{
    private double[] aCoeff;
    private double[] bCoeff;
    private double[] buffer;

    public IIRFilter(double[] aCoeff, double[] bCoeff)
    {
        this.aCoeff = aCoeff;
        this.bCoeff = bCoeff;
        this.buffer = new double[bCoeff.Length];
    }

    public double Filter(double input)
    {
        double output = bCoeff[0] * input + buffer[0];

        // Update the filter state
        for (int i = 1; i < bCoeff.Length; i++)
        {
            buffer[i - 1] = bCoeff[i] * input - aCoeff[i] * output + buffer[i];
        }

        return output;
    }
}




public class Programfilter
{
    public static void Main()
    {
        double[] aCoeffs = new double[] { 1.0, -1.24525246315612,0.565934636606677,0.453940070082387,-0.758680408607225,0.733083672469547,-0.55107677850137,0.415805482366671,-0.0880647353085067,-0.5054529018436,0.830418630397054,-0.638694701867444,0.10823149282507,0.133008559524082,-0.242733761945305,0.223586693463637,-0.219087362059433,0.125945370420115,-0.0367776740863153,-0.10951781916157,0.0760868679368595,-0.014750755144319 };
        //double[] aCoeffs = new double[] { 0.2 };
        double[] bCoeffs = new double[] { 1.05310447293916,-1.13698831708141,0.283535155146942,0.145309416735428,0.336706613337538,-0.183191039608832,0.100394660352824,-0.331736302598393,0.0475539710505442,0.431845505683033,-0.240945428286761,-0.287668303894534,0.18794103378925,-0.0736671002460522,-0.0468048018940633,0.0561272830910883,-0.10620039298891,-0.0211094669594043,0.00868745136004784,0.0467312004194969,-0.0243495451302891,-0.00238416395026929 };
        //double[] bCoeffs = new double[] { 0.7 };

        double[] audioSignal = { 
            0.2, 0.3, 0.1, -0.1, -0.3, -0.2, 0.0, 0.1, 0.3, 0.2, 0.1, -0.1, -0.2, -0.1, 0.1, 0.2 
        };


        IIRFilter filter = new IIRFilter(aCoeffs, bCoeffs);
        for (int i = 0; i < audioSignal.Length; i++)
        {
            Console.WriteLine(filter.Filter(audioSignal[i]));
        }
    }
}
