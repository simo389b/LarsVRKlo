using System;
using System.IO;
using System.Collections.Generic;




public static class txtReader {
    public static float[][] Read(string filename) {
        List<float[]> arrays = new List<float[]>(); // list of arrays

        try {
            using (StreamReader sr = new StreamReader(filename)) {
                string line;

                while ((line = sr.ReadLine()) != null) {
                    string[] numbers = line.Split(',');
                    List<float> numberList = new List<float>();

                    foreach (string number in numbers) {
                        float parsedNumber;
                        if (float.TryParse(number, out parsedNumber)) {
                            numberList.Add(parsedNumber);
                        }
                    }

                    float[] numberArray = numberList.ToArray();
                    arrays.Add(numberArray);
                }
            }
        }
        catch (Exception e) {
            Console.WriteLine("The file could not be read:");
            Console.WriteLine(e.Message);
        }

        return arrays.ToArray();
    }
}
