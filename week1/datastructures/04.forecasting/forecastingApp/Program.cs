using System;
using System.Collections.Generic;

public class FinancialData
{
    public int Month { get; set; }
    public double Revenue { get; set; }

    public FinancialData(int month, double revenue)
    {
        Month = month;
        Revenue = revenue;
    }
}

public class FinancialForecaster
{
    private List<FinancialData> data = new List<FinancialData>();

    public void AddData(int month, double revenue)
    {
        data.Add(new FinancialData(month, revenue));
    }

    // Forecast using Simple Linear Regression
    public double ForecastNextMonth_LinearRegression()
    {
        int n = data.Count;
        if (n == 0) return 0;

        double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

        foreach (var item in data)
        {
            sumX += item.Month;
            sumY += item.Revenue;
            sumXY += item.Month * item.Revenue;
            sumX2 += item.Month * item.Month;
        }

        double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
        double intercept = (sumY - slope * sumX) / n;

        int nextMonth = data[n - 1].Month + 1;
        return slope * nextMonth + intercept;
    }

    // Forecast using Moving Average
    public double ForecastNextMonth_MovingAverage(int windowSize)
    {
        if (data.Count < windowSize) return 0;

        Queue<double> window = new Queue<double>();
        double sum = 0;

        for (int i = data.Count - windowSize; i < data.Count; i++)
        {
            window.Enqueue(data[i].Revenue);
            sum += data[i].Revenue;
        }

        return sum / windowSize;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        FinancialForecaster forecaster = new FinancialForecaster();

        // Sample Revenue Data: Months 1 to 6
        forecaster.AddData(1, 10000);
        forecaster.AddData(2, 12000);
        forecaster.AddData(3, 13000);
        forecaster.AddData(4, 15000);
        forecaster.AddData(5, 16000);
        forecaster.AddData(6, 17000);

        // Forecast using Linear Regression
        double predictedLinear = forecaster.ForecastNextMonth_LinearRegression();
        Console.WriteLine("Forecast (Linear Regression) for next month: ₹" + predictedLinear);

        // Forecast using 3-month Moving Average
        double predictedMA = forecaster.ForecastNextMonth_MovingAverage(3);
        Console.WriteLine("Forecast (3-Month Moving Average): ₹" + predictedMA);
    }
}

