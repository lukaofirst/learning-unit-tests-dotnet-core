namespace Application.UseCases
{
	public static class CalculatorUseCase
	{
		public static double Sum(double a, double b) => a + b;

		public static bool IsPositiveNumber(double number)
		{
			if (number < 0)
				return false;

			return true;
		}

		public static double Divide(double a, double b)
		{
			if (b == 0)
				throw new InvalidOperationException($"{b} should not be zero");

			return a / b;
		}
	}
}