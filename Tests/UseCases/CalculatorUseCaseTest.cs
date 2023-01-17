using Application.UseCases;

namespace Tests;

public class CalculatorUseCaseTest
{
	[Fact]
	public void IsPositiveNumber_Should_Return_True_If_Number_Is_Positive()
	{
		var n1 = 10;

		var result = CalculatorUseCase.IsPositiveNumber(n1);

		Assert.True(result);
	}


	[Fact]
	public void Sum_Should_Return_True_If_Number_Type_Is_A_Double()
	{
		var n1 = 10;
		var n2 = 20;

		var result = CalculatorUseCase.Sum(n1, n2);

		Assert.IsType<double>(result);
	}

	[Fact]
	public void Divide_Should_Throw_InvalidOperationException_If_Second_Number_Is_Zero()
	{
		var n1 = 10;
		var n2 = 0;

		Assert.Throws<InvalidOperationException>(() => CalculatorUseCase.Divide(n1, n2));
	}
}